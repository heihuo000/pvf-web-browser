// PVF文件语法高亮定义
// 基于 pvfut 项目的 PvfScriptLexer.cs 实现

import { LanguageSupport, StreamLanguage } from "@codemirror/language";
import { HighlightStyle, syntaxHighlighting } from "@codemirror/language";
import { tags } from "@lezer/highlight";

// 定义PVF语法高亮样式
let isLine330 = false; // 全局变量标记是否是330这一行

// 默认配色方案
const defaultColors = {
    labelName: "#4ec9b0",      // 标签（包括开始和结束）- 青绿色
    string: "#ce9178",          // 字符串 - 橙红色
    url: "#9cdcfe",             // 字符串链接 - 浅蓝色
    number: "#b5cea8",          // 数字 - 浅绿色
    comment: "#6a9955",         // 注释 - 绿色
    variableName: "#9cdcfe",    // 变量名 - 浅蓝色
    operator: "#d4d4d4",        // 操作符 - 浅灰色
    punctuation: "#d4d4d4",     // 标点符号 - 浅灰色
    constant: "#d7ba7d",        // 常量 - 黄色
    link: "#9cdcfe",            // 链接 - 浅蓝色
    text: "#d4d4d4"             // 默认文本 - 浅灰色
};

// 从本地配置文件加载自定义配色
async function loadCustomColors() {
    try {
        // 先尝试从服务器加载配置文件
        const response = await fetch('/api/load-colors');
        if (response.ok) {
            const colors = await response.json();
            if (Object.keys(colors).length > 0) {
                console.log('已从配置文件加载配色:', colors);
                return { ...defaultColors, ...colors };
            }
        }
    } catch (e) {
        console.warn('从服务器加载配色失败:', e);
    }

    // 如果服务器加载失败，尝试从 localStorage 加载
    try {
        const saved = localStorage.getItem('pvf-codemirror-colors');
        if (saved) {
            console.log('已从 localStorage 加载配色');
            return { ...defaultColors, ...JSON.parse(saved) };
        }
    } catch (e) {
        console.warn('从 localStorage 加载配色失败:', e);
    }

    return defaultColors;
}

// 保存自定义配色到本地配置文件
export async function saveCustomColors(colors) {
    try {
        // 保存到 localStorage 用于临时存储
        localStorage.setItem('pvf-codemirror-colors', JSON.stringify(colors));

        // 通过 API 保存到服务器本地配置文件
        const response = await fetch('/api/save-colors', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(colors)
        });

        if (!response.ok) {
            throw new Error('保存配色失败');
        }

        return await response.json();
    } catch (e) {
        console.warn('保存配色失败:', e);
        throw e;
    }
}

// 恢复默认配色
export function resetColors() {
    try {
        localStorage.removeItem('pvf-codemirror-colors');
    } catch (e) {
        console.warn('重置配色失败:', e);
    }
    return defaultColors;
}

// 获取当前配色
export async function getCurrentColors() {
    return await loadCustomColors();
}

// 重新创建高亮样式（用于配色更新后）
let currentHighlightStyle = null;
let currentColors = null;

// 使用默认颜色初始化 highlightStyle（同步）
function initDefaultHighlightStyle() {
    const style = HighlightStyle.define([
        // 标签（方括号包围的内容，如 [MOTION], [BASE ANI]）
        { tag: tags.labelName, color: defaultColors.labelName, fontWeight: "bold" },

        // 结束标签（如 [/MOTION], [/SUB ANI]）- 使用与开始标签相同的颜色
        { tag: tags.tagName, color: defaultColors.labelName, fontWeight: "bold" },

        // 字符串（反引号包围的内容，如 `Potion`）
        { tag: tags.string, color: defaultColors.string },

        // 字符串链接（尖括号包围的内容，如 <.str对话>）
        { tag: tags.url, color: defaultColors.url },

        // 数字（如 0, 100, 390）
        { tag: tags.number, color: defaultColors.number },

        // 注释（// 开头 或 # 开头）
        { tag: tags.comment, color: defaultColors.comment, fontStyle: "italic" },

        // 默认文本
        { tag: tags.standard(tags.variableName), color: defaultColors.text },

        // 标识符（变量名）
        { tag: tags.variableName, color: defaultColors.variableName },

        // 操作符（如 =, +, -）
        { tag: tags.operator, color: defaultColors.operator },

        // 标点符号（如 [, ], `, <, >）
        { tag: tags.punctuation, color: defaultColors.punctuation },

        // LST文件特定：ID部分（数字）
        { tag: tags.constant, color: defaultColors.constant, fontWeight: "bold" },

        // LST文件特定：路径部分（文件路径）
        { tag: tags.link, color: defaultColors.link },

        // 松鼠脚本 (.nut) 特殊样式
        { tag: tags.keyword, color: "#569cd6", fontWeight: "bold" }, // 紫蓝色，加粗（关键字如 if, for, local, return 等）
        { tag: tags.function(tags.variableName), color: "#dcdcaa" }, // 浅黄色（函数名）
        { tag: tags.propertyName, color: "#9cdcfe" }, // 浅蓝色（属性名）
        { tag: tags.bool, color: "#569cd6" }, // 紫蓝色（布尔值 true/false）
        { tag: tags.null, color: "#569cd6" }, // 紫蓝色（null）
    ]);
    currentHighlightStyle = style;
    currentColors = defaultColors;
}

