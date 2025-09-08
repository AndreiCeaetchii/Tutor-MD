<template>
  <div class="md:p-6">
    <div
      class="p-6 bg-white rounded-xl shadow-lg flex flex-col md:flex-row items-start md:items-center space-y-6 md:space-y-0 md:space-x-12 max-w-7xl mx-auto"
    >
      <div class="w-full md:w-2/3 p-4">
        <h2 class="text-3xl font-bold mb-6 text-gray-800">
          Student
          <span class="text-[#5f22d9]"> Ratings </span>& Feedback
        </h2>
        <div class="flex items-center mb-4 space-x-4">
          <div class="flex-shrink-0">
            <p class="text-5xl font-bold text-gray-800">4.7</p>
            <div class="flex text-yellow-400 mt-1">
              <svg class="w-6 h-6 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
                />
              </svg>
              <svg class="w-6 h-6 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
                />
              </svg>
              <svg class="w-6 h-6 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
                />
              </svg>
              <svg class="w-6 h-6 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 .587l3.668 7.425 8.214 1.192-5.948 5.793 1.405 8.172-7.34-3.868-7.34 3.868 1.405-8.172-5.948-5.793 8.214-1.192z"
                />
              </svg>
              <svg class="w-6 h-6 fill-current" viewBox="0 0 24 24">
                <path
                  d="M12 17.27l-6.18 3.73 1.64-7.03L1.24 9.1l7.2-.62L12 2l3.56 6.48 7.2.62-5.92 5.87 1.64 7.03z"
                />
              </svg>
            </div>
            <p class="text-sm text-gray-500 mt-2">6 reviews</p>
          </div>

          <div class="flex-grow space-y-2">
            <div v-for="rating in ratings" :key="rating.stars" class="flex items-center space-x-2">
              <span class="text-sm font-medium text-gray-700 w-4">{{ rating.stars }}</span>
              <div class="relative flex-grow h-2 rounded-full bg-gray-200 overflow-hidden">
                <div
                  class="absolute inset-0 bg-yellow-400"
                  :style="{ width: `${(rating.count / totalReviews) * 100}%` }"
                ></div>
              </div>
              <span class="text-sm font-medium text-gray-700 w-4 text-right">{{
                rating.count
              }}</span>
            </div>
          </div>
        </div>
      </div>

      <div class="w-full md:w-1/3 p-4 border-t md:border-t-0 md:border-l border-gray-200">
        <h2 class="text-2xl font-semibold mb-6 text-gray-800">Quick Stats</h2>
        <div class="space-y-4">
          <div class="bg-blue-100 p-4 rounded-xl text-center">
            <p class="text-3xl font-bold text-blue-800">5</p>
            <p class="text-sm text-blue-600 mt-1">Verified Reviews</p>
          </div>
          <div class="bg-green-100 p-4 rounded-xl text-center">
            <p class="text-3xl font-bold text-green-800">100%</p>
            <p class="text-sm text-green-600 mt-1">Positive Reviews</p>
          </div>
          <div class="bg-purple-100 p-4 rounded-xl text-center">
            <p class="text-3xl font-bold text-purple-800">51</p>
            <p class="text-sm text-purple-600 mt-1">Helpful Votes</p>
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
