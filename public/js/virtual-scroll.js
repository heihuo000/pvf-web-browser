// 虚拟滚动模块 - 用于大文件优化

import { applySyntaxHighlighting, applyWhitespaceDisplay } from './file-formatter.js';

// 缩放管理器 - 内联实现
class ZoomManager {
    constructor() {
        this.scale = 1.0;
        this.minScale = 0.5;
        this.maxScale = 3.0;
        this.pinchDistance = 0;
        this.isPinching = false;
        this.onZoomChange = null;
        this.targetElement = null;
        this.loadScale();
    }
    
    setTarget(element) {
        this.targetElement = element;
        this.applyScale();
    }
    
    setZoomCallback(callback) {
        this.onZoomChange = callback;
    }
    
    zoomIn() {
        this.scale = Math.min(this.scale + 0.1, this.maxScale);
        this.applyScale();
        this.saveScale();
        if (this.onZoomChange) {
            this.onZoomChange(this.scale);
        }
    }
    
    zoomOut() {
        this.scale = Math.max(this.scale - 0.1, this.minScale);
        this.applyScale();
        this.saveScale();
        if (this.onZoomChange) {
            this.onZoomChange(this.scale);
        }
    }
    
    zoomTo(newScale) {
        this.scale = Math.max(this.minScale, Math.min(newScale, this.maxScale));
        this.applyScale();
        this.saveScale();
        if (this.onZoomChange) {
            this.onZoomChange(this.scale);
        }
    }
    
    reset() {
        this.scale = 1.0;
        this.applyScale();
        this.saveScale();
        if (this.onZoomChange) {
            this.onZoomChange(this.scale);
        }
    }
    
    applyScale() {
        if (!this.targetElement) return;
        
        const codeWithLines = this.targetElement.querySelector('.code-with-lines');
        if (codeWithLines) {
            // 保存当前的状态
            const currentTransform = codeWithLines.style.transform;
            
            // 只有当缩放比例改变时才更新
            const newTransform = `scale(${this.scale})`;
            if (currentTransform !== newTransform) {
                codeWithLines.style.transform = newTransform;
                codeWithLines.style.transformOrigin = 'top left';
                
                // 不修改codeWithLines的宽度和高度，因为这会影响虚拟滚动的布局
                // 缩放只通过transform实现，布局由viewport和content控制
            }
        }
    }
    
    setupPinchZoom(element) {
        if (!element) return;
        
        element.addEventListener('touchstart', (e) => {
            if (e.touches.length === 2) {
                this.isPinching = true;
                this.pinchDistance = this.getDistance(e.touches[0], e.touches[1]);
                e.preventDefault();
            }
        }, { passive: false });
        
        element.addEventListener('touchmove', (e) => {
            if (this.isPinching && e.touches.length === 2) {
                const currentDistance = this.getDistance(e.touches[0], e.touches[1]);
                const delta = currentDistance - this.pinchDistance;
                const sensitivity = 0.005;
                const newScale = this.scale + delta * sensitivity;
                this.zoomTo(newScale);
                this.pinchDistance = currentDistance;
                e.preventDefault();
            }
        }, { passive: false });
        
        element.addEventListener('touchend', (e) => {
            if (e.touches.length < 2) {
                this.isPinching = false;
            }
        });
    }
    
    setupKeyboardZoom() {
        document.addEventListener('keydown', (e) => {
            if ((e.ctrlKey || e.metaKey) && (e.key === '+' || e.key === '=')) {
                e.preventDefault();
                this.zoomIn();
            }
            if ((e.ctrlKey || e.metaKey) && e.key === '-') {
                e.preventDefault();
                this.zoomOut();
            }
            if ((e.ctrlKey || e.metaKey) && e.key === '0') {
                e.preventDefault();
                this.reset();
            }
        });
    }
    
    getDistance(touch1, touch2) {
        const dx = touch1.clientX - touch2.clientX;
        const dy = touch1.clientY - touch2.clientY;
        return Math.sqrt(dx * dx + dy * dy);
    }
    
    saveScale() {
        try {
            localStorage.setItem('pvf-zoom-scale', this.scale.toString());
        } catch (e) {
            console.warn('Failed to save zoom scale:', e);
        }
    }
    
    loadScale() {
        try {
            const saved = localStorage.getItem('pvf-zoom-scale');
            if (saved) {
                this.scale = Math.max(this.minScale, Math.min(parseFloat(saved), this.maxScale));
            }
        } catch (e) {
            console.warn('Failed to load zoom scale:', e);
        }
    }
}

// 创建单例
const zoomManager = new ZoomManager();

