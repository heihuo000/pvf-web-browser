// CodeMirror Viewer - 替代当前的 FileFormatter
// 使用 CodeMirror 6 实现更强大的文件查看器

import { EditorView, basicSetup, EditorView as CMEditorView } from "codemirror";
import { EditorState, StateField, StateEffect, RangeSetBuilder } from "@codemirror/state";
import { Compartment } from "@codemirror/state";
import { keymap, drawSelection, WidgetType, ViewPlugin, Decoration, highlightSpecialChars } from "@codemirror/view";
import { defaultKeymap } from "@codemirror/commands";
import { searchKeymap, highlightSelectionMatches } from "@codemirror/search";
import { syntaxHighlighting, defaultHighlightStyle, HighlightStyle } from "@codemirror/language";
import { tags } from "@lezer/highlight";
import pvfLanguageSupport from './pvf-language.js';

// 用于存储路径和名称映射的缓存
class NamePreviewCache {
  constructor() {
    this.cache = new Map();
    this.promises = new Map();
  }

  has(path) {
    return this.cache.has(path);
  }

  get(path) {
    return this.cache.get(path);
  }

  set(path, name) {
    this.cache.set(path, name);
  }

  hasPromise(path) {
    return this.promises.has(path);
  }

  getPromise(path) {
    return this.promises.get(path);
  }

  setPromise(path, promise) {
    this.promises.set(path, promise);
    promise.finally(() => {
      this.promises.delete(path);
    });
  }

  clear() {
    this.cache.clear();
    this.promises.clear();
  }
}

// 路径预览小部件
class PathPreviewWidget extends WidgetType {
  constructor(path, namePreviewCache, onPathClick) {
    super();
    this.path = path;
    this.namePreviewCache = namePreviewCache;
    this.onPathClick = onPathClick;
    this.name = null;
    this.isLoading = true;
  }

  async loadName() {
    if (this.namePreviewCache.has(this.path)) {
      this.name = this.namePreviewCache.get(this.path);
      this.isLoading = false;
      return this.name;
    }

    if (this.namePreviewCache.hasPromise(this.path)) {
      const promise = this.namePreviewCache.getPromise(this.path);
      this.name = await promise;
      this.isLoading = false;
      return this.name;
    }

    const promise = this.fetchName();
    this.namePreviewCache.setPromise(this.path, promise);
    this.name = await promise;
    this.isLoading = false;
    return this.name;
  }

