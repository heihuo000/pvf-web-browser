// 主入口文件 - 整合所有模块

import { BookmarkManager } from './bookmark.js';
import { FileMenuManager } from './file-menu.js';
import { ModalManager } from './modal.js';
import { FileFormatter, applySyntaxHighlighting, applyWhitespaceDisplay, applyBracketValueHighlighting } from './file-formatter.js';
import { API } from './api.js';
import { showLoading, downloadFile, positionDropdown, getLanguageClass, formatSize, escapeHtml, updateSelectedCount } from './utils.js';
import { ViewerManager } from './viewer-manager.js';
import CodeMirrorViewer from './codemirror-viewer.js';

// 全局状态
let currentFile = null;
let currentPath = '';
let batchMode = false;
let selectedFiles = new Set();
let selectedFolders = new Set(); // 存储被选中的文件夹路径
let showWhitespaceMode = true;
let showTagDescriptionsMode = true; // 标签描述模式开关

// 搜索结果存储
let currentSearchResults = [];
let searchResultsOffset = 0;
const SEARCH_RESULTS_PAGE_SIZE = 500;
let searchResultsTotal = 0;
let searchResultsHasMore = false;
let searchResultsLoading = false;
let currentSearchParams = null; // 存储当前搜索参数

// 文件内容缓存
const fileContentCache = new Map(); // key -> { content, encoding, lines, timestamp }

// 标签页管理
let tabs = [];
let activeTabId = null;

// 全局名称预览缓存（用于优化加载速度）
const globalNamePreviewCache = new Map();
const globalNamePreviewPromises = new Map();

// 查看器管理器
const viewerManager = new ViewerManager();

// 文件格式化器
const fileFormatter = new FileFormatter(null, async (path) => {
    const actualPath = await fileFormatter.findFileIgnoreCase(path);
    if (actualPath) {
        loadFileContent(actualPath);
    } else {
        updateStatus('文件未找到: ' + path);
    }
}, globalNamePreviewCache, globalNamePreviewPromises);

// DOM 元素
const elements = {
    toggleSidebarBtn: document.getElementById('toggleSidebar'),
    sidebar: document.getElementById('sidebar'),
    sidebarOverlay: document.getElementById('sidebarOverlay'),
    openBtn: document.getElementById('openBtn'),
    saveBtn: document.getElementById('saveBtn'),
    editBtn: document.getElementById('editBtn'),
    extractBtn: document.getElementById('extractBtn'),
    batchModeBtn: document.getElementById('batchModeBtn'),
    toggleWhitespaceBtn: document.getElementById('toggleWhitespaceBtn'),
    toggleTagDescriptionsBtn: document.getElementById('toggleTagDescriptionsBtn'),
    addBookmarkBtn: document.getElementById('addBookmarkBtn'),
    advancedSearchBtn: document.getElementById('advancedSearchBtn'),
    colorSettingsBtn: document.getElementById('colorSettingsBtn'),
    switchViewerBtn: document.getElementById('switchViewerBtn'),
    encodingSelect: document.getElementById('encodingSelect'),
    searchInput: document.getElementById('searchInput'),
    fileTree: document.getElementById('fileTree'),
    fileViewer: document.getElementById('fileViewer'),
    breadcrumb: document.getElementById('breadcrumb'),
    statusBar: document.getElementById('statusBar'),
    loading: document.getElementById('loading'),
    copyPathBtn: document.getElementById('copyPathBtn'),
    locateDirBtn: document.getElementById('locateDirBtn'),
    fileMenu: document.getElementById('fileMenu'),
    bookmarkMenu: document.getElementById('bookmarkMenu'),
    pvfFileMenu: document.getElementById('pvfFileMenu')
};

// 管理器实例
const bookmarkManager = new BookmarkManager();
const fileMenuManager = new FileMenuManager();
const modalManager = new ModalManager();

// 注册模态框
const openModal = modalManager.register('openModal');
const extractModal = modalManager.register('extractModal');
const saveModal = modalManager.register('saveModal');
const editModal = modalManager.register('editModal');
const batchExtractModal = modalManager.register('batchExtractModal');
const searchModal = modalManager.register('searchModal');
const editBookmarkModal = modalManager.register('editBookmarkModal');
const colorSettingsModal = modalManager.register('colorSettingsModal');

// 更新状态栏
function updateStatus(message) {
    elements.statusBar.textContent = message;
}

// 更新面包屑

function updateBreadcrumb(path) {

    if (!path) {

        elements.breadcrumb.textContent = '未选择文件';

        return;

    }



    elements.breadcrumb.textContent = path;
}

// 创建新标签页
function createTab(key) {
    const tabId = Date.now().toString();
    const fileName = key.split('/').pop();

    // 检查是否已存在
    const existingTab = tabs.find(t => t.key === key);
    if (existingTab) {
        switchToTab(existingTab.id);
        return existingTab.id;
    }

    const tab = {
        id: tabId,
        key: key,
        name: fileName,
        isActive: true,
        isLoaded: false
    };

    // 将其他标签设为非活动
    tabs.forEach(t => t.isActive = false);
    tabs.push(tab);
    activeTabId = tabId;

    renderTabs();
    return tabId;
}

// 切换标签页
function switchToTab(tabId) {
    const tab = tabs.find(t => t.id === tabId);
    if (!tab) return;

    tabs.forEach(t => t.isActive = t.id === tabId);
    activeTabId = tabId;

    currentFile = tab.key;
    updateBreadcrumb(tab.key);

    // 检查缓存
    const cached = fileContentCache.get(tab.key);
    if (cached) {
        // 使用缓存渲染
        renderFileFromCache(tab.key, cached);
        tab.isLoaded = true;
    } else {
        // 加载文件内容
        loadFileContent(tab.key);
    }

    renderTabs();
}

// 关闭标签页
function closeTab(tabId, event) {
    event.stopPropagation();

    const tabIndex = tabs.findIndex(t => t.id === tabId);
    if (tabIndex === -1) return;

    const tab = tabs[tabIndex];
    const wasActive = tab.isActive;

    // 检查是否还有其他标签使用这个文件
    const otherTabsWithSameFile = tabs.filter(t => t.key === tab.key && t.id !== tabId);

    // 如果没有其他标签使用这个文件，清除缓存
    if (otherTabsWithSameFile.length === 0) {
        fileContentCache.delete(tab.key);
    }

    tabs.splice(tabIndex, 1);

    if (wasActive && tabs.length > 0) {
        // 如果关闭的是当前活动标签，切换到相邻标签
        const newActiveIndex = Math.min(tabIndex, tabs.length - 1);
        switchToTab(tabs[newActiveIndex].id);
    } else if (tabs.length === 0) {
        // 如果没有标签了，清空文件查看器
        currentFile = null;
        elements.fileViewer.innerHTML = '<div class="empty">选择一个文件查看内容</div>';
        updateBreadcrumb('');
        activeTabId = null;
    }

    // 清理旧缓存（保留最近10个文件）
    if (fileContentCache.size > 10) {
        const entries = Array.from(fileContentCache.entries());
        entries.sort((a, b) => a[1].timestamp - b[1].timestamp);
        const toDelete = entries.slice(0, entries.length - 10);
        toDelete.forEach(([key]) => fileContentCache.delete(key));
    }

    renderTabs();
}

