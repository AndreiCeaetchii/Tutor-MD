<script setup lang="ts">
import hero1_img from '../../assets/hero-1-img.png';
import { DotLottieVue } from '@lottiefiles/dotlottie-vue';
import { useRouter } from 'vue-router';
import { useUserStore } from '../../store/userStore.ts';

const store = useUserStore();
const router = useRouter();

const startAsStudent = () => {
  store.updateRole('student');
  router.push('/signup');
};

const joinAsInstructor = () => {
  store.updateRole('tutor');
  router.push('/signup');
};
</script>

<template>
  <div class="py-16 bg-gray-50 lg:py-24">
    <div
      class="relative flex flex-col items-center justify-between px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 lg:flex-row"
    >
      <div class="mb-12 text-center lg:w-1/2 lg:text-left lg:mb-0">
        <h1 class="text-4xl font-bold leading-tight sm:text-5xl lg:text-5xl">
          A good <span class="text-[#5f22d9]">#education</span> is always a foundation of
          <br class="hidden lg:inline" />
          <span class="bg-[#5f22d9] text-white px-4 py-2 rounded-lg inline-block mt-4">
            A bright future
          </span>
        </h1>
        <p class="max-w-lg mx-auto mt-6 text-gray-600 lg:mx-0">
          Consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna
          aliqua. Ad minim veniam.
        </p>
        <div
          class="flex flex-col items-center justify-center mt-8 space-y-4 sm:flex-row sm:items-start lg:justify-start sm:space-y-0 sm:space-x-4"
        >
          <button
            class="relative w-full px-8 py-3 font-semibold text-white transition-colors bg-orange-500 rounded-full shadow-lg sm:w-auto hover:bg-orange-600"
            @click="startAsStudent"
          >
            Start as student
            <DotLottieVue
              class="absolute hidden w-16 h-16 rotate-90 lg:block -top-5 right-52"
              autoplay
              loop
              src="https://lottie.host/9592bc01-b3e5-442a-afce-42d3856692e4/50BPVWNyw9.lottie"
            />
          </button>
          <button
            class="w-full px-8 py-3 font-semibold text-gray-800 transition-colors bg-transparent border border-gray-300 rounded-full sm:w-auto hover:bg-gray-100"
            @click="joinAsInstructor"
          >
            Join as Instructor
          </button>
        </div>
        <p class="mt-4 text-sm text-gray-500">
          A world of knowledge awaits.
          <a href="#" class="text-[#5f22d9] underline hover:no-underline">Join our community</a>.
        </p>
      </div>

      <div class="lg:w-1/2 relative w-full h-96 lg:h-[500px] flex items-center justify-center">
        <div class="relative w-full h-full">
          <div
            class="w-full h-full bg-center bg-cover rounded-lg shadow-lg"
            :style="{ backgroundImage: `url(${hero1_img})` }"
          ></div>
        </div>
      </div>
    </div>
  </div>
</template>
