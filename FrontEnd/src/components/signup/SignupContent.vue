<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faArrowRight, faSmile, faCheckCircle, faClock } from '@fortawesome/free-solid-svg-icons';

library.add(faArrowRight, faSmile, faCheckCircle, faClock);
</script>

<template>
  <div class="flex items-center justify-center h-full p-8 overflow-y-auto bg-gradient-to-br from-purple-50 to-white">
    <div class="w-full max-w-lg mx-auto">
      <div class="mb-6">
        <svg width="60" height="25" viewBox="0 0 80 30" class="text-orange-500">
          <path d="M0,15 C10,5 20,25 40,15 C60,5 70,25 80,15" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" />
        </svg>
      </div>
      
      <p class="mb-1 text-base font-medium text-gray-700">
        Why our working is so unique
      </p>
      
      <h1 class="mb-10 text-4xl font-bold text-gray-800">
        How We Make Learning Easy
      </h1>
      
      <div class="flex items-start mb-8">
        <div class="flex items-center justify-center flex-shrink-0 bg-purple-100 rounded-full h-14 w-14">
          <font-awesome-icon :icon="['fas', 'smile']" class="text-2xl text-purple-500" />
        </div>
        <div class="flex-1 ml-4"> 
          <h3 class="mb-2 text-xl font-semibold text-gray-800">Easy to use</h3>
          <p class="text-gray-600">
            Find and book your tutor in just a few clicks.
          </p>
        </div>
      </div>
      
      <div class="flex items-start mb-8">
        <div class="flex items-center justify-center flex-shrink-0 bg-green-100 rounded-full h-14 w-14">
          <font-awesome-icon :icon="['fas', 'check-circle']" class="text-2xl text-green-500" />
        </div>
        <div class="flex-1 ml-4"> 
          <h3 class="mb-2 text-xl font-semibold text-gray-800">Trust you can see</h3>
          <p class="text-gray-600">
            Every tutor is verified, and every review is real.
          </p>
        </div>
      </div>
      
      <div class="flex items-start">
        <div class="flex items-center justify-center flex-shrink-0 bg-red-100 rounded-full h-14 w-14">
          <font-awesome-icon :icon="['fas', 'clock']" class="text-2xl text-red-400" />
        </div>
        <div class="flex-1 ml-4">
          <h3 class="mb-2 text-xl font-semibold text-gray-800">Your schedule, your way</h3>
          <p class="text-gray-600">
            Get real-time notifications and manage your lessons with ease.
          </p>
        </div>
      </div>
    </div>
  </div>
</template>