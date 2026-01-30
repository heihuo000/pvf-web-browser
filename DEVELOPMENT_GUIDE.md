# PVF Web Browser 开发指南

## 项目概述

PVF Web Browser 是一个基于 Web 的 PVF 文件浏览器，支持多标签页、虚拟滚动、语法高亮、书签管理等高级功能。项目采用模块化设计，前后端分离，便于维护和扩展。

## 项目架构

### 目录结构

```
pvf-web-browser/
├── src/                    # TypeScript 源代码（服务器端）
│   ├── api.ts             # API 路由定义
│   ├── crypto.ts          # 加密解密功能
│   ├── index.ts           # 服务器入口
│   ├── model.ts           # 数据模型
│   ├── server.ts          # HTTP 服务器
│   ├── pvfFile.ts         # PVF 文件处理
│   ├── stringTable.ts     # 字符串表处理
│   ├── services/          # 服务层
│   │   ├── codeSearchService.ts
│   │   ├── fileSearchService.ts
│   │   └── stringRefSearchService.ts
│   └── ...
├── public/                 # 前端文件
│   ├── index.html         # 主 HTML 文件
│   ├── js/                # JavaScript 模块
│   │   ├── api.js         # API 调用模块
│   │   ├── bookmark.js    # 书签管理模块
│   │   ├── file-formatter.js  # 文件格式化模块
│   │   ├── file-menu.js   # 文件菜单管理模块
│   │   ├── main.js        # 主入口模块
│   │   ├── modal.js       # 模态框管理模块
│   │   ├── utils.js       # 工具函数模块
│   │   └── virtual-scroll.js  # 虚拟滚动模块
│   └── libs/              # 第三方库
│       ├── prism-tomorrow.min.css
│       └── prism.min.js
├── server/                 # 服务器代码（JavaScript）
│   └── index.js
└── package.json           # 项目依赖
```

### 技术栈

- **前端**：
  - 原生 JavaScript (ES6+)
  - CSS3
  - HTML5
  - Prism.js (语法高亮)
  
- **后端**：
  - Node.js
  - TypeScript
  - Express.js
  - PVF 文件处理库

## 模块化设计理念

### 前端模块划分

每个模块都有明确的职责，遵循单一职责原则。

#### 1. `main.js` - 主入口模块
**职责**：
- 应用初始化
- 模块集成
- 全局状态管理
- DOM 元素引用

**何时修改**：
- 添加新的全局变量
- 集成新模块
- 修改应用初始化逻辑
- 添加新的事件监听器

**重要变量**：
```javascript
const elements = { ... }  // DOM 元素引用
const currentFile = null  // 当前打开的文件
const tabs = []          // 标签页列表
const activeTabId = null // 当前活动标签页 ID
const batchMode = false  // 批量模式状态
```

#### 2. `api.js` - API 调用模块
**职责**：
- 封装所有后端 API 调用
- 处理请求和响应
- 错误处理

**何时修改**：
- 添加新的 API 端点
- 修改现有 API 调用逻辑
- 添加请求/响应拦截器

**使用示例**：
```javascript
const response = await API.getFiles(path);
const data = await API.getFile(key);
```

#### 3. `virtual-scroll.js` - 虚拟滚动模块
**职责**：
- 大文件（>500行）的虚拟滚动渲染
- 动态加载可见行
- 性能优化

**何时修改**：
- 优化虚拟滚动性能
- 修复滚动相关问题
- 添加新的滚动功能

**关键类**：
```javascript
export class VirtualScrollManager {
    init(containerId, lines, languageClass, showWhitespace, callbacks)
    renderVisibleLines(scrollTop, savedScrollLeft)
    loadNamePreview(path, previewElement)
    destroy()
}
```

#### 4. `file-formatter.js` - 文件格式化模块
**职责**：
- 路径链接生成
- 名称预览加载
- 语法高亮
- 空白字符显示

**何时修改**：
- 添加新的格式化功能
- 修改路径链接逻辑
- 优化语法高亮
- 修改空白字符显示方式

**主要导出**：
```javascript
export class FileFormatter { ... }
export function applySyntaxHighlighting(container) { ... }
export function applyWhitespaceDisplay(container) { ... }
```