export class VirtualScrollManager {
    constructor() {
        this.container = null;
        this.viewport = null;
        this.currentFileName = '';
        this.data = {
            lines: [],
            languageClass: '',
            showWhitespace: false,
            lineHeight: 19.2, // 与CSS中的line-height一致 (1.6 * 12px)
            containerHeight: 0,
            visibleLines: 0
        };
        this.savedScrollLeft = 0;
        this.scrollTimeout = null;
        this.onScroll = null;
        this.onPathClick = null;
        
        // 名称预览缓存
        this.namePreviewCache = new Map();
        this.namePreviewPromises = new Map();
    }

    init(containerId, lines, languageClass, showWhitespace, callbacks = {}) {
        this.container = document.getElementById(containerId);
        if (!this.container) {
            console.error(`Container with id "${containerId}" not found`);
            return false;
        }

        // 确保 fileViewer 有正确的样式
        this.container.style.display = 'flex';
        this.container.style.flexDirection = 'column';
        this.container.style.flex = '1';
        this.container.style.minHeight = '0';
        this.container.style.overflow = 'hidden';

        // 使用全局缓存或实例缓存
        if (callbacks.namePreviewCache && callbacks.namePreviewPromises) {
            this.namePreviewCache = callbacks.namePreviewCache;
            this.namePreviewPromises = callbacks.namePreviewPromises;
        }

        this.currentFileName = callbacks.currentFile || '';
        this.data.lines = lines;
        this.data.languageClass = languageClass;
        this.data.showWhitespace = showWhitespace;
        this.data.totalHeight = lines.length * this.data.lineHeight;
        this.onScroll = callbacks.onScroll;
        this.onPathClick = callbacks.onPathClick;

        console.log(`VirtualScroll init START: container=${containerId}, clientHeight=${this.container.clientHeight}, totalLines=${lines.length}, totalHeight=${this.data.totalHeight}px`);

        // 清空容器
        this.container.innerHTML = '';
        
        // 创建虚拟滚动容器（这是实际滚动的容器）
        const virtualContainer = document.createElement('div');
        virtualContainer.className = 'virtual-scroll-container';
        virtualContainer.style.position = 'relative';
        virtualContainer.style.height = '100%';
        virtualContainer.style.width = '100%';
        virtualContainer.style.flex = '1';
        virtualContainer.style.minHeight = '0';
        this.container.appendChild(virtualContainer);
        
        // 更新 this.container 指向虚拟滚动容器
        this.container = virtualContainer;
        
        // 创建内容元素（用于撑开滚动区域）- 关键！
        this.content = document.createElement('div');
        this.content.className = 'virtual-scroll-content';
        this.content.style.height = `${this.data.totalHeight}px`;
        this.content.style.minHeight = `${this.data.totalHeight}px`;
        this.content.style.position = 'absolute';
        this.content.style.top = '0';
        this.content.style.left = '0';
        this.content.style.width = '100%';
        this.content.style.pointerEvents = 'none';
        this.content.style.zIndex = '0';
        this.container.appendChild(this.content);
        
        console.log(`Created content element with height=${this.content.style.height}, scrollHeight=${this.content.scrollHeight}`);
        
        // 创建视口元素（用于显示内容）
        this.viewport = document.createElement('div');
        this.viewport.className = 'virtual-scroll-viewport';
        this.viewport.style.position = 'absolute';
        this.viewport.style.top = '0';
        this.viewport.style.left = '0';
        this.viewport.style.width = '100%';
        // viewport 使用 pointerEvents: none 不会拦截滚动事件
        // 但链接元素会通过 CSS 设置 pointerEvents: auto
        this.viewport.style.pointerEvents = 'none';
        this.viewport.style.zIndex = '1';
        this.container.appendChild(this.viewport);
        
        // 保存原始容器的引用（即 virtual-scroll-container）
        this.scrollContainer = this.container;
        
        // 验证容器有正确的高度
        let computedHeight = this.container.clientHeight;
        if (computedHeight === 0 || computedHeight < 100) {
            // 如果容器高度为0或太小，尝试多种方法获取高度
            const computedStyle = window.getComputedStyle(this.container);
            const styleHeight = parseFloat(computedStyle.height);
            
            if (styleHeight > 0) {
                computedHeight = styleHeight;
            } else if (this.container.parentElement) {
                computedHeight = this.container.parentElement.clientHeight;
            }
            
            // 设置最小高度
            if (computedHeight < 200) {
                computedHeight = 500;
            }
            
            this.container.style.height = `${computedHeight}px`;
        }
        
        this.data.containerHeight = computedHeight;
        
        // 计算可见行数
        this.data.visibleLines = Math.ceil(this.data.containerHeight / this.data.lineHeight) + 100;
        
        console.log(`VirtualScroll init FINAL: containerHeight=${this.data.containerHeight}px, contentHeight=${this.content.scrollHeight}px, lineHeight=${this.data.lineHeight}px, visibleLines=${this.data.visibleLines}, totalLines=${lines.length}`);

        // 初始渲染
        this.renderVisibleLines(0, 0);
        
        // 绑定事件
        this.bindEvents();
        
        // 设置缩放支持
        this.setupZoom();

        return true;
    }

