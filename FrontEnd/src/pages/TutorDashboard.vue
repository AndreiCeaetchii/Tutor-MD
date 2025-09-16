<script setup lang="ts">
  import { onMounted } from 'vue';
  import { useRouter } from 'vue-router';
  import { useUserStore } from '../store/userStore.ts';
  import NavigationBar from '../components/navigation/NavigationBar.vue';
  import Footer from '../components/Footer.vue';

  const store = useUserStore();
  const router = useRouter();

  // Verificăm autentificarea și rolul
  onMounted(() => {
    if (!store.isAuthenticated) {
      router.push('/login');
    } else if (store.userRole !== 'tutor') {
      router.push(store.userRole === 'student' ? '/student-dashboard' : '/landing');
    }
  });
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="p-4 mx-auto max-w-7xl">
      <h1 class="text-2xl font-bold mb-6">Tutor Dashboard</h1>
      <NavigationBar />

      <div class="mt-8">
        <router-view />
      </div>
    </div>
    <Footer />
  </div>
</template>