#### 5. `bookmark.js` - 书签管理模块
**职责**：
- 书签的增删改查
- 书签持久化（localStorage）
- 书签渲染

**何时修改**：
- 添加新的书签功能
- 修改书签存储方式
- 优化书签管理逻辑

#### 6. `file-menu.js` - 文件菜单管理模块
**职责**：
- 文件菜单的渲染和管理
- 菜单项点击事件处理

**何时修改**：
- 添加新的菜单项
- 修改菜单显示逻辑
- 优化菜单交互

#### 7. `modal.js` - 模态框管理模块
**职责**：
- 模态框的注册和管理
- 模态框显示/隐藏
- 模态框事件处理

**何时修改**：
- 添加新的模态框
- 修改模态框行为
- 优化模态框管理

#### 8. `utils.js` - 工具函数模块
**职责**：
- 通用工具函数
- 辅助功能

**何时修改**：
- 添加新的工具函数
- 优化现有工具函数

### 后端模块划分

#### 1. `server.ts` - HTTP 服务器
**职责**：
- 启动 HTTP 服务器
- 路由配置
- 中间件配置

#### 2. `api.ts` - API 路由
**职责**：
- 定义所有 API 路由
- 请求参数验证
- 响应格式化

#### 3. `pvfFile.ts` - PVF 文件处理
**职责**：
- PVF 文件读取
- 文件内容解析
- 文件加密解密

#### 4. `services/` - 服务层
**职责**：
- 业务逻辑处理
- 数据搜索
- 复杂操作封装

## 代码规范

### 命名规范

1. **文件名**：使用小写字母和连字符
   - ✅ `file-formatter.js`
   - ❌ `FileFormatter.js`

2. **变量名**：使用 camelCase
   - ✅ `currentFile`
   - ❌ `CurrentFile`

3. **常量名**：使用 UPPER_SNAKE_CASE
   - ✅ `MAX_CACHE_SIZE`
   - ❌ `maxCacheSize`

4. **类名**：使用 PascalCase
   - ✅ `VirtualScrollManager`
   - ❌ `virtualScrollManager`

5. **函数名**：使用 camelCase
   - ✅ `loadFileContent`
   - ❌ `LoadFileContent`

### 模块导入导出

#### 导出方式

1. **命名导出**：适用于多个导出
```javascript
export function loadFileContent() { }
export function saveFileContent() { }
```

2. **默认导出**：适用于单一导出（如类）
```javascript
export default class VirtualScrollManager { }
```

3. **混合导出**：不推荐，避免使用

#### 导入方式

```javascript
// 命名导入
import { loadFileContent, saveFileContent } from './api.js';

// 默认导入
import VirtualScrollManager from './virtual-scroll.js';

// 命名空间导入
import * as API from './api.js';
```

### 代码组织

1. **文件顶部**：导入语句
```javascript
import { API } from './api.js';
import { FileFormatter } from './file-formatter.js';
```

2. **全局变量**：在文件顶部定义
```javascript
const currentFile = null;
const tabs = [];
```

3. **函数/类定义**：按逻辑顺序排列
4. **事件监听器**：在 `init()` 函数中注册
5. **导出**：在文件末尾

### 注释规范

1. **单行注释**：用于简短说明
```javascript
// 加载文件内容
async function loadFileContent() { }
```

2. **多行注释**：用于复杂逻辑
```javascript
/**
 * 虚拟滚动管理器
 * 用于处理大文件的滚动渲染
 */
export class VirtualScrollManager { }
```

3. **TODO 注释**：标记待办事项
```javascript
// TODO: 优化大文件加载性能
```

## 功能开发指南

### 添加新功能

#### 1. 确定功能模块

根据功能类型，确定应该修改哪个模块：

| 功能类型 | 模块 |
|---------|------|
| 文件操作（打开、保存、下载） | `main.js` |
| 文件格式化（语法高亮、路径链接） | `file-formatter.js` |
| 滚动优化 | `virtual-scroll.js` |
| 书签管理 | `bookmark.js` |
| 菜单功能 | `file-menu.js` |
| 弹窗功能 | `modal.js` |
| API 调用 | `api.js` |
| 工具函数 | `utils.js` |

