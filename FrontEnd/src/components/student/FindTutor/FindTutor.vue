<script setup lang="ts">
import { ref } from 'vue';
import SearchBar from './SearchBar.vue';
import TutorFilters from './TutorFilters.vue';
import TutorList from './TutorList.vue';
// import { useFindTutorStore } from '../../../store/findTutorStore';

// const findTutorStore = useFindTutorStore();
const showFilters = ref(false);
</script>

<template>
  <div class="container px-4 mx-auto">

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
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
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
</template>