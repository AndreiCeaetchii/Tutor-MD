<script setup lang="ts">
import SignupForm from '../components/signup/SignupForm.vue';
import SignupContent from '../components/signup/SignupContent.vue';
</script>

<template>
  <div class="fixed inset-0 z-50 flex items-center justify-center p-4 backdrop-blur-sm">
    <div class="flex flex-col w-full mx-auto overflow-hidden shadow-xl md:flex-row max-w-7xl bg-white/80 rounded-2xl">
      <div class="hidden md:block md:w-1/2">
        <SignupContent />
      </div>

      <div class="w-full md:w-1/2">
        <SignupForm />
      </div>
    </div>
  </div>
</template>

