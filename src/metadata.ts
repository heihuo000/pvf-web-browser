import { PvfModel } from './model';
import * as path from 'path';
import * as fs from 'fs/promises';
import { getEncodingMode } from './config.js';

interface ScriptMetadata {
  name?: string;
  icon?: { img: string; frame: number };
  code?: number;
}

function parseScriptMetadata(content: string): ScriptMetadata {
  const meta: ScriptMetadata = {};
  const nameMatch = content.match(/\[name\]\s*([^\r\n]+)/i);
  if (nameMatch) meta.name = nameMatch[1].trim();

  const iconMatch = content.match(/\[icon\]\s*([^\r\n]+)\s*,\s*(\d+)/i);
  if (iconMatch) {
    meta.icon = { img: iconMatch[1].trim(), frame: parseInt(iconMatch[2], 10) };
  }

  const codeMatch = content.match(/\[code\]\s*(\d+)/i);
  if (codeMatch) meta.code = parseInt(codeMatch[1], 10);

  return meta;
}

function getExcludeList(): string[] {
  const excludeDefault = '.nut,.lst,.ani,.ani.als,.als,.ui,.png,.jpg,.jpeg,.dds,.bmp,.tga,.gif,.wav,.ogg,.mp3,.bin';
  const excludeList = excludeDefault.split(/[;,:\n\r\t ]+/).map(s=>s.trim().toLowerCase()).filter(Boolean);
  return excludeList.map(e => e.startsWith('.') ? e : '.'+e);
}

function shouldExclude(key: string, excludes: string[]): boolean {
  const lower = key.toLowerCase();
  for (const ext of excludes) if (lower.endsWith(ext)) return true;
  return false;
}

async function parseOne(model: PvfModel, key: string, excludes: string[], scanned: Set<string>) {
  if (scanned.has(key)) return;
  scanned.add(key);
  if (shouldExclude(key, excludes)) { return; }
  try {
    const bytes = await model.readFileBytes(key);
    if (!bytes || bytes.length === 0) { return; }
    let content = Buffer.from(bytes).toString('utf8');
    if (content.charCodeAt(0) === 0xFEFF) content = content.slice(1);
    if (!/\[(name|code)\]/i.test(content)) { return; }
    const meta = parseScriptMetadata(content);
    if (meta.name) (model as any).setDisplayName?.(key, meta.name);
  } catch { /* ignore */ }
}

/**
 * 为模型构建脚本文件的元数据映射（仅解析含 [name] 的文件）
 */
export async function parseMetadataForKeys(model: PvfModel, keys: string[]): Promise<void> {
  if (!model) return;
  try {
    const excludes = getExcludeList();
    const scanned = new Set<string>();
    for (const key of keys) {
      await parseOne(model, key, excludes, scanned);
    }
  } catch { /* ignore */ }
}

export async function buildMetadataMaps(model: PvfModel): Promise<void> {
  if (!model) return;
  const allKeys = model.getAllKeys();
  await parseMetadataForKeys(model, allKeys);
}