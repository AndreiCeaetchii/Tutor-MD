<template>
  <div
    class="bg-gradient-to-r from-blue-50 to-purple-50 border border-blue-100 rounded-lg shadow-md flex items-center justify-between p-4 cursor-pointer"
  >
    <div class="flex items-center space-x-3">
      <div
        class="w-10 h-10 flex items-center justify-center bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] rounded-lg text-white"
      >
        <font-awesome-icon :icon="['fas', iconName]" />
      </div>
      <div>
        <div class="text-gray-800 font-medium">{{ subject.name }}</div>
      </div>
    </div>

    <div class="flex items-center space-x-4">
      <div class="w-20">
        <span class="text-xs text-gray-400">Price</span>
        <input
          :value="subject.price"
          @input="$emit('update:price', $event.target.value)"
          type="number"
          min="0"
          class="w-full text-base font-bold text-right bg-white text-gray-800 rounded-lg px-1 py-1 border border-gray-300 focus:outline-none focus:ring-1 focus:ring-[#5f22d9]"
        />
      </div>
      <div class="w-16">
        <span class="text-xs text-gray-400">Currency</span>
        <DropdownSelect
          :options="availableCurrencies"
          :placeholder="subject.currency"
          :modelValue="subject.currency"
          @update:modelValue="$emit('update:currency', $event)"
        />
      </div>
      <button @click.prevent="$emit('remove')" class="text-red-500 hover:text-red-700 h-5">
        <font-awesome-icon :icon="['fas', 'trash']" />
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { defineProps, defineEmits, computed } from 'vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import {
    faTrash,
    faCalculator,
    faAtom,
    faLaptopCode,
    faBook,
  } from '@fortawesome/free-solid-svg-icons';
  import DropdownSelect from './DropdownSelect.vue';

  library.add(faTrash, faCalculator, faAtom, faLaptopCode, faBook);

  const props = defineProps({
    subject: {
      type: Object,
      required: true,
    },
    index: {
      type: Number,
      required: true,
    },
  });

  const availableCurrencies = ['RON', 'EUR', 'USD', 'MDL', 'IDK'];

  const emit = defineEmits(['remove', 'update:currency', 'update:price']);

  const iconName = computed(() => {
    switch (props.subject.name.toLowerCase()) {
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
