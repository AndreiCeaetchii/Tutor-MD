<script setup lang="ts">
import { ref } from 'vue';
    import NavigationBar from "../components/navigation/NavigationBar.vue";

    const currentUserRole = ref('student');
</script>

<template>
  <div class="p-4">
    <h1 class="text-2xl font-bold mb-4">Student Dashboard</h1>
    <p>Welcome to your dashboard! Here you can manage your tutoring sessions, view teacher progress, and update your profile.</p>
  </div>
  <NavigationBar :userRole="currentUserRole" />
</template>