import express from 'express';
import { PvfModel } from './model';
import { setConfig } from './config';
import * as path from 'path';
import * as fs from 'fs/promises';
import * as iconv from 'iconv-lite';

// 带时间戳的日志输出函数
function log(message: string) {
  const now = new Date();
  const timestamp = now.toLocaleString('zh-CN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    hour12: false
  });
  console.log(`[${timestamp}] ${message}`);
}

// 编码映射：将简写转换为iconv-lite支持的完整编码名称
function mapEncoding(encoding: string): string {
  const encodingMap: { [key: string]: string } = {
    'AUTO': 'utf8',
    'TW': 'cp950',      // 繁体中文
    'CN': 'gb18030',    // 简体中文
    'KR': 'euc-kr',     // 韩文
    'JP': 'shift_jis',  // 日文
    'UTF8': 'utf8'
  };
  return encodingMap[encoding] || encoding.toLowerCase();
}

const app = express();
const PORT = 3000;

app.use(express.json());
app.use(express.static('public'));

let currentModel: PvfModel | null = null;

// 自动扫描并打开项目目录下的 PVF 文件
async function autoOpenPvfFile() {
  try {
    const projectDir = process.cwd();
    const files = await fs.readdir(projectDir);
    const pvfFiles = files.filter(f => f.toLowerCase().endsWith('.pvf'));
    
    if (pvfFiles.length > 0) {
      const pvfPath = path.join(projectDir, pvfFiles[0]);
      log(`发现 PVF 文件: ${pvfPath}`);
      
      const model = new PvfModel();
      await model.open(pvfPath, (n) => {
        log(`自动加载进度: ${n}%`);
      });
      
      currentModel = model;
      log(`✓ 已自动打开 PVF 文件: ${pvfFiles[0]}`);
      return pvfFiles[0];
    }
  } catch (error) {
    console.error('自动打开 PVF 文件失败:', error);
  }
  return null;
}

