// File formatting module - handles path links, name previews, whitespace display

export class FileFormatter {
    constructor(currentFile, onPathClick, onUpdate, namePreviewCache, namePreviewPromises) {
        this.currentFile = currentFile;
        this.onPathClick = onPathClick;
        this.onUpdate = onUpdate;
        
        // 使用全局缓存或创建实例缓存
        this.namePreviewCache = namePreviewCache || new Map();
        this.namePreviewPromises = namePreviewPromises || new Map();
    }

    setCurrentFile(file) {
        this.currentFile = file;
    }

    async addPathLinks(container) {
        if (!container) return;

        const isLstFile = this.currentFile && this.currentFile.toLowerCase().endsWith('.lst');
        const pathRegex = /`([^`]*?\/?[^`]*?\.[a-zA-Z0-9]+)`|"([^"]*?\/?[^"]*?\.[a-zA-Z0-9]+)"|'([^']*?\/?[^']*?\.[a-zA-Z0-9]+)'/gi;

        // 判断是否支持标签注释的文件类型
        const extension = this.currentFile ? this.currentFile.split('.').pop().toLowerCase() : '';
        const validExtensions = ['act', 'equ', 'lst', 'stk', 'ai', 'aic', 'key', 'nut', 'als', 'txt', 'tbl', 'str'];
        const supportTagDescriptions = validExtensions.includes(extension);

        const walker = document.createTreeWalker(
            container,
            NodeFilter.SHOW_TEXT,
            {
                acceptNode: function(node) {
                    if (node.textContent && !node.textContent.includes('<') && 
                        (pathRegex.test(node.textContent) || 
                         (supportTagDescriptions && /\[[^\]]+\]/.test(node.textContent)))) {
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

        textNodes.forEach(textNode => {
            const parent = textNode.parentNode;
            if (parent) {
                const fragment = document.createDocumentFragment();
                let lastIndex = 0;
                let match;

                // 先检查是否是标签行（行首有方括号）
                const lineText = textNode.textContent;
                const tagRegex = /^\s*\[([^\]]+)\]/;
                const tagMatch = lineText.match(tagRegex);

                if (supportTagDescriptions && tagMatch) {
                    // 这是标签行，只处理行首的标签

                    const tagName = tagMatch[1];
                    const tagStartIndex = tagMatch.index;
                    const tagEndIndex = tagStartIndex + tagMatch[0].length;

                    // 添加标签前的文本（包括空格）
                    if (tagStartIndex > 0) {
                        fragment.appendChild(document.createTextNode(lineText.substring(0, tagStartIndex)));
                    }

                    // 添加标签span（带特殊标记，让语法高亮识别）
                    const tagSpan = document.createElement('span');
                    tagSpan.className = 'tag-name';
                    tagSpan.textContent = `[${tagName}]`;
                    tagSpan.dataset.tag = tagName;
                    tagSpan.dataset.preserveHighlight = 'true'; // 标记需要保留
                    fragment.appendChild(tagSpan);

                    // 添加标签描述占位符
                    const tagPreview = document.createElement('span');
                    tagPreview.className = 'name-preview';
                    tagPreview.textContent = ' 加载中...';
                    tagPreview.dataset.tag = tagName;
                    tagPreview.dataset.preserveHighlight = 'true'; // 标记需要保留
                    fragment.appendChild(tagPreview);

                    // 添加标签后的文本（处理路径）
                    const remainingText = lineText.substring(tagEndIndex);
                    lastIndex = 0;

                    pathRegex.lastIndex = 0;
                    while ((match = pathRegex.exec(remainingText)) !== null) {
                        if (match.index > lastIndex) {
                            fragment.appendChild(document.createTextNode(remainingText.substring(lastIndex, match.index)));
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

                        // 跳过包含方括号的值（如 [demonic swordman] 或 [技能書 - 綠]·神圣支援）
                        if (path.includes('[') || path.includes(']')) {
                            // 不创建链接，但保留文本让语法高亮处理
                            fragment.appendChild(document.createTextNode(quoteChar + path + quoteChar));
                            lastIndex = match.index + fullPath.length;
                            continue;
                        }

                        fragment.appendChild(document.createTextNode(quoteChar));

                        const link = document.createElement('span');
                        link.className = 'path-link';
                        link.textContent = path;
                        link.addEventListener('click', async (e) => {
                            e.stopPropagation();
                            if (this.onPathClick) {
                                this.onPathClick(path);
                            }
                        });

                        fragment.appendChild(link);
                        fragment.appendChild(document.createTextNode(quoteChar));

                        if (isLstFile) {
                            const namePreview = document.createElement('span');
                            namePreview.className = 'name-preview';
                            namePreview.textContent = ' 加载中...';
                            namePreview.dataset.path = path;
                            fragment.appendChild(namePreview);
                        }

                        lastIndex = match.index + fullPath.length;
                    }

                    // 添加剩余文本
                    if (lastIndex < remainingText.length) {
                        fragment.appendChild(document.createTextNode(remainingText.substring(lastIndex)));
                    }
                } else {
                    // 不是标签行，只处理路径
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
                        
                                                // 跳过包含方括号的值（如 [demonic swordman] 或 [技能書 - 綠]·神圣支援）
                                                if (path.includes('[') || path.includes(']')) {
                                                    // 不创建链接，但保留文本让语法高亮处理
                                                    fragment.appendChild(document.createTextNode(quoteChar + path + quoteChar));
                                                    lastIndex = match.index + fullPath.length;
                                                    continue;
                                                }
                        
                                                fragment.appendChild(document.createTextNode(quoteChar));
                        
                                                const link = document.createElement('span');
                                                link.className = 'path-link';
                                                link.textContent = path;
                                                link.addEventListener('click', async (e) => {
                                                    e.stopPropagation();
                                                    if (this.onPathClick) {
                                                        this.onPathClick(path);
                                                    }
                                                });
                        
                                                fragment.appendChild(link);
                                                fragment.appendChild(document.createTextNode(quoteChar));
                        
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
                                        }
                        
                                        parent.replaceChild(fragment, textNode);
                                    }
                                });

        if (isLstFile) {
            container.querySelectorAll('.name-preview[data-path]').forEach(previewElement => {
                this.loadNamePreview(previewElement.dataset.path, previewElement);
            });
        }

        // 检查是否启用标签注释
        const showTagDescriptions = window.showTagDescriptionsMode !== undefined ? window.showTagDescriptionsMode : true;

        const tagPreviewElements = container.querySelectorAll('.name-preview[data-tag]');

        if (supportTagDescriptions && showTagDescriptions) {
            tagPreviewElements.forEach(previewElement => {
                this.loadTagDescription(extension, previewElement.dataset.tag, previewElement);
            });
        } else {
            // 隐藏所有标签预览元素
            tagPreviewElements.forEach(previewElement => {
                previewElement.textContent = '';
                previewElement.style.display = 'none';
            });
        }

        // 处理职业值描述（[usable job] 标签下的值）- 融合到标签注释逻辑中
        // 这个功能在语法高亮之后调用，所以不在这里处理
        // 值描述在 applyBracketValueHighlighting 中处理
    }

    async loadNamePreview(path, previewElement) {
        if (!previewElement) return;

        if (this.namePreviewCache.has(path)) {
            const cachedName = this.namePreviewCache.get(path);
            if (cachedName) {
                previewElement.textContent = cachedName;
                previewElement.style.display = 'inline';
            } else {
                previewElement.textContent = '';
                previewElement.style.display = 'none';
            }
            return;
        }

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

        const promise = (async () => {
            try {
                const actualPath = await this.findFileIgnoreCase(path);
                if (!actualPath) {
                    this.namePreviewCache.set(path, '');
                    return '';
                }

                const response = await fetch('/api/file?key=' + encodeURIComponent(actualPath));
                const data = await response.json();
                const content = data.content;

                // 更灵活的 [name] 标签匹配，支持多种格式
                const nameMatch = content.match(/\[name\][ \t]*(?:\r?\n|\r)[ \t]*([^\r\n]+)/i);
                if (nameMatch && nameMatch[1]) {
                    const name = nameMatch[1].trim().replace(/^["'`]|["'`]$/g, '');
                    this.namePreviewCache.set(path, name);
                    return name;
                }

                const commentMatch = content.match(/^--.*?名称\s*[：:]\s*(.+?)(?:\n|$)/im);
                if (commentMatch && commentMatch[1]) {
                    const name = commentMatch[1].trim();
                    this.namePreviewCache.set(path, name);
                    return name;
                }

                const firstLineMatch = content.match(/^(.+?)[\r\n]/);
                if (firstLineMatch && firstLineMatch[1]) {
                    const line = firstLineMatch[1].trim();
                    if (line.length > 0 && !line.startsWith('#') && !line.startsWith('--') && line.length < 50) {
                        this.namePreviewCache.set(path, line);
                        return line;
                    }
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

    async loadTagDescription(extension, tagName, previewElement) {
        if (!previewElement) return;

        // 判断是标签描述还是值描述
        const isValueDescription = previewElement.classList.contains('value-description');
        const cacheKey = isValueDescription ? `value:${tagName}` : `tag:${extension}:${tagName}`;

        if (this.namePreviewCache.has(cacheKey)) {
            const cachedDescription = this.namePreviewCache.get(cacheKey);
            if (cachedDescription) {
                previewElement.textContent = cachedDescription;
                previewElement.style.display = 'inline';
            } else {
                previewElement.textContent = '';
                previewElement.style.display = 'none';
            }
            return;
        }

        if (this.namePreviewPromises.has(cacheKey)) {
            const promise = this.namePreviewPromises.get(cacheKey);
            const description = await promise;
            if (description) {
                previewElement.textContent = description;
                previewElement.style.display = 'inline';
            } else {
                previewElement.textContent = '';
                previewElement.style.display = 'none';
            }
            return;
        }

        const promise = (async () => {
            try {
                let description = '';

                if (isValueDescription) {
                    // 加载值描述
                    const response = await fetch('/api/value-descriptions/job_values');
                    if (response.ok) {
                        const valueDescriptions = await response.json();
                        description = valueDescriptions[tagName] || '';
                    }
                } else {
                    // 加载标签描述
                    const response = await fetch(`/api/tag-descriptions/${extension}`);
                    if (response.ok) {
                        const tagDescriptions = await response.json();
                        const tagKey = `[${tagName}]`;
                        description = tagDescriptions[tagKey] || '';
                    }
                }

                this.namePreviewCache.set(cacheKey, description);
                return description;
            } catch (error) {
                console.error('加载描述失败:', error);
                this.namePreviewCache.set(cacheKey, '');
                return '';
            } finally {
                this.namePreviewPromises.delete(cacheKey);
            }
        })();

        this.namePreviewPromises.set(cacheKey, promise);
        const description = await promise;

        if (description) {
            previewElement.textContent = description;
            previewElement.style.display = 'inline';
        } else {
            previewElement.textContent = '';
            previewElement.style.display = 'none';
        }
    }

    async findFileIgnoreCase(path) {
        const currentDir = this.currentFile ? this.currentFile.substring(0, this.currentFile.lastIndexOf('/')) : '';
        const lowerPath = path.toLowerCase();
        
        const variations = [
            path,
            path.toLowerCase(),
            path.toUpperCase(),
            currentDir ? currentDir + '/' + path : null,
            currentDir ? currentDir + '/' + lowerPath : null,
            'sqr/' + path,
            'sqr/' + lowerPath,
            'sqr/character/' + path,
            'sqr/character/' + lowerPath,
        ].filter(v => v !== null);

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

    clearCache() {
        // 不清空全局缓存，因为它是跨标签页共享的
        // 只清空 Promise 缓存
        this.namePreviewPromises.clear();
    }

    destroy() {
        // 不清空全局缓存，因为它是跨标签页共享的
        // 只清空 Promise 缓存
        this.namePreviewPromises.clear();
    }
}

export function applySyntaxHighlighting(container) {
    if (typeof Prism !== 'undefined') {
        const codeLines = container.querySelectorAll('.code-line');
        const codeWithLines = container.querySelector('.code-with-lines');

        if (codeLines.length > 0) {
            // 获取语言类型
            const langClass = codeWithLines ? Array.from(codeWithLines.classList).find(cls => cls.startsWith('language-')) : null;
            const lang = langClass ? langClass.replace('language-', '') : 'pvf';

            // 逐行高亮
            codeLines.forEach((line) => {
                // 检查行内是否有需要保留的元素
                const preserveElements = line.querySelectorAll('[data-preserve-highlight="true"]');

                if (preserveElements.length > 0) {
                    // 有需要保留的元素，跳过这行的高亮
                    return;
                }

                const originalLine = line.textContent || '';

                if (originalLine.trim() && !originalLine.startsWith('#')) {
                    try {
                        const highlighted = Prism.highlight(originalLine, Prism.languages[lang] || Prism.languages.pvf, lang);
                        line.innerHTML = highlighted;
                    } catch (e) {
                        // 高亮失败，保持原样
                    }
                }
            });
        } else {
            // 降级方案：查找pre元素
            const preElement = container.querySelector('pre');
            if (preElement) {
                Prism.highlightElement(preElement);
            }
        }
    }
}

// 给包含方括号的字符串添加颜色（在语法高亮之后调用）
export function applyBracketValueHighlighting(container, extension = '') {
    if (!container) return;

    const codeLines = container.querySelectorAll('.code-line');

    // 如果没有传入extension，尝试从language-class获取
    if (!extension) {
        const codeWithLines = container.querySelector('.code-with-lines');
        const langClass = codeWithLines ? Array.from(codeWithLines.classList).find(cls => cls.startsWith('language-')) : null;
        if (langClass) {
            extension = langClass.replace('language-', '');
        }
    }
    
    const showTagDescriptions = window.showTagDescriptionsMode !== undefined ? window.showTagDescriptionsMode : true;

    console.log('[applyBracketValueHighlighting] extension:', extension, 'showTagDescriptions:', showTagDescriptions);

    codeLines.forEach((line, lineIndex) => {
        // 查找所有 token.string 元素
        const stringElements = line.querySelectorAll('.token.string');

        stringElements.forEach((stringElement) => {
            const text = stringElement.textContent;

            // 检查是否包含方括号
            if (text.includes('[') && text.includes(']')) {
                console.log(`[applyBracketValueHighlighting] 行${lineIndex}找到字符串:`, text.substring(0, 40));
                
                // 确保有正确的颜色
                stringElement.style.color = '#ce9178';
                
                // 如果是equ文件且启用标签注释，查找方括号值并添加描述
                if (extension === 'equ' && showTagDescriptions) {
                    const bracketRegex = /\[([^\]]+)\]/g;
                    let match;
                    
                    while ((match = bracketRegex.exec(text)) !== null) {
                        const value = match[0];
                        console.log('[applyBracketValueHighlighting] 找到值:', value);
                        
                        // 检查该行是否已经有这个值的描述
                        const existingDesc = line.querySelector(`.value-description[data-value="${value}"]`);
                        if (existingDesc) {
                            continue; // 已有描述，跳过
                        }

                        // 在行尾添加值描述
                        const valuePreview = document.createElement('span');
                        valuePreview.className = 'name-preview value-description';
                        valuePreview.textContent = ' 加载中...';
                        valuePreview.dataset.value = value;
                        valuePreview.dataset.preserveHighlight = 'true';

                        line.appendChild(valuePreview);

                        // 加载值描述
                        loadValueDescriptionForElement(value, valuePreview);
                    }
                }
            }
        });
    });
}

// 辅助函数：加载值描述
async function loadValueDescriptionForElement(value, previewElement) {
    if (!previewElement) return;

    const cacheKey = `value:${value}`;

    // 使用全局缓存
    if (window.namePreviewCache && window.namePreviewCache.has(cacheKey)) {
        const cachedDescription = window.namePreviewCache.get(cacheKey);
        if (cachedDescription) {
            previewElement.textContent = cachedDescription;
            previewElement.style.display = 'inline';
        } else {
            previewElement.textContent = '';
            previewElement.style.display = 'none';
        }
        return;
    }

    try {
        const response = await fetch('/api/value-descriptions/job_values');
        if (!response.ok) {
            if (window.namePreviewCache) window.namePreviewCache.set(cacheKey, '');
            previewElement.textContent = '';
            previewElement.style.display = 'none';
            return;
        }

        const valueDescriptions = await response.json();
        const description = valueDescriptions[value];

        if (description) {
            if (window.namePreviewCache) window.namePreviewCache.set(cacheKey, description);
            previewElement.textContent = description;
            previewElement.style.display = 'inline';
        } else {
            if (window.namePreviewCache) window.namePreviewCache.set(cacheKey, '');
            previewElement.textContent = '';
            previewElement.style.display = 'none';
        }
    } catch (error) {
        console.error('加载值描述失败:', error);
        if (window.namePreviewCache) window.namePreviewCache.set(cacheKey, '');
        previewElement.textContent = '';
        previewElement.style.display = 'none';
    }
}

export function applyWhitespaceDisplay(container) {
    if (!container) return;

    // 查找所有文本节点，替换其中的 Tab 和空格
    const walker = document.createTreeWalker(
        container,
        NodeFilter.SHOW_TEXT,
        null,
        false
    );

    const textNodes = [];
    let node;
    while (node = walker.nextNode()) {
        // 只处理文本节点，并且不包含 HTML 标签
        if (node.textContent && !node.textContent.includes('<')) {
            textNodes.push(node);
        }
    }

    textNodes.forEach(textNode => {
        const parent = textNode.parentNode;
        if (parent) {
            // 替换 Tab
            const parts = textNode.textContent.split('\t');
            if (parts.length > 1) {
                const fragment = document.createDocumentFragment();
                for (let i = 0; i < parts.length; i++) {
                    fragment.appendChild(document.createTextNode(parts[i]));
                    if (i < parts.length - 1) {
                        const tabSpan = document.createElement('span');
                        tabSpan.className = 'whitespace-tab';
                        tabSpan.textContent = '→';
                        fragment.appendChild(tabSpan);
                    }
                }
                parent.replaceChild(fragment, textNode);
            }
        }
    });
}