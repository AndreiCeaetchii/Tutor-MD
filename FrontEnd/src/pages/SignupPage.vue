<template>
  <div class="fixed inset-0 backdrop-blur-sm flex justify-center items-center z-50 p-4">
    <div class="flex flex-col md:flex-row w-full max-w-7xl mx-auto bg-white/80 rounded-2xl shadow-xl overflow-hidden">
      <div class="hidden md:block md:w-1/2">
        <SignupContent />
      </div>

      <div class="w-full md:w-1/2">
        <SignupForm />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import SignupForm from '../components/signup/SignupForm.vue';
import SignupContent from '../components/signup/SignupContent.vue';
</script>
