<script setup lang="ts">
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';
import NavigationBar from '../components/navigation/NavigationBar.vue';
import { useUserStore } from '../store/userStore';
import SearchBar from '../components/student/FindTutor/SearchBar.vue';

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
  <div class="min-h-screen bg-gray-50">
    <div class="p-4 mx-auto md:p-8 max-w-7xl">
      <div class="mb-6">
        <h1 class="mb-2 text-2xl font-bold">Student Dashboard</h1>
        <p class="text-gray-600">
          Welcome to your dashboard! Here you can manage your tutoring sessions, view your progress, and update your profile.
        </p>
      </div>

      <NavigationBar />

      <div class="mt-8">
        <SearchBar />
        <!-- Aici poți adăuga TutorList, filtre, etc. -->
      </div>
    </div>
  </div>
</template>
