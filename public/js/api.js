// API 调用封装模块

const API_BASE = '';

export const API = {
    // 获取文件列表
    async getFiles(path = '') {
        const response = await fetch(`${API_BASE}/api/files?path=${encodeURIComponent(path)}`);
        return await response.json();
    },

    // 获取文件内容
    async getFile(key) {
        const response = await fetch(`${API_BASE}/api/file?key=${encodeURIComponent(key)}`);
        return await response.json();
    },

    // 打开 PVF 文件
    async openPvf(filePath, encodingMode = 'TW') {
        const response = await fetch(`${API_BASE}/api/open`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ filePath, encodingMode })
        });
        return await response.json();
    },

    // 保存 PVF 文件
    async savePvf(filePath) {
        const response = await fetch(`${API_BASE}/api/save`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ filePath })
        });
        return await response.json();
    },

    // 保存文件到 PVF
    async saveFile(key, content, encoding) {
        const response = await fetch(`${API_BASE}/api/save-file`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ key, content, encoding })
        });
        return await response.json();
    },

    // 获取书签列表
    async getBookmarks() {
        const response = await fetch(`${API_BASE}/api/bookmarks`);
        return await response.json();
    },

    // 保存书签列表
    async saveBookmarks(bookmarks) {
        const response = await fetch(`${API_BASE}/api/bookmarks`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ bookmarks })
        });
        return await response.json();
    },

    // 获取 PVF 文件列表
    async getPvfFiles() {
        const response = await fetch(`${API_BASE}/api/pvf-files`);
        return await response.json();
    },

    // 搜索文件
    async searchFiles(query) {
        const response = await fetch(`${API_BASE}/api/search?q=${encodeURIComponent(query)}`);
        return await response.json();
    },

    // 搜索（兼容旧接口）
    async search(query, useRegex = false, caseSensitive = false) {
        return await this.searchFiles(query);
    },

    // 高级搜索
    async advancedSearch(params) {
        const searchParams = new URLSearchParams({
            type: params.type,
            keyword: params.keyword,
            startMatch: params.startMatch,
            useRegex: params.useRegex,
            caseSensitive: params.caseSensitive,
            offset: params.offset || 0,
            limit: params.limit || 500
        });
        const response = await fetch(`${API_BASE}/api/advanced-search?${searchParams}`);
        return await response.json();
    },

    // 批量提取文件
    async batchExtract(keys, destPath) {
        const response = await fetch(`${API_BASE}/api/batch-extract`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ keys, destPath })
        });
        return await response.json();
    },

    // 批量提取文件（别名，用于兼容）
    async batchDownload(keys, destPath) {
        return await this.batchExtract(keys, destPath);
    },

    // 批量提取文件并打包成zip
    async batchExtractZip(keys, destPath) {
        const response = await fetch(`${API_BASE}/api/batch-extract-zip`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ keys, destPath })
        });

        if (!response.ok) {
            const errorData = await response.json();
            return { error: errorData.error || '下载失败' };
        }

        // 获取ZIP文件的blob
        const blob = await response.blob();

        // 创建下载链接
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'extracted_files.zip';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);

        return { success: true };
    },

    // Get all files in a folder (recursive)
    async getFilesInFolder(folderPath) {
        const response = await fetch(`${API_BASE}/api/files-in-folder?path=${encodeURIComponent(folderPath)}`);
        return await response.json();
    },

    // 获取状态
    async getStatus() {
        const response = await fetch(`${API_BASE}/api/status`);
        return await response.json();
    }
};