    bindEvents() {
        // 监听scroll事件（在 scrollContainer 上）
        this.scrollContainer.addEventListener('scroll', () => {
            this.savedScrollLeft = this.scrollContainer.scrollLeft;
            
            clearTimeout(this.scrollTimeout);
            this.scrollTimeout = setTimeout(() => {
                this.renderVisibleLines(this.scrollContainer.scrollTop, this.savedScrollLeft);
                if (this.onScroll) {
                    this.onScroll(this.scrollContainer.scrollTop, this.scrollContainer.scrollLeft);
                }
            }, 16);
        }, { passive: true });

        // 触摸事件处理（移动端）
        let touchStartScrollLeft = 0;
        this.scrollContainer.addEventListener('touchstart', (e) => {
            touchStartScrollLeft = this.scrollContainer.scrollLeft;
        }, { passive: true });

        this.scrollContainer.addEventListener('touchend', () => {
            this.savedScrollLeft = this.scrollContainer.scrollLeft;
        }, { passive: true });

        // 监听窗口大小变化
        this.resizeHandler = () => {
            this.data.containerHeight = this.scrollContainer.clientHeight;
            this.data.visibleLines = Math.ceil(this.data.containerHeight / this.data.lineHeight) + 100;
            this.renderVisibleLines(this.scrollContainer.scrollTop, this.scrollContainer.scrollLeft);
            
            // 窗口大小变化时更新内容宽度
            requestAnimationFrame(() => this.updateContentWidth());
        };
        
        window.addEventListener('resize', this.resizeHandler);
    }
    
    setupZoom() {
        // 设置缩放管理器的目标容器
        zoomManager.setTarget(this.viewport);
        
        // 设置缩放变化回调
        zoomManager.setZoomCallback((scale) => {
            // 缩放后重新渲染内容
            this.renderVisibleLines(this.scrollContainer.scrollTop, this.scrollContainer.scrollLeft);
            
            // 缩放后更新内容宽度
            requestAnimationFrame(() => this.updateContentWidth());
        });
        
        // 设置捏合缩放
        zoomManager.setupPinchZoom(this.scrollContainer);
        
        // 设置键盘缩放（只初始化一次）
        if (!zoomManager.keyboardInitialized) {
            zoomManager.setupKeyboardZoom();
            zoomManager.keyboardInitialized = true;
        }
    }