// 异步创建高亮样式（用于加载自定义配色）
export async function createHighlightStyle() {
    const colors = await loadCustomColors();
    currentColors = colors;
    const style = HighlightStyle.define([
        // 标签（方括号包围的内容，如 [MOTION], [BASE ANI]）
        { tag: tags.labelName, color: colors.labelName, fontWeight: "bold" },

        // 结束标签（如 [/MOTION], [/SUB ANI]）- 使用与开始标签相同的颜色
        { tag: tags.tagName, color: colors.labelName, fontWeight: "bold" },

        // 字符串（反引号包围的内容，如 `Potion`）
        { tag: tags.string, color: colors.string },

        // 字符串链接（尖括号包围的内容，如 <.str对话>）
        { tag: tags.url, color: colors.url },

        // 数字（如 0, 100, 390）
        { tag: tags.number, color: colors.number },

        // 注释（// 开头 或 # 开头）
        { tag: tags.comment, color: colors.comment, fontStyle: "italic" },

        // 默认文本
        { tag: tags.standard(tags.variableName), color: colors.text },

        // 标识符（变量名）
        { tag: tags.variableName, color: colors.variableName },

        // 操作符（如 =, +, -）
        { tag: tags.operator, color: colors.operator },

        // 标点符号（如 [, ], `, <, >）
        { tag: tags.punctuation, color: colors.punctuation },

        // LST文件特定：ID部分（数字）
        { tag: tags.constant, color: colors.constant, fontWeight: "bold" },

        // LST文件特定：路径部分（文件路径）
        { tag: tags.link, color: colors.link },

        // 松鼠脚本 (.nut) 特殊样式
        { tag: tags.keyword, color: "#569cd6", fontWeight: "bold" }, // 紫蓝色，加粗（关键字如 if, for, local, return 等）
        { tag: tags.function(tags.variableName), color: "#dcdcaa" }, // 浅黄色（函数名）
        { tag: tags.propertyName, color: "#9cdcfe" }, // 浅蓝色（属性名）
        { tag: tags.bool, color: "#569cd6" }, // 紫蓝色（布尔值 true/false）
        { tag: tags.null, color: "#569cd6" }, // 紫蓝色（null）
    ]);
    currentHighlightStyle = style;
    return style;
}

// 初始化默认高亮样式（同步）
initDefaultHighlightStyle();

// 异步加载自定义配色（在后台执行）
createHighlightStyle().catch(err => console.warn('加载自定义配色失败:', err));

// 同步版本的 getColors，用于需要同步获取颜色的场景
export function getColorsSync() {
    return currentColors || defaultColors;
}

export const pvfHighlightStyle = currentHighlightStyle;

