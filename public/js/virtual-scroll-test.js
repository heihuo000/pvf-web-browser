// 测试虚拟滚动容器高度
function testContainerHeight() {
    const container = document.getElementById('fileViewer');
    if (container) {
        console.log('=== File Viewer Container Info ===');
        console.log('clientHeight:', container.clientHeight);
        console.log('scrollHeight:', container.scrollHeight);
        console.log('offsetHeight:', container.offsetHeight);
        console.log('computed height:', window.getComputedStyle(container).height);
        console.log('computed display:', window.getComputedStyle(container).display);
        console.log('computed flex:', window.getComputedStyle(container).flex);
        console.log('computed flexDirection:', window.getComputedStyle(container).flexDirection);
        
        const virtualContainer = container.querySelector('.virtual-scroll-container');
        if (virtualContainer) {
            console.log('=== Virtual Container Info ===');
            console.log('clientHeight:', virtualContainer.clientHeight);
            console.log('scrollHeight:', virtualContainer.scrollHeight);
            console.log('offsetHeight:', virtualContainer.offsetHeight);
            console.log('computed height:', window.getComputedStyle(virtualContainer).height);
            console.log('computed overflowY:', window.getComputedStyle(virtualContainer).overflowY);
            console.log('computed overflowX:', window.getComputedStyle(virtualContainer).overflowX);
            console.log('children count:', virtualContainer.children.length);
            
            for (let i = 0; i < virtualContainer.children.length; i++) {
                console.log(`  Child ${i}:`, virtualContainer.children[i].className, 'height:', virtualContainer.children[i].style.height);
            }
        } else {
            console.log('NO virtual-scroll-container found!');
        }
        
        const content = container.querySelector('.virtual-scroll-content');
        if (content) {
            console.log('=== Virtual Content Info ===');
            console.log('clientHeight:', content.clientHeight);
            console.log('scrollHeight:', content.scrollHeight);
            console.log('style.height:', content.style.height);
            console.log('style.minHeight:', content.style.minHeight);
        } else {
            console.log('NO virtual-scroll-content found!');
        }
        
        const viewport = container.querySelector('.virtual-scroll-viewport');
        if (viewport) {
            console.log('=== Virtual Viewport Info ===');
            console.log('clientHeight:', viewport.clientHeight);
            console.log('offsetHeight:', viewport.offsetHeight);
            console.log('children count:', viewport.children.length);
        } else {
            console.log('NO virtual-scroll-viewport found!');
        }
    } else {
        console.log('NO fileViewer container found!');
    }
}

// 在页面加载后运行测试
setTimeout(testContainerHeight, 1000);
setTimeout(testContainerHeight, 2000);