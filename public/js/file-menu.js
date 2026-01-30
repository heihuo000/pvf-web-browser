// 文件菜单功能模块

import { API } from './api.js';
import { positionDropdown } from './utils.js';

export class FileMenuManager {
    constructor() {
        this.container = null;
        this.menuItem = null;
        this.files = [];
        this.onFileSelect = null;
    }

    init(containerId, menuItemId, callbacks = {}) {
        console.log('FileMenuManager.init() called with:', containerId, menuItemId);
        this.container = document.getElementById(containerId);
        this.menuItem = document.getElementById(menuItemId);
        this.onFileSelect = callbacks.onFileSelect;
        
        console.log('Container element:', this.container);
        console.log('MenuItem element:', this.menuItem);
        
        if (!this.container) {
            console.error(`Container with id "${containerId}" not found`);
        }
        if (!this.menuItem) {
            console.error(`MenuItem with id "${menuItemId}" not found`);
        }
    }

    async load() {
        console.log('FileMenuManager.load() called');
        try {
            const data = await API.getPvfFiles();
            console.log('PVF files data:', data);
            this.files = data.files || [];
            console.log('Files array:', this.files);
            this.render();
        } catch (error) {
            console.error('加载 PVF 文件列表失败:', error);
            if (this.container) {
                this.container.innerHTML = '<div class="menu-dropdown-empty">加载失败</div>';
            }
        }
    }

    render() {
        console.log('FileMenuManager.render() called');
        console.log('Container:', this.container);
        console.log('MenuItem:', this.menuItem);
        console.log('Files:', this.files);
        
        if (!this.container) {
            console.error('pvfFileMenu element not found');
            return;
        }
        if (!this.menuItem) {
            console.error('fileMenu element not found');
            return;
        }

        positionDropdown(this.menuItem, this.container);

        if (!this.files || this.files.length === 0) {
            console.log('No files to display');
            this.container.innerHTML = '<div class="menu-dropdown-empty">暂无 PVF 文件</div>';
            return;
        }

        console.log('Rendering', this.files.length, 'files');
        
        // 不再克隆节点，直接替换内容
        this.container.innerHTML = this.files.map(file => `
            <div class="menu-dropdown-item" data-file-path="${encodeURIComponent(file.path)}">
                ${file.name}
            </div>
        `).join('');

        console.log('Menu HTML:', this.container.innerHTML);

        // 添加点击事件 - 使用事件委托
        this.container.addEventListener('click', (e) => {
            const item = e.target.closest('.menu-dropdown-item');
            if (item) {
                e.stopPropagation();
                e.preventDefault();
                const filePath = decodeURIComponent(item.dataset.filePath);
                console.log('File selected:', filePath);
                if (this.onFileSelect) {
                    this.onFileSelect(filePath);
                }
            }
        });
    }

    getFiles() {
        return this.files;
    }
}