#### 2. 开发步骤

1. **需求分析**：明确功能需求和用户场景
2. **模块选择**：根据功能类型选择合适的模块
3. **代码实现**：遵循代码规范编写代码
4. **测试验证**：确保功能正常工作
5. **文档更新**：更新相关文档

#### 3. 示例：添加新按钮功能

**场景**：在工具栏添加一个新按钮

**步骤**：

1. **HTML**：在 `index.html` 添加按钮
```html
<button id="newFeatureBtn">新功能</button>
```

2. **JavaScript**：在 `main.js` 添加事件监听器
```javascript
// 在 init() 函数中添加
if (elements.newFeatureBtn) {
    elements.newFeatureBtn.addEventListener('click', () => {
        // 实现功能逻辑
    });
}
```

3. **全局变量**：如果需要，添加到 `elements` 对象
```javascript
const elements = {
    // ... 其他元素
    newFeatureBtn: document.getElementById('newFeatureBtn'),
};
```

### 修改现有功能

1. **定位代码**：使用搜索工具找到相关代码
2. **理解逻辑**：仔细阅读现有代码，理解实现原理
3. **谨慎修改**：确保修改不会破坏其他功能
4. **充分测试**：测试所有相关场景

### 性能优化

1. **虚拟滚动**：大文件使用 `virtual-scroll.js`
2. **缓存策略**：使用 `fileContentCache` 缓存文件内容
3. **延迟加载**：使用 `requestAnimationFrame` 优化 DOM 操作
4. **事件委托**：减少事件监听器数量

## 调试指南

### 常见问题

1. **文件无法滚动**
   - 检查 CSS 样式是否正确
   - 检查容器高度是否正确设置
   - 检查 `overflow` 属性

2. **链接点击无效**
   - 检查 `pointer-events` 设置
   - 检查事件监听器是否正确绑定

3. **标签页切换失败**
   - 检查缓存是否正确
   - 检查 `switchToTab` 函数逻辑

### 调试工具

1. **浏览器控制台**：查看错误日志
2. **Network 面板**：检查 API 请求
3. **Elements 面板**：检查 DOM 结构
4. **Sources 面板**：设置断点调试

### 日志输出

```javascript
console.log('信息日志');
console.warn('警告日志');
console.error('错误日志');
console.debug('调试日志');
```

## 版本管理

### Git 提交规范

提交信息格式：
```
<type>: <subject>

<body>
```

类型（type）：
- `feat`: 新功能
- `fix`: 修复 bug
- `docs`: 文档更新
- `style`: 代码格式调整
- `refactor`: 重构
- `perf`: 性能优化
- `test`: 测试相关
- `chore`: 构建/工具相关

示例：
```
fix: 修复小文件滚动问题

- 修改文件渲染逻辑
- 添加延迟加载机制
- 优化 CSS 样式
```

### 版本号规范

遵循语义化版本：`MAJOR.MINOR.PATCH`

- `MAJOR`：不兼容的 API 变更
- `MINOR`：向下兼容的功能性新增
- `PATCH`：向下兼容的问题修正

## 最佳实践

### 1. 单一职责原则

每个模块/函数只做一件事。

### 2. 开闭原则

对扩展开放，对修改关闭。

### 3. 依赖倒置原则

依赖抽象而非具体实现。

### 4. 代码复用

避免重复代码，提取公共逻辑到工具函数。

### 5. 错误处理

所有异步操作都应该有错误处理。

```javascript
try {
    const response = await API.getFiles(path);
    // 处理响应
} catch (error) {
    console.error('加载失败:', error);
    // 显示错误信息
}
```

### 6. 用户体验

- 提供加载提示
- 显示操作反馈
- 友好的错误提示

## 常用命令

### 开发

```bash
# 安装依赖
npm install

# 启动开发服务器
npm run dev

# 构建生产版本
npm run build
```

### 代码检查

```bash
# 检查 TypeScript 类型
npm run type-check

# 运行测试
npm test
```

## 联系方式

如有问题，请查阅项目文档或联系开发团队。

---

**最后更新**：2026-01-28  
**维护者**：iFlow CLI