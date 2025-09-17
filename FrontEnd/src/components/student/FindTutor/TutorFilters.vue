<script setup lang="ts">
import { ref } from 'vue';

const educationLevels = [
  'Primary (Class 1-5)',
  'Middle (Class 6-10)',
  'Intermediate',
  'High school'
];
const educationLevel = ref(educationLevels[0]);
const showDropdown = ref(false);

// Lista de subiecte mai mare
const subjects = ref(['English', 'Informatics', 'Mathematics', 'Science', 'Physics', 'Chemistry', 'Biology', 'History', 'Geography', 'Economics', 'Art', 'Music', 'Foreign Languages', 'Literature', 'Computer Science', 'Physical Education']);
const selectedSubjects = ref<string[]>([]);

const priceMin = ref(0);
const priceMax = ref(1000);

const ratings = ref([5, 4, 3, 2, 1]);
const selectedRatings = ref<number[]>([]);
const location = ref('');
const radius = ref(25);
const profilePhoto = ref(false);
const maleOnly = ref(false);
const femaleOnly = ref(false);

function applyFilters() {
  // Emit filters to parent or fetch tutors
}

function clearFilters() {
  educationLevel.value = educationLevels[0];
  selectedSubjects.value = [];
  priceMin.value = 0;
  priceMax.value = 1000;
  selectedRatings.value = [];
  location.value = '';
  radius.value = 25;
  profilePhoto.value = false;
  maleOnly.value = false;
  femaleOnly.value = false;
}

function selectEducationLevel(level: string) {
  educationLevel.value = level;
  showDropdown.value = false;
}

// Close dropdown on outside click
function handleClickOutside(event: MouseEvent) {
  const dropdown = document.getElementById('education-dropdown');
  if (dropdown && !dropdown.contains(event.target as Node)) {
    showDropdown.value = false;
  }
}
if (typeof window !== 'undefined') {
  window.addEventListener('mousedown', handleClickOutside);
}
</script>

<template>
  <div class="w-full p-4 bg-white rounded-lg shadow md:w-72">
    <!-- Education level custom dropdown -->
    <div class="relative mb-6" id="education-dropdown">
      <label class="block mb-2 font-semibold text-gray-700">Education level</label>
      <button
        class="flex items-center justify-between w-full px-4 py-2 text-gray-700 bg-white border rounded cursor-pointer focus:outline-none"
        @click="showDropdown = !showDropdown"
        type="button"
      >
        <span>{{ educationLevel }}</span>
        <svg class="w-5 h-5 ml-2 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
        </svg>
      </button>
      <div
        v-if="showDropdown"
        class="absolute left-0 z-10 w-full mt-2 overflow-y-auto bg-white border rounded-lg shadow-lg max-h-56"
      >
        <ul>
          <li
            v-for="level in educationLevels"
            :key="level"
            @click="selectEducationLevel(level)"
            class="px-6 py-3 text-base text-gray-700 cursor-pointer hover:bg-gray-100"
          >
            {{ level }}
          </li>
        </ul>
      </div>
    </div>

    <!-- Subjects cu scroll -->
    <div class="mb-6">
      <label class="block mb-2 font-semibold text-gray-700">Choose subjects</label>
      <div class="flex flex-col gap-2 pr-2 overflow-y-auto max-h-40">
        <label
          v-for="subject in subjects"
          :key="subject"
          class="flex items-center gap-2 text-gray-600 cursor-pointer"
        >
          <input
            type="checkbox"
            :value="subject"
            v-model="selectedSubjects"
            class="w-4 h-4 rounded accent-purple-600"
          />
          {{ subject }}
        </label>
      </div>
    </div>

    <!-- Price range simplu -->
    <div class="mb-6">
      <label class="block mb-2 font-semibold text-gray-700">Price range</label>
      <div class="flex items-center gap-2">
        <input type="number" v-model="priceMin" class="w-20 px-2 py-1 border rounded" min="0" placeholder="Min" />
        <span>-</span>
        <input type="number" v-model="priceMax" class="w-20 px-2 py-1 border rounded" min="0" placeholder="Max" />
      </div>
    </div>

    <!-- Rating -->
    <div class="mb-6">
      <label class="block mb-2 font-semibold text-gray-700">Rating</label>
      <div class="flex flex-col gap-2">
        <label
          v-for="star in ratings"
          :key="star"
          class="flex items-center gap-2 cursor-pointer"
        >
          <input
            type="checkbox"
            :value="star"
            v-model="selectedRatings"
            class="w-4 h-4 rounded accent-purple-600"
          />
          <span class="flex items-center">
            <span v-for="i in 5" :key="i" class="text-xl"
              :class="i <= star ? 'text-yellow-400' : 'text-gray-300'">&#9733;</span>
            <span class="ml-2 font-semibold text-gray-700">{{ star }}.0</span>
            <span class="ml-1 text-gray-400">/5.0</span>
          </span>
        </label>
      </div>
    </div>

    <!-- Location -->
    <div class="mb-6">
      <label class="block mb-2 font-semibold text-gray-700">Location</label>
      <input type="text" v-model="location" class="w-full px-3 py-2 mb-2 border rounded" placeholder="Enter city or country" />
    </div>
    
    <!-- Buttons -->
    <div class="flex gap-2 mt-4">
      <button @click="applyFilters" class="flex-1 py-2 text-white transition bg-purple-700 rounded hover:bg-purple-800">Apply filters</button>
      <button @click="clearFilters" class="flex-1 py-2 text-gray-700 transition bg-gray-100 rounded hover:bg-gray-200">Clear all filters</button>
    </div>
  </div>
</template>