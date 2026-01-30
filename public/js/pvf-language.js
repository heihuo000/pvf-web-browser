// PVF文件语法高亮定义
// 基于 pvfut 项目的 PvfScriptLexer.cs 实现

import { LanguageSupport, StreamLanguage } from "@codemirror/language";
import { HighlightStyle, syntaxHighlighting } from "@codemirror/language";
import { tags } from "@lezer/highlight";

// 定义PVF语法高亮样式
let isLine330 = false; // 全局变量标记是否是330这一行

export const pvfHighlightStyle = HighlightStyle.define([
    // 标签（方括号包围的内容，如 [MOTION], [BASE ANI]）
    { tag: tags.labelName, color: "#4ec9b0", fontWeight: "bold" }, // 青绿色，加粗

    // 结束标签（如 [/MOTION], [/SUB ANI]）- 使用同样的 labelName 标签，但在 token 返回时会特殊处理
    { tag: tags.tagName, color: "#ce9178", fontWeight: "bold" }, // 橙红色，加粗

    // 字符串（反引号包围的内容，如 `Potion`）
    { tag: tags.string, color: "#ce9178" }, // 橙红色

    // 字符串链接（尖括号包围的内容，如 <.str对话>）
    { tag: tags.url, color: "#9cdcfe" }, // 浅蓝色

    // 数字（如 0, 100, 390）
    { tag: tags.number, color: "#b5cea8" }, // 浅绿色

    // 注释（// 开头 或 # 开头）
    { tag: tags.comment, color: "#6a9955", fontStyle: "italic" }, // 绿色，斜体

    // 默认文本
    { tag: tags.standard(tags.variableName), color: "#d4d4d4" }, // 浅灰色

    // 标识符（变量名）
    { tag: tags.variableName, color: "#9cdcfe" }, // 浅蓝色

    // 操作符（如 =, +, -）
    { tag: tags.operator, color: "#d4d4d4" }, // 浅灰色

    // 标点符号（如 [, ], `, <, >）
    { tag: tags.punctuation, color: "#d4d4d4" }, // 浅灰色

    // LST文件特定：ID部分（数字）
    { tag: tags.constant, color: "#d7ba7d", fontWeight: "bold" }, // 黄色，加粗

    // LST文件特定：路径部分（文件路径）
    { tag: tags.link, color: "#9cdcfe" }, // 浅蓝色
]);

// 创建PVF语言支持
export const pvfLanguage = new StreamLanguage({
    name: "pvf",
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
            } else if (lineStart.startsWith('#PVF_File')) {
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
        if (stream.peek() === '`') {
            stream.next(); // 跳过第一个反引号
            state.inString = true;
            return "string";
        }

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