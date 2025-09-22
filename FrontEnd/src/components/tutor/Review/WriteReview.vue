<template>
  <div class="font-sans">
    <div
      class="flex items-center justify-between p-4 bg-orange-50 border border-orange-300 rounded-xl shadow-sm"
    >
      <div class="flex items-center space-x-4">
        <img
          src="https://placehold.co/50x50/F8D8C0/text"
          alt="Carlos Rodriguez"
          class="w-12 h-12 rounded-full object-cover"
        />
        <div class="flex flex-col">
          <span class="font-semibold text-gray-900">Carlos Rodriguez</span>
          <span class="text-sm text-gray-500">Spanish • 06.09.2025</span>
        </div>
      </div>
      <button
        @click="isModalOpen = true"
        class="px-6 py-2 bg-orange-500 text-white font-semibold rounded-full hover:bg-orange-600 transition-colors duration-300"
      >
        Write Review
      </button>
    </div>

    <!-- Full-screen modal for writing review -->
    <div
      v-if="isModalOpen"
      class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-gray-900 bg-opacity-75"
    >
      <div class="bg-white rounded-lg p-6 w-full max-w-lg">
        <!-- Modal header -->
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-lg font-bold">Write a Review</h2>
          <button @click="closeModal" class="text-gray-500 hover:text-gray-700">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="h-6 w-6"
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
        <!-- Star rating component -->
        <div class="flex items-center justify-center mb-4">
          <span
            v-for="star in 5"
            :key="star"
            @click="setRating(star)"
            class="cursor-pointer text-2xl"
            :class="{ 'text-yellow-400': star <= rating, 'text-gray-300': star > rating }"
          >
            ★
          </span>
        </div>
        <!-- Review textarea -->
        <textarea
          v-model="reviewText"
          rows="5"
          placeholder="Write your review here..."
          class="w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50 mb-4 p-2"
        ></textarea>
        <!-- Submit button -->
        <div class="flex justify-end">
          <button
            @click="handleSubmit"
            class="px-6 py-2 bg-purple-600 text-white font-semibold rounded-full hover:bg-purple-700 transition-colors duration-300"
          >
            Submit
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

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

      console.log('Recenzia a fost trimisă cu succes!');
      closeModal();
      rating.value = 0;
      reviewText.value = '';
    } catch (error) {
      console.error('Eroare la trimiterea recenziei:', error);
      alert('A apărut o eroare la trimiterea recenziei. Vă rugăm să încercați din nou.');
    } finally {
      isLoading.value = false;
    }
  };
</script>
