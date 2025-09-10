<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';

library.add(faArrowRight);
</script>

<template>
  <div class="flex items-center justify-center h-full p-8 overflow-y-auto bg-gradient-to-br from-purple-50 to-white">
    <div class="w-full max-w-lg mx-auto">
      <div class="mb-6">
        <svg width="60" height="25" viewBox="0 0 80 30" class="text-orange-500">
          <path d="M0,15 C10,5 20,25 40,15 C60,5 70,25 80,15" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" />
        </svg>
      </div>
      
      <p class="mb-1 text-base font-medium text-gray-700">Better Learning. Better Results</p>
      
      <h1 class="mb-4 text-4xl font-bold text-gray-800">
        Online education platform that fits for everyone
      </h1>
      
      <p class="mb-8 text-base text-gray-600">
        Connect with expert tutors or find motivated students. Our platform makes learning and teaching simple and tailored to your individual needs.
      </p>
      
      <div>
        <button
          type="button"
          class="w-64 py-3.5 bg-gradient-to-r from-[#5f22d9] to-[#2c016d] text-white font-semibold rounded-full text-center transition-all hover:shadow-lg group flex items-center justify-center gap-2"
        >
          <span>Explore more about us</span>
          <font-awesome-icon :icon="['fas', 'arrow-right']" class="transition-transform group-hover:translate-x-1" />
        </button>
      </div>
    </div>
  </div>
</template>