  async fetchName() {
    try {
      // 尝试查找文件
      const actualPath = await this.findFileIgnoreCase(this.path);
      if (!actualPath) {
        this.namePreviewCache.set(this.path, '');
        return '';
      }

      const response = await fetch('/api/file?key=' + encodeURIComponent(actualPath));
      if (!response.ok) {
        this.namePreviewCache.set(this.path, '');
        return '';
      }

      const data = await response.json();
      if (data.error) {
        this.namePreviewCache.set(this.path, '');
        return '';
      }

      const content = data.content;

      // 尝试多种方式查找名称
      // 1. 查找 [name] 标签
      const nameMatch = content.match(/\[name\]\s*\n\s*([^\r\n]+)/i);
      if (nameMatch && nameMatch[1]) {
        const name = nameMatch[1].trim().replace(/^["'`]|["'`]$/g, '');
        this.namePreviewCache.set(this.path, name);
        return name;
      }

      // 2. 查找 -- 注释中的名称
      const commentMatch = content.match(/--.*?名称\s*[：:]\s*([^\r\n]+)/i);
      if (commentMatch && commentMatch[1]) {
        const name = commentMatch[1].trim();
        this.namePreviewCache.set(this.path, name);
        return name;
      }

      // 3. 查找第一行非注释内容
      const lines = content.split('\n');
      for (const line of lines) {
        const trimmedLine = line.trim();
        if (trimmedLine && !trimmedLine.startsWith('#') && !trimmedLine.startsWith('--') && trimmedLine.length < 50) {
          // 检查是否是路径而不是名称
          if (!trimmedLine.includes('/') && !trimmedLine.includes('\\')) {
            this.namePreviewCache.set(this.path, trimmedLine);
            return trimmedLine;
          }
        }
      }

      this.namePreviewCache.set(this.path, '');
      return '';
    } catch (error) {
      console.error('加载名称预览失败:', error);
      this.namePreviewCache.set(this.path, '');
      return '';
    }
  }

  async findFileIgnoreCase(path) {
    const lowerPath = path.toLowerCase();

    const variations = [
      path,
      path.toLowerCase(),
      path.toUpperCase(),
      'sqr/' + path,
      'sqr/' + lowerPath,
      'sqr/character/' + path,
      'sqr/character/' + lowerPath,
      'sprite/' + path,
      'sprite/' + lowerPath,
      'sound/' + path,
      'sound/' + lowerPath,
      'script/' + path,
      'script/' + lowerPath,
    ];

    for (const variation of variations) {
      try {
        const response = await fetch('/api/file?key=' + encodeURIComponent(variation));
        if (response.ok) {
          const data = await response.json();
          if (data.content && data.content.trim().length > 0 && !data.error) {
            return variation;
          }
        }
      } catch (error) {
        continue;
      }
    }
    return null;
  }

  eq(other) {
    return other.path === this.path && other.name === this.name;
  }

  toDOM() {
    const wrapper = document.createElement('span');
    wrapper.className = 'name-preview-wrapper';
    wrapper.style.display = 'inline-block';
    wrapper.style.marginLeft = '8px';
    wrapper.style.fontSize = '0.85em';
    wrapper.style.opacity = '0.7';
    wrapper.style.fontStyle = 'italic';
    wrapper.style.color = '#4ec9b0';

    const content = document.createElement('span');
    content.className = 'name-preview-content';

    if (this.isLoading) {
      content.textContent = '加载中...';
      content.style.color = '#858585';
    } else if (this.name) {
      content.textContent = this.name;
    } else {
      content.textContent = '';
    }

    wrapper.appendChild(content);

    // 添加点击事件（如果需要）
    if (this.onPathClick) {
      wrapper.style.cursor = 'pointer';
      wrapper.style.textDecoration = 'underline';
      wrapper.addEventListener('click', (e) => {
        e.preventDefault();
        e.stopPropagation();
        this.onPathClick(this.path);
      });
    }

    return wrapper;
  }

  ignoreEvent() {
    return false;
  }
}

// 名称预览插件
function namePreviewPlugin(namePreviewCache, onPathClick) {
  return ViewPlugin.fromClass(class {
    constructor(view) {
      this.view = view;
      this.namePreviewCache = namePreviewCache || new NamePreviewCache();
      this.onPathClick = onPathClick;
      this.widgets = new Map(); // 存储已创建的小部件
      this.loadPendingPreviews();
    }

    update(update) {
      if (update.docChanged || update.viewportChanged) {
        // 文档改变或视口改变时，重新加载预览
        setTimeout(() => this.loadPendingPreviews(), 100);
      }
    }

    async loadPendingPreviews() {
      const widgetsToLoad = [];

      // 遌历文档查找路径
      const text = this.view.state.doc.toString();
      const pathRegex = /`([^`]*?\/?[^`]*?\.(?:lst|equ|stk|act|ai|aic|key|nut|als|txt|tbl|str))`|"([^"]*?\/?[^"]*?\.(?:lst|equ|stk|act|ai|aic|key|nut|als|txt|tbl|str))"|'([^']*?\/?[^']*?\.(?:lst|equ|stk|act|ai|aic|key|nut|als|txt|tbl|str))'/gi;

      let match;
      while ((match = pathRegex.exec(text)) !== null) {
        let path;
        if (match[1]) path = match[1];
        else if (match[2]) path = match[2];
        else if (match[3]) path = match[3];
        else continue;

        const offset = match.index;

        // 检查是否已经创建了小部件
        if (!this.widgets.has(offset)) {
          const widget = new PathPreviewWidget(path, this.namePreviewCache, this.onPathClick);
          this.widgets.set(offset, widget);
          widgetsToLoad.push(widget);
        }
      }

      // 加载所有待加载的小部件
      for (const widget of widgetsToLoad) {
        await widget.loadName();
      }
    }

    destroy() {
      // 清理资源
      this.widgets.clear();
    }
  });
}

// 创建自定义语法高亮样式 - 使用 pvf-language.js 中的定义，这里仅做补充或覆盖
// 实际上 pvf-language.js 已经包含了完整的配色，这里保留为空或用于非PVF文件
const customHighlightStyle = HighlightStyle.define([]);

// 空白字符显示插件
const spaceDeco = Decoration.mark({ class: "cm-show-space" });
const tabDeco = Decoration.mark({ class: "cm-show-tab" });

const whitespacePlugin = ViewPlugin.fromClass(class {
    constructor(view) {
        this.decorations = this.getDecorations(view);
    }
    update(update) {
        if (update.docChanged || update.viewportChanged)
            this.decorations = this.getDecorations(update.view);
    }
    getDecorations(view) {
        const builder = new RangeSetBuilder();
        let count = 0;
        for (const { from, to } of view.visibleRanges) {
            const text = view.state.doc.sliceString(from, to);
            let pos = from;
            for (let i = 0; i < text.length; i++) {
                const char = text[i];
                if (char === ' ') {
                    builder.add(pos + i, pos + i + 1, spaceDeco);
                    count++;
                } else if (char === '\t') {
                    builder.add(pos + i, pos + i + 1, tabDeco);
                    count++;
                }
            }
        }
        // console.log(`Found ${count} whitespace characters in visible ranges`);
        return builder.finish();
    }
}, {
    decorations: v => v.decorations
});

const whitespaceTheme = EditorView.baseTheme({
    ".cm-show-space": {
        "background-image": "radial-gradient(#6e6e6e 1.5px, transparent 1.5px)",
        "background-size": "100% 100%",
        "background-position": "center",
        "background-repeat": "no-repeat"
    },
    ".cm-show-tab": {
        "background-image": "linear-gradient(to right, transparent 5%, #6e6e6e 15%, transparent 15%, transparent 85%, #6e6e6e 95%, transparent 95%)",
        "background-position": "bottom",
        "background-size": "100% 1px",
        "background-repeat": "no-repeat",
        "position": "relative"
    },
    ".cm-show-tab::after": {
        "content": "'→'",
        "position": "absolute",
        "top": "50%",
        "left": "50%",
        "transform": "translate(-50%, -50%)",
        "font-size": "10px",
        "color": "#6e6e6e",
        "pointer-events": "none",
        "opacity": "0.5"
    }
});

class CodeMirrorViewer {
    constructor() {
        this.view = null;
        this.currentFile = null;
        this.onPathClick = null;
        this.zoomLevel = 1;
        this.themeCompartment = new Compartment();
        this.languageCompartment = new Compartment();
        this.whitespaceCompartment = new Compartment();
        this.editableCompartment = new Compartment();
    }

    /**
     * 初始化 CodeMirror 编辑器
     * @param {HTMLElement} container - 容器元素
     * @param {Object} options - 配置选项
     */
    initialize(container, options = {}) {
        // 检查容器是否有效
        if (!container || !(container instanceof HTMLElement)) {
            throw new Error('Invalid container: must be anHTMLElement');
        }

        // 清空容器内容（移除"empty"提示或其他旧内容）
        container.innerHTML = '';

        // 使用官方推荐的 basicSetup，但添加只读模式
        const extensions = [
            basicSetup,
            this.editableCompartment.of(EditorView.editable.of(false)),

            // 自定义主题
            EditorView.theme({
                '&': {
                    height: '100%',
                    fontSize: '14px',
                    fontFamily: 'Consolas, Monaco, monospace',
                    backgroundColor: '#1e1e1e'
                },
                '.cm-content': {
                    padding: '10px',
                    backgroundColor: '#1e1e1e',
                    color: '#dcdcdc', // RGB(220, 220, 220) - 浅灰色
                    minHeight: '100%'
                },
                '.cm-gutters': {
                    backgroundColor: '#252526',
                    color: '#858585',
                    border: 'none',
                    borderRight: '1px solid #3c3c3c'
                },
                '.cm-activeLineGutter': {
                    backgroundColor: '#2d2d2d',
                    color: '#c6c6c6'
                },
                '.cm-activeLine': {
                    backgroundColor: '#2d2d2d'
                },
                '.cm-cursor': {
                    borderLeftColor: '#aeafad'
                },
                '&::selection': {
                    backgroundColor: '#3e6fa6'
                },
                '.cm-selectionBackground': {
                    backgroundColor: '#3e6fa6 !important'
                },
                '.cm-selectionMatch': {
                    backgroundColor: '#3e6fa6 !important'
                },
                '.cm-scroller': {
                    fontFamily: 'Consolas, Monaco, monospace',
                    overflow: 'auto'
                },
                '.cm-scroller::-webkit-scrollbar': {
                    width: '10px',
                    height: '10px'
                },
                '.cm-scroller::-webkit-scrollbar-track': {
                    background: '#1e1e1e'
                },
                '.cm-scroller::-webkit-scrollbar-thumb': {
                    background: '#424242',
                    borderRadius: '5px'
                },
                '.cm-scroller::-webkit-scrollbar-thumb:hover': {
                    background: '#4e4e4e'
                }
            }, { dark: true }),

            // 选择高亮渲染
            drawSelection(),
            highlightSelectionMatches(),

            // 语法高亮
            syntaxHighlighting(customHighlightStyle),
            syntaxHighlighting(defaultHighlightStyle),

            // 空白字符显示
            whitespacePlugin,
            whitespaceTheme,

            // PVF 语言支持
            this.languageCompartment.of([]),

            // 搜索
            keymap.of(searchKeymap),

            // 额外的快捷键
            keymap.of(defaultKeymap),

            // 缩放支持的主题
            this.themeCompartment.of(EditorView.theme({
                '&': { fontSize: '14px' }
            })),
        ];

        // 创建编辑器状态
        const state = EditorState.create({
            doc: '',
            extensions: extensions
        });

        // 确保容器有合适的样式
        container.style.height = '100%';
        container.style.width = '100%';
        container.style.display = 'flex';
        container.style.flexDirection = 'column';
        container.style.overflow = 'hidden';

        // 创建编辑器视图
        this.view = new EditorView({
            state: state,
            parent: container
        });

        // 添加点击事件监听
        this.view.dom.addEventListener('click', this.handleClick.bind(this));
    }

    /**
     * 处理点击事件
     * @param {MouseEvent} event 
     */
    handleClick(event) {
        const target = event.target;
        
        // 检查是否点击了路径链接
        if (target.classList.contains('path-link')) {
            event.preventDefault();
            event.stopPropagation();
            
            const path = target.dataset.path || target.textContent;
            if (path && this.onPathClick) {
                this.onPathClick(path);
            }
        }
    }

    /**
     * 加载文件内容
     * @param {string} content - 文件内容
     * @param {string} filename - 文件名
     */
    loadFile(content, filename) {
        this.currentFile = filename;

        // 设置语言支持
        this.setLanguage(filename);

        // 启用空白字符显示
        this.setShowWhitespace(true);

        // 更新编辑器内容
        this.view.dispatch({
            changes: { from: 0, to: this.view.state.doc.length, insert: content }
        });
    }

    /**
     * 设置语言支持
     * @param {string} filename - 文件名
     */
    setLanguage(filename) {
        const extension = filename ? filename.split('.').pop().toLowerCase() : '';
        
        // 如果是 PVF 相关文件，启用 PVF 语言
        if (['equ', 'lst', 'stk', 'act', 'ai', 'aic', 'key', 'nut', 'als', 'txt', 'tbl', 'str'].includes(extension)) {
            this.view.dispatch({
                effects: this.languageCompartment.reconfigure(pvfLanguageSupport)
            });
        } else {
            this.view.dispatch({
                effects: this.languageCompartment.reconfigure([])
            });
        }
    }

    /**
     * 设置路径点击回调
     * @param {Function} callback - 回调函数
     */
    setOnPathClick(callback) {
        this.onPathClick = callback;
    }

    /**
     * 设置缩放级别
     * @param {number} level - 缩放级别 (0.5 - 3.0)
     */
    setZoom(level) {
        this.zoomLevel = Math.max(0.5, Math.min(3.0, level));
        
        // 更新主题中的字体大小
        const themeExtension = EditorView.theme({
            '&': { fontSize: `${14 * this.zoomLevel}px` }
        });
        
        this.view.dispatch({
            effects: this.themeCompartment.reconfigure(themeExtension)
        });
    }

    /**
     * 获取当前缩放级别
     * @returns {number} 缩放级别
     */
    getZoom() {
        return this.zoomLevel;
    }

    /**
     * 销毁编辑器
     */
    destroy() {
        if (this.view) {
            this.view.destroy();
            this.view = null;
        }
    }

    /**
     * 获取当前文件名
     * @returns {string} 文件名
     */
    getCurrentFile() {
        return this.currentFile;
    }

    setShowWhitespace(show) {
        let extension = [];
        if (show) {
            // 使用 highlightSpecialChars 显示不可见字符，并添加自定义 CSS 样式
            extension = [
                highlightSpecialChars({
                    render: (code) => {
                        let span = document.createElement("span");
                        span.textContent = code === 32 ? "·" : (code === 9 ? "→" : "•");
                        span.style.color = "#6e6e6e";
                        span.style.pointerEvents = "none";
                        return span;
                    },
                    addSpecialChars: /[ \t]/ // 仅匹配空格和制表符
                })
            ];
        }

        this.view.dispatch({
            effects: this.whitespaceCompartment.reconfigure(extension)
        });
    }

    /**
     * 设置编辑模式
     * @param {boolean} editable - true 为可编辑模式，false 为只读模式
     */
    setEditable(editable) {
        if (!this.view) {
            return;
        }
        this.view.dispatch({
            effects: this.editableCompartment.reconfigure(
                EditorView.editable.of(editable)
            )
        });
    }

    /**
     * 获取当前编辑状态
     * @returns {boolean} 是否可编辑
     */
    isEditable() {
        if (!this.view) {
            return false;
        }
        return this.view.state.facet(EditorView.editable);
    }

    /**
     * 获取当前内容
     * @returns {string} 编辑器内容
     */
    getContent() {
        if (!this.view) {
            return '';
        }
        return this.view.state.doc.toString();
    }
}

// 导出类
export default CodeMirrorViewer;
