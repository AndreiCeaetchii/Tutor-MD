import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [vue()],
  server: {
    host: true, // echivalent cu 0.0.0.0
    port: 5173,
    strictPort: true,
    cors: true,
    hmr: {
      clientPort: 443,
      host: '*.ngrok-free.app',
    },
    
    allowedHosts: [
      '.ngrok-free.app',
      '.ngrok.io',
      'localhost',
    ],
  },
});
