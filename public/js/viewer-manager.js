// 文件查看器管理器 - 支持虚拟滚动和 CodeMirror 6 双查看器

import CodeMirrorViewer from './codemirror-viewer.js';
import { VirtualScrollManager } from './virtual-scroll.js';

export class ViewerManager {
    constructor() {
        this.codemirrorViewer = new CodeMirrorViewer();
        this.virtualScrollViewer = new VirtualScrollManager();
        this.currentViewer = this.codemirrorViewer;
        this.currentViewerType = 'codemirror'; // 'virtual' 或 'codemirror'
        this.onPathClick = null;
        this.globalNamePreviewCache = null;
        this.globalNamePreviewPromises = null;
        this.currentFile = null;
        this.currentContent = null;
        this.currentLines = null;
        this.currentLanguageClass = null;
        this.currentShowWhitespace = null;
    }

    initialize(containerId) {
        this.container = document.getElementById(containerId);
        if (!this.container) {
            console.error(`Container with id "${containerId}" not found`);
            return false;
        }

        // 注意：初始化时不创建查看器实例，只有在加载文件时才创建
        // 这样可以显示欢迎页面而不被编辑器覆盖

        // 设置路径点击回调
        if (this.onPathClick) {
            this.setPathClickCallback(this.onPathClick);
        }

        return true;
    }

    /**
     * 切换查看器类型
     * @param {string} viewerType - 'virtual' 或 'codemirror'
     */
    switchViewer(viewerType) {
        if (viewerType === this.currentViewerType) {
            return;
        }

        // 销毁当前查看器
        if (this.currentViewerType === 'codemirror') {
            this.codemirrorViewer.destroy();
        } else {
            this.virtualScrollViewer.destroy();
        }

        // 切换查看器类型
        this.currentViewerType = viewerType;

        // 清空容器
        if (this.container) {
            this.container.innerHTML = '';
        }

        // 初始化新查看器
        if (viewerType === 'codemirror') {
            this.currentViewer = this.codemirrorViewer;
            this.codemirrorViewer.initialize(this.container);
            // 先设置路径点击回调
            if (this.onPathClick) {
                this.codemirrorViewer.setOnPathClick(this.onPathClick);
            }
            // 只有当有当前文件时才加载内容
            if (this.currentContent && this.currentFile) {
                const filename = this.currentFile.split('/').pop();
                this.codemirrorViewer.loadFile(this.currentContent, filename);
                // loadFile 后再次设置，确保回调正确应用
                if (this.onPathClick) {
                    this.codemirrorViewer.setOnPathClick(this.onPathClick);
                }
            }
        } else {
            this.currentViewer = this.virtualScrollViewer;
            // 只有当有当前文件时才加载内容
            if (this.currentLines && this.currentFile) {
                this.virtualScrollViewer.init('fileViewer', this.currentLines, this.currentLanguageClass, this.currentShowWhitespace, {
                    currentFile: this.currentFile,
                    namePreviewCache: this.globalNamePreviewCache,
                    namePreviewPromises: this.globalNamePreviewPromises,
                    onPathClick: this.onPathClick
                });
            }
        }

        console.log(`Switched to ${viewerType} viewer`);
    }

    /**
     * 获取当前查看器类型
     * @returns {string} 'virtual' 或 'codemirror'
     */
    getCurrentViewerType() {
        return this.currentViewerType;
    }

    loadFile(key, content, lines, languageClass, showWhitespace, options = {}) {
        // 保存当前文件信息
        this.currentFile = key;
        this.currentContent = content;
        this.currentLines = lines;
        this.currentLanguageClass = languageClass;
        this.currentShowWhitespace = showWhitespace;

        const filename = key.split('/').pop();

        // 设置全局缓存和回调
        if (options.namePreviewCache) {
            this.globalNamePreviewCache = options.namePreviewCache;
        }
        if (options.namePreviewPromises) {
            this.globalNamePreviewPromises = options.namePreviewPromises;
        }
        if (options.onPathClick) {
            this.onPathClick = options.onPathClick;
        }

        // 根据当前查看器类型加载文件
        if (this.currentViewerType === 'codemirror') {
            this.codemirrorViewer.setOnPathClick(options.onPathClick);
            
            // 确保 CodeMirror 已初始化
            if (!this.codemirrorViewer.view && this.container) {
                console.log('Initializing CodeMirror viewer...');
                this.codemirrorViewer.initialize(this.container);
            }
            
            const loaded = this.codemirrorViewer.loadFile(content, filename);
            if (!loaded) {
                // 如果加载失败（CodeMirror 未初始化），重新初始化
                console.warn('CodeMirror load failed, reinitializing...');
                if (this.container) {
                    this.codemirrorViewer.initialize(this.container);
                    // 再次尝试加载
                    this.codemirrorViewer.loadFile(content, filename);
                }
            }
        } else {
            // 虚拟滚动查看器
            this.virtualScrollViewer.init('fileViewer', lines, languageClass, showWhitespace, {
                currentFile: key,
                namePreviewCache: this.globalNamePreviewCache,
                namePreviewPromises: this.globalNamePreviewPromises,
                onPathClick: options.onPathClick
            });
        }
    }

    setPathClickCallback(callback) {
        this.onPathClick = callback;
        if (this.currentViewerType === 'codemirror') {
            this.codemirrorViewer.setOnPathClick(callback);
        }
    }

    setFileLoadCallback(callback) {
        this.onFileLoad = callback;
    }

    setGlobalCaches(namePreviewCache, namePreviewPromises) {
        this.globalNamePreviewCache = namePreviewCache;
        this.globalNamePreviewPromises = namePreviewPromises;
    }

    setZoom(level) {
        if (this.currentViewerType === 'codemirror') {
            this.codemirrorViewer.setZoom(level);
        }
        // 虚拟滚动的缩放通过 ZoomManager 单例管理
    }

    getZoom() {
        if (this.currentViewerType === 'codemirror') {
            return this.codemirrorViewer.getZoom();
        }
        return 1.0; // 虚拟滚动的默认缩放
    }

    setShowWhitespace(show) {
        if (this.currentViewerType === 'codemirror') {
            this.codemirrorViewer.setShowWhitespace(show);
        }
        // 虚拟滚动的空白字符显示在渲染时处理
    }

    /**
     * 设置编辑模式
     * @param {boolean} editable - true 为可编辑模式，false 为只读模式
     */
    setEditable(editable) {
        if (this.currentViewerType === 'codemirror') {
            this.codemirrorViewer.setEditable(editable);
        }
        // 虚拟滚动不支持编辑
    }

    /**
     * 获取当前编辑状态
     * @returns {boolean} 是否可编辑
     */
    isEditable() {
        if (this.currentViewerType === 'codemirror') {
            return this.codemirrorViewer.isEditable();
        }
        return false;
    }

    /**
     * 获取当前内容
     * @returns {string} 编辑器内容
     */
    getContent() {
        if (this.currentViewerType === 'codemirror') {
            return this.codemirrorViewer.getContent();
        }
        return this.currentContent || '';
    }

    /**
     * 清空当前状态（关闭所有标签时调用）
     */
    clearState() {
        this.currentFile = null;
        this.currentContent = null;
        this.currentLines = null;
        this.currentLanguageClass = null;
        this.currentShowWhitespace = false;
    }

    destroy() {
        this.codemirrorViewer.destroy();
        this.virtualScrollViewer.destroy();
    }
}