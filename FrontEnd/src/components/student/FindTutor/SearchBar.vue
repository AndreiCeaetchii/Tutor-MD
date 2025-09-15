<script setup lang="ts">
  import { ref, onMounted, onBeforeUnmount } from 'vue';

  const search = ref('');
  const category = ref('');
  const categories = [
    'Mathematics',
    'English',
    'Science',
    'Social studies',
    'Drawing',
    'Music',
    'Foreign languages',
    'Computer science',
    // ...alte categorii
  ];

  const tags = ref([
    'Pre-School',
    'Middle Class 6-10',
    'Intermediate',
    '5.0 Stars',
    'Online bookings',
    'Male only',
    // ...alte tag-uri
  ]);

  const showDropdown = ref(false);
  const categoryBtnRef = ref<HTMLElement | null>(null);

  function onSearch() {
    // Emit event sau logica de search
  }

  function removeTag(tag: string) {
    tags.value = tags.value.filter((t) => t !== tag);
  }

  function selectCategory(cat: string) {
    category.value = cat;
    showDropdown.value = false;
  }

  // Close dropdown on outside click
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
    <div class="flex flex-col gap-3 md:flex-row md:items-center">
      <div class="relative flex flex-1">
        <!-- Search Icon -->
        <span class="absolute text-gray-400 -translate-y-1/2 left-3 top-1/2">
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
          v-model="search"
          type="text"
          class="w-full py-2 pl-10 pr-4 border rounded-l focus:outline-none"
          placeholder="What do you want to explore?"
        />
        <!-- Select Category & Search Button -->
        <div class="absolute flex items-center gap-4 -translate-y-1/2 right-2 top-1/2">
          <!-- Category Dropdown Trigger -->
          <div class="relative ml-4" ref="categoryBtnRef">
            <button
              class="flex items-center w-56 gap-2 px-4 py-2 text-gray-500 bg-white hover:text-gray-700 focus:outline-none"
              @click="showDropdown = !showDropdown"
              type="button"
              style="border: none; box-shadow: none"
            >
              <!-- Stack Icon -->
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
              <span class="flex-1 text-left whitespace-nowrap">
                {{ category || 'Select category' }}
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
            <!-- Dropdown -->
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
          <!-- Search Button -->
          <button
            class="px-5 py-1 font-semibold text-white transition bg-orange-500 rounded hover:bg-orange-600"
            @click="onSearch"
          >
            Search now
          </button>
        </div>
      </div>
    </div>
    <div class="flex flex-wrap gap-2 mt-4">
      <span
        v-for="tag in tags"
        :key="tag"
        class="flex items-center gap-1 px-3 py-1 text-sm text-gray-700 bg-gray-100 rounded-full"
      >
        {{ tag }}
        <button class="ml-1 text-gray-400 hover:text-gray-600" @click="removeTag(tag)">
          &times;
        </button>
      </span>
    </div>
  </div>
</template>