// 创建PVF语言支持
export const pvfLanguage = new StreamLanguage({
    name: "pvf",
    tokenTable: {
        number: tags.number,
        string: tags.string,
        url: tags.url,
        comment: tags.comment,
        variableName: tags.variableName,
        operator: tags.operator,
        punctuation: tags.punctuation,
        constant: tags.constant,
        link: tags.link,
        specialPunctuation: tags.punctuation,
        labelName: tags.labelName,
        tagName: tags.tagName
    },
    startState: () => ({
        inString: false,
        inStringLink: false,
        inStringLinkQuote: false,
        inLstFormat: false,  // 标记是否在处理lst格式
        lstPart: 0           // 0=初始, 1=ID部分, 2=tab分隔符, 3=路径部分
    }),
    token: (stream, state) => {
        // 检测是否是lst文件格式（以数字开头，后面跟tab和路径）
        if (stream.sol()) {
            const lineStart = stream.string.trim();
            if (/^\d+\t`/.test(lineStart)) {
                state.inLstFormat = true;
                state.lstPart = 0;
            } else if (lineStart.startsWith('#')) {
                stream.skipToEnd();
                return "comment";
            } else {
                state.inLstFormat = false;
                state.lstPart = 0;
            }
        }

        // 处理注释（// 开头）
        if (stream.match('//')) {
            stream.skipToEnd();
            return "comment";
        }

        // 处理文件头注释（# 开头的行）
        if (stream.sol() && stream.peek() === '#') {
            stream.skipToEnd();
            // 重置所有状态
            state.inString = false;
            state.inStringLink = false;
            state.inStringLinkQuote = false;
            state.inLstFormat = false;
            state.lstPart = 0;
            return "comment";
        }

        // 如果是lst格式，按特定格式处理
        if (state.inLstFormat) {
            // 跳过空白字符但保留它们的位置信息
            const ws = stream.match(/^\s+/);
            if (ws) {
                return null; // 空白字符不需要特殊样式
            }

            // 根据当前部分决定如何处理
            if (state.lstPart === 0) {
                // 尝试匹配数字（ID部分）
                if (stream.match(/^\d+/)) {
                    state.lstPart = 1;
                    return "number"; // 使用数字样式突出显示ID
                }
            } else if (state.lstPart === 1) {
                // 尝试匹配tab分隔符
                if (stream.match(/^\t/)) {
                    state.lstPart = 2;
                    return "punctuation"; // 分隔符样式
                }
            } else if (state.lstPart === 2) {
                // 尝试匹配路径（反引号包围）
                if (stream.peek() === '`') {
                    stream.next(); // 跳过第一个反引号
                    state.inString = true;
                    state.lstPart = 3;
                    return "string";
                }
            } else if (state.lstPart === 3 && state.inString) {
                // 处理路径字符串内容
                if (stream.peek() === '`') {
                    stream.next();
                    state.inString = false;
                    state.lstPart = 0; // 重置，准备下一行
                    return "string";
                } else {
                    stream.next();
                    return "string";
                }
            }
        }

        // 跳过空格和制表符
        const ateSpace = stream.eatSpace();
        if (ateSpace) {
            return null;
        }

        // 处理标签（方括号包围的内容）- 如 [MOTION], [/MOTION], [BASE ANI]
        if (stream.peek() === '[') {
            stream.next(); // 跳过 [

            // 检查是否是结束标签
            const isClosing = stream.peek() === '/';
            if (isClosing) {
                stream.next(); // 跳过 /
            }

            // 读取标签名直到 ]
            while (stream.peek() !== null && stream.peek() !== ']') {
                stream.next();
            }

            if (stream.peek() === ']') {
                stream.next(); // 跳过 ]
            }

            // 返回标签类型：结束标签使用 tagName，普通标签使用 labelName
            return isClosing ? "tagName" : "labelName";
        }

        // 处理字符串（反引号包围的内容）- 如 `Potion`
        // 先检查是否已经在字符串模式中
        if (state.inString) {
            if (stream.peek() === '`') {
                stream.next();
                state.inString = false;
                return "string";
            } else {
                stream.next();
                return "string";
            }
        }

        // 如果不在字符串模式中，检查是否遇到开始反引号
        if (stream.peek() === '`') {
            stream.next(); // 跳过第一个反引号
            state.inString = true;
            return "string";
        }

        // 处理字符串链接（尖括号包围的内容，可能包含反引号）- 如 <.str对话>
        if (stream.peek() === '<') {
            stream.next(); // 跳过 <
            state.inStringLink = true;
            state.inStringLinkQuote = false;
            return "specialPunctuation"; // 使用自定义token名称
        }

        if (state.inStringLink) {
            if (stream.peek() === '`') {
                stream.next();
                state.inStringLinkQuote = !state.inStringLinkQuote; // 切换状态
                return "url";
            } else if (stream.peek() === '>' && !state.inStringLinkQuote) {
                stream.next();
                state.inStringLink = false;
                return "specialPunctuation"; // 使用自定义token名称
            } else {
                stream.next();
                return "url";
            }
        }

        // 处理数字（独立的数字，包括负数、小数、十六进制）
        if (stream.match(/^-?\d+\.\d+/)) {
            return "number"; // 浮点数
        }

        if (stream.match(/^0[xX][0-9a-fA-F]+/)) {
            return "number"; // 十六进制
        }

        // 匹配整数
        const numberMatch = stream.match(/^\d+/);
        if (numberMatch) {
            // 检查下一个字符
            const nextChar = stream.peek();
            // 如果下一个字符不是数字、字母a-f、A-F或x，则这是一个纯数字
            if (!nextChar || !/[0-9a-fA-Fx]/.test(nextChar)) {
                return "number";
            } else {
                // 如果后面跟着字母，退回让标识符匹配处理
                stream.backUp(stream.current().length);
            }
        }

        // 处理标识符（字母开头的单词）
        if (stream.match(/^[a-zA-Z_][a-zA-Z0-9_]*/)) {
            return "variableName";
        }

        stream.next();
        return null;
    },
    languageData: {
        commentTokens: { line: "//" },
        indentOnInput: /^\s*[}\])]/,
        wordChars: "._-"
    }
});

// 创建语言支持
export const pvfLanguageSupport = new LanguageSupport(pvfLanguage, [
    syntaxHighlighting(pvfHighlightStyle)
]);

// 同时导出默认导出以兼容
export default pvfLanguageSupport;