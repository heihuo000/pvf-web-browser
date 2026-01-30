// 文件查看器管理器 - 使用 CodeMirror 6

import CodeMirrorViewer from './codemirror-viewer.js';

export class ViewerManager {
    constructor() {
        this.codemirrorViewer = new CodeMirrorViewer();
        this.currentViewer = this.codemirrorViewer;
        this.onPathClick = null;
        this.globalNamePreviewCache = null;
        this.globalNamePreviewPromises = null;
    }

    initialize(containerId) {
        this.container = document.getElementById(containerId);
        if (!this.container) {
            console.error(`Container with id "${containerId}" not found`);
            return false;
        }

        // 初始化 CodeMirror 查看器
        this.codemirrorViewer.initialize(this.container);

        // 设置路径点击回调
        if (this.onPathClick) {
            this.setPathClickCallback(this.onPathClick);
        }

        return true;
    }

    loadFile(key, content, lines, languageClass, showWhitespace, options = {}) {
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
            this.codemirrorViewer.setOnPathClick(options.onPathClick);
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
    }

    setPathClickCallback(callback) {
        this.onPathClick = callback;
        this.codemirrorViewer.setOnPathClick(callback);
    }

    setFileLoadCallback(callback) {
        this.onFileLoad = callback;
    }

    setGlobalCaches(namePreviewCache, namePreviewPromises) {
        this.globalNamePreviewCache = namePreviewCache;
        this.globalNamePreviewPromises = namePreviewPromises;
    }

    setZoom(level) {
        this.codemirrorViewer.setZoom(level);
    }

    getZoom() {
        return this.codemirrorViewer.getZoom();
    }
    
    setShowWhitespace(show) {
        this.codemirrorViewer.setShowWhitespace(show);
    }

    destroy() {
        if (this.codemirrorViewer) {
            this.codemirrorViewer.destroy();
        }
    }
}