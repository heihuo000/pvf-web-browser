// 工具函数模块

// 转义HTML特殊字符
export function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// 格式化文件大小
export function formatSize(bytes) {
    if (bytes === 0) return '0 B';
    const k = 1024;
    const sizes = ['B', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

// 获取语言类型
export function getLanguageClass(ext) {
    const languageMap = {
        'lst': 'language-pvf',
        'equ': 'language-pvf',
        'stk': 'language-pvf',
        'str': 'language-pvf',
        'key': 'language-pvf',
        'als': 'language-pvf',
        'act': 'language-pvf',
        'ai': 'language-pvf',
        'aic': 'language-pvf',
        'obj': 'language-pvf',
        'mob': 'language-pvf',
        'npc': 'language-pvf',
        'qst': 'language-pvf',
        'shp': 'language-pvf',
        'cre': 'language-pvf',
        'apd': 'language-pvf',
        'twn': 'language-pvf',
        'dgn': 'language-pvf',
        'rgn': 'language-pvf',
        'wdm': 'language-pvf',
        'etc': 'language-pvf',
        'co': 'language-pvf',
        'skl': 'language-pvf',
        'mm': 'language-pvf',
        'ani': 'language-pvf',
        'nut': 'language-squirrel',
        'sql': 'language-sql',
        'txt': 'language-plaintext',
        'cfg': 'language-plaintext',
        'def': 'language-plaintext',
        'inc': 'language-plaintext'
    };
    
    return languageMap[ext] || 'language-plaintext';
}

// 创建下载链接并触发下载
export function downloadFile(filename, content) {
    try {
        const blob = new Blob([content], { type: 'text/plain;charset=utf-8' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename.split('/').pop();
        document.body.appendChild(a);
        a.click();
        setTimeout(() => {
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        }, 100);
    } catch (error) {
        throw new Error('创建下载链接失败: ' + error.message);
    }
}

// 显示/隐藏加载状态
export function showLoading(loadingElement, show) {
    loadingElement.classList.toggle('show', show);
}

// 定位下拉菜单
export function positionDropdown(menuItem, dropdown) {
    const rect = menuItem.getBoundingClientRect();
    dropdown.style.top = (rect.bottom) + 'px';
    dropdown.style.left = rect.left + 'px';
    dropdown.style.background = '#252526';
    dropdown.style.border = '1px solid #3c3c3c';
}

// 更新选中文件数量（导出到全局作用域以便在main.js中使用）
export function updateSelectedCount() {
    // 只统计文件的复选框，不包括文件夹的复选框
    const selectedFiles = document.querySelectorAll('.file-item input[type="checkbox"]:checked:not([data-is-folder="true"])').length;
    const countSpan = document.getElementById('selectedCount');
    if (countSpan) {
        countSpan.textContent = selectedFiles;
    }
    return selectedFiles;
}