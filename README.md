# PVF Web Browser

PVF Web Browser 是一个用于浏览和分析 DNF (地下城与勇士) 游戏 PVF 文件的 Web 应用程序。

## 项目简介

这是一个基于 Web 的 PVF 文件浏览器，允许用户浏览、搜索和提取 DNF 游戏中的 PVF 文件内容。项目采用客户端-服务器架构，前端使用 HTML/CSS/JavaScript，后端使用 Node.js/Express。

## 功能特性

### 核心功能
- 浏览 PVF 文件内容
- 高级搜索功能（支持文件名、内容、字符串等多种搜索方式）
- 文件提取功能
- 支持多种编码模式（韩文、繁体、简体、日文等）

### 编辑与管理
- **多标签页**: 支持同时打开多个文件，在标签之间快速切换
- **文件编辑**: 可直接编辑文件内容并保存到 PVF
- **批量操作**: 支持批量提取文件
- **书签管理**: 使用 JSON 文件存储书签，方便编辑和导入导出

### 显示增强
- **路径链接**: 自动识别代码中的文件路径，点击可在新标签中打开
- **语法高亮**: 支持 PVF、脚本等多种语言的高亮显示
- **空白字符显示**: 显示 Tab 字符为箭头符号（→），方便识别格式
- **虚拟滚动**: 高效处理大文件（超过 500 行自动启用）

### 界面优化
- **响应式设计**: 支持桌面和移动设备
- **菜单栏和工具栏**: 支持折叠/展开，最大化显示区域
- **性能优化**: 字符串搜索性能大幅提升（限制搜索文件数和结果数）

## 项目结构

```
pvf-web-browser/
├── src/                    # 源代码
│   ├── aniCompiler.ts      # 动画编译器
│   ├── config.ts          # 配置管理
│   ├── crypto.ts          # 加密解密功能
│   ├── lstDecompiler.ts   # LST文件反编译器
│   ├── model.ts           # 数据模型
│   ├── pvfFile.ts         # PVF文件解析
│   ├── scriptCompiler.ts  # 脚本编译器
│   ├── server.ts          # 服务器代码
│   └── services/          # 服务模块
├── public/                # 静态资源
│   ├── index.html         # 主页面
│   └── libs/              # 第三方库
├── server/                # 服务器代码
├── dist/                  # 编译后的代码
├── package.json           # 项目配置
└── tsconfig.json          # TypeScript配置
```

## 本地运行

### 环境要求

- Node.js
- npm

### 安装和运行

1. 克隆项目：
```bash
git clone https://github.com/yourusername/pvf-web-browser.git
cd pvf-web-browser
```

2. 安装依赖：
```bash
npm install
```

3. 构建项目：
```bash
npm run build
```

4. 启动服务器：
```bash
npm run dev
```

5. 访问应用：
在浏览器中打开 `http://localhost:3000`

## 架构说明

- **前端**: HTML5, CSS3, JavaScript (ES2020)，支持语法高亮和虚拟滚动
- **后端**: Node.js + Express.js，提供 RESTful API 接口
- **开发语言**: TypeScript
- **构建工具**: TypeScript 编译器 (tsc)

## 许可证

MIT License