// 渲染标签页
function renderTabs() {
    const container = document.getElementById('tabsContainer');
    if (!container) return;

    if (tabs.length === 0) {
        container.innerHTML = '';
        return;
    }

    container.innerHTML = tabs.map(tab => `
        <div class="tab ${tab.isActive ? 'active' : ''}" data-tab-id="${tab.id}" onclick="switchToTab('${tab.id}')">
            <span class="tab-title">${tab.name}</span>
            <span class="tab-close" onclick="closeTab('${tab.id}', event)">×</span>
        </div>
    `).join('');
}

// 从缓存渲染文件内容
function renderFileFromCache(key, cached) {
    // 更新文件格式化器的当前文件
    fileFormatter.setCurrentFile(key);
    
    const ext = key.split('.').pop().toLowerCase();
    const languageClass = getLanguageClass(ext);
    let lines = cached.lines;
    let content = cached.content;
    
    // 移除 BOM (Byte Order Mark) - UTF-8 BOM 是 \uFEFF
    if (content.charCodeAt(0) === 0xFEFF) {
        content = content.slice(1);
        // 重新分割行
        lines = content.split('\n');
    }
    
    const lineCount = lines.length;

    // 更新按钮状态
    elements.extractBtn.disabled = false;
    const copyPathBtn = document.getElementById('copyPathBtn');
    const locateDirBtn = document.getElementById('locateDirBtn');
    if (copyPathBtn) copyPathBtn.disabled = false;
    if (locateDirBtn) locateDirBtn.disabled = false;

    const editableExtensions = ['txt', 'nut', 'str', 'lst', 'equ', 'stk', 'ai', 'aic', 'key', 'als', 'act', 'stm', 'ora', 'map', 'obj', 'dgn'];
    elements.editBtn.disabled = !editableExtensions.includes(ext);

    // 使用查看器管理器
    if (!viewerManager.container || !viewerManager.codemirrorViewer.view || !viewerManager.container.contains(viewerManager.codemirrorViewer.view.dom)) {
        viewerManager.initialize('fileViewer');
    }
    
    viewerManager.setGlobalCaches(globalNamePreviewCache, globalNamePreviewPromises);
    viewerManager.setPathClickCallback(async (path) => {
        const actualPath = await fileFormatter.findFileIgnoreCase(path);
        if (actualPath) {
            loadFileContent(actualPath);
        } else {
            updateStatus('文件未找到: ' + path);
        }
    });

    viewerManager.loadFile(key, content, lines, languageClass, showWhitespaceMode, {
        namePreviewCache: globalNamePreviewCache,
        namePreviewPromises: globalNamePreviewPromises,
        onPathClick: async (path) => {
            const actualPath = await fileFormatter.findFileIgnoreCase(path);
            if (actualPath) {
                loadFileContent(actualPath);
            } else {
                updateStatus('文件未找到: ' + path);
            }
        }
    });

    updateStatus(`${key.split('/').pop()} (${lineCount} 行)`);
}

// 切换侧边栏
function toggleSidebar(show) {
    if (show) {
        elements.sidebar.classList.remove('collapsed');
        elements.sidebarOverlay.classList.add('show');
    } else {
        elements.sidebar.classList.add('collapsed');
        elements.sidebarOverlay.classList.remove('show');
    }
}

// 加载文件内容
// 启动搜索（调用此函数开始搜索）
async function startSearch(searchParams) {
    currentSearchParams = searchParams;
    searchResultsOffset = 0;
    searchResultsTotal = 0;
    searchResultsHasMore = false;
    updateStatus('正在搜索...');
    await renderSearchResults(false);
}

// 渲染搜索结果（支持后端分页）
async function renderSearchResults(append = false) {
    const searchResultsContainer = document.getElementById('searchResults');
    if (!searchResultsContainer) {
        return;
    }

    if (!append) {
        // 首次搜索
        if (!currentSearchParams) {
            return;
        }

        searchResultsContainer.innerHTML = '<div style="padding: 20px; text-align: center; color: #858585;">搜索中...</div>';
    }

    try {
        searchResultsLoading = true;
        const response = await API.advancedSearch({
            ...currentSearchParams,
            offset: searchResultsOffset,
            limit: SEARCH_RESULTS_PAGE_SIZE
        });

        searchResultsLoading = false;

        if (response.error) {
            searchResultsContainer.innerHTML = `<div style="padding: 20px; text-align: center; color: #ff6b6b;">搜索失败: ${response.error}</div>`;
            return;
        }

        searchResultsTotal = response.total || 0;
        searchResultsHasMore = response.hasMore || false;
        
        // 更新状态栏显示搜索结果数量
        if (!append) {
            updateStatus(`找到 ${searchResultsTotal} 个匹配结果`);
        }

        const results = response.results || [];

        if (!append) {
            // 首次渲染
            if (results.length === 0) {
                searchResultsContainer.innerHTML = '<div style="padding: 20px; text-align: center; color: #858585;">未找到匹配的文件</div>';
                return;
            }

            let html = '<div class="file-tree-content search-results-list">';
            results.forEach((item, index) => {
                const fileName = item.name || item.key.split('/').pop();
                const globalIndex = searchResultsOffset + index + 1;
                html += `
                    <div class="search-result-item" data-key="${item.key}">
                        <span class="result-index">${globalIndex}</span>
                        <span class="result-filename" title="${escapeHtml(fileName)}">${escapeHtml(fileName)}</span>
                        <span class="result-path" title="${escapeHtml(item.key)}">${escapeHtml(item.key)}</span>
                    </div>
                `;
            });

            if (searchResultsHasMore) {
                html += '<div class="search-results-loading" id="searchResultsLoading" style="padding: 20px; text-align: center; color: #858585;">向下滚动加载更多...</div>';
            }

            html += '</div>';
            searchResultsContainer.innerHTML = html;
        } else {
            // 追加渲染
            const listContainer = searchResultsContainer.querySelector('.search-results-list');
            const loadingElement = document.getElementById('searchResultsLoading');

            if (loadingElement) {
                loadingElement.remove();
            }

            results.forEach((item, index) => {
                const fileName = item.name || item.key.split('/').pop();
                const globalIndex = searchResultsOffset + index + 1;
                const div = document.createElement('div');
                div.className = 'search-result-item';
                div.setAttribute('data-key', item.key);
                div.innerHTML = `
                    <span class="result-index">${globalIndex}</span>
                    <span class="result-filename" title="${escapeHtml(fileName)}">${escapeHtml(fileName)}</span>
                    <span class="result-path" title="${escapeHtml(item.key)}">${escapeHtml(item.key)}</span>
                `;

                div.addEventListener('click', () => {
                    const key = div.getAttribute('data-key');
                    loadFileContent(key);
                });

                div.addEventListener('contextmenu', (e) => {
                    e.preventDefault();
                    const key = div.getAttribute('data-key');
                    toggleFileSelection(key, div);
                });

                listContainer.appendChild(div);
            });

            // 如果还有更多结果，继续显示加载提示
            if (searchResultsHasMore) {
                const loadingDiv = document.createElement('div');
                loadingDiv.className = 'search-results-loading';
                loadingDiv.id = 'searchResultsLoading';
                loadingDiv.style.cssText = 'padding: 20px; text-align: center; color: #858585;';
                loadingDiv.textContent = '向下滚动加载更多...';
                listContainer.appendChild(loadingDiv);
            }
        }

        searchResultsOffset += SEARCH_RESULTS_PAGE_SIZE;

        // 绑定滚动事件（只绑定一次）
        if (!searchResultsContainer.dataset.hasScrollListener && searchResultsHasMore) {
            searchResultsContainer.dataset.hasScrollListener = 'true';
            searchResultsContainer.addEventListener('scroll', () => {
                // 检查是否滚动到底部附近（距离底部50px内）
                const scrollTop = searchResultsContainer.scrollTop;
                const scrollHeight = searchResultsContainer.scrollHeight;
                const clientHeight = searchResultsContainer.clientHeight;

                if (scrollTop + clientHeight >= scrollHeight - 50 &&
                    searchResultsHasMore &&
                    !searchResultsLoading) {

                    renderSearchResults(true);
                }
            });
        }
    } catch (error) {
        console.error('搜索失败:', error);
        searchResultsLoading = false;
        if (!append) {
            searchResultsContainer.innerHTML = '<div style="padding: 20px; text-align: center; color: #ff6b6b;">搜索失败</div>';
        }
    }
}

