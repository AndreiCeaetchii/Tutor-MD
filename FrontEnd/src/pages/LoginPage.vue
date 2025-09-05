<script setup lang="ts">
  import LoginForm from '../components/login/LoginForm.vue';
  import LoginContent from '../components/login/LoginContent.vue';
</script>

<template>
  <div class="fixed inset-0 backdrop-blur-sm flex justify-center items-center z-50 p-4">
    <div class="flex flex-col md:flex-row w-full max-w-7xl mx-auto bg-white/80 rounded-2xl shadow-xl overflow-hidden">
      <div class="hidden md:block md:w-1/2">
        <LoginContent />
      </div>

      <div class="w-full md:w-1/2">
        <LoginForm />
      </div>
    </div>
  </div>
</template>
