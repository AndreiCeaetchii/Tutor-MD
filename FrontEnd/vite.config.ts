import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [vue()],
  server: {
    host: true,
    port: 5173,
    strictPort: true,
    cors: true,
    hmr: {
      host: 'localhost',
      protocol: 'ws',
    },
    allowedHosts: ['.ngrok-free.app', 'localhost', '.ngrok.io'],
  },
  build: {
    minify: 'terser',
    terserOptions: ( {
      compress: {
        drop_console: true,
        drop_debugger: true,
      },
    } as any ),
  },
});
