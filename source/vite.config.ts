import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import alias from '@rollup/plugin-alias'
import { resolve } from 'path'

// Đường dẫn gốc project
const projectRootDir = resolve(__dirname);

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    alias()
  ],
  resolve: {
    alias: {
      "@": resolve(projectRootDir, "src"),
    },
  },
  css: {
    preprocessorOptions: {
      scss: {
        additionalData: `@import '@/assets/styles/_variables.scss';`
      }
    }
  },
  build: {
    outDir: 'dist',
  }
})