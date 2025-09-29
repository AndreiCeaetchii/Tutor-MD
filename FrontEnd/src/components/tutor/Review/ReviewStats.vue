<script setup lang="ts">
import { computed } from 'vue';

interface ReviewItem {
  id: number;
  rating: number;
}

interface TutorReviewsData {
  reviews: ReviewItem[];
  averageRating: number | null;
}

const props = defineProps<{
  reviewsData: TutorReviewsData;
}>();

const starPath = "M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z";
const emptyColor = "#D1D5DB";

const reviewsData = computed(() => props.reviewsData);

const totalReviews = computed(() => {
  return reviewsData.value.reviews.length;
});

const actualAverageRating = computed(() => {
  if (totalReviews.value === 0) return 0;

  const sum = reviewsData.value.reviews.reduce((acc, r) => acc + r.rating, 0);
  return parseFloat((sum / totalReviews.value).toFixed(1));
});

const visualRating = computed(() => {
  return Math.round(actualAverageRating.value * 2) / 2;
});


const ratingsDistribution = computed(() => {
  const distribution: Record<number, number> = { 5: 0, 4: 0, 3: 0, 2: 0, 1: 0 };

  reviewsData.value.reviews.forEach(review => {
    const star = review.rating;
    if (star >= 1 && star <= 5) {
      distribution[star]++;
    }
  });

  return Object.keys(distribution)
    .map(key => ({
      stars: parseInt(key),
      count: distribution[parseInt(key)],
    }))
    .sort((a, b) => b.stars - a.stars);
});

const fullStars = computed(() => Math.floor(visualRating.value));
const hasHalfStar = computed(() => visualRating.value % 1 !== 0);
const emptyStars = computed(() => 5 - fullStars.value - (hasHalfStar.value ? 1 : 0));
</script>

<template>
  <div class="content-container">
    <div
      class="flex flex-col items-start p-6 space-y-6 bg-white shadow-lg rounded-2xl md:p-8 md:flex-row md:items-center md:space-y-0"
    >
      <div class="w-full">
        <h2 class="mb-4 text-xl font-bold text-gray-800">
          Student
          <span class="text-purple-600"> Ratings </span>& Feedback
        </h2>
        <div class="flex items-center mb-4 space-x-4">
          <div class="flex-shrink-0">
            <p class="text-4xl font-bold text-gray-800">{{ actualAverageRating.toFixed(1) }}</p>

            <div class="flex mt-1 text-yellow-400">
              <svg
                v-for="n in fullStars"
                :key="'full-' + n"
                class="w-5 h-5 fill-current"
                viewBox="0 0 24 24"
              >
                <path :d="starPath" />
              </svg>

              <svg
                v-if="hasHalfStar"
                class="w-5 h-5"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <defs>
                  <clipPath id="cut-half">
                    <rect x="0" y="0" width="12" height="24" />
                  </clipPath>
                </defs>
                <path
                  :fill="emptyColor"
                  :d="starPath"
                />
                <path
                  fill="currentColor"
                  :d="starPath"
                  clip-path="url(#cut-half)"
                />
              </svg>

              <svg
                v-for="n in emptyStars"
                :key="'empty-' + n"
                class="w-5 h-5 fill-current"
                viewBox="0 0 24 24"
                :style="{ color: emptyColor }"
              >
                <path :d="starPath" />
              </svg>
            </div>

            <p class="mt-2 text-sm text-gray-500">{{ totalReviews }} reviews</p>
          </div>

          <div class="flex-grow space-y-2">
            <div v-for="rating in ratingsDistribution" :key="rating.stars" class="flex items-center space-x-2">
              <span class="w-4 text-sm font-medium text-gray-700">{{ rating.stars }}</span>
              <div class="relative flex-grow h-2 overflow-hidden bg-gray-200 rounded-full">
                <div
                  class="absolute inset-0 bg-yellow-400"
                  :style="{ width: `${(rating.count / totalReviews) * 100}%` }"
                ></div>
              </div>
              <span class="w-4 text-sm font-medium text-right text-gray-700">{{
                  rating.count
                }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.content-container {
  max-width: 1200px;
  width: 100%;
  margin: 0 auto;
}

@media (max-width: 640px) {
  .content-container {
    padding: 0 0.5rem;
  }
}
</style>