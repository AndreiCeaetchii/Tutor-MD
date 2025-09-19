<script setup lang="ts">
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import { useUserStore } from '../store/userStore.ts';
  import NavigationBar from '../components/navigation/NavigationBar.vue';
  import Footer from '../components/Footer.vue';

  const store = useUserStore();
  const router = useRouter();
  const activeTab = ref('calendar');

  const handleTabChange = (tab: string) => {
    activeTab.value = tab;
  };
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="p-4 mx-auto md:p-8 max-w-7xl">
      <div class="mb-6">
        <h1 class="mb-2 text-2xl font-bold">Student Dashboard</h1>
        <p class="text-gray-600">
          Welcome to your dashboard! Here you can manage your sessions, view progress, and update
          your profile.
        </p>
      </div>

      <NavigationBar @tabChange="handleTabChange" />

      <div class="mt-8">
        <router-view />
      </div>
    </div>
    <Footer />
  </div>
</template>