    renderVisibleLines(scrollTop, savedScrollLeft = 0) {
        if (!this.viewport || !this.scrollContainer) return;
        
        // 如果没有传入savedScrollLeft，使用当前值
        if (typeof savedScrollLeft !== 'number' || savedScrollLeft === 0) {
            savedScrollLeft = this.scrollContainer.scrollLeft || 0;
        }
        
        // 确保滚动位置在有效范围内
        const maxScrollTop = Math.max(0, this.data.totalHeight - this.data.containerHeight);
        scrollTop = Math.min(Math.max(0, scrollTop), maxScrollTop);

        // 计算起始行和结束行
        const startLine = Math.floor(scrollTop / this.data.lineHeight);

        // 确保渲染到文件末尾，特别处理边界情况
        let endLine = Math.min(startLine + this.data.visibleLines, this.data.lines.length);

        // 如果接近文件末尾，确保渲染所有剩余行
        // 当剩余行数少于可见行数时，直接渲染到文件末尾
        if (this.data.lines.length - startLine < this.data.visibleLines) {
            endLine = this.data.lines.length;
        }
        
        // 计算视口位置
        const viewportTop = startLine * this.data.lineHeight;
        const viewportHeight = (endLine - startLine) * this.data.lineHeight;
        
        this.viewport.style.top = `${viewportTop}px`;
        this.viewport.style.height = `${viewportHeight}px`;
        
        console.log(`Render: scrollTop=${scrollTop.toFixed(1)}, startLine=${startLine}, endLine=${endLine}, rendered=${endLine - startLine}/${this.data.lines.length}`);
        
        // 构建HTML
        let linesHtml = `<div class="code-with-lines ${this.data.languageClass}">`;
        let lineNumbersHtml = '<div class="line-numbers">';
        let codeContentHtml = '<div class="code-content">';

        for (let i = startLine; i < endLine; i++) {
            const line = this.data.lines[i] || '';
            lineNumbersHtml += `<div class="line-number">${i + 1}</div>`;
            const displayLine = this.escapeHtml(line);
            codeContentHtml += `<div class="code-line">${displayLine}</div>`;
        }

        lineNumbersHtml += '</div>';
        codeContentHtml += '</div>';
        linesHtml += lineNumbersHtml + codeContentHtml + '</div>';

        this.viewport.innerHTML = linesHtml;

        // 立即应用缩放，避免闪烁
        zoomManager.applyScale();
        
        // 更新内容宽度
        this.updateContentWidth();

        // 应用语法高亮
        if (typeof Prism !== 'undefined') {
            requestAnimationFrame(() => {
                applySyntaxHighlighting(this.viewport);
                
                // 在语法高亮后应用空白字符显示
                if (this.data.showWhitespace) {
                    applyWhitespaceDisplay(this.viewport);
                }

                // 添加路径链接和名称预览
                const container = this.viewport;
                const pathRegex = /`([^`]*?\/?[^`]*?\.[a-zA-Z0-9]+)`|"([^"]*?\/?[^"]*?\.[a-zA-Z0-9]+)"|'([^']*?\/?[^']*?\.[a-zA-Z0-9]+)'/gi;
                const walker = document.createTreeWalker(
                    container,
                    NodeFilter.SHOW_TEXT,
                    {
                        acceptNode: function(node) {
                            if (node.textContent && !node.textContent.includes('<') && pathRegex.test(node.textContent)) {
                                return NodeFilter.FILTER_ACCEPT;
                            }
                            return NodeFilter.FILTER_REJECT;
                        }
                    },
                    false
                );

                const textNodes = [];
                let node;
                while (node = walker.nextNode()) {
                    textNodes.push(node);
                }

                // 检查是否是 .lst 文件
                const isLstFile = this.currentFileName && this.currentFileName.toLowerCase().endsWith('.lst');

                textNodes.forEach(textNode => {
                    const parent = textNode.parentNode;
                    if (parent) {
                        const fragment = document.createDocumentFragment();
                        let lastIndex = 0;
                        let match;

                        pathRegex.lastIndex = 0;

                        while ((match = pathRegex.exec(textNode.textContent)) !== null) {
                            if (match.index > lastIndex) {
                                fragment.appendChild(document.createTextNode(textNode.textContent.substring(lastIndex, match.index)));
                            }

                            const fullPath = match[0];
                            const backtickPath = match[1];
                            const doubleQuotePath = match[2];
                            const singleQuotePath = match[3];

                            let path, quoteChar;
                            if (backtickPath) {
                                path = backtickPath;
                                quoteChar = '`';
                            } else if (doubleQuotePath) {
                                path = doubleQuotePath;
                                quoteChar = '"';
                            } else if (singleQuotePath) {
                                path = singleQuotePath;
                                quoteChar = "'";
                            } else {
                                continue;
                            }

                            fragment.appendChild(document.createTextNode(quoteChar));

                            const link = document.createElement('span');
                            link.className = 'path-link';
                            link.textContent = path;
                            link.addEventListener('click', (e) => {
                                e.stopPropagation();
                                if (this.onPathClick) {
                                    this.onPathClick(path);
                                }
                            });

                            fragment.appendChild(link);
                            fragment.appendChild(document.createTextNode(quoteChar));

                            // 如果是 .lst 文件，添加名称预览占位符
                            if (isLstFile) {
                                const namePreview = document.createElement('span');
                                namePreview.className = 'name-preview';
                                namePreview.textContent = ' 加载中...';
                                namePreview.dataset.path = path;
                                fragment.appendChild(namePreview);
                            }

                            lastIndex = match.index + fullPath.length;
                        }

                        if (lastIndex < textNode.textContent.length) {
                            fragment.appendChild(document.createTextNode(textNode.textContent.substring(lastIndex)));
                        }

                        parent.replaceChild(fragment, textNode);
                    }
                });

                // 异步加载所有名称预览
                if (isLstFile) {
                    container.querySelectorAll('.name-preview[data-path]').forEach(previewElement => {
                        this.loadNamePreview(previewElement.dataset.path, previewElement);
                    });
                }
                
                // 恢复scrollLeft（在DOM操作后）
                requestAnimationFrame(() => {
                    if (this.scrollContainer.scrollLeft !== savedScrollLeft) {
                        this.scrollContainer.scrollLeft = savedScrollLeft;
                    }
                    
                    // 确保缩放状态正确（语法高亮可能会影响DOM结构）
                    requestAnimationFrame(() => {
                        zoomManager.applyScale();
                    });
                });
            });
        }
    }

    escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    destroy() {
        if (this.resizeHandler) {
            window.removeEventListener('resize', this.resizeHandler);
        }
        if (this.scrollTimeout) {
            clearTimeout(this.scrollTimeout);
        }
        if (this.scrollContainer) {
            this.scrollContainer.innerHTML = '';
        }
        // 不清空全局缓存，因为它是跨标签页共享的
        // 只清空 Promise 缓存
        this.namePreviewPromises.clear();
    }

    updateLines(lines, languageClass, showWhitespace) {
        this.data.lines = lines;
        this.data.languageClass = languageClass;
        this.data.showWhitespace = showWhitespace;
        this.data.totalHeight = lines.length * this.data.lineHeight;
        
        if (this.content) {
            this.content.style.height = `${this.data.totalHeight}px`;
        }
        
        this.renderVisibleLines(this.scrollContainer.scrollTop, this.scrollContainer.scrollLeft);
    }
    
    updateContentWidth() {
        if (!this.viewport || !this.content) return;
        
        // 获取代码内容的实际宽度
        const codeWithLines = this.viewport.querySelector('.code-with-lines');
        if (codeWithLines) {
            // 临时移除transform以获取未缩放的宽度
            const originalTransform = codeWithLines.style.transform;
            codeWithLines.style.transform = 'none';
            
            const contentWidth = codeWithLines.scrollWidth;
            const currentContentWidth = this.content.clientWidth;
            
            // 恢复transform
            codeWithLines.style.transform = originalTransform;
            
            // 考虑缩放比例，计算目标宽度
            const targetWidth = contentWidth * zoomManager.scale + 500;
            
            // 只在内容宽度真正变化时才更新
            if (Math.abs(targetWidth - currentContentWidth) > 10) {
                // 保存当前的滚动位置
                const savedScrollLeft = this.scrollContainer.scrollLeft;
                
                this.content.style.width = `${targetWidth}px`;
                
                // 恢复滚动位置
                this.scrollContainer.scrollLeft = savedScrollLeft;
            }
        }
    }

    async loadNamePreview(path, previewElement) {
        if (!previewElement) return;

        // 检查缓存
        if (this.namePreviewCache.has(path)) {
            const cachedName = this.namePreviewCache.get(path);
            if (cachedName) {
                previewElement.textContent = '  ' + cachedName;
                previewElement.style.display = 'inline';
            } else {
                previewElement.textContent = '';
                previewElement.style.display = 'none';
            }
            return;
        }

        // 检查是否有正在进行的请求
        if (this.namePreviewPromises.has(path)) {
            const promise = this.namePreviewPromises.get(path);
            const name = await promise;
            if (name) {
                previewElement.textContent = name;
                previewElement.style.display = 'inline';
            } else {
                previewElement.textContent = '';
                previewElement.style.display = 'none';
            }
            return;
        }

        // 创建新的请求Promise并缓存
        const promise = (async () => {
            try {
                const response = await fetch(`/api/file?key=${encodeURIComponent(path)}`);
                const data = await response.json();
                const content = data.content;

                // 提取 [name] 标签的内容
                const nameMatch = content.match(/\[name\]\s*\n\s*(.+?)(?:\n|$)/i);
                if (nameMatch && nameMatch[1]) {
                    const name = nameMatch[1].trim().replace(/^["'`]|["'`]$/g, '');
                    this.namePreviewCache.set(path, name);
                    return name;
                }

                // 尝试其他格式
                const commentMatch = content.match(/^--.*?名称\s*[：:]\s*(.+?)(?:\n|$)/im);
                if (commentMatch && commentMatch[1]) {
                    const name = commentMatch[1].trim();
                    this.namePreviewCache.set(path, name);
                    return name;
                }

                this.namePreviewCache.set(path, '');
                return '';
            } catch (error) {
                console.error('加载名称预览失败:', error);
                this.namePreviewCache.set(path, '');
                return '';
            } finally {
                this.namePreviewPromises.delete(path);
            }
        })();

        this.namePreviewPromises.set(path, promise);
        const name = await promise;

        if (name) {
            previewElement.textContent = name;
            previewElement.style.display = 'inline';
        } else {
            previewElement.textContent = '';
            previewElement.style.display = 'none';
        }
    }
}