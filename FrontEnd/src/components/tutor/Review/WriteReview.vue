<script setup lang="ts">
  import { ref } from 'vue';
  import { createReview } from '../../../services/reviewService.ts';

  const isModalOpen = ref(false);
  const rating = ref(0);
  const reviewText = ref('');
  const isLoading = ref(false);

  // Aici trebuie ID-ul real al rezervării.
  const bookingId = 1;

  const closeModal = () => {
    isModalOpen.value = false;
  };

  const setRating = (star: number) => {
    rating.value = star;
  };

  const handleSubmit = async () => {
    if (rating.value === 0 || reviewText.value.trim() === '') {
      alert('Vă rugăm să oferiți o notă și să scrieți o recenzie.');
      return;
    }

    isLoading.value = true;
    try {
      const reviewData = {
        BookingId: bookingId,
        Rating: rating.value,
        Description: reviewText.value,
      };

      await createReview(reviewData);

      closeModal();
      rating.value = 0;
      reviewText.value = '';
    } catch (error) {
      console.error('Error creating review:', error);
      alert('An error occurred while creating the review. Please try again.');
    } finally {
      isLoading.value = false;
    }
  };
</script>

<template>
  <div class="font-sans">
    <div
      class="flex items-center justify-between p-4 border border-orange-300 shadow-sm bg-orange-50 rounded-xl"
    >
      <div class="flex items-center space-x-4">
        <img
          src="https://placehold.co/50x50/F8D8C0/text"
          alt="Carlos Rodriguez"
          class="object-cover w-12 h-12 rounded-full"
        />
        <div class="flex flex-col">
          <span class="font-semibold text-gray-900">Carlos Rodriguez</span>
          <span class="text-sm text-gray-500">Spanish • 06.09.2025</span>
        </div>
      </div>
      <button
        @click="isModalOpen = true"
        class="px-6 py-2 font-semibold text-white transition-colors duration-300 bg-orange-500 rounded-full hover:bg-orange-600"
      >
        Write Review
      </button>
    </div>

    <div
      v-if="isModalOpen"
      class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-gray-900 bg-opacity-75"
    >
      <div class="w-full max-w-lg p-6 bg-white rounded-lg">
        <div class="flex items-center justify-between mb-4">
          <h2 class="text-lg font-bold">Write a Review</h2>
          <button @click="closeModal" class="text-gray-500 hover:text-gray-700">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-6 h-6"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M6 18L18 6M6 6l12 12"
              />
            </svg>
          </button>
        </div>
        <div class="flex items-center justify-center mb-4">
          <span
            v-for="star in 5"
            :key="star"
            @click="setRating(star)"
            class="text-2xl cursor-pointer"
            :class="{ 'text-yellow-400': star <= rating, 'text-gray-300': star > rating }"
          >
            ★
          </span>
        </div>
        <textarea
          v-model="reviewText"
          rows="5"
          placeholder="Write your review here..."
          class="w-full p-2 mb-4 border-gray-300 rounded-md shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
        ></textarea>
        <div class="flex justify-end">
          <button
            @click="handleSubmit"
            class="px-6 py-2 font-semibold text-white transition-colors duration-300 bg-purple-600 rounded-full hover:bg-purple-700"
          >
            Submit
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
