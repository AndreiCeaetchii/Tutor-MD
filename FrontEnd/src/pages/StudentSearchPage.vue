<script setup lang="ts">
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';
  import SearchBar from '../components/student/FindTutor/SearchBar.vue';
  import TutorFilters from '../components/student/FindTutor/TutorFilters.vue';
  import TutorList from '../components/student/FindTutor/TutorList.vue';

  const router = useRouter();
  const showFilters = ref(false);
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="p-4 mx-auto md:p-8 max-w-7xl">
      <div class="mb-6">
        <h1 class="mb-2 text-2xl font-bold">
          Find the perfect <span class="text-[#5f22d9]">tutor</span> for your
          <span class="text-[#5f22d9]">learning needs</span>
        </h1>
        <p class="text-gray-600">Search and filter to connect with experts in any subject.</p>
      </div>

      <div class="w-full mt-8 mb-6">
        <SearchBar />
      </div>

      <div class="flex flex-col gap-8 md:flex-row">
        <div class="w-full md:w-1/4">
          <button
            class="flex items-center justify-center w-full py-2 mb-4 text-purple-700 bg-white border border-purple-700 rounded-lg md:hidden"
            @click="showFilters = !showFilters"
          >
            <span>{{ showFilters ? 'Hide Filters' : 'Show Filters' }}</span>
            <svg
              class="w-5 h-5 ml-2"
              :class="showFilters ? 'transform rotate-180' : ''"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M19 9l-7 7-7-7"
              />
            </svg>
          </button>

          <div :class="{ 'hidden md:block': !showFilters }">
            <TutorFilters />
          </div>
        </div>

        <div class="w-full md:w-3/4">
          <TutorList />
        </div>
      </div>
    </div>
  </div>
</template>
