<script setup lang="ts">
  import { ref } from 'vue';
  import AdminNavBar from '../components/navigation/AdminNavBar.vue';
  import Footer from '../components/Footer.vue';

  const activeTab = ref('overview');
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="p-4 mx-auto md:p-8 max-w-7xl">
      <div class="mb-6">
        <h1 class="mb-2 font-bold">
          <span class="block text-2xl md:text-3xl">Welcome to the</span>
          <span class="text-purple-600 text-3xl md:text-4xl">Admin Dashboard</span>
        </h1>
        <p class="text-gray-600 mt-2 text-sm md:text-base">
          Manage <span class="text-purple-600 font-semibold">users</span>, monitor
          <span class="text-purple-600 font-semibold">activity</span>, and configure
          <span class="text-purple-600 font-semibold">platform settings</span> with ease.
        </p>
      </div>

      <AdminNavBar />

      <div class="mt-8">
        <router-view />
      </div>
    </div>

    <Footer />
  </div>
</template>
