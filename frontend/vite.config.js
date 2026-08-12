import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import { tanstackRouter } from '@tanstack/router-plugin/vite'
import tailwindcss from '@tailwindcss/vite'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    tailwindcss(),
    tanstackRouter({
      target: 'react',
      autoCodeSplitting: true,
    }),
    react(),
  ],
  server: {
    host: true, // Listen on all addresses (0.0.0.0)
    port: 3000,
    watch: {
      usePolling: true, // Enable polling for Docker file system
    },
    hmr: {
      host: 'localhost', // Or your Docker host IP
    }
  }
})