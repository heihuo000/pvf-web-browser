// 书签功能模块

import { API } from './api.js';
import { positionDropdown } from './utils.js';

export class BookmarkManager {
    constructor() {
        this.bookmarks = [];
        this.container = null;
        this.menuItem = null;
        this.onBookmarkClick = null;
        this.onEditBookmark = null;
        this.onRemoveBookmark = null;
    }

    init(containerId, menuItemIndex, callbacks = {}) {
        this.container = document.getElementById(containerId);
        this.menuItem = document.querySelectorAll('.menu-item')[menuItemIndex];
        this.onBookmarkClick = callbacks.onBookmarkClick;
        this.onEditBookmark = callbacks.onEditBookmark;
        this.onRemoveBookmark = callbacks.onRemoveBookmark;
    }

    async load() {
        try {
            const data = await API.getBookmarks();
            this.bookmarks = data.bookmarks || [];
            this.render();
        } catch (error) {
            console.error('加载书签失败:', error);
            this.bookmarks = [];
            this.render();
        }
    }

    async save() {
        try {
            await API.saveBookmarks(this.bookmarks);
        } catch (error) {
            console.error('保存书签失败:', error);
        }
    }

    add(key, name) {
        const exists = this.bookmarks.some(b => b.key === key);
        if (exists) {
            throw new Error('该书签已存在');
        }

        const bookmark = {
            key,
            name,
            addedAt: new Date().toISOString()
        };

        this.bookmarks.push(bookmark);
        this.save();
        this.render();
        return bookmark;
    }

    remove(key) {
        this.bookmarks = this.bookmarks.filter(b => b.key !== key);
        this.save();
        this.render();
    }

    updateName(key, newName) {
        const bookmark = this.bookmarks.find(b => b.key === key);
        if (bookmark) {
            bookmark.name = newName;
            this.save();
            this.render();
        }
    }

    render() {
        if (!this.container || !this.menuItem) {
            console.error('bookmarkMenu or menu-item element not found');
            return;
        }

        positionDropdown(this.menuItem, this.container);

        if (this.bookmarks.length === 0) {
            this.container.innerHTML = '<div class="menu-dropdown-empty">暂无书签</div>';
            return;
        }

        let html = '';
        this.bookmarks.forEach(bookmark => {
            html += `
                <div class="menu-dropdown-item" data-key="${bookmark.key}">
                    <div style="flex: 1;">
                        <div style="font-weight: 500;">${bookmark.name}</div>
                        <div style="font-size: 11px; color: #858585;">${bookmark.key}</div>
                    </div>
                    <div style="display: flex; gap: 4px;">
                        <button class="delete-btn" onclick="event.stopPropagation(); this.dispatchEvent(new CustomEvent('editBookmark', { 
                            bubbles: true, 
                            detail: { key: '${bookmark.key}' }
                        }))">✎</button>
                        <button class="delete-btn" onclick="event.stopPropagation(); this.dispatchEvent(new CustomEvent('removeBookmark', { 
                            bubbles: true, 
                            detail: { key: '${bookmark.key}' }
                        }))">✕</button>
                    </div>
                </div>
            `;
        });

        this.container.innerHTML = html;

        // 添加点击事件
        this.container.querySelectorAll('.menu-dropdown-item').forEach(item => {
            item.addEventListener('click', (e) => {
                if (e.target.tagName !== 'BUTTON') {
                    const key = item.dataset.key;
                    if (this.onBookmarkClick) {
                        this.onBookmarkClick(key);
                    }
                }
            });

            // 编辑按钮事件
            const editBtn = item.querySelector('.delete-btn:first-child');
            if (editBtn) {
                editBtn.addEventListener('editBookmark', (e) => {
                    if (this.onEditBookmark) {
                        this.onEditBookmark(e.detail.key);
                    }
                });
            }

            // 删除按钮事件
            const removeBtn = item.querySelector('.delete-btn:last-child');
            if (removeBtn) {
                removeBtn.addEventListener('removeBookmark', (e) => {
                    if (this.onRemoveBookmark) {
                        this.onRemoveBookmark(e.detail.key);
                    }
                });
            }
        });
    }

    getBookmarks() {
        return this.bookmarks;
    }

    clear() {
        this.bookmarks = [];
        this.save();
        this.render();
    }
}