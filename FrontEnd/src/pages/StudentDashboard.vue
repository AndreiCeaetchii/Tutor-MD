<script setup lang="ts">
  import { onMounted } from 'vue';
  import { useRouter } from 'vue-router';
  import NavigationBar from '../components/navigation/NavigationBar.vue';
  import { useUserStore } from '../store/userStore';

  const store = useUserStore();
  const router = useRouter();

  onMounted(() => {
    if (!store.isAuthenticated) {
      router.push('/login');
      return;
    }

    if (store.userRole !== 'student') {
      if (store.userRole === 'tutor') {
        router.push('/tutor-dashboard');
      } else if (store.userRole === 'admin') {
        router.push('/admin-dashboard');
      } else {
        router.push('/landing');
      }
    }
  });
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <NavigationBar />

    <div class="p-4 mx-auto md:p-8 max-w-7xl">
      <header class="mb-6">
        <h1 class="text-2xl font-bold mb-2">Student Dashboard</h1>
        <p class="text-gray-600">
          Welcome! Use the navigation bar to find tutors, check bookings, leave reviews, or manage
          your account.
        </p>
      </header>
      <router-view />
    </div>
  </div>
</template>
