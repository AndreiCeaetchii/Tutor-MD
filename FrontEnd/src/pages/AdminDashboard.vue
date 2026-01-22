<script setup lang="ts">
  import AdminNavBar from '../components/navigation/AdminNavBar.vue';
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="p-4 mx-auto md:p-8 max-w-7xl">
      <div class="mb-6">
        <h1 class="mb-2 text-2xl font-bold md:text-3xl">
          Welcome to the <span class="text-purple-600">Admin Dashboard</span>
        </h1>
        <p class="mt-2 text-sm text-gray-600 md:text-base">
          Manage <span class="font-semibold text-purple-600">users</span>, monitor
          <span class="font-semibold text-purple-600">activity</span>, and configure
          <span class="font-semibold text-purple-600">platform settings</span> with ease.
        </p>
      </div>

      <AdminNavBar />

      <div class="mt-8">
        <router-view />
      </div>
    </div>
  </div>
</template>