async function loadFileContent(key) {
    currentFile = key;
    updateBreadcrumb(key);
    
    // 更新文件格式化器的当前文件
    fileFormatter.setCurrentFile(key);
    
    // 创建标签页
    createTab(key);
    
    showLoading(elements.loading, true);
    updateStatus('正在加载文件...');

    try {
        // 检查缓存
        const cached = fileContentCache.get(key);
        let content, lines;
        
        if (cached) {
            content = cached.content;
            lines = cached.lines;
        } else {
            const data = await API.getFile(key);
            content = data.content;
            
            // 移除 BOM (Byte Order Mark) - UTF-8 BOM 是 \uFEFF
            if (content.charCodeAt(0) === 0xFEFF) {
                content = content.slice(1);
            }
            
            lines = content.split('\n');
            
            // 缓存文件内容
            fileContentCache.set(key, {
                content,
                lines,
                timestamp: Date.now()
            });
            
            // 清理旧缓存（保留最近10个文件）
            if (fileContentCache.size > 10) {
                const entries = Array.from(fileContentCache.entries());
                entries.sort((a, b) => a[1].timestamp - b[1].timestamp);
                const toDelete = entries.slice(0, entries.length - 10);
                toDelete.forEach(([cacheKey]) => fileContentCache.delete(cacheKey));
            }
        }

        const ext = key.split('.').pop().toLowerCase();
        const languageClass = getLanguageClass(ext);
        const lineCount = lines.length;

        // 更新按钮状态
        elements.extractBtn.disabled = false;
        const copyPathBtn = document.getElementById('copyPathBtn');
        const locateDirBtn = document.getElementById('locateDirBtn');
        if (copyPathBtn) copyPathBtn.disabled = false;
        if (locateDirBtn) locateDirBtn.disabled = false;

        const editableExtensions = ['txt', 'nut', 'str', 'lst', 'equ', 'stk', 'ai', 'aic', 'key', 'als', 'act', 'stm', 'ora', 'map', 'obj', 'dgn'];
        elements.editBtn.disabled = !editableExtensions.includes(ext);

        // 确保 CodeMirror 已初始化
        if (!viewerManager.container || !viewerManager.codemirrorViewer.view || !viewerManager.container.contains(viewerManager.codemirrorViewer.view.dom)) {
            viewerManager.initialize('fileViewer');
        }

        // 使用 ViewerManager 加载文件（支持 CodeMirror 6）
        viewerManager.loadFile(key, content, lines, languageClass, showWhitespaceMode, {
            namePreviewCache: globalNamePreviewCache,
            namePreviewPromises: globalNamePreviewPromises,
            onPathClick: async (path) => {
                const actualPath = await fileFormatter.findFileIgnoreCase(path);
                if (actualPath) {
                    loadFileContent(actualPath);
                } else {
                    updateStatus('文件未找到: ' + path);
                }
            }
        });

        updateStatus(`${key.split('/').pop()} (${lineCount} 行)`);
    } catch (error) {
        console.error('加载文件内容失败:', error);
        elements.fileViewer.innerHTML = '<div class="empty">加载失败</div>';
    } finally {
        showLoading(elements.loading, false);
    }
}

// 初始化书签管理器
function initBookmarkManager() {
    bookmarkManager.init('bookmarkMenu', 1, {
        onBookmarkClick: (key) => {
            loadFileContent(key);
            // 在移动端关闭侧边栏
            if (window.innerWidth <= 768) {
                toggleSidebar(false);
            }
        },
        onEditBookmark: (key) => {
            const bookmark = bookmarkManager.getBookmarks().find(b => b.key === key);
            if (bookmark) {
                document.getElementById('editBookmarkPath').textContent = bookmark.key;
                document.getElementById('editBookmarkName').value = bookmark.name;
                document.getElementById('editBookmarkName').dataset.key = key;
                modalManager.show('editBookmarkModal');
            }
        },
        onRemoveBookmark: (key) => {
            if (confirm('确定要删除这个书签吗？')) {
                bookmarkManager.remove(key);
                updateStatus('已删除书签');
            }
        }
    });
}

// 初始化文件菜单管理器
function initFileManager() {
    fileMenuManager.init('pvfFileMenu', 'fileMenu', {
        onFileSelect: async (filePath) => {
            if (!confirm('确定要打开新的 PVF 文件吗？这将关闭当前文件。')) {
                return;
            }

            showLoading(elements.loading, true);
            try {
                const data = await API.openPvf(filePath);
                if (data.success) {
                    currentFile = null;
                    await loadFiles(''); // 重新加载根目录
                    bookmarkManager.load();
                    updateStatus('已打开 PVF 文件');
                } else {
                    alert('打开文件失败: ' + data.error);
                }
            } catch (error) {
                console.error('打开文件失败:', error);
                alert('打开文件失败: ' + error);
            } finally {
                showLoading(elements.loading, false);
            }
        }
    });
}

