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
      class="absolute z-50 w-full mt-2 overflow-y-auto bg-white border border-gray-300 rounded-lg shadow-lg max-h-48"
    >
      <div
        v-for="option in options"
        :key="option"
        @click="selectOption(option)"
        class="p-2 text-sm text-gray-800 transition-colors cursor-pointer hover:bg-purple-100 hover:text-purple-700"
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
      type: Array as () => string[],
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
