# pvfUtility 项目文档

## 项目概述

pvfUtility 是一个用于处理 PVF（Pack Visual File）文件格式的 Windows 桌面应用程序。PVF 是一种游戏资源打包格式，主要用于 DNF（Dungeon & Fighter）等游戏的资源管理。

### 主要功能

- **PVF 文件管理**：打开、编辑、保存 PVF 文件
- **文件提取与导入**：从 PVF 中提取文件或将文件导入到 PVF 中
- **脚本编辑**：支持脚本文件的编辑、编译和反编译
- **二进制文件编辑**：提供十六进制编辑器
- **文件搜索**：支持按名称、代码、内容等多种方式搜索文件
- **批处理操作**：支持批量替换、批量提取等操作
- **HTTP 服务**：提供 REST API 接口供第三方工具调用
- **暗黑模式**：支持深色主题
- **多语言支持**：支持简体中文、繁体中文、韩文、日文等多种编码

### 技术栈

- **框架**：.NET Framework 4.6.1
- **语言**：C#
- **UI 框架**：Windows Forms (WinForms)
- **IDE**：Visual Studio 2019+
- **第三方库**：
  - SevenZipSharp：7-Zip 压缩支持
  - Aga.Controls：树形视图控件
  - Be.HexEditor：十六进制编辑器
  - ScintillaNET：代码编辑器控件
  - WinFormsUI：可停靠面板（DockPanel）
  - WindowsAPICodePack：Windows API 封装
  - AutoUpdater.NET：自动更新功能

## 项目结构

```
pvfut源码/
├── pvfUtility/           # 主应用程序项目
│   ├── Action/          # 核心业务逻辑
│   │   ├── Batch/       # 批处理功能
│   │   ├── Extract/     # 文件提取
│   │   ├── Import/      # 文件导入
│   │   └── Search/      # 文件搜索
│   ├── Dialog/          # 对话框
│   ├── Dock/            # 可停靠面板
│   │   ├── CollectionExplorer/  # 文件集浏览器
│   │   ├── ErrorList/          # 错误列表
│   │   ├── FileExplorer/       # 文件资源管理器
│   │   └── Output/             # 输出窗口
│   ├── Document/        # 文档编辑器
│   │   ├── TextEditor/  # 文本编辑器
│   │   └── HexEditorFrom.cs  # 十六进制编辑器
│   ├── Helper/          # 辅助类
│   ├── Interface/       # 接口定义
│   ├── Model/           # 数据模型
│   │   └── PvfOperation/  # PVF 文件操作相关
│   ├── NpkOperation/    # NPK 文件操作
│   ├── Service/         # 服务层
│   ├── Tool/            # 工具类
│   ├── Properties/      # 项目属性
│   ├── Resources/       # 资源文件
│   ├── MainForm.cs      # 主窗体
│   ├── MainPresenter.cs # 主控制器
│   ├── Program.cs       # 程序入口
│   └── App.config       # 应用配置
├── pvfUtility.Script/   # 脚本 API 项目
│   ├── File.cs          # 文件操作 API
│   ├── Output.cs        # 输出 API
│   ├── PVF.cs           # PVF 操作 API
│   └── ScriptFile.cs    # 脚本文件 API
├── 3rd/                 # 第三方库
│   ├── 7zSupport/       # 7-Zip 支持
│   ├── Aga.Controls/    # 树形控件
│   ├── AutoUpdater.NET/ # 自动更新
│   ├── Be.HexEditor-master/  # 十六进制编辑器
│   ├── ScintillaNET-3.6.3/   # 代码编辑器
│   ├── WindowsAPICodePack/   # Windows API
│   └── WinFormsUI/      # 可停靠面板
├── packages/            # NuGet 包
├── pvfUtility.sln       # Visual Studio 解决方案文件
└── README.md            # 项目说明
```

## 核心组件

### 1. PVF 文件操作 (`Model/PvfOperation/`)

