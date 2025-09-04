

<template>
  <div class="bg-gray-50 py-16 lg:py-24">
    <div class="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 flex flex-col lg:flex-row items-center justify-between">
      <div class="lg:w-1/2 text-center lg:text-left mb-12 lg:mb-0">
        <h1 class="text-4xl sm:text-5xl lg:text-5xl font-bold leading-tight">
          A good <span class="text-[#5f22d9]">#education</span> is always a foundation of
          <br class="hidden lg:inline" />
          <span class="bg-[#5f22d9] text-white px-4 py-2 rounded-lg inline-block mt-4">
            A bright future
          </span>
        </h1>
        <p class="mt-6 text-gray-600 max-w-lg mx-auto lg:mx-0">
          Consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ad minim veniam.
        </p>
        <div class="mt-8 flex flex-col sm:flex-row items-center sm:items-start justify-center lg:justify-start space-y-4 sm:space-y-0 sm:space-x-4">
          <button class="w-full sm:w-auto px-8 py-3 bg-orange-500 text-white font-semibold rounded-full shadow-lg hover:bg-orange-600 transition-colors">
            Start as student
          </button>
          <button class="w-full sm:w-auto px-8 py-3 bg-transparent border border-gray-300 text-gray-800 font-semibold rounded-full hover:bg-gray-100 transition-colors">
            Join as Instructor
          </button>
        </div>
        <p class="mt-4 text-sm text-gray-500">
          A world of knowledge awaits.   <a href="#" class="text-[#5f22d9] underline hover:no-underline">Join our community</a> .
        </p>
      </div>

      <div class="lg:w-1/2 relative w-full h-96 lg:h-[500px] flex items-center justify-center">
        <div class="relative w-full h-full">
          <!-- Updated div to use the imported image -->
          <div
              class="w-full h-full bg-cover bg-center rounded-lg shadow-lg"
              :style="{ backgroundImage: `url(${hero1_img})` }">
          </div>
        </div>
      </div>
      <DotLottieVue
          class=" lg:block absolute top-88 -left-15 w-16 h-16 rotate-90"
          autoplay
          loop
          src="https://lottie.host/9592bc01-b3e5-442a-afce-42d3856692e4/50BPVWNyw9.lottie"
      />
    </div>
  </div>
</template>
<script setup lang="ts">
  import hero1_img from '../assets/hero-1-img.png';
  import { DotLottieVue } from '@lottiefiles/dotlottie-vue'

</script>