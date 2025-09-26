<script setup lang="ts">
import { ref, computed } from 'vue';
import { createReview } from '../../../services/reviewService.ts';
import NotificationMessage from '../../ui/AlertMessage.vue';
import DefaultImage from '../../../assets/DefaultImg.png';

const isModalOpen = ref(false);
const rating = ref(0);
const reviewText = ref('');
const isLoading = ref(false);
const showNotification = ref(false);
const notificationMessage = ref('');
const notificationType = ref('error');

const currentDate = computed(() => {
  const today = new Date();
  const day = String(today.getDate()).padStart(2, '0');
  const month = String(today.getMonth() + 1).padStart(2, '0');
  const year = today.getFullYear();
  return `${day}.${month}.${year}`;
});

const props = defineProps<{
  bookingIdForReview: number;
  tutorName: string | null;
  subjectName: string | null;
  tutorPhoto: string | null;
  lessonDate: string | null;
}>();

const bookingId = props.bookingIdForReview;

const closeModal = () => {
  isModalOpen.value = false;
};

const setRating = (star: number) => {
  rating.value = star;
};

const closeNotification = () => {
  showNotification.value = false;
};

const handleSubmit = async () => {
  if (rating.value === 0 || reviewText.value.trim() === '') {
    notificationMessage.value = 'Please provide a rating and review text.';
    notificationType.value = 'warning';
    showNotification.value = true;
    return;
  }

  isLoading.value = true;
  try {
    const reviewData = {
      bookingId: bookingId,
      rating: rating.value,
      description: reviewText.value,
    };

    await createReview(reviewData);

    closeModal();
    rating.value = 0;
    reviewText.value = '';

    notificationMessage.value = 'Review submitted successfully!';
    notificationType.value = 'success';
    showNotification.value = true;
  } catch (error) {
    console.error('Error creating review:', error);
    notificationMessage.value = 'An error occurred while creating the review. Please try again.';
    notificationType.value = 'error';
    showNotification.value = true;
  } finally {
    isLoading.value = false;
  }
};

const { tutorName, subjectName, tutorPhoto, lessonDate } = props;

</script>

<template>
  <div>
  <div class="font-sans">
    <div
      class="flex items-center justify-between p-4 border border-orange-300 shadow-sm bg-orange-50 rounded-xl"
    >
      <div class="flex items-center space-x-4">
        <img
          :src="tutorPhoto || DefaultImage"
          :alt="tutorName || 'Tutor'"
          class="object-cover w-12 h-12 rounded-full"
        />
        <div class="flex flex-col">
          <span class="font-semibold text-gray-900">{{ tutorName || 'Nume Indisponibil' }}</span>
          <span class="text-sm text-gray-500">{{ subjectName || 'Subiect Indisponibil' }} • {{ lessonDate || 'Data indisponibilă' }}</span>
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
      <div class="w-full max-w-2xl p-6 bg-white rounded-lg">
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

        <div class="flex items-center space-x-4 mb-6 p-4 bg-gray-100 rounded-lg">
          <img
            :src="tutorPhoto || DefaultImage"
            :alt="tutorName || 'Tutor'"
            class="object-cover w-16 h-16 rounded-full"
          />
          <div class="flex flex-col">
            <span class="font-semibold text-gray-900">{{ tutorName || 'Numele tutorelui' }}</span>
            <span class="text-sm text-gray-500">{{ subjectName || 'Subiect' }} • {{ lessonDate || 'Data indisponibilă' }}</span>
            <span class="text-xs text-gray-400">Review la data de: {{ currentDate }}</span>
          </div>
        </div>

        <p class="text-sm text-gray-500 text-center mb-2">Please select your rating by clicking on a star:</p>

        <div class="flex items-center justify-center mb-4">
          <span
            v-for="star in 5"
            :key="star"
            @click="setRating(star)"
            class="text-4xl cursor-pointer"
            :class="{ 'text-yellow-400': star <= rating, 'text-gray-300': star > rating }"
          >
            ★
          </span>
        </div>
        <textarea
          v-model="reviewText"
          rows="5"
          placeholder="Spune-ne cum a fost experiența ta și ce ți-a plăcut cel mai mult."
          class="w-full p-2 mb-4 rounded-md shadow-sm resize-none border-2 border-gray-300 focus:ring focus:ring-purple-200 focus:ring-opacity-50 focus:border-purple-500 outline-none"
        ></textarea>
        <div class="flex justify-end">
          <button
            @click="handleSubmit"
            :disabled="isLoading"
            class="px-6 py-2 font-semibold text-white transition-colors duration-300 bg-purple-600 rounded-full hover:bg-purple-700 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {{ isLoading ? 'Submitting...' : 'Submit Review' }}
          </button>
        </div>
      </div>
    </div>

    <NotificationMessage
      :show="showNotification"
      :message="notificationMessage"
      :type="notificationType"
      :duration="5000"
      position="top-right"
      @close="closeNotification"
    />
  </div>
  </div>
</template>