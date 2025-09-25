<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { faThumbsUp } from '@fortawesome/free-solid-svg-icons';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import type { TutorReview } from '../../../services/reviewService';

  library.add(faThumbsUp);

  const props = defineProps<{
    review: TutorReview;
  }>();

  const isLiked = ref(false);

  const formattedDate = computed(() => {
    if (props.review.createdAt) {
      const date = new Date(props.review.createdAt);
      return date.toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' });
    }
    return '';
  });

  const ratingStars = computed(() => {
    return props.review.rating;
  });

  const toggleLike = () => {
    isLiked.value = !isLiked.value;
  };
</script>

<template>
  <div
    class="p-6 transition-all duration-300 bg-white border border-gray-200 rounded-lg hover:shadow-md"
  >
    <div class="flex items-center space-x-4">
      <div
        class="flex items-center justify-center w-10 h-10 font-semibold text-gray-600 bg-gray-200 rounded-full"
      >
        {{ props.review.userName.charAt(0) }}
      </div>
      <div class="flex-1">
        <div class="flex items-center space-x-2">
          <h3 class="text-base font-semibold text-gray-900">
            {{ props.review.userName }}
          </h3>
        </div>
        <div class="flex items-center mt-1 space-x-2 text-xs text-gray-500">
          <div class="flex text-yellow-400">
            <svg
              v-for="i in 5"
              :key="i"
              class="w-3 h-3 fill-current"
              viewBox="0 0 24 24"
              :class="{ 'text-yellow-400': i <= ratingStars, 'text-gray-300': i > ratingStars }"
            >
              <path
                d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
              />
            </svg>
          </div>
          <span>•</span>
          <span>{{ formattedDate }}</span>
        </div>
      </div>
    </div>

    <p class="mt-4 text-sm leading-relaxed text-gray-700">
      {{ props.review.description }}
    </p>

    <div class="flex items-center justify-between pt-3 mt-4 border-t border-gray-200">
      <button
        @click="toggleLike"
        :class="[
          'flex items-center space-x-2 transition-colors duration-200 text-sm',
          isLiked ? 'text-purple-600' : 'text-gray-500 hover:text-purple-600',
        ]"
      >
        <FontAwesomeIcon
          :icon="['fas', 'thumbs-up']"
          :class="['w-4 h-4', { 'text-purple-600': isLiked }]"
        />
      </button>
      <span class="text-xs text-gray-500">{{ formattedDate }}</span>
    </div>
  </div>
</template>
