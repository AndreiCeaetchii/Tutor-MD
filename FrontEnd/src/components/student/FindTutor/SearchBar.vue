<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, watch } from 'vue';
import { useFindTutorStore } from '../../../store/findTutorStore';

const tutorStore = useFindTutorStore();

const categories = [
  'Mathematics',
  'English',
  'Science',
  'Physics',
  'Chemistry',
  'Biology',
  'Geography',
  'History',
  'Foreign languages',
  'German',
  'French',
  'Russian',
  'Spanish',
  'Italian',
  'Computer science',
  'Economics',
  'Philosophy',
  'Psychology',
  'Sociology',
  'Physical Education',
  'Health Education',
  'Drawing',
  'Music',
  'Astronomy',
  'Literature',
  'Creative Writing',
  'Statistics'
];


const showDropdown = ref(false);
const categoryBtnRef = ref<HTMLElement | null>(null);

watch(() => tutorStore.searchQuery, () => {
  tutorStore.debouncedSearch();
});

watch(() => tutorStore.selectedCategory, () => {
  tutorStore.debouncedSearch();
});

function onSearch() {
  tutorStore.debouncedSearch();
}

function selectCategory(cat: string) {
  tutorStore.selectedCategory = cat;
  showDropdown.value = false;
}

function handleClickOutside(event: MouseEvent) {
  if (categoryBtnRef.value && !categoryBtnRef.value.contains(event.target as Node)) {
    showDropdown.value = false;
  }
}

onMounted(() => {
  document.addEventListener('mousedown', handleClickOutside);
});
onBeforeUnmount(() => {
  document.removeEventListener('mousedown', handleClickOutside);
});
</script>

<template>
  <div class="p-4 mb-4 bg-white rounded-lg shadow">
    <div class="flex flex-col sm:flex-row sm:items-center sm:space-x-2">
      <div class="relative flex flex-col flex-1 w-full sm:flex-row">
        <span class="absolute hidden text-gray-400 -translate-y-1/2 left-3 top-1/2 sm:block">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="w-5 h-5"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M21 21l-4.35-4.35m0 0A7.5 7.5 0 104.5 4.5a7.5 7.5 0 0012.15 12.15z"
            />
          </svg>
        </span>
        
        <div class="flex items-center w-full sm:hidden">
          <span class="text-gray-400">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-5 h-5"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M21 21l-4.35-4.35m0 0A7.5 7.5 0 104.5 4.5a7.5 7.5 0 0012.15 12.15z"
              />
            </svg>
          </span>
          <input
            v-model="tutorStore.searchQuery"
            type="text"
            class="w-full py-2 pl-2 border-b focus:outline-none focus:border-purple-500"
            placeholder="What do you want to explore?"
          />
        </div>
        
        <input
          v-model="tutorStore.searchQuery"
          type="text"
          class="hidden w-full py-2 pl-10 pr-4 border rounded-l sm:block focus:outline-none focus:ring-1 focus:ring-purple-500"
          placeholder="What do you want to explore?"
        />
        
        <div class="relative mt-3 sm:mt-0" ref="categoryBtnRef">
          <button
            class="flex items-center w-full gap-2 px-4 py-2 text-gray-500 bg-white border rounded-r sm:border-l-0 sm:w-56 hover:text-gray-700 focus:outline-none"
            @click="showDropdown = !showDropdown"
            type="button"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-5 h-5"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M4 17l8 4 8-4M4 12l8 4 8-4M4 7l8 4 8-4"
              />
            </svg>
            <span class="flex-1 text-left truncate whitespace-nowrap">
              {{ tutorStore.selectedCategory || 'Select category' }}
            </span>
            <svg class="w-4 h-4 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M19 9l-7 7-7-7"
              />
            </svg>
          </button>
          <div
            v-if="showDropdown"
            class="absolute left-0 z-10 w-full py-2 mt-2 overflow-y-auto bg-white border rounded-lg shadow-lg max-h-64"
          >
            <ul>
              <li
                v-for="cat in categories"
                :key="cat"
                @click="selectCategory(cat)"
                class="px-4 py-2 text-base text-gray-700 cursor-pointer hover:bg-gray-100"
              >
                {{ cat }}
              </li>
            </ul>
          </div>
        </div>
      </div>
      
      <button
        class="w-full px-5 py-2 mt-3 text-white transition bg-orange-500 rounded sm:w-auto sm:mt-0 sm:whitespace-nowrap hover:bg-orange-600"
        @click="onSearch"
      >
        Search now
      </button>
    </div>
  </div>
</template>
