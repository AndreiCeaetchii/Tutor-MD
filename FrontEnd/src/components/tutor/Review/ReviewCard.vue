<template>
  <div
    class="p-6 transition-all duration-300 bg-white border border-gray-200 rounded-lg hover:shadow-md"
  >
    <div class="flex items-center space-x-4">
      <img :src="reviewData.avatar" alt="Avatar" class="object-cover w-10 h-10 rounded-full" />
      <div class="flex-1">
        <div class="flex items-center space-x-2">
          <h3 class="text-base font-semibold text-gray-900">
            {{ reviewData.name }}
          </h3>
          <span class="bg-green-50 text-green-700 text-xs font-medium px-2 py-0.5 rounded-full"
            >Verified</span
          >
        </div>
        <div class="flex items-center mt-1 space-x-2 text-xs text-gray-500">
          <div class="flex text-yellow-400">
            <svg v-for="i in 5" :key="i" class="w-3 h-3 fill-current" viewBox="0 0 24 24">
              <path
                d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
              />
            </svg>
          </div>
          <span>•</span>
          <span>{{ reviewData.subject }}</span>
          <span>•</span>
          <span>{{ reviewData.date }}</span>
        </div>
      </div>
    </div>

    <p class="mt-4 text-sm leading-relaxed text-gray-700">
      {{ reviewData.text }}
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
        <span>Helpful ({{ reviewData.helpfulVotes }})</span>
      </button>
      <span class="text-xs text-gray-500">{{ reviewData.displayDate }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { faThumbsUp } from '@fortawesome/free-solid-svg-icons';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

  library.add(faThumbsUp);

  const reviewData = ref({
    avatar:
      'https://plus.unsplash.com/premium_photo-1689568126014-06fea9d5d341?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8cHJvZmlsZXxlbnwwfHwwfHx8MA%3D%3D',
    name: 'Alex Chen',
    subject: 'Calculus',
    date: '9/10/2025',
    text: "Sarah is an amazing math tutor! She helped me understand calculus concepts that I was struggling with for weeks. Her explanations are clear and she's very patient. Highly recommended!",
    helpfulVotes: 12,
    displayDate: 'September 10, 2025',
  });

  const isLiked = ref(false);

  const toggleLike = () => {
    isLiked.value = !isLiked.value;
    if (isLiked.value) {
      reviewData.value.helpfulVotes++;
    } else {
      reviewData.value.helpfulVotes--;
    }
  };
</script>
