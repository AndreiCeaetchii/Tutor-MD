<script setup lang="ts">
import Header from './components/Header.vue';
import { onMounted } from 'vue';
import { useUserStore } from './store/userStore';
import { fetchCsrfToken } from './services/csrfService';

const userStore = useUserStore();

onMounted(async () => {
  if (userStore.isAuthenticated) {
    await fetchCsrfToken();
  }
});
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <Header userType="tutor" />
    <router-view />
  </div>
</template>

<style scoped>
</style>