// 打开 PVF 文件
app.post('/api/open', async (req, res) => {
  try {
    const { filePath, encodingMode } = req.body;
    
    if (!filePath) {
      return res.status(400).json({ error: '文件路径不能为空' });
    }

    // 设置编码模式
    if (encodingMode) {
      setConfig({ encodingMode });
    }

    const model = new PvfModel();
    await model.open(filePath, (n) => {
      log(`加载进度: ${n}%`);
    });

    currentModel = model;
    res.json({ success: true, message: 'PVF 文件已打开' });
  } catch (error) {
    console.error('打开文件失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 获取项目目录下的所有 PVF 文件
app.get('/api/pvf-files', async (req, res) => {
  try {
    const projectDir = process.cwd();
    const files = await fs.readdir(projectDir);
    const pvfFiles = files
      .filter(f => f.toLowerCase().endsWith('.pvf'))
      .map(f => ({
        name: f,
        path: path.join(projectDir, f)
      }));
    
    res.json({ files: pvfFiles });
  } catch (error) {
    console.error('获取 PVF 文件列表失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 获取文件列表
app.get('/api/files', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const path = req.query.path as string || '';
    const children = currentModel.getChildren(path);

    res.json({ files: children });
  } catch (error) {
    console.error('获取文件列表失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 获取文件夹中的所有文件（递归）
app.get('/api/files-in-folder', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const folderPath = req.query.path as string;
    if (!folderPath) {
      return res.status(400).json({ error: '文件夹路径不能为空' });
    }

    const files = currentModel.getAllFilesInFolder(folderPath);
    res.json({ files });
  } catch (error) {
    console.error('获取文件夹文件失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 获取文件内容
app.get('/api/file', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const key = req.query.key as string;
    if (!key) {
      return res.status(400).json({ error: '文件 key 不能为空' });
    }

    // 转换为小写以实现大小写不敏感访问
    const lowerKey = key.toLowerCase();

    const bytes = await currentModel.readFileBytes(lowerKey);
    const content = Buffer.from(bytes).toString('utf8');
    
    res.json({ content, size: bytes.length });
  } catch (error) {
    console.error('获取文件内容失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 提取文件
app.post('/api/extract', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const { key, destPath } = req.body;
    if (!key || !destPath) {
      return res.status(400).json({ error: '参数不完整' });
    }

    await currentModel.exportFile(key, destPath);
    res.json({ success: true, message: '文件已提取' });
  } catch (error) {
    console.error('提取文件失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 高级搜索
app.get('/api/advanced-search', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const type = req.query.type as string || 'filename';
    const keyword = req.query.keyword as string;
    const startMatch = req.query.startMatch === 'true';
    const useRegex = req.query.useRegex === 'true';
    const caseSensitive = req.query.caseSensitive === 'true';
    const offset = parseInt(req.query.offset as string) || 0;
    const limit = parseInt(req.query.limit as string) || 500;

    if (!keyword) {
      return res.status(400).json({ error: '搜索关键词不能为空' });
    }

    const allKeys = currentModel.getAllKeys();
    const allResults: Array<{ key: string; name: string }> = [];
    
    // 对于字符串搜索，限制搜索的文件数量以提高性能
    const MAX_FILES_FOR_STRING_SEARCH = 200; // 降低到200个文件以提高性能
    const MAX_STRING_RESULTS = 50; // 最多返回50个结果
    let keysToSearch = allKeys;

    if (type === 'string') {
      const scriptExtensions = ['.act', '.ani', '.skl', '.lst', '.str', '.equ', '.stk', '.ai', '.aic', '.key', '.nut', '.als'];
      const scriptFiles = allKeys.filter(key => scriptExtensions.some(ext => key.endsWith(ext)));
      keysToSearch = scriptFiles.slice(0, MAX_FILES_FOR_STRING_SEARCH);
      log(`字符串搜索：限制搜索 ${keysToSearch.length} 个脚本文件（总共 ${scriptFiles.length} 个）`);
    }

    for (const key of keysToSearch) {
      // 对于字符串搜索，限制最大结果数以提高性能
      if (type === 'string' && allResults.length >= MAX_STRING_RESULTS) {
        log(`已达到字符串搜索结果数量上限 ${MAX_STRING_RESULTS}，停止搜索`);
        break;
      }
      const fileName = key.split('/').pop() || key;
      let match = false;

      switch (type) {
        case 'filename':
          if (useRegex) {
            try {
              const regex = new RegExp(keyword, caseSensitive ? '' : 'i');
              match = regex.test(fileName);
            } catch {
              match = false;
            }
          } else {
            const searchKey = caseSensitive ? fileName : fileName.toLowerCase();
            const searchKeyword = caseSensitive ? keyword : keyword.toLowerCase();
            if (startMatch) {
              match = searchKey.startsWith(searchKeyword);
            } else {
              match = searchKey.includes(searchKeyword);
            }
          }
          break;

        case 'name':
          const displayName = currentModel.getDisplayNameForFile(key) || fileName;
          if (useRegex) {
            try {
              const regex = new RegExp(keyword, caseSensitive ? '' : 'i');
              match = regex.test(displayName);
            } catch {
              match = false;
            }
          } else {
            const searchKey = caseSensitive ? displayName : displayName.toLowerCase();
            const searchKeyword = caseSensitive ? keyword : keyword.toLowerCase();
            if (startMatch) {
              match = searchKey.startsWith(searchKeyword);
            } else {
              match = searchKey.includes(searchKeyword);
            }
          }
          break;

        case 'number':
          const num = parseInt(keyword, 10);
          if (!isNaN(num)) {
            // 简化版：只检查文件名是否包含数字
            match = fileName.includes(keyword);
          }
          break;

        case 'string':
          // 字符串搜索：先在StringTable中搜索，然后检查哪些文件使用了这些字符串索引
          try {
            const stringTable = (currentModel as any).strtable;
            if (!stringTable) {
              match = false;
              break;
            }
            
            // 在StringTable中搜索匹配的字符串，获取索引列表
            const matchedIndices: number[] = [];
            const stringCount = stringTable.getCount();
            
            for (let i = 0; i < stringCount; i++) {
              const str = stringTable.get(i);
              if (!str || str.startsWith('#{')) continue; // 跳过无效或占位符
              
              let stringMatch = false;
              if (useRegex) {
                try {
                  const regex = new RegExp(keyword, caseSensitive ? '' : 'i');
                  stringMatch = regex.test(str);
                } catch {
                  stringMatch = false;
                }
              } else {
                const searchString = caseSensitive ? str : str.toLowerCase();
                const searchKeyword = caseSensitive ? keyword : keyword.toLowerCase();
                if (startMatch) {
                  stringMatch = searchString.startsWith(searchKeyword);
                } else {
                  stringMatch = searchString.includes(searchKeyword);
                }
              }
              
              if (stringMatch) {
                matchedIndices.push(i);
              }
            }
            
            // 如果没有找到匹配的字符串，跳过
            if (matchedIndices.length === 0) {
              match = false;
              break;
            }
            
            // 读取文件内容，检查是否使用了这些字符串索引
            const bytes = await currentModel.readFileBytes(key);
            const data = new DataView(bytes.buffer, bytes.byteOffset, bytes.byteLength);
            const dataLen = bytes.length;
            
            // 优化：使用Set进行快速查找
            const matchedIndicesSet = new Set(matchedIndices);
            
            // 按照pvfut的逻辑搜索：每5个字节一个指令
            for (let i = 2; i < dataLen - 4; i += 5) {
              const opcode = data.getUint8(i);
              
              // 检查是否引用了字符串（opcode 5, 7, 10）
              if (opcode === 5 || opcode === 7 || opcode === 10) {
                const index = data.getInt32(i + 1, true); // little-endian
                if (matchedIndicesSet.has(index)) {
                  match = true;
                  break;
                }
              }
              
              // 特殊情况检查（opcode 10且前一个opcode是9）
              if (i > 4 && opcode === 10 && data.getUint8(i - 5) === 9) {
                const index = data.getUint8(i - 4) * 0x1000000 + data.getInt32(i + 1, true);
                if (matchedIndicesSet.has(index)) {
                  match = true;
                  break;
                }
              }
            }
          } catch (err) {
            console.error('字符串搜索错误:', err);
            match = false;
          }
          break;

        case 'content':
          try {
            const bytes = await currentModel.readFileBytes(key);
            const content = Buffer.from(bytes).toString('utf8');
            if (useRegex) {
              try {
                const regex = new RegExp(keyword, caseSensitive ? '' : 'i');
                match = regex.test(content);
              } catch {
                match = false;
              }
            } else {
              const searchContent = caseSensitive ? content : content.toLowerCase();
              const searchKeyword = caseSensitive ? keyword : keyword.toLowerCase();
              match = searchContent.includes(searchKeyword);
            }
          } catch {
            match = false;
          }
          break;
      }

      if (match) {
        allResults.push({ key, name: fileName });
      }
    }

    // 应用分页
    const paginatedResults = allResults.slice(offset, offset + limit);
    const hasMore = offset + limit < allResults.length;

    res.json({ 
      results: paginatedResults, 
      total: allResults.length,
      hasMore: hasMore
    });
  } catch (error) {
    console.error('搜索失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 搜索文件
app.get('/api/search', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const query = req.query.q as string;
    if (!query) {
      return res.status(400).json({ error: '搜索关键词不能为空' });
    }

    const allKeys = currentModel.getAllKeys();
    const results = allKeys.filter(key =>
      key.toLowerCase().includes(query.toLowerCase())
    );

    res.json({ results });
  } catch (error) {
    console.error('搜索失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 获取书签列表
app.get('/api/bookmarks', async (req, res) => {
  try {
    const bookmarksPath = path.join(__dirname, '../bookmarks.json');
    const data = await fs.readFile(bookmarksPath, 'utf8');
    const bookmarks = JSON.parse(data);
    res.json({ bookmarks });
  } catch (error) {
    console.error('获取书签失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 保存书签列表
app.post('/api/bookmarks', async (req, res) => {
  try {
    const { bookmarks } = req.body;
    if (!Array.isArray(bookmarks)) {
      return res.status(400).json({ error: '书签必须是数组' });
    }

    const bookmarksPath = path.join(__dirname, '../bookmarks.json');
    await fs.writeFile(bookmarksPath, JSON.stringify(bookmarks, null, 2), 'utf8');
    res.json({ success: true, message: '书签已保存' });
  } catch (error) {
    console.error('保存书签失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 保存单个文件到 PVF
app.post('/api/save-file', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const { key, content, encoding } = req.body;
    if (!key || content === undefined) {
      return res.status(400).json({ error: '参数不完整' });
    }

    // 将内容转换为字节数组
    const encodingToUse = mapEncoding(encoding || 'utf8');
    const bytes = iconv.encode(content, encodingToUse);

    // 保存文件到PVF
    await currentModel.updateFile(key, bytes);

    res.json({ success: true, message: '文件已保存到 PVF' });
  } catch (error) {
    console.error('保存文件失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 批量提取文件
app.post('/api/batch-extract', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const { keys, destPath } = req.body;
    if (!keys || !Array.isArray(keys) || keys.length === 0) {
      return res.status(400).json({ error: '请选择要提取的文件' });
    }
    if (!destPath) {
      return res.status(400).json({ error: '目标路径不能为空' });
    }

    const results = [];
    for (const key of keys) {
      try {
        const fileDestPath = path.join(destPath, key.replace(/\//g, path.sep));
        const fileDir = path.dirname(fileDestPath);

        // 确保目录存在
        await fs.mkdir(fileDir, { recursive: true });

        await currentModel.exportFile(key, fileDestPath);
        results.push({ key, success: true });
      } catch (error) {
        console.error(`提取文件失败 ${key}:`, error);
        results.push({ key, success: false, error: String(error) });
      }
    }

    res.json({ success: true, results, total: keys.length, successCount: results.filter(r => r.success).length });
  } catch (error) {
    console.error('批量提取失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 批量提取文件并打包成ZIP
app.post('/api/batch-extract-zip', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const { keys } = req.body;
    if (!keys || !Array.isArray(keys) || keys.length === 0) {
      return res.status(400).json({ error: '请选择要提取的文件' });
    }

    const archiver = require('archiver');
    const archive = archiver('zip', { zlib: { level: 9 } });

    // 设置响应头
    res.attachment('extracted_files.zip');
    res.setHeader('Content-Type', 'application/zip');

    // 捕获错误
    archive.on('error', (err: any) => {
      console.error('ZIP打包错误:', err);
      res.status(500).json({ error: 'ZIP打包失败: ' + err.message });
    });

    // 将ZIP流输出到响应
    archive.pipe(res);

    // 添加所有文件到ZIP
    for (const key of keys) {
      try {
        const bytes = await currentModel.readFileBytes(key);
        archive.append(Buffer.from(bytes), { name: key });
      } catch (error) {
        console.error(`添加文件到ZIP失败 ${key}:`, error);
      }
    }

    // 完成打包
    archive.finalize();
  } catch (error) {
    console.error('批量提取ZIP失败:', error);
    if (!res.headersSent) {
      res.status(500).json({ error: String(error) });
    }
  }
});

// 保存 PVF 文件
app.post('/api/save', async (req, res) => {
  try {
    if (!currentModel) {
      return res.status(400).json({ error: '请先打开 PVF 文件' });
    }

    const { filePath } = req.body;
    if (!filePath) {
      return res.status(400).json({ error: '保存路径不能为空' });
    }

    await currentModel.save(filePath, (n) => {
      log(`保存进度: ${n}%`);
    });

    res.json({ success: true, message: 'PVF 文件已保存' });
  } catch (error) {
    console.error('保存文件失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 更新配置
app.post('/api/config', (req, res) => {
  try {
    const { encodingMode, npkRoot } = req.body;
    setConfig({ encodingMode, npkRoot });
    res.json({ success: true });
  } catch (error) {
    console.error('更新配置失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 获取标签描述
app.get('/api/tag-descriptions/:extension', async (req, res) => {
  try {
    const extension = req.params.extension.toLowerCase();
    const path = require('path');
    const fs = require('fs').promises;

    const sampleDir = path.join(process.cwd(), 'sample(官方注释)', 'tag_descriptions');
    const tagFile = path.join(sampleDir, `${extension}.txt`);

    try {
      const content = await fs.readFile(tagFile, 'utf8');
      const tagDescriptions: { [key: string]: string } = {};

      const lines = content.split('\n');
      let currentTag = '';
      let currentDescription = '';

      for (const line of lines) {
        const trimmedLine = line.trim();

        // 跳过注释行（以 ## 开头）
        if (trimmedLine.startsWith('##')) {
          continue;
        }

        if (trimmedLine.startsWith('[') && trimmedLine.includes(']')) {
          // 保存上一个标签
          if (currentTag && currentDescription) {
            tagDescriptions[currentTag] = currentDescription;
          }

          // 开始新标签
          const tagMatch = trimmedLine.match(/^\[([^\]]+)\]\s*-\s*(.+)$/);
          if (tagMatch) {
            currentTag = `[${tagMatch[1]}]`;
            currentDescription = tagMatch[2];
          } else {
            currentTag = trimmedLine;
            currentDescription = '';
          }
        } else if (currentTag && trimmedLine) {
          // 添加描述文本
          if (currentDescription) {
            currentDescription += ' ' + trimmedLine;
          } else {
            currentDescription = trimmedLine;
          }
        }
      }

      // 保存最后一个标签
      if (currentTag && currentDescription) {
        tagDescriptions[currentTag] = currentDescription;
      }

      res.json(tagDescriptions);
    } catch (fileError) {
      // 文件不存在，返回空对象
      res.json({});
    }
  } catch (error) {
    console.error('获取标签描述失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 获取值描述（职业值、装备类型等）
app.get('/api/value-descriptions/:file', async (req, res) => {
  try {
    const file = req.params.file;
    const path = require('path');
    const fs = require('fs').promises;

    const sampleDir = path.join(process.cwd(), 'sample(官方注释)', 'tag_descriptions');
    const valueFile = path.join(sampleDir, `${file}.txt`);

    try {
      const content = await fs.readFile(valueFile, 'utf8');
      const valueDescriptions: { [key: string]: string } = {};

      const lines = content.split('\n');
      let currentValue = '';
      let currentDescription = '';

      for (const line of lines) {
        const trimmedLine = line.trim();

        if (trimmedLine.startsWith('[') && trimmedLine.includes(']')) {
          // 保存上一个值
          if (currentValue && currentDescription) {
            valueDescriptions[currentValue] = currentDescription;
          }

          // 开始新值
          const valueMatch = trimmedLine.match(/^\[([^\]]+)\]\s*-\s*(.+)$/);
          if (valueMatch) {
            currentValue = `[${valueMatch[1]}]`;
            currentDescription = valueMatch[2];
          } else {
            currentValue = trimmedLine;
            currentDescription = '';
          }
        } else if (currentValue && trimmedLine && !trimmedLine.startsWith('#')) {
          // 添加描述文本
          if (currentDescription) {
            currentDescription += ' ' + trimmedLine;
          } else {
            currentDescription = trimmedLine;
          }
        }
      }

      // 保存最后一个值
      if (currentValue && currentDescription) {
        valueDescriptions[currentValue] = currentDescription;
      }

      res.json(valueDescriptions);
    } catch (fileError) {
      // 文件不存在，返回空对象
      res.json({});
    }
  } catch (error) {
    console.error('获取值描述失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 获取状态
app.get('/api/status', (req, res) => {
  res.json({
    hasOpenFile: currentModel !== null,
    pvfPath: currentModel?.pvfPath || '',
    encodingMode: req.app.locals?.encodingMode || 'AUTO'
  });
});

// 保存配色方案到本地配置文件
app.post('/api/save-colors', async (req, res) => {
  try {
    const colors = req.body;
    const configPath = path.join(process.cwd(), 'color-config.json');

    await fs.writeFile(configPath, JSON.stringify(colors, null, 2), 'utf8');

    log(`✓ 配色方案已保存到: ${configPath}`);
    res.json({ success: true });
  } catch (error) {
    console.error('保存配色失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

// 加载配色方案
app.get('/api/load-colors', async (req, res) => {
  try {
    const configPath = path.join(process.cwd(), 'color-config.json');

    try {
      const content = await fs.readFile(configPath, 'utf8');
      const colors = JSON.parse(content);
      res.json(colors);
    } catch (fileError) {
      // 配置文件不存在，返回空对象
      res.json({});
    }
  } catch (error) {
    console.error('加载配色失败:', error);
    res.status(500).json({ error: String(error) });
  }
});

app.listen(PORT, '0.0.0.0', async () => {
  const os = require('os');
  const interfaces = os.networkInterfaces();
  let localIP = 'localhost';
  
  // 获取本机局域网IP
  for (const name of Object.keys(interfaces)) {
    for (const iface of interfaces[name]) {
      if (iface.family === 'IPv4' && !iface.internal) {
        localIP = iface.address;
        break;
      }
    }
    if (localIP !== 'localhost') break;
  }
  
  log(`PVF Web Browser 服务器运行在:`);
  log(`  - 本地访问: http://localhost:${PORT}`);
  log(`  - 局域网访问: http://${localIP}:${PORT}`);
  log(`项目目录: ${process.cwd()}`);
  
  // 自动打开项目目录下的 PVF 文件
  await autoOpenPvfFile();
});