// 加载文件列表
async function loadFiles(path = '') {
    // 树形结构模式下，只加载根目录
    if (path !== '') {
        console.warn('树形结构模式下不支持直接加载子目录，请从根目录展开');
        return;
    }
    
    currentPath = '';
    currentFile = null;
    updateBreadcrumb('');

    try {
        const data = await API.getFiles('');
        if (data.error) {
            console.error('加载文件列表失败:', data.error);
            elements.fileTree.innerHTML = `<div style="padding: 20px; text-align: center; color: #ff6b6b;">加载失败: ${data.error}</div>`;
            return;
        }

        renderFileTree(data.files, '');
    } catch (error) {
        console.error('加载文件列表失败:', error);
        elements.fileTree.innerHTML = '<div style="padding: 20px; text-align: center; color: #ff6b6b;">加载失败: 无法连接到服务器</div>';
    }
}

// 渲染文件树
function renderFileTree(files, currentPath) {
    if (files.length === 0) {
        elements.fileTree.innerHTML = '<div style="padding: 20px; text-align: center; color: #858585;">文件夹为空</div>';
        return;
    }

    const folders = files.filter(f => !f.isFile);
    const fileItems = files.filter(f => f.isFile);

    let html = '';

    folders.forEach(folder => {
        const checkbox = batchMode ? `<div class="checkbox folder-checkbox"><input type="checkbox" data-key="${folder.key}" data-is-folder="true"></div>` : '';
        html += `
            <div class="file-item folder-item" data-key="${folder.key}" data-is-file="false">
                ${checkbox}
                <span class="toggle-icon">▶</span>
                <span class="icon">📁</span>
                <span class="name">${folder.name}</span>
            </div>
            <div class="folder-children" data-parent="${folder.key}" style="display: none; padding-left: 20px;"></div>
        `;
    });

    fileItems.forEach(file => {
        const checkbox = batchMode ? `<div class="checkbox"><input type="checkbox" data-key="${file.key}"></div>` : '';
        html += `
            <div class="file-item" data-key="${file.key}" data-is-file="true">
                ${checkbox}
                <span class="toggle-icon" style="visibility: hidden;"></span>
                <span class="icon">📄</span>
                <span class="name">${file.name}</span>
                ${file.size ? `<span class="size">${formatSize(file.size)}</span>` : ''}
            </div>
        `;
    });

    elements.fileTree.innerHTML = html;

    // 应用选中状态：如果有被选中的文件夹，恢复其选中状态
    if (selectedFolders.size > 0) {
        elements.fileTree.querySelectorAll('.file-item').forEach(item => {
            const key = item.dataset.key;
            if (selectedFolders.has(key)) {
                const checkbox = item.querySelector('input[type="checkbox"]');
                if (checkbox) {
                    checkbox.checked = true;
                }
            }
        });
    }

    // 添加点击事件
    elements.fileTree.querySelectorAll('.file-item').forEach(item => {
        item.addEventListener('click', async (e) => {
            const key = item.dataset.key;
            const isFile = item.dataset.isFile === 'true';

            // 处理复选框点击
            if (e.target.type === 'checkbox') {
                e.stopPropagation();

                // 如果选中的是文件夹，选中/取消选中所有子文件
                if (e.target.dataset.isFolder === 'true') {
                    const isChecked = e.target.checked;
                    selectAllFilesInFolder(key, isChecked);
                } else {
                    // 如果选中的是文件，更新选中数量
                    updateSelectedCount();
                }
                return;
            }

            if (isFile) {
                loadFileContent(key);
            } else {
                // 展开文件夹
                const childrenContainer = elements.fileTree.querySelector(`.folder-children[data-parent="${key}"]`);
                const toggleIcon = item.querySelector('.toggle-icon');
                
                if (childrenContainer.style.display === 'none') {
                    toggleIcon.textContent = '▼';
                    childrenContainer.style.display = 'block';
                    
                    if (childrenContainer.innerHTML.trim() === '') {
                        const response = await API.getFiles(key);
                        if (response.files && response.files.length > 0) {
                            renderSubTree(response.files, key, childrenContainer);
                        } else {
                            childrenContainer.innerHTML = '<div style="padding: 10px; color: #858585;">(空文件夹)</div>';
                        }
                    }
                } else {
                    toggleIcon.textContent = '';
                    childrenContainer.style.display = 'none';
                }
            }
        });
    });
}

// 渲染子树
function renderSubTree(files, parentPath, container) {
    const folders = files.filter(f => !f.isFile);
    const fileItems = files.filter(f => f.isFile);

    let html = '';
    
    folders.forEach(folder => {
        const checkbox = batchMode ? `<div class="checkbox folder-checkbox"><input type="checkbox" data-key="${folder.key}" data-is-folder="true"></div>` : '';
        html += `
            <div class="file-item folder-item" data-key="${folder.key}" data-is-file="false">
                ${checkbox}
                <span class="toggle-icon">▶</span>
                <span class="icon">📁</span>
                <span class="name">${folder.name}</span>
            </div>
            <div class="folder-children" data-parent="${folder.key}" style="display: none; padding-left: 20px;"></div>
        `;
    });

    fileItems.forEach(file => {
        const checkbox = batchMode ? `<div class="checkbox"><input type="checkbox" data-key="${file.key}"></div>` : '';
        html += `
            <div class="file-item" data-key="${file.key}" data-is-file="true">
                ${checkbox}
                <span class="toggle-icon" style="visibility: hidden;"></span>
                <span class="icon">📄</span>
                <span class="name">${file.name}</span>
                ${file.size ? `<span class="size">${formatSize(file.size)}</span>` : ''}
            </div>
        `;
    });

    container.innerHTML = html;

    // 应用选中状态：如果父文件夹被选中，则自动勾选所有子文件夹和文件
    if (selectedFolders.size > 0) {
        container.querySelectorAll('.file-item').forEach(item => {
            const key = item.dataset.key;
            // 检查是否有被选中的父文件夹
            const parentFolder = Array.from(selectedFolders).find(folder =>
                key === folder || key.startsWith(folder + '/')
            );
            if (parentFolder) {
                const checkbox = item.querySelector('input[type="checkbox"]');
                if (checkbox) {
                    checkbox.checked = true;
                }
            }
        });
    }

    container.querySelectorAll('.file-item').forEach(item => {
        item.addEventListener('click', async (e) => {
            const key = item.dataset.key;
            const isFile = item.dataset.isFile === 'true';

            // 处理复选框点击
            if (e.target.type === 'checkbox') {
                e.stopPropagation();

                // 如果选中的是文件夹，选中/取消选中所有子文件
                if (e.target.dataset.isFolder === 'true') {
                    const isChecked = e.target.checked;
                    selectAllFilesInFolder(key, isChecked);
                } else {
                    // 如果选中的是文件，更新选中数量
                    updateSelectedCount();
                }
                return;
            }

            if (isFile) {
                loadFileContent(key);
            } else {
                const childrenContainer = container.querySelector(`.folder-children[data-parent="${key}"]`);
                const toggleIcon = item.querySelector('.toggle-icon');
                
                if (childrenContainer.style.display === 'none') {
                    toggleIcon.textContent = '▼';
                    childrenContainer.style.display = 'block';
                    
                    if (childrenContainer.innerHTML.trim() === '') {
                        const response = await API.getFiles(key);
                        if (response.files && response.files.length > 0) {
                            renderSubTree(response.files, key, childrenContainer);
                        } else {
                            childrenContainer.innerHTML = '<div style="padding: 10px; color: #858585;">(空文件夹)</div>';
                        }
                    }
                } else {
                    toggleIcon.textContent = '▶';
                    childrenContainer.style.display = 'none';
                }
            }
        });
    });
}

