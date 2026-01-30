#!/bin/bash

# PVF Web Browser Termux 运行脚本
# 此脚本在Termux环境中启动PVF Web Browser

set -e  # 出错时退出

echo "正在启动 PVF Web Browser..."

# 检查必要工具
if ! command -v node &> /dev/null; then
    echo "错误: 未找到 node，请先安装 Node.js"
    exit 1
fi

# 进入项目目录
PROJECT_DIR="/data/data/com.termux/files/home/DNF私服研究/pvf-web-browser"
cd $PROJECT_DIR

# 检查是否已构建
if [ ! -f "dist/server.js" ]; then
    echo "正在构建项目..."
    npm run build
fi

echo "正在启动服务器..."
echo "请在浏览器中访问 http://localhost:3000"
echo "按 Ctrl+C 停止服务器"

# 启动服务器
node server/index.js