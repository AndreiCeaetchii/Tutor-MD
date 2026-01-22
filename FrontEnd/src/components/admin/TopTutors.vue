<script setup lang="ts">
  import { computed } from 'vue';
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
    return [...props.tutors].sort((a, b) => {
      if (b.rating !== a.rating) {
        return b.rating - a.rating;
      }
      return b.totalReviews - a.totalReviews;
    });
  });

  const getRankingColor = (index: number) => {
    const colors = ['bg-yellow-400', 'bg-gray-400', 'bg-red-400', 'bg-purple-400', 'bg-blue-400'];
    return index < 5 ? colors[index] : 'bg-gray-300';
  };
</script>

<template>
  <div class="p-6 bg-white rounded-lg shadow-md">
    <h2 class="mb-4 text-xl font-bold text-gray-800">Top Performing Tutors</h2>
    <p class="mb-4 text-sm text-gray-500">All tutors ranked by rating and reviews</p>

    <div v-if="sortedTutors.length === 0" class="py-8 text-center text-gray-400">
      No tutors available
    </div>

    <ul v-else class="space-y-4 max-h-[500px] overflow-y-auto pr-2">
      <li
        v-for="(tutor, index) in sortedTutors"
        :key="tutor.id"
        class="flex items-center pb-4 mb-4 space-x-4 border-b border-gray-200"
      >
        <div
          class="flex items-center justify-center w-8 h-8 text-sm font-bold text-white rounded-full"
          :class="getRankingColor(index)"
        >
          {{ index + 1 }}
        </div>
        <img
          :src="tutor.imageLink"
          alt="Tutor avatar"
          class="object-cover w-12 h-12 rounded-full"
        />
        <div class="flex-1">
          <h3 class="font-semibold text-gray-800">{{ tutor.firstName }} {{ tutor.lastName }}</h3>
          <p class="text-sm text-gray-500">
            {{ tutor.rating }}
            <font-awesome-icon icon="fa-solid fa-star" class="text-yellow-400" /> •
            {{ tutor.totalReviews }} reviews
          </p>
        </div>
      </li>
    </ul>
  </div>
</template>