// 选中/取消选中文件夹中的所有文件（记录状态，延迟渲染时应用）
async function selectAllFilesInFolder(folderKey, select) {
    try {
        // 更新全局选中状态
        if (select) {
            selectedFolders.add(folderKey);
        } else {
            selectedFolders.delete(folderKey);
            // 取消选中父文件夹时，也要取消选中所有子文件夹
            const foldersToRemove = [];
            selectedFolders.forEach(selectedFolder => {
                if (selectedFolder.startsWith(folderKey + '/') || selectedFolder === folderKey) {
                    foldersToRemove.push(selectedFolder);
                }
            });
            foldersToRemove.forEach(f => selectedFolders.delete(f));
        }

        // 调用后端API获取文件夹中的所有文件（递归），用于统计文件数量
        const response = await API.getFilesInFolder(folderKey);
        if (response.error) {
            console.error('获取文件夹文件失败:', response.error);
            return;
        }

        const files = response.files || [];

        // 处理已经渲染在DOM中的文件复选框
        files.forEach(fileKey => {
            const checkbox = document.querySelector(`input[type="checkbox"][data-key="${fileKey}"]`);
            if (checkbox) {
                checkbox.checked = select;
            }
        });

        // 处理已经渲染在DOM中的子文件夹复选框
        const allFolderCheckboxes = document.querySelectorAll('input[type="checkbox"][data-is-folder="true"]');
        allFolderCheckboxes.forEach(checkbox => {
            const folderKey2 = checkbox.dataset.key;
            // 如果是当前文件夹的子文件夹（包括自身）
            if (folderKey2 === folderKey || folderKey2.startsWith(folderKey + '/')) {
                checkbox.checked = select;
            }
        });

        // 更新文件数量统计（基于所有选中文件夹）
        await updateTotalSelectedCount();
    } catch (error) {
        console.error('选择文件夹文件失败:', error);
    }
}

// 更新总选中文件数量（从后端获取）
async function updateTotalSelectedCount() {
    if (selectedFolders.size === 0) {
        updateSelectedCount();
        return;
    }

    try {
        // 获取所有选中文件夹的文件
        const allFileKeys = new Set();
        for (const folderKey of selectedFolders) {
            const response = await API.getFilesInFolder(folderKey);
            if (response.files) {
                response.files.forEach(fileKey => allFileKeys.add(fileKey));
            }
        }

        const countSpan = document.getElementById('selectedCount');
        if (countSpan) {
            countSpan.textContent = allFileKeys.size;
        }
    } catch (error) {
        console.error('更新选中文件数量失败:', error);
        updateSelectedCount();
    }
}

