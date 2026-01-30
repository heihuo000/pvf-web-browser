// 缩放管理器 - 支持捏合缩放和键盘缩放

export class ZoomManager {
    constructor() {
        this.scale = 1.0;
        this.minScale = 0.5;
        this.maxScale = 3.0;
        this.pinchDistance = 0;
        this.isPinching = false;
        this.onZoomChange = null;
        this.targetElement = null;
        
        // 从localStorage加载缩放比例
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
        
        // 应用缩放到内容容器
        const codeWithLines = this.targetElement.querySelector('.code-with-lines');
        if (codeWithLines) {
            codeWithLines.style.transform = `scale(${this.scale})`;
            codeWithLines.style.transformOrigin = 'top left';
            
            // 调整容器的宽度和高度以适应缩放
            if (this.scale !== 1.0) {
                const originalWidth = codeWithLines.scrollWidth;
                const originalHeight = codeWithLines.scrollHeight;
                codeWithLines.style.width = `${originalWidth * this.scale}px`;
                codeWithLines.style.height = `${originalHeight * this.scale}px`;
            } else {
                codeWithLines.style.width = '';
                codeWithLines.style.height = '';
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
                
                // 计算新的缩放比例
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
            // Ctrl/Cmd + Plus 放大
            if ((e.ctrlKey || e.metaKey) && (e.key === '+' || e.key === '=')) {
                e.preventDefault();
                this.zoomIn();
            }
            
            // Ctrl/Cmd + Minus 缩小
            if ((e.ctrlKey || e.metaKey) && e.key === '-') {
                e.preventDefault();
                this.zoomOut();
            }
            
            // Ctrl/Cmd + 0 重置
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

// 导出单例实例
export const zoomManager = new ZoomManager();