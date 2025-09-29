<template>
  <div
    class="bg-gradient-to-r from-blue-50 to-purple-50 border border-blue-100 rounded-lg shadow-md flex items-center justify-between p-4 cursor-pointer transition-transform duration-300 hover:scale-[1.02]"
  >
    <div class="flex items-center space-x-3">
      <div
        class="w-10 h-10 flex items-center justify-center bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] rounded-lg text-white"
      >
        <font-awesome-icon :icon="['fas', iconName]" />
      </div>
      <div>
        <div class="text-gray-800 font-medium">{{ subject }}</div>
        <div class="text-xs text-gray-500">{{ availability }}</div>
      </div>
    </div>
    <div class="text-lg font-bold text-[#5f22d9] text-right">
      {{ price }}<br />
      <span class="text-sm font-normal text-gray-500">/hour</span>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { defineProps, computed } from 'vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { faCalculator, faAtom, faLaptopCode, faBook } from '@fortawesome/free-solid-svg-icons';

  library.add(faCalculator, faAtom, faLaptopCode, faBook);

  const props = defineProps({
    subject: String,
    price: String,
    availability: String,
  });

  const iconName = computed(() => {
    switch (props.subject.toLowerCase()) {
      case 'matematică':
        return 'calculator';
      case 'fizică':
        return 'atom';
      case 'informatică':
        return 'laptop-code';
      default:
        return 'book';
    }
  });
</script>
