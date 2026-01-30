# JavaScript 模块说明

本项目已将 JavaScript 代码模块化，分为以下几个独立模块：

## 模块列表

### 1. **utils.js** - 工具函数模块
提供通用的工具函数：
- `escapeHtml(text)` - 转义 HTML 特殊字符
- `formatSize(bytes)` - 格式化文件大小
- `getLanguageClass(ext)` - 获取语言类型
- `downloadFile(filename, content)` - 创建下载链接
- `showLoading(loadingElement, show)` - 显示/隐藏加载状态
- `positionDropdown(menuItem, dropdown)` - 定位下拉菜单

### 2. **api.js** - API 调用封装模块
封装所有后端 API 调用：
- `getFiles(path)` - 获取文件列表
- `getFile(key)` - 获取文件内容
- `openPvf(filePath, encodingMode)` - 打开 PVF 文件
- `savePvf(filePath)` - 保存 PVF 文件
- `saveFile(key, content, encoding)` - 保存文件到 PVF
- `getBookmarks()` - 获取书签列表
- `saveBookmarks(bookmarks)` - 保存书签列表
- `getPvfFiles()` - 获取 PVF 文件列表
- `searchFiles(query)` - 搜索文件
- `advancedSearch(params)` - 高级搜索
- `batchExtract(keys, destPath)` - 批量提取文件
- `getStatus()` - 获取状态

### 3. **bookmark.js** - 书签功能模块
管理书签的增删改查：
- `BookmarkManager` 类
- `init(containerId, menuItemIndex, callbacks)` - 初始化书签管理器
- `load()` - 加载书签
- `save()` - 保存书签
- `add(key, name)` - 添加书签
- `remove(key)` - 删除书签
- `updateName(key, newName)` - 更新书签名称
- `render()` - 渲染书签菜单

### 4. **file-menu.js** - 文件菜单功能模块
管理 PVF 文件菜单：
- `FileMenuManager` 类
- `init(containerId, menuItemId, callbacks)` - 初始化文件菜单管理器
- `load()` - 加载 PVF 文件列表
- `render()` - 渲染文件菜单

### 5. **modal.js** - 模态框管理模块
统一管理所有模态框：
- `ModalManager` 类
- `register(id)` - 注册模态框
- `show(id)` - 显示模态框
- `hide(id)` - 隐藏模态框
- `hideAll()` - 隐藏所有模态框
- `isShown(id)` - 检查模态框是否显示

### 6. **virtual-scroll.js** - 虚拟滚动模块
优化大文件（超过500行）的渲染性能：
- `VirtualScrollManager` 类
- `init(containerId, lines, languageClass, showWhitespace, callbacks)` - 初始化虚拟滚动
- `renderVisibleLines(scrollTop, savedScrollLeft)` - 渲染可见行
- `bindEvents()` - 绑定滚动事件
- `updateLines(lines, languageClass, showWhitespace)` - 更新行数据
- `destroy()` - 销毁虚拟滚动实例

**特性：**
- 只渲染可见区域内的行，大幅提升性能
- 智能横向滚动位置保持
- 支持触摸滚动（移动端）
- 自动适应窗口大小变化
- 多层 scrollLeft 保存机制

### 7. **main.js** - 主入口文件
整合所有模块，初始化应用：
- 全局状态管理（包括文件内容缓存）
- DOM 元素引用
- 管理器实例化
- 事件监听器绑定
- 页面初始化逻辑
- 文件加载和渲染逻辑（自动判断是否使用虚拟滚动）

**性能优化：**
- 文件内容缓存（保留最近10个文件）
- 自动切换虚拟滚动（>500行使用）
- 请求AnimationFrame 优化渲染

## 使用方式

所有模块使用 ES6 模块语法（import/export）：

```javascript
// 导入模块
import { API } from './api.js';
import { BookmarkManager } from './bookmark.js';
import { showLoading } from './utils.js';

// 使用模块
const bookmarkManager = new BookmarkManager();
bookmarkManager.init('bookmarkMenu', 1);
bookmarkManager.load();
```

## 模块依赖关系

```
main.js
├── utils.js (工具函数)
├── api.js (API 调用)
├── bookmark.js (书签管理)
│   ├── api.js
│   └── utils.js
├── file-menu.js (文件菜单)
│   ├── api.js
│   └── utils.js
├── modal.js (模态框管理)
└── virtual-scroll.js (虚拟滚动)
    └── utils.js
```

## 优势

1. **代码组织清晰**：每个模块负责单一功能，职责明确
2. **易于维护**：修改某个功能时，只需关注对应的模块文件
3. **可复用性强**：工具函数和 API 封装可以在多个地方复用
4. **便于测试**：每个模块可以独立进行单元测试
5. **减少文件大小**：从 4044 行减少到 1605 行（约 60%）
6. **提高可读性**：代码结构更清晰，便于理解和协作

## 注意事项

1. 所有模块都使用 ES6 模块语法，需要在支持 ES6 模块的浏览器中运行
2. 模块之间通过 import/export 进行通信
3. 主入口文件（main.js）负责初始化和整合所有模块
4. 全局状态（如 currentFile、currentPath）在 main.js 中管理
5. 文件内容缓存自动管理，保留最近10个文件，超过限制自动清理
6. 超过500行的文件自动使用虚拟滚动，保证性能
7. 虚拟滚动会自动保持横向滚动位置，避免滚动时跳回左侧