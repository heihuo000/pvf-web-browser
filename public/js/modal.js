// 模态框管理模块

export class ModalManager {
    constructor() {
        this.modals = {};
    }

    register(id) {
        const modal = document.getElementById(id);
        if (modal) {
            this.modals[id] = modal;
            return modal;
        }
        console.error(`Modal with id "${id}" not found`);
        return null;
    }

    show(id) {
        const modal = this.modals[id];
        if (modal) {
            modal.classList.add('show');
        }
    }

    hide(id) {
        const modal = this.modals[id];
        if (modal) {
            modal.classList.remove('show');
        }
    }

    hideAll() {
        Object.values(this.modals).forEach(modal => {
            modal.classList.remove('show');
        });
    }

    isShown(id) {
        const modal = this.modals[id];
        return modal && modal.classList.contains('show');
    }
}