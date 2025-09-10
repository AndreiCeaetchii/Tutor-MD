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
      }
    }
  });
</script>

<template>
  <div>
    <NavigationBar />

    <div class="p-4">
      <h1 class="text-2xl font-bold mb-4">Student Dashboard</h1>
      <p>
        Welcome to your dashboard! Here you can manage your tutoring sessions, view teacher
        progress, and update your profile.
      </p>

      <div class="mt-6">
        <h2 class="text-xl font-semibold mb-3">Your Upcoming Sessions</h2>
      </div>

      <div class="mt-6">
        <h2 class="text-xl font-semibold mb-3">Your Tutors</h2>
      </div>
    </div>
  </div>
</template>
