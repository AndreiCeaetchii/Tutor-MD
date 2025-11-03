<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

const goBack = () => {
  router.back();
};

const goHome = () => {
  router.push('/');
};
</script>

<template>
  <div class="flex items-center justify-center px-4 py-4 bg-gray-50" style="min-height: calc(100vh - 80px);">
    <div class="grid items-center w-full max-w-6xl gap-8 lg:grid-cols-2">
      <div class="space-y-6 text-center lg:text-left">
        <div class="inline-block">
          <h1 class="font-black text-violet-600 text-8xl md:text-9xl">
            404
          </h1>
        </div>
        
        <h2 class="text-3xl font-bold text-gray-800 md:text-4xl">
          Oops! <span class="text-orange-500">Lost in Learning?</span>
        </h2>
        
        <p class="max-w-md mx-auto text-lg text-gray-600 lg:mx-0">
          Looks like this page took a study break. Let's get you back on track with your educational journey!
        </p>

        <div class="flex flex-col justify-center gap-4 pt-4 sm:flex-row lg:justify-start">
          <button
            @click="goBack"
            class="px-8 py-3 font-semibold text-white transition-all duration-300 shadow-lg bg-violet-600 rounded-xl hover:shadow-xl hover:bg-violet-700"
          >
            Go Back
          </button>
          <button
            @click="goHome"
            class="px-8 py-3 font-semibold text-white transition-all duration-300 bg-orange-500 shadow-lg rounded-xl hover:shadow-xl hover:bg-orange-600"
          >
            Back to Home
          </button>
        </div>

        <div class="pt-6">
          <p class="text-sm text-gray-500">
            Need help finding a tutor?
            <router-link to="/contact-us" class="ml-1 font-medium transition-colors text-violet-600 hover:text-orange-500">
              Contact us
            </router-link>
          </p>
        </div>
      </div>

      <div class="relative flex items-center justify-center lg:justify-end">
        <img 
          src="../assets/404image.png" 
          alt="Lost globe illustration" 
          class="w-full max-w-lg"
        />
      </div>
    </div>
  </div>
</template>