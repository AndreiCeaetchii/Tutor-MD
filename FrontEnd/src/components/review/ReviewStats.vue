<template>
  <div class="content-container">
    <div
      class="flex flex-col items-start p-6 space-y-6 bg-white shadow-lg rounded-2xl md:p-8 md:flex-row md:items-center md:space-y-0 md:space-x-8"
    >
      <div class="w-full md:w-2/3">
        <h2 class="mb-4 text-xl font-bold text-gray-800">
          Student
          <span class="text-purple-600"> Ratings </span>& Feedback
        </h2>
        <div class="flex items-center mb-4 space-x-4">
          <div class="flex-shrink-0">
            <p class="text-4xl font-bold text-gray-800">4.7</p>
            <div class="flex mt-1 text-yellow-400">
              <svg class="w-5 h-5 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
                />
              </svg>
              <svg class="w-5 h-5 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
                />
              </svg>
              <svg class="w-5 h-5 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
                />
              </svg>
              <svg class="w-5 h-5 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
                />
              </svg>
              <svg class="w-5 h-5 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 17.27l-6.18 3.73 1.64-7.03L1.24 9.1l7.2-.62L12 2l3.56 6.48 7.2.62-5.92 5.87 1.64 7.03z"
                />
              </svg>
            </div>
            <p class="mt-2 text-sm text-gray-500">6 reviews</p>
          </div>

          <div class="flex-grow space-y-2">
            <div v-for="rating in ratings" :key="rating.stars" class="flex items-center space-x-2">
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

      <div class="w-full pt-6 border-t border-gray-200 md:w-1/3 md:border-t-0 md:border-l md:pl-8 md:pt-0">
        <h3 class="mb-4 text-lg font-semibold text-gray-800">Quick Stats</h3>
        <div class="grid grid-cols-3 gap-4 md:grid-cols-1">
          <div class="p-3 text-center bg-blue-50 rounded-xl">
            <p class="text-xl font-bold text-blue-700">5</p>
            <p class="text-xs text-blue-600">Verified Reviews</p>
          </div>
          <div class="p-3 text-center bg-green-50 rounded-xl">
            <p class="text-xl font-bold text-green-700">100%</p>
            <p class="text-xs text-green-600">Positive Reviews</p>
          </div>
          <div class="p-3 text-center bg-purple-50 rounded-xl">
            <p class="text-xl font-bold text-purple-700">51</p>
            <p class="text-xs text-purple-600">Helpful Votes</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed } from 'vue';

  const ratings = ref([
    { stars: 5, count: 4 },
    { stars: 4, count: 2 },
    { stars: 3, count: 0 },
    { stars: 2, count: 0 },
    { stars: 1, count: 0 },
  ]);

  const totalReviews = computed(() => {
    return ratings.value.reduce((sum, rating) => sum + rating.count, 0);
  });
</script>

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