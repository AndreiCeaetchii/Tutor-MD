<script setup lang="ts">
import { ref, watch } from 'vue';

const props = defineProps({
  show: {
    type: Boolean,
    required: true
  },
  message: {
    type: String,
    default: ''
  },
  type: {
    type: String,
    default: 'success',
    validator: (val: string) => ['success', 'error', 'warning', 'info'].includes(val)
  },
  duration: {
    type: Number,
    default: 3000
  },
  position: {
    type: String,
    default: 'top-right'
  }
});

const emit = defineEmits(['close']);
const visible = ref(props.show);

watch(() => props.show, (newVal) => {
  visible.value = newVal;
  
  if (newVal && props.duration > 0) {
    setTimeout(() => {
      visible.value = false;
      emit('close');
    }, props.duration);
  }
});

const getTypeClasses = () => {
  switch (props.type) {
    case 'success':
      return 'bg-green-100 border-green-500 text-green-800';
    case 'error':
      return 'bg-red-100 border-red-500 text-red-800';
    case 'warning':
      return 'bg-yellow-100 border-yellow-500 text-yellow-800';
    case 'info':
      return 'bg-blue-100 border-blue-500 text-blue-800';
    default:
      return 'bg-green-100 border-green-500 text-green-800';
  }
};

const getPositionClasses = () => {
  switch (props.position) {
    case 'top-right': return 'top-6 right-6';
    case 'top-left': return 'top-6 left-6';
    case 'bottom-right': return 'bottom-6 right-6';
    case 'bottom-left': return 'bottom-6 left-6';
    default: return 'top-6 right-6';
  }
};

const handleClose = () => {
  visible.value = false;
  emit('close');
};
</script>

<template>
  <transition
    enter-active-class="transition duration-300 ease-out"
    enter-from-class="transform scale-95 opacity-0"
    enter-to-class="transform scale-100 opacity-100"
    leave-active-class="transition duration-200 ease-in"
    leave-from-class="transform scale-100 opacity-100"
    leave-to-class="transform scale-95 opacity-0"
  >
    <div
      v-if="visible"
      :class="[
        'fixed z-50 p-4 rounded-md shadow-md border-l-4 flex items-center',
        getTypeClasses(),
        getPositionClasses()
      ]"
    >
      <div v-if="type === 'success'" class="mr-3 text-green-500">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
        </svg>
      </div>

      <div v-else-if="type === 'error'" class="mr-3 text-red-500">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
        </svg>
      </div>

      <div v-else-if="type === 'warning'" class="mr-3 text-yellow-500">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"></path>
        </svg>
      </div>

      <div v-else class="mr-3 text-blue-500">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
        </svg>
      </div>

      <p>{{ message }}</p>
      <button @click="handleClose" class="ml-auto text-gray-600 hover:text-gray-800">
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
        </svg>
      </button>
    </div>
  </transition>
</template>