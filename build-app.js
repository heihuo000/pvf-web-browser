#!/usr/bin/env node

const esbuild = require('esbuild');

console.log('Building PVF Web Browser for CodeMirror 6...');

// 使用 ESM 格式构建
esbuild.build({
    entryPoints: ['public/js/main.js'],
    bundle: true,
    outfile: 'public/js/main-bundle.js',
    format: 'esm',
    platform: 'browser',
    external: [],
    minify: false,
    sourcemap: true,
    treeShaking: false,
    banner: {
        js: '/* PVF Web Browser - Built with CodeMirror 6 */'
    }
}).then(() => {
    console.log('✓ ESM bundle created: public/js/main-bundle.js');
    console.log('Note: To use ESM in browser, add this to HTML:');
    console.log('<script type="module" src="js/main-bundle.js"></script>');
}).catch((error) => {
    console.error('Build failed:', error);
    process.exit(1);
});