// 初始化
function init() {
    console.log('Initializing PVF Web Browser...');

    // 检查关键 DOM 元素
    if (!elements.fileTree) {
        console.error('fileTree element not found!');
        return;
    }

    // 初始化管理器
    initBookmarkManager();
    initFileManager();

    console.log('Managers initialized');

    // 添加书签按钮
    if (elements.addBookmarkBtn) {
        elements.addBookmarkBtn.addEventListener('click', () => {
            if (!currentFile) {
                alert('请先选择一个文件');
                return;
            }
            try {
                const fileName = currentFile.split('/').pop();
                bookmarkManager.add(currentFile, fileName);
                updateStatus('已添加书签: ' + fileName);
            } catch (error) {
                alert(error.message);
            }
        });
    }

    // 复制路径按钮
    if (elements.copyPathBtn) {
        elements.copyPathBtn.addEventListener('click', () => {
            if (!currentFile) {
                alert('请先选择一个文件');
                return;
            }
            navigator.clipboard.writeText(currentFile).then(() => {
                updateStatus('已复制路径: ' + currentFile);
            }).catch(err => {
                console.error('复制失败:', err);
                alert('复制失败: ' + err.message);
            });
        });
    }

    // 定位目录按钮
    if (elements.locateDirBtn) {
        elements.locateDirBtn.addEventListener('click', async () => {
            if (!currentFile) {
                alert('请先选择一个文件');
                return;
            }
            
            // 大小写不敏感查找文件
            const currentFileLower = currentFile.toLowerCase();
            const fileTreeItems = document.querySelectorAll('.file-item');
            let fileTreeItem = null;
            
            for (const item of fileTreeItems) {
                if (item.dataset.key && item.dataset.key.toLowerCase() === currentFileLower) {
                    fileTreeItem = item;
                    break;
                }
            }
            
            if (fileTreeItem) {
                // 文件已在当前视图中
                fileTreeItem.scrollIntoView({ behavior: 'smooth', block: 'center' });
                fileTreeItem.style.background = 'rgba(78, 201, 176, 0.3)';
                setTimeout(() => {
                    fileTreeItem.style.background = '';
                }, 2000);
                updateStatus('已定位到文件: ' + currentFile.split('/').pop());
            } else {
                // 文件不在当前视图中，逐个展开文件夹
                const dirPath = currentFile.substring(0, currentFile.lastIndexOf('/'));
                
                async function expandAndLocate(path) {
                    if (!path) return;
                    
                    const parts = path.split('/');
                    let currentPath = '';
                    
                    for (const part of parts) {
                        currentPath += (currentPath ? '/' : '') + part;
                        
                        // 大小写不敏感查找文件夹
                        const currentPathLower = currentPath.toLowerCase();
                        const folderItems = document.querySelectorAll('.file-item.folder-item');
                        let folderItem = null;
                        
                        for (const item of folderItems) {
                            if (item.dataset.key && item.dataset.key.toLowerCase() === currentPathLower) {
                                folderItem = item;
                                break;
                            }
                        }
                        
                        if (folderItem) {
                            const toggleIcon = folderItem.querySelector('.toggle-icon');
                            const childrenContainer = document.querySelector('.folder-children[data-parent="' + folderItem.dataset.key + '"]');
                            
                            if (toggleIcon && childrenContainer) {
                                // 展开文件夹
                                toggleIcon.textContent = '▼';
                                childrenContainer.style.display = 'block';
                                
                                // 如果子容器为空，加载子目录
                                if (childrenContainer.innerHTML.trim() === '') {
                                    try {
                                        const response = await API.getFiles(folderItem.dataset.key);
                                        if (response.files && response.files.length > 0) {
                                            renderSubTree(response.files, folderItem.dataset.key, childrenContainer);
                                        }
                                    } catch (error) {
                                        console.error('加载子目录失败:', error);
                                    }
                                }
                            }
                        }
                    }
                    
                    // 所有文件夹展开后，查找文件
                    setTimeout(() => {
                        const allItems = document.querySelectorAll('.file-item');
                        let newItem = null;
                        
                        for (const item of allItems) {
                            if (item.dataset.key && item.dataset.key.toLowerCase() === currentFileLower) {
                                newItem = item;
                                break;
                            }
                        }
                        
                        if (newItem) {
                            newItem.scrollIntoView({ behavior: 'smooth', block: 'center' });
                            newItem.style.background = 'rgba(78, 201, 176, 0.3)';
                            setTimeout(() => {
                                newItem.style.background = '';
                            }, 2000);
                            updateStatus('已定位到文件: ' + currentFile.split('/').pop());
                        } else {
                            updateStatus('未找到文件: ' + currentFile);
                        }
                    }, 300);
                }
                
                if (dirPath) {
                    expandAndLocate(dirPath);
                } else {
                    // 文件在根目录，再次查找
                    const allItems = document.querySelectorAll('.file-item');
                    for (const item of allItems) {
                        if (item.dataset.key && item.dataset.key.toLowerCase() === currentFileLower) {
                            item.scrollIntoView({ behavior: 'smooth', block: 'center' });
                            item.style.background = 'rgba(78, 201, 176, 0.3)';
                            setTimeout(() => {
                                item.style.background = '';
                            }, 2000);
                            updateStatus('已定位到文件: ' + currentFile.split('/').pop());
                            break;
                        }
                    }
                }
            }
        });
    }

    // 侧边栏切换
    if (elements.toggleSidebarBtn) {
        elements.toggleSidebarBtn.addEventListener('click', () => toggleSidebar(true));
    }
    if (elements.sidebarOverlay) {
        elements.sidebarOverlay.addEventListener('click', () => toggleSidebar(false));
    }

    // 菜单事件
    const menuItems = document.querySelectorAll('.menu-item');
    
    // 书签菜单
    if (menuItems.length > 1) {
        menuItems[1].addEventListener('mouseenter', () => {
            document.querySelectorAll('.menu-dropdown').forEach(dropdown => {
                if (dropdown.id !== 'bookmarkMenu') {
                    dropdown.classList.remove('show');
                }
            });
            bookmarkManager.render();
        });

        menuItems[1].addEventListener('click', (e) => {
            e.stopPropagation();
            document.querySelectorAll('.menu-dropdown').forEach(dropdown => {
                if (dropdown.id !== 'bookmarkMenu') {
                    dropdown.classList.remove('show');
                }
            });
            bookmarkManager.render();
            elements.bookmarkMenu.classList.toggle('show');
        });
    }

    // 文件菜单
    if (elements.fileMenu) {
        elements.fileMenu.addEventListener('mouseenter', () => {
            document.querySelectorAll('.menu-dropdown').forEach(dropdown => {
                if (dropdown.id !== 'pvfFileMenu') {
                    dropdown.classList.remove('show');
                }
            });
            fileMenuManager.load();
        });

        elements.fileMenu.addEventListener('click', (e) => {
            if (e.target.closest('.menu-dropdown-item')) {
                return;
            }
            e.stopPropagation();
            e.preventDefault();
            document.querySelectorAll('.menu-dropdown').forEach(dropdown => {
                if (dropdown.id !== 'pvfFileMenu') {
                    dropdown.classList.remove('show');
                }
            });
            elements.pvfFileMenu.classList.toggle('show');
            fileMenuManager.load();
        });
    }

    // 点击其他地方关闭菜单
    document.addEventListener('click', () => {
        document.querySelectorAll('.menu-dropdown').forEach(dropdown => {
            dropdown.classList.remove('show');
        });
    });

    // 高级搜索按钮
    if (elements.advancedSearchBtn) {
        elements.advancedSearchBtn.addEventListener('click', () => {
            modalManager.show('searchModal');
        });
    }

    // 提取按钮
    if (elements.extractBtn) {
        elements.extractBtn.addEventListener('click', () => {
            if (!currentFile) {
                alert('请先选择一个文件');
                return;
            }
            document.getElementById('extractFileName').textContent = currentFile;
            modalManager.show('extractModal');
        });
    }

    // 编辑按钮
    if (elements.editBtn) {
        elements.editBtn.addEventListener('click', async () => {
            if (!currentFile) {
                alert('请先选择一个文件');
                return;
            }

            const currentEditable = viewerManager.isEditable();

            if (currentEditable) {
                // 当前是编辑模式，切换回预览模式，保存内容
                try {
                    const newContent = viewerManager.getContent();
                    const response = await API.saveFile(currentFile, newContent, 'utf8');
                    if (response.success || response.result === 'success') {
                        updateStatus('文件保存成功: ' + currentFile);
                        // 更新缓存
                        const lines = newContent.split('\n');
                        fileContentCache.set(currentFile, {
                            content: newContent,
                            lines: lines,
                            encoding: 'utf8',
                            timestamp: Date.now()
                        });
                        // 切换到只读模式
                        viewerManager.setEditable(false);
                        elements.editBtn.textContent = '编辑';
                    } else {
                        alert('保存文件失败: ' + (response.message || response.error || '未知错误'));
                    }
                } catch (error) {
                    alert('保存文件失败: ' + error.message);
                }
            } else {
                // 当前是预览模式，切换到编辑模式
                viewerManager.setEditable(true);
                elements.editBtn.textContent = '预览';
                updateStatus('进入编辑模式');
            }
        });
    }

    // 查看器切换按钮
    if (elements.switchViewerBtn) {
        elements.switchViewerBtn.addEventListener('click', () => {
            const currentType = viewerManager.getCurrentViewerType();
            const newType = currentType === 'codemirror' ? 'virtual' : 'codemirror';

            // 切换查看器
            viewerManager.switchViewer(newType);

            // 更新按钮文本
            const viewerName = newType === 'codemirror' ? 'CM' : 'VS';
            elements.switchViewerBtn.textContent = viewerName;

            updateStatus(`已切换到 ${newType === 'codemirror' ? 'CodeMirror' : '虚拟滚动'} 查看器`);
        });
    }

    // 配色设置按钮
    if (elements.colorSettingsBtn) {
        elements.colorSettingsBtn.addEventListener('click', async () => {
            console.log('Color settings button clicked');
            // 动态导入配色模块
            try {
                const module = await import('./pvf-language.js');
                console.log('pvf-language module loaded:', module);
                const currentColors = await module.getCurrentColors();
                console.log('Current colors:', currentColors);

                // 填充颜色输入框
                document.getElementById('color-labelName').value = currentColors.labelName;
                document.getElementById('color-string').value = currentColors.string;
                document.getElementById('color-url').value = currentColors.url;
                document.getElementById('color-number').value = currentColors.number;
                document.getElementById('color-comment').value = currentColors.comment;
                document.getElementById('color-variableName').value = currentColors.variableName;
                document.getElementById('color-operator').value = currentColors.operator;
                document.getElementById('color-punctuation').value = currentColors.punctuation;
                document.getElementById('color-constant').value = currentColors.constant;
                document.getElementById('color-link').value = currentColors.link;
                document.getElementById('color-text').value = currentColors.text;

                console.log('Showing color settings modal');
                modalManager.show('colorSettingsModal');
            } catch (error) {
                console.error('Failed to load pvf-language module:', error);
            }
        });
    }

    // 恢复默认配色
    document.getElementById('resetColorsBtn')?.addEventListener('click', () => {
        import('./pvf-language.js').then((module) => {
            const defaultColors = module.resetColors();

            // 填充默认颜色
            document.getElementById('color-labelName').value = defaultColors.labelName;
            document.getElementById('color-string').value = defaultColors.string;
            document.getElementById('color-url').value = defaultColors.url;
            document.getElementById('color-number').value = defaultColors.number;
            document.getElementById('color-comment').value = defaultColors.comment;
            document.getElementById('color-variableName').value = defaultColors.variableName;
            document.getElementById('color-operator').value = defaultColors.operator;
            document.getElementById('color-punctuation').value = defaultColors.punctuation;
            document.getElementById('color-constant').value = defaultColors.constant;
            document.getElementById('color-link').value = defaultColors.link;
            document.getElementById('color-text').value = defaultColors.text;

            updateStatus('已恢复默认配色');
        });
    });

    // 保存配色设置
    document.getElementById('saveColorSettingsBtn')?.addEventListener('click', async () => {
        try {
            const module = await import('./pvf-language.js');
            const newColors = {
                labelName: document.getElementById('color-labelName').value,
                string: document.getElementById('color-string').value,
                url: document.getElementById('color-url').value,
                number: document.getElementById('color-number').value,
                comment: document.getElementById('color-comment').value,
                variableName: document.getElementById('color-variableName').value,
                operator: document.getElementById('color-operator').value,
                punctuation: document.getElementById('color-punctuation').value,
                constant: document.getElementById('color-constant').value,
                link: document.getElementById('color-link').value,
                text: document.getElementById('color-text').value
            };

            // 保存配色到服务器配置文件
            await module.saveCustomColors(newColors);

            // 重新加载 CodeMirror 的高亮样式
            if (viewerManager.currentViewerType === 'codemirror') {
                await viewerManager.codemirrorViewer.reloadHighlightStyle();
            }

            updateStatus('配色已应用');
            modalManager.hide('colorSettingsModal');
        } catch (error) {
            console.error('保存配色失败:', error);
            updateStatus('保存配色失败: ' + error.message);
        }
    });

    // 取消配色设置
    document.getElementById('cancelColorSettingsBtn')?.addEventListener('click', () => {
        modalManager.hide('colorSettingsModal');
    });

    // 批量模式按钮
    if (elements.batchModeBtn) {
        elements.batchModeBtn.addEventListener('click', () => {
            batchMode = !batchMode;
            elements.batchModeBtn.textContent = batchMode ? '退出批量' : '批量';
            elements.batchModeBtn.style.background = batchMode ? '#4ec9b0' : '';
            elements.batchModeBtn.style.color = batchMode ? '#1e1e1e' : '';
            
            // 显示/隐藏批量操作区域
            const batchActions = document.getElementById('batchActions');
            if (batchActions) {
                batchActions.style.display = batchMode ? 'flex' : 'none';
            }
            
            updateStatus(batchMode ? '批量模式已启用' : '批量模式已关闭');
            loadFiles(''); // 重新加载文件树以显示/隐藏复选框
        });
    }

    // 批量提取按钮（侧边栏）
    document.getElementById('batchExtractBtn')?.addEventListener('click', async () => {
        try {
            let fileKeys = [];

            // 1. 收集DOM中被直接勾选的文件
            const selectedCheckboxes = document.querySelectorAll('.file-item input[type="checkbox"]:checked:not([data-is-folder="true"])');
            selectedCheckboxes.forEach(checkbox => {
                const key = checkbox.dataset.key;
                if (key) {
                    fileKeys.push(key);
                }
            });

            // 2. 如果有选中的文件夹，获取其中的所有文件
            if (selectedFolders.size > 0) {
                for (const folderKey of selectedFolders) {
                    const response = await API.getFilesInFolder(folderKey);
                    if (response.files && response.files.length > 0) {
                        fileKeys.push(...response.files);
                    }
                }
            }

            // 去重
            fileKeys = [...new Set(fileKeys)];

            if (fileKeys.length === 0) {
                alert('请先选择要提取的文件');
                return;
            }

            // 判断是否需要打包成zip
            if (fileKeys.length > 10) {
                // 调用zip打包API
                const response = await API.batchExtractZip(fileKeys, '');
                if (response.error) {
                    alert('批量提取失败: ' + response.error);
                } else {
                    updateStatus(`已打包并下载 ${fileKeys.length} 个文件`);
                }
            } else {
                // 单个文件下载
                const response = await API.batchDownload(fileKeys, '');
                if (response.error) {
                    alert('批量提取失败: ' + response.error);
                } else {
                    updateStatus('已提取 ' + fileKeys.length + ' 个文件');
                }
            }
        } catch (error) {
            alert('批量提取失败: ' + error.message);
        }
    });

    // 高级搜索模态框按钮
    document.getElementById('cancelSearchBtn')?.addEventListener('click', () => {
        modalManager.hide('searchModal');
    });

    document.getElementById('confirmSearchBtn')?.addEventListener('click', async (e) => {
        const type = document.getElementById('searchTypeSelect')?.value;
        const keyword = document.getElementById('searchKeywordInput')?.value;
        const startMatch = document.getElementById('searchStartMatch')?.checked;
        const useRegex = document.getElementById('searchUseRegex')?.checked;
        const caseSensitive = document.getElementById('searchCaseSensitive')?.checked;
        
        if (!keyword) {
            alert('请输入搜索内容');
            return;
        }

        modalManager.hide('searchModal');
        
        // 使用新的分页搜索
        await startSearch({
            type,
            keyword,
            startMatch: startMatch.toString(),
            useRegex: useRegex.toString(),
            caseSensitive: caseSensitive.toString()
        });
        
        // 切换到搜索结果标签页
        const searchTab = document.querySelector('.sidebar-tab[data-tab="search"]');
        if (searchTab) {
            searchTab.click();
        }
    });

    // 提取模态框按钮
    document.getElementById('cancelExtractBtn')?.addEventListener('click', () => {
        modalManager.hide('extractModal');
    });

    document.getElementById('confirmExtractBtn')?.addEventListener('click', async () => {
        if (!currentFile) return;
        
        try {
            const response = await API.download(currentFile);
            if (response.error) {
                alert('下载失败: ' + response.error);
            } else {
                updateStatus('已下载: ' + currentFile.split('/').pop());
                modalManager.hide('extractModal');
            }
        } catch (error) {
            alert('下载失败: ' + error.message);
        }
    });

    // 保存模态框按钮
    document.getElementById('cancelSaveBtn')?.addEventListener('click', () => {
        modalManager.hide('saveModal');
    });

    document.getElementById('confirmSaveBtn')?.addEventListener('click', async () => {
        const path = document.getElementById('savePathInput').value;
        if (!path) {
            alert('请输入保存路径');
            return;
        }

        try {
            const response = await API.savePvf(path);
            if (response.error) {
                alert('保存失败: ' + response.error);
            } else {
                updateStatus('已保存到: ' + path);
                modalManager.hide('saveModal');
            }
        } catch (error) {
            alert('保存失败: ' + error.message);
        }
    });

    // 编辑模态框按钮
    document.getElementById('cancelEditBtn')?.addEventListener('click', () => {
        modalManager.hide('editModal');
    });

    document.getElementById('confirmEditBtn')?.addEventListener('click', async () => {
        if (!currentFile) return;
        
        const content = document.getElementById('editFileContent').value;
        
        try {
            const response = await API.updateFile(currentFile, content);
            if (response.error) {
                alert('保存失败: ' + response.error);
            } else {
                updateStatus('已保存: ' + currentFile.split('/').pop());
                modalManager.hide('editModal');
                // 刷新缓存并重新加载文件
                fileContentCache.delete(currentFile);
                await loadFileContent(currentFile);
            }
        } catch (error) {
            alert('保存失败: ' + error.message);
        }
    });

    // 批量提取模态框按钮
    document.getElementById('cancelBatchExtractBtn')?.addEventListener('click', () => {
        modalManager.hide('batchExtractModal');
    });

    document.getElementById('confirmBatchExtractBtn')?.addEventListener('click', async () => {
        const checkboxes = document.querySelectorAll('.file-item input[type="checkbox"]:checked');
        const destPath = document.getElementById('batchDestPathInput').value || '';
        
        if (checkboxes.length === 0) {
            alert('请先选择要提取的文件');
            return;
        }

        const files = Array.from(checkboxes).map(cb => cb.dataset.key);
        
        try {
            const response = await API.batchDownload(files, destPath);
            if (response.error) {
                alert('批量提取失败: ' + response.error);
            } else {
                updateStatus('已提取 ' + files.length + ' 个文件');
                modalManager.hide('batchExtractModal');
            }
        } catch (error) {
            alert('批量提取失败: ' + error.message);
        }
    });

    // 编辑书签模态框按钮
    document.getElementById('cancelEditBookmarkBtn')?.addEventListener('click', () => {
        modalManager.hide('editBookmarkModal');
    });

    document.getElementById('confirmEditBookmarkBtn')?.addEventListener('click', () => {
        const key = document.getElementById('editBookmarkName').dataset.key;
        const name = document.getElementById('editBookmarkName').value;
        
        if (!name) {
            alert('请输入别名');
            return;
        }

        bookmarkManager.update(key, name);
        updateStatus('已更新书签别名');
        modalManager.hide('editBookmarkModal');
    });

    // 显示空白字符按钮
    if (elements.toggleWhitespaceBtn) {
        // 初始化按钮状态
        elements.toggleWhitespaceBtn.style.background = showWhitespaceMode ? '#4ec9b0' : '';
        elements.toggleWhitespaceBtn.style.color = showWhitespaceMode ? '#1e1e1e' : '';

        elements.toggleWhitespaceBtn.addEventListener('click', () => {
            showWhitespaceMode = !showWhitespaceMode;
            
            // 更新按钮样式
            if (showWhitespaceMode) {
                elements.toggleWhitespaceBtn.style.background = '#4ec9b0';
                elements.toggleWhitespaceBtn.style.color = '#1e1e1e';
            } else {
                elements.toggleWhitespaceBtn.style.background = '';
                elements.toggleWhitespaceBtn.style.color = '';
            }

            updateStatus(showWhitespaceMode ? '已显示空白字符' : '已隐藏空白字符');
            
            // 更新当前查看器
            viewerManager.setShowWhitespace(showWhitespaceMode);
        });
    }

    // 移动端默认折叠侧边栏
    if (window.innerWidth <= 768) {
        elements.sidebar.classList.add('collapsed');
    }

    // 标签描述按钮
    if (elements.toggleTagDescriptionsBtn) {
        elements.toggleTagDescriptionsBtn.style.background = showTagDescriptionsMode ? '#4ec9b0' : '';
        elements.toggleTagDescriptionsBtn.style.color = showTagDescriptionsMode ? '#1e1e1e' : '';

        elements.toggleTagDescriptionsBtn.addEventListener('click', () => {
            showTagDescriptionsMode = !showTagDescriptionsMode;
            window.showTagDescriptionsMode = showTagDescriptionsMode;

            // 更新按钮样式
            if (showTagDescriptionsMode) {
                elements.toggleTagDescriptionsBtn.style.background = '#4ec9b0';
                elements.toggleTagDescriptionsBtn.style.color = '#1e1e1e';
            } else {
                elements.toggleTagDescriptionsBtn.style.background = '';
                elements.toggleTagDescriptionsBtn.style.color = '';
            }

            updateStatus(showTagDescriptionsMode ? '已启用标签注释' : '已禁用标签注释');

            // 重新加载当前文件
            if (currentFile) {
                loadFileContent(currentFile);
            }
        });
    }

    // 加载数据
    console.log('Loading data...');

    // 强制清除 "选择一个文件查看内容" 并初始化查看器
    if (elements.fileViewer) {
        console.log('Clearing fileViewer initial state...');
        elements.fileViewer.innerHTML = '';
        viewerManager.initialize('fileViewer');
    }

    bookmarkManager.load();
    fileMenuManager.load();
    loadFiles('');
    console.log('Data loaded');

    // 暴露标签页函数到全局作用域（用于 HTML onclick）
    window.switchToTab = switchToTab;
    window.closeTab = closeTab;
}

// DOM 加载完成后初始化
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
} else {
    init();
}