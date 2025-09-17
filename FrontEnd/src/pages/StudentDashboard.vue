<script setup lang="ts">
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';
import NavigationBar from '../components/navigation/NavigationBar.vue';
import { useUserStore } from '../store/userStore';
import SearchBar from '../components/student/FindTutor/SearchBar.vue';
import TutorFilters from '../components/student/FindTutor/TutorFilters.vue';
import TutorList from '../components/student/FindTutor/TutorList.vue';

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

      <div class="w-full mt-8 mb-6">
        <SearchBar />
      </div>

      <div class="flex flex-col gap-8 md:flex-row">
        <div class="w-full md:w-1/4">
          <TutorFilters />
        </div>
        <div class="w-full md:w-3/4">
          <TutorList />
        </div>
      </div>
    </div>
  </div>
</template>
