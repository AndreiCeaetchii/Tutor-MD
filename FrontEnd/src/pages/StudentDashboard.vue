<script setup lang="ts">
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import { useUserStore } from '../store/userStore.ts';
  import NavigationBar from '../components/navigation/NavigationBar.vue';

  const store = useUserStore();
  const router = useRouter();
  const activeTab = ref('find');
  // const activeTab = ref('calendar');

  const handleTabChange = (tab: string) => {
    activeTab.value = tab;
  };

  onMounted(() => {
    if (!store.isAuthenticated) {
      router.push('/login');
    } else if (store.userRole !== 'student') {
      router.push(store.userRole === 'tutor' ? '/tutor-dashboard' : '/landing');
    }
  });
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="p-4 mx-auto md:p-8 max-w-7xl">
      <div class="mb-6">
        <h1 class="mb-2 text-2xl font-bold md:text-3xl">
          Welcome to your <span class="text-purple-600">Student Dashboard</span>
        </h1>
        <p class="mt-2 text-sm text-gray-600 md:text-base">
          Find the perfect <span class="font-semibold text-purple-600">tutor</span>, manage your
          <span class="font-semibold text-purple-600">learning sessions</span>, and track your
          <span class="font-semibold text-purple-600">educational progress</span> with ease.
        </p>
      </div>

      <NavigationBar @tabChange="handleTabChange" />

      <div class="mt-8">
        <router-view />
      </div>
    </div>
  </div>
</template>
