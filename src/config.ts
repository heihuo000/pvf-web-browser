// 配置管理（替代 VS Code 配置）
export interface PvfConfig {
  encodingMode: 'AUTO' | 'KR' | 'TW' | 'CN' | 'JP' | 'UTF8';
  npkRoot: string;
  showScriptDisplayName: boolean;
  showScriptCode: boolean;
}

let config: PvfConfig = {
  encodingMode: 'TW',  // 默认使用台湾编码 (cp950)
  npkRoot: '',
  showScriptDisplayName: true,
  showScriptCode: true
};

export function setConfig(newConfig: Partial<PvfConfig>) {
  config = { ...config, ...newConfig };
}

export function getConfig(): PvfConfig {
  return config;
}

export function getEncodingMode(): string {
  return config.encodingMode;
}

export function getNpkRoot(): string {
  return config.npkRoot;
}