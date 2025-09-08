<template>
  <RewviewStats />

  <div class="md:p-6">
    <div class="max-w-7xl mx-auto mt-12 mb-12">
      <div class="bg-white rounded-xl shadow-lg p-6 space-y-6">
        <div
          class="flex flex-col sm:flex-row sm:items-center justify-between pb-4 border-b border-gray-200"
        >
          <h2 class="text-2xl font-semibold text-gray-900 mb-4 sm:mb-0">Student Reviews</h2>
          <div class="flex space-x-3">
            <div class="relative inline-block text-left">
              <button
                @click="toggleRatingsDropdown"
                class="flex items-center px-4 py-2 rounded-full border border-gray-300 bg-white text-gray-600 shadow-sm transition-colors duration-200 cursor-pointer hover:bg-[#5f22d9] hover:text-white hover:border-[#5f22d9] focus:outline-none focus:ring-2 focus:ring-[#5f22d9] focus:border-transparent"
              >
                <span>{{ selectedRating }}</span>
                <svg
                  :class="{ 'transform rotate-180': ratingsOpen }"
                  class="ml-2 -mr-1 h-5 w-5 transition-transform duration-200"
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
                class="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-xl border border-gray-300 bg-white shadow-lg focus:outline-none"
              >
                <div class="p-1" role="none">
                  <a
                    v-for="rating in ratingsOptions"
                    :key="rating"
                    href="#"
                    @click.prevent="selectRating(rating)"
                    class="block px-4 py-2 text-sm text-gray-700 transition-colors duration-200 hover:bg-purple-50 hover:text-[#5f22d9] rounded-lg"
                    >{{ rating }}</a
                  >
                </div>
              </div>
            </div>

            <div class="relative inline-block text-left">
              <button
                @click="toggleSortDropdown"
                class="flex items-center px-4 py-2 rounded-full border border-gray-300 bg-white text-gray-600 shadow-sm transition-colors duration-200 cursor-pointer hover:bg-[#5f22d9] hover:text-white hover:border-[#5f22d9] focus:outline-none focus:ring-2 focus:ring-[#5f22d9] focus:border-transparent"
              >
                <span>{{ selectedSort }}</span>
                <svg
                  :class="{ 'transform rotate-180': sortOpen }"
                  class="ml-2 -mr-1 h-5 w-5 transition-transform duration-200"
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
                class="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-xl border border-gray-300 bg-white shadow-lg focus:outline-none"
              >
                <div class="p-1" role="none">
                  <a
                    v-for="sort in sortOptions"
                    :key="sort"
                    href="#"
                    @click.prevent="selectSort(sort)"
                    class="block px-4 py-2 text-sm text-gray-700 transition-colors duration-200 hover:bg-purple-50 hover:text-[#5f22d9] rounded-lg"
                    >{{ sort }}</a
                  >
                </div>
              </div>
            </div>
          </div>
        </div>

        <ReviewCard />
        <ReviewCard />
        <ReviewCard />
        <ReviewCard />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue';
  import RewviewStats from '../components/rewiev/ReviewStats.vue';
  import ReviewCard from '../components/rewiev/ReviewCard.vue';

  const ratingsOpen = ref(false);
  const selectedRating = ref('All Ratings');
  const ratingsOptions = ['All Ratings', '5 Stars', '4 Stars', '3 Stars', '2 Stars', '1 Star'];

  const sortOpen = ref(false);
  const selectedSort = ref('Most Recent');
  const sortOptions = ['Most Recent', 'Highest Rated', 'Lowest Rated'];

  function toggleRatingsDropdown() {
    ratingsOpen.value = !ratingsOpen.value;
    sortOpen.value = false; // Închide celălalt dropdown
  }

  function selectRating(rating) {
    selectedRating.value = rating;
    ratingsOpen.value = false;
  }

  function toggleSortDropdown() {
    sortOpen.value = !sortOpen.value;
    ratingsOpen.value = false; // Închide celălalt dropdown
  }

  function selectSort(sort) {
    selectedSort.value = sort;
    sortOpen.value = false;
  }
</script>
