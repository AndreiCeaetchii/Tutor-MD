<script setup lang="ts">
import Header from './components/Header.vue';
</script>

<template>
  <div class="bg-gray-50 min-h-screen">
    <Header userType="tutor" />
    <router-view />
  </div>
</template>

<style scoped>
</style>