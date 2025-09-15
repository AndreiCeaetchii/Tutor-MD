<template>
  <div class="relative inline-block w-full text-center">
    <button
      @click.prevent="showDropdown = !showDropdown"
      type="button"
      class="w-full text-[#5f22d9] font-semibold py-2 px-4 border border-[#5f22d9] rounded-full hover:bg-purple-50 transition-colors"
    >
      {{ placeholder }}
    </button>
    <div
      v-if="showDropdown"
      class="absolute z-10 mt-2 w-full bg-white border border-gray-300 rounded-lg shadow-lg max-h-48 overflow-y-auto"
    >
      <div
        v-for="option in options"
        :key="option"
        @click="selectOption(option)"
        class="p-2 cursor-pointer text-sm text-gray-800 hover:bg-purple-100 hover:text-purple-700 transition-colors"
      >
        {{ option }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue';

  const props = defineProps({
    options: {
      type: Array,
      required: true,
    },
    placeholder: {
      type: String,
      default: 'Selectează',
    },
  });

  const emit = defineEmits(['update:modelValue']);

  const showDropdown = ref(false);

  const selectOption = (option: string) => {
    emit('update:modelValue', option);
    showDropdown.value = false;
  };
</script>
