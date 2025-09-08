<template>
  <div class="content-container">
    <ReviewStats />
    <div class="my-6">
      <div class="p-6 bg-white shadow-lg rounded-2xl md:p-8">
        <div
          class="flex flex-col justify-between pb-4 mb-6 border-b border-gray-200 sm:flex-row sm:items-center"
        >
          <h2 class="mb-4 text-xl font-semibold text-gray-900 sm:mb-0">Student Reviews</h2>
          <div class="flex space-x-3">
            <div class="relative inline-block text-left">
              <button
                @click="toggleRatingsDropdown"
                class="flex items-center px-4 py-2 text-gray-600 transition-colors duration-200 bg-white border border-gray-300 rounded-full shadow-sm cursor-pointer hover:bg-purple-600 hover:text-white hover:border-purple-600 focus:outline-none focus:ring-2 focus:ring-purple-600 focus:border-transparent"
              >
                <span>{{ selectedRating }}</span>
                <svg
                  :class="{ 'transform rotate-180': ratingsOpen }"
                  class="w-5 h-5 ml-2 -mr-1 transition-transform duration-200"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                  aria-hidden="true"
                >
                  <path
                    fill-rule="evenodd"
                    d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                    clip-rule="evenodd"
                  />
                </svg>
              </button>
              <div
                v-if="ratingsOpen"
                class="absolute right-0 z-10 w-56 mt-2 origin-top-right bg-white border border-gray-300 shadow-lg rounded-xl focus:outline-none"
              >
                <div class="p-1" role="none">
                  <a
                    v-for="rating in ratingsOptions"
                    :key="rating"
                    href="#"
                    @click.prevent="selectRating(rating)"
                    class="block px-4 py-2 text-sm text-gray-700 transition-colors duration-200 rounded-lg hover:bg-purple-50 hover:text-purple-600"
                    >{{ rating }}</a
                  >
                </div>
              </div>
            </div>

            <div class="relative inline-block text-left">
              <button
                @click="toggleSortDropdown"
                class="flex items-center px-4 py-2 text-gray-600 transition-colors duration-200 bg-white border border-gray-300 rounded-full shadow-sm cursor-pointer hover:bg-purple-600 hover:text-white hover:border-purple-600 focus:outline-none focus:ring-2 focus:ring-purple-600 focus:border-transparent"
              >
                <span>{{ selectedSort }}</span>
                <svg
                  :class="{ 'transform rotate-180': sortOpen }"
                  class="w-5 h-5 ml-2 -mr-1 transition-transform duration-200"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                  aria-hidden="true"
                >
                  <path
                    fill-rule="evenodd"
                    d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                    clip-rule="evenodd"
                  />
                </svg>
              </button>
              <div
                v-if="sortOpen"
                class="absolute right-0 z-10 w-56 mt-2 origin-top-right bg-white border border-gray-300 shadow-lg rounded-xl focus:outline-none"
              >
                <div class="p-1" role="none">
                  <a
                    v-for="sort in sortOptions"
                    :key="sort"
                    href="#"
                    @click.prevent="selectSort(sort)"
                    class="block px-4 py-2 text-sm text-gray-700 transition-colors duration-200 rounded-lg hover:bg-purple-50 hover:text-purple-600"
                    >{{ sort }}</a
                  >
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="space-y-6">
          <ReviewCard />
          <ReviewCard />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue';
  import ReviewStats from '../review/ReviewStats.vue';
  import ReviewCard from '../review/ReviewCard.vue';

  const ratingsOpen = ref(false);
  const selectedRating = ref('All Ratings');
  const ratingsOptions = ['All Ratings', '5 Stars', '4 Stars', '3 Stars', '2 Stars', '1 Star'];

  const sortOpen = ref(false);
  const selectedSort = ref('Most Recent');
  const sortOptions = ['Most Recent', 'Highest Rated', 'Lowest Rated'];

  function toggleRatingsDropdown() {
    ratingsOpen.value = !ratingsOpen.value;
    sortOpen.value = false;
  }

  function selectRating(rating: string) {
    selectedRating.value = rating;
    ratingsOpen.value = false;
  }

  function toggleSortDropdown() {
    sortOpen.value = !sortOpen.value;
    ratingsOpen.value = false;
  }

  function selectSort(sort: string) {
    selectedSort.value = sort;
    sortOpen.value = false;
  }
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