- **PvfFile.cs**：PVF 文件实体类，包含文件名、数据、校验和等信息
- **PvfPack.Core.cs**：PVF 打包核心功能
- **PvfPack.FileOperation.cs**：PVF 文件操作
- **PvfPack.ScriptFileUtility.cs**：脚本文件工具
- **Stringtable.cs**：字符串表处理
- **ListFileTable.cs**：列表文件表处理
- **Encoder/**：编译器（脚本文件、二进制动画文件）
- **Praser/**：解析器（脚本文件解析）

### 2. 主界面 (`MainForm.cs`)

- 使用 DockPanel 实现可停靠界面
- 支持多主题（浅色、深色、蓝色）
- 集成文件资源管理器、文件集浏览器、错误列表、输出窗口等面板

### 3. 服务层 (`Service/`)

- **PackService**：PVF 打包服务
- **HttpRestServerService**：HTTP REST 服务
- **DialogService**：对话框服务
- **Logger**：日志记录
- **NavigationServices**：导航服务

### 4. 编辑器

- **TextEditor**：基于 ScintillaNET 的代码编辑器，支持语法高亮、代码折叠等功能
- **HexEditor**：基于 Be.HexEditor 的十六进制编辑器

## 构建和运行

### 环境要求

- Visual Studio 2019 或更高版本
- .NET Framework 4.8（推荐）或 4.6.1+
- Windows 操作系统

### 构建步骤

1. 使用 Visual Studio 打开 `pvfUtility.sln`
2. 选择配置（Debug 或 Release）
3. 选择平台（Any CPU 或 x64）
4. 点击"生成解决方案"或按 F6

### 命令行构建

```bash
# 使用 MSBuild 构建
msbuild pvfUtility.sln /p:Configuration=Release /p:Platform="Any CPU"
```

### 运行

构建完成后，可执行文件位于：
- Debug 模式：`pvfUtility/bin/Debug/pvfUtility.exe`
- Release 模式：`pvfUtility/bin/Release/pvfUtility.exe`

## 开发约定

### 代码风格

- 使用 C# 命名约定：
  - 类名：PascalCase（如 `PvfFile`）
  - 方法名：PascalCase（如 `GetFileName`）
  - 属性名：PascalCase（如 `FileName`）
  - 局部变量：camelCase（如 `fileName`）
  - 私有字段：camelCase 或 _camelCase

### 文件组织

- 每个类通常对应一个文件
- 文件名与类名保持一致
- 相关功能放在同一目录下

### 架构模式

- **MVP 模式**：使用 Presenter 模式分离视图和业务逻辑
- **服务层**：公共服务集中在 Service 目录
- **接口抽象**：关键接口定义在 Interface 目录

### 配置管理

- 应用配置存储在 `Setting.bin` 文件中（经 GZip 压缩）
- 配置类：`Model/Config.cs`
- 支持暗黑模式、编辑器设置、编码设置等多种配置项

### 日志记录

- 使用 `Service/Logger.cs` 进行日志记录
- 错误日志保存在 `log/` 目录下
- 文件名格式：`_error-yy-MM-dd.log`

### 编码支持

项目支持多种编码类型（`Model/PvfOperation/Enum.cs`）：
- TW（繁体中文，Code Page 950）
- CN（简体中文，Code Page 936）
- KR（韩文，Code Page 949）
- JP（日文，Code Page 932）
- UTF8（UTF-8，Code Page 65001）
- Unicode（Unicode，Code Page 1200）

## 关键功能说明

### 1. 文件搜索

支持多种搜索方式：
- 按文件名搜索
- 按代码搜索
- 按内容搜索
- 支持正则表达式

### 2. 脚本编辑

- 支持脚本文件的编辑、编译和反编译
- 提供语法高亮和代码折叠
- 支持快捷键操作（F3 搜索、F4 重命名、Ctrl+E 获取代码等）

### 3. 批处理

- 支持批量替换
- 支持批量提取
- 支持批量导入

### 4. HTTP 服务

- 提供 REST API 接口
- 支持第三方工具调用
- 便于扩展自动化功能

## 注意事项

1. **.NET Framework 版本**：项目需要 .NET Framework 4.8 才能使用全部功能
2. **长路径支持**：Windows 默认路径长度限制为 260 字符
3. **异常处理**：项目实现了全局异常捕获机制
4. **线程安全**：使用 `Control.Invoke` 和 `Control.BeginInvoke` 处理跨线程 UI 操作

## 扩展开发

### 添加新的文件类型支持

1. 在 `Model/PvfOperation/Encoder/` 中添加编译器
2. 在 `Model/PvfOperation/Praser/` 中添加解析器
3. 在 `Action/Extract/` 和 `Action/Import/` 中添加相应的提取和导入逻辑

### 添加新的编辑器

1. 在 `Document/` 目录下创建新的编辑器类
2. 实现 `IEditor` 接口
3. 在 `MainForm.cs` 中注册编辑器

### 添加新的服务

1. 在 `Service/` 目录下创建服务类
2. 使用单例模式或静态方法提供服务
3. 在需要的地方调用服务

## 版本历史

主要更新记录在 `README.md` 文件中，包括：
- 2020.2.7：恢复名称搜索，修复多个 bug
- 2020.2.0：新增脚本文件代码显示，优化搜索器
- 2020.1.x：各种 bug 修复和功能优化
- 2018 版本：基础功能实现

## 许可和贡献

项目作者 QQ：1300271842

如有问题或建议，请联系作者或提交 Issue。