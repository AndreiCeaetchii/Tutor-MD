<script setup lang="ts">
import { defineProps, computed } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faStar } from '@fortawesome/free-solid-svg-icons';

library.add(faStar);

interface Tutor {
  id: number;
  firstName: string;
  lastName: string;
  imageLink: string;
  rating: number;
  totalReviews: number;
}

const props = defineProps({
  tutors: {
    type: Array as () => Tutor[],
    required: true,
  },
});

const sortedTutors = computed(() => {
  return [...props.tutors].sort((a, b) => b.rating - a.rating).slice(0, 5);
});

const getRankingColor = (index: number) => {
  const colors = ['bg-yellow-400', 'bg-gray-400', 'bg-red-400', 'bg-purple-400', 'bg-blue-400'];
  return colors[index] || 'bg-gray-400';
};
</script>

<template>
  <div class="bg-white p-6 rounded-lg shadow-md">
    <h2 class="text-xl font-bold text-gray-800 mb-4">Top Performing Tutors</h2>
    <ul class="space-y-4">
      <li
        v-for="(tutor, index) in sortedTutors"
        :key="tutor.id"
        class="flex items-center space-x-4 pb-4 mb-4 border-b border-gray-200"
      >
        <div
          class="flex items-center justify-center w-8 h-8 rounded-full text-white font-bold text-sm"
          :class="getRankingColor(index)"
        >
          {{ index + 1 }}
        </div>
        <img
          :src="tutor.imageLink"
          alt="Tutor avatar"
          class="w-12 h-12 rounded-full object-cover"
        />
        <div class="flex-1">
          <h3 class="font-semibold text-gray-800">{{ tutor.firstName }} {{ tutor.lastName }}</h3>
          <p class="text-sm text-gray-500">
            {{ tutor.rating }} <font-awesome-icon icon="fa-solid fa-star" class="text-yellow-400" /> • {{ tutor.totalReviews }} reviews
          </p>
        </div>
      </li>
    </ul>
  </div>
</template>