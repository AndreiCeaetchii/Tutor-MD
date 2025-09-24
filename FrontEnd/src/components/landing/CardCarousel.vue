<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue';

const currentIndex = ref(0);
const touchStartX = ref(0);
const touchEndX = ref(0);

const carouselTrack = ref<HTMLElement | null>(null);

const slotChildren = ref<number>(0);

const cardsToShow = computed(() => {
  if (window.innerWidth >= 1024) return 3;
  if (window.innerWidth >= 768) return 2;
  return 1;
});

const cardWidthPercentage = computed(() => 100 / cardsToShow.value);

const prevSlide = () => {
  currentIndex.value = (currentIndex.value > 0) ? currentIndex.value - 1 : slotChildren.value - cardsToShow.value;
};

const nextSlide = () => {
  const maxIndex = slotChildren.value - cardsToShow.value;
  currentIndex.value = (currentIndex.value < maxIndex) ? currentIndex.value + 1 : 0;
};

const handleTouchStart = (e: TouchEvent) => {
  touchStartX.value = e.touches[0].clientX;
};

const handleTouchMove = (e: TouchEvent) => {
  touchEndX.value = e.touches[0].clientX;
};

const handleTouchEnd = () => {
  if (touchStartX.value - touchEndX.value > 50) {
    nextSlide();
  } else if (touchEndX.value - touchStartX.value > 50) {
    prevSlide();
  }
};

const handleResize = () => {
  const maxIndex = slotChildren.value - cardsToShow.value;
  if (currentIndex.value > maxIndex) {
    currentIndex.value = maxIndex;
  }
  if (currentIndex.value < 0) {
    currentIndex.value = 0;
  }
};

onMounted(() => {
  window.addEventListener('resize', handleResize);
  if (carouselTrack.value) {
    slotChildren.value = carouselTrack.value.children.length;
  }
});

onUnmounted(() => {
  window.removeEventListener('resize', handleResize);
});
</script>

<template>
  <div class="relative w-full py-12 mx-auto overflow-hidden max-w-7xl">
    <div
        ref="carouselTrack"
        class="flex transition-transform duration-500 ease-in-out"
        :style="{ transform: `translateX(-${currentIndex * cardWidthPercentage}%)` }"
        @touchstart="handleTouchStart"
        @touchmove="handleTouchMove"
        @touchend="handleTouchEnd"
    >
      <slot></slot>
    </div>

    <button
        @click="prevSlide"
        class="absolute left-0 hidden p-2 text-gray-800 transition-colors -translate-y-1/2 bg-gray-200 rounded-full shadow-lg md:block top-1/2 hover:bg-gray-300"
    >
      <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7"></path></svg>
    </button>
    <button
        @click="nextSlide"
        class="absolute right-0 items-center justify-center hidden p-2 text-white transition-colors -translate-y-1/2 bg-purple-600 rounded-full shadow-lg md:block top-1/2 hover:bg-purple-700"
    >
      <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"></path></svg>
    </button>
  </div>
</template>
