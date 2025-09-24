<script setup lang="ts">
  import { ref, watch, onMounted, onUnmounted } from 'vue';
  import { useFindTutorStore } from '../../../store/findTutorStore';
  import debounce from 'lodash/debounce';

  const tutorStore = useFindTutorStore();

  const localPriceMin = ref(tutorStore.priceMin);
  const localPriceMax = ref(tutorStore.priceMax);

  const educationLevels = [
    'Primary (Class 1-5)',
    'Middle (Class 6-10)',
    'Intermediate',
    'High school',
  ];

  const showDropdown = ref(false);

  const subjects = [
    'English',
    'Informatics',
    'Mathematics',
    'Science',
    'Physics',
    'Chemistry',
    'Biology',
    'History',
    'Geography',
    'Economics',
    'Art',
    'Music',
    'Foreign Languages',
    'Literature',
    'Computer Science',
    'Physical Education',
  ];

  const ratings = [5, 4, 3, 2, 1];

  const services = ['My home', "Student's home", 'Online'];

  const debouncedUpdatePriceRange = debounce(() => {
    tutorStore.priceMin = localPriceMin.value;
    tutorStore.priceMax = localPriceMax.value;
  }, 500);

  watch([localPriceMin, localPriceMax], () => {
    debouncedUpdatePriceRange();
  });

  watch(
    () => tutorStore.priceMin,
    (newVal) => {
      localPriceMin.value = newVal;
    },
  );

  watch(
    () => tutorStore.priceMax,
    (newVal) => {
      localPriceMax.value = newVal;
    },
  );

  function selectEducationLevel(level: string) {
    tutorStore.educationLevel = level;
    showDropdown.value = false;
  }

  function handleClickOutside(event: MouseEvent) {
    const dropdown = document.getElementById('education-dropdown');
    if (dropdown && !dropdown.contains(event.target as Node)) {
      showDropdown.value = false;
    }
  }

  onMounted(() => {
    if (typeof window !== 'undefined') {
      window.addEventListener('mousedown', handleClickOutside);
    }
  });

  onUnmounted(() => {
    if (typeof window !== 'undefined') {
      window.removeEventListener('mousedown', handleClickOutside);
    }
  });
</script>

<template>
  <div class="w-full p-3 bg-white rounded-lg shadow md:p-4">
    <div class="relative mb-5 md:mb-6" id="education-dropdown">
      <label class="block mb-1 font-semibold text-gray-700 md:mb-2">Education level</label>
      <button
        class="flex items-center justify-between w-full px-3 py-1 text-gray-700 bg-white border rounded cursor-pointer md:px-4 md:py-2 focus:outline-none"
        @click="showDropdown = !showDropdown"
        type="button"
      >
        <span class="text-sm truncate md:text-base">{{ tutorStore.educationLevel || 'Select education level' }}</span>
        <svg
          class="flex-shrink-0 w-4 h-4 ml-2 text-gray-400 md:w-5 md:h-5"
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
      <div
        v-if="showDropdown"
        class="absolute left-0 z-10 w-full mt-1 overflow-y-auto bg-white border rounded-lg shadow-lg md:mt-2 max-h-48 md:max-h-56"
      >
        <ul>
          <li
            v-for="level in educationLevels"
            :key="level"
            @click="selectEducationLevel(level)"
            class="px-3 py-1 text-xs text-gray-700 cursor-pointer md:px-4 md:py-2 md:text-sm lg:text-base hover:bg-gray-100"
          >
            {{ level }}
          </li>
        </ul>
      </div>
    </div>

    <div class="mb-5 md:mb-6">
      <label class="block mb-1 font-semibold text-gray-700 md:mb-2">Choose subjects</label>
      <div class="flex flex-col gap-1 pr-1 overflow-y-auto md:pr-2 max-h-36 md:max-h-40 lg:max-h-48">
        <label
          v-for="subject in subjects"
          :key="subject"
          class="flex items-center gap-1 py-1 text-xs text-gray-600 cursor-pointer md:gap-2 md:text-sm lg:text-base"
        >
          <input
            type="checkbox"
            :value="subject"
            v-model="tutorStore.selectedSubjects"
            class="flex-shrink-0 w-3 h-3 rounded md:w-4 md:h-4 accent-purple-600"
          />
          <span class="truncate">{{ subject }}</span>
        </label>
      </div>
    </div>

    <div class="mb-5 md:mb-6">
      <label class="block mb-1 font-semibold text-gray-700 md:mb-2">Price range</label>
      <div class="flex items-center w-full">
        <input
          type="number"
          v-model="localPriceMin"
          class="w-full px-2 py-1 text-sm border rounded flex-1 min-w-[60px] md:text-base"
          min="0"
          placeholder="Min"
        />
        <span class="flex-shrink-0 mx-2">-</span>
        <input
          type="number"
          v-model="localPriceMax"
          class="w-full px-2 py-1 text-sm border rounded flex-1 min-w-[60px] md:text-base"
          min="0"
          placeholder="Max"
        />
      </div>
    </div>

    <div class="mb-5 md:mb-6">
      <label class="block mb-1 font-semibold text-gray-700 md:mb-2">Rating</label>
      <div class="flex flex-col gap-1 md:gap-2">
        <label v-for="star in ratings" :key="star" class="flex items-center gap-1 cursor-pointer md:gap-2">
          <input
            type="checkbox"
            :value="star"
            v-model="tutorStore.selectedRatings"
            class="flex-shrink-0 w-3 h-3 rounded md:w-4 md:h-4 accent-purple-600"
          />
          <span class="flex flex-wrap items-center">
            <span
              v-for="i in 5"
              :key="i"
              class="text-sm md:text-lg lg:text-xl"
              :class="i <= star ? 'text-yellow-400' : 'text-gray-300'"
              >&#9733;</span
            >
            <span class="ml-1 text-xs font-semibold text-gray-700 md:ml-2 md:text-sm">{{ star }}.0</span>
            <span class="ml-0.5 md:ml-1 text-xs md:text-sm text-gray-400">/5.0</span>
          </span>
        </label>
      </div>
    </div>

    <div class="mb-5 md:mb-6">
      <label class="block mb-1 font-semibold text-gray-700 md:mb-2">Location</label>
      <input
        type="text"
        v-model="tutorStore.location"
        class="w-full px-2 py-1 mb-1 text-sm border rounded md:px-3 md:py-2 md:mb-2 md:text-base"
        placeholder="Enter city or country"
      />
    </div>

    <div class="mb-5 md:mb-6">
      <label class="block mb-1 font-semibold text-gray-700 md:mb-2">Teaching Services</label>
      <div class="flex flex-col gap-1 md:gap-2">
        <label
          v-for="service in services"
          :key="service"
          class="flex items-center gap-1 text-xs text-gray-600 cursor-pointer md:gap-2 md:text-sm lg:text-base"
        >
          <input
            type="checkbox"
            :value="service"
            v-model="tutorStore.serviceTypes"
            class="flex-shrink-0 w-3 h-3 rounded md:w-4 md:h-4 accent-purple-600"
          />
          {{ service }}
        </label>
      </div>
    </div>

    <div class="flex flex-col gap-2 mt-3 md:flex-row md:mt-4">
      <button
        @click="tutorStore.clearFilters()"
        class="py-1 text-sm text-purple-600 transition bg-purple-200 rounded md:py-2 md:px-2 md:text-base hover:bg-purple-200"
      >
        Clear filters
      </button>
    </div>
  </div>
</template>
