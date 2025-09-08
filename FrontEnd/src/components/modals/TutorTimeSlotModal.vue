<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue';

const props = defineProps<{
  isOpen: boolean;
}>();

const emit = defineEmits(['close', 'save']);

// Time options for dropdown
const timeOptions = [
  '08:00', '08:30', '09:00', '09:30', '10:00', '10:30',
  '11:00', '11:30', '12:00', '12:30', '13:00', '13:30',
  '14:00', '14:30', '15:00', '15:30', '16:00', '16:30',
  '17:00', '17:30', '18:00', '18:30', '19:00', '19:30',
  '20:00', '20:30', '21:00'
];

const showStartTimeDropdown = ref(false);
const showEndTimeDropdown = ref(false);
const startTime = ref('');
const endTime = ref('');
const startTimeText = ref('Select start time');
const endTimeText = ref('Select end time');

const closeModal = () => {
  emit('close');
  resetForm();
};

const saveTimeSlot = () => {
  if (startTime.value && endTime.value) {
    emit('save', {
      startTime: startTime.value,
      endTime: endTime.value
    });
    resetForm();
  }
};

const resetForm = () => {
  startTime.value = '';
  endTime.value = '';
  startTimeText.value = 'Select start time';
  endTimeText.value = 'Select end time';
  showStartTimeDropdown.value = false;
  showEndTimeDropdown.value = false;
};

const selectStartTime = (time: string) => {
  startTime.value = time;
  startTimeText.value = time;
  showStartTimeDropdown.value = false;
};

const selectEndTime = (time: string) => {
  endTime.value = time;
  endTimeText.value = time;
  showEndTimeDropdown.value = false;
};

const toggleStartTimeDropdown = () => {
  showStartTimeDropdown.value = !showStartTimeDropdown.value;
  if (showStartTimeDropdown.value) {
    showEndTimeDropdown.value = false;
  }
};

const toggleEndTimeDropdown = () => {
  showEndTimeDropdown.value = !showEndTimeDropdown.value;
  if (showEndTimeDropdown.value) {
    showStartTimeDropdown.value = false;
  }
};

// Filter end times to only show times after the selected start time
const filteredEndTimes = computed(() => {
  if (!startTime.value) return timeOptions;
  
  const startIndex = timeOptions.findIndex(time => time === startTime.value);
  if (startIndex === -1) return timeOptions;
  
  return timeOptions.slice(startIndex + 1);
});

// Close dropdowns when clicking outside
const startTimeRef = ref<HTMLElement | null>(null);
const endTimeRef = ref<HTMLElement | null>(null);

onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside);
});

const handleClickOutside = (event: MouseEvent) => {
  if (startTimeRef.value && !startTimeRef.value.contains(event.target as Node)) {
    showStartTimeDropdown.value = false;
  }
  if (endTimeRef.value && !endTimeRef.value.contains(event.target as Node)) {
    showEndTimeDropdown.value = false;
  }
};
</script>

<template>
  <!-- Modal Overlay -->
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <!-- Modal Content -->
    <div class="relative w-full max-w-md p-6 mx-auto bg-white rounded-lg shadow-xl">
      <!-- Close Button -->
      <button @click="closeModal" class="absolute text-gray-500 top-4 right-4 hover:text-gray-700">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
      
      <!-- Modal Header -->
      <h2 class="mb-6 text-xl font-semibold text-gray-800">Add Time Slot</h2>
      
      <!-- Start Time Dropdown -->
      <div class="mb-6">
        <label class="block mb-2 text-sm font-medium text-gray-700">Start Time</label>
        <div ref="startTimeRef" class="relative">
          <button 
            @click="toggleStartTimeDropdown"
            class="flex items-center justify-between w-full px-4 py-2 text-left bg-white border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <span class="block text-gray-700 truncate">{{ startTimeText }}</span>
            <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
            </svg>
          </button>
          
          <div 
            v-show="showStartTimeDropdown"
            class="absolute z-10 w-full mt-1 overflow-y-auto bg-white border border-gray-300 rounded-md shadow-lg max-h-60"
          >
            <ul class="py-1">
              <li 
                v-for="time in timeOptions" 
                :key="`start-${time}`"
                @click="selectStartTime(time)"
                class="px-4 py-2 text-sm text-gray-700 cursor-pointer hover:bg-gray-100"
              >
                {{ time }}
              </li>
            </ul>
          </div>
        </div>
      </div>
      
      <!-- End Time Dropdown -->
      <div class="mb-8">
        <label class="block mb-2 text-sm font-medium text-gray-700">End Time</label>
        <div ref="endTimeRef" class="relative">
          <button 
            @click="toggleEndTimeDropdown"
            class="flex items-center justify-between w-full px-4 py-2 text-left bg-white border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <span class="block text-gray-700 truncate">{{ endTimeText }}</span>
            <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
            </svg>
          </button>
          
          <div 
            v-show="showEndTimeDropdown"
            class="absolute z-10 w-full mt-1 overflow-y-auto bg-white border border-gray-300 rounded-md shadow-lg max-h-60"
          >
            <ul class="py-1">
              <li 
                v-for="time in filteredEndTimes" 
                :key="`end-${time}`"
                @click="selectEndTime(time)"
                class="px-4 py-2 text-sm text-gray-700 cursor-pointer hover:bg-gray-100"
              >
                {{ time }}
              </li>
            </ul>
          </div>
        </div>
      </div>
      
      <!-- Add Time Slot Button -->
      <button 
        @click="saveTimeSlot"
        :disabled="!startTime || !endTime"
        class="w-full py-3 text-white transition-colors bg-orange-500 rounded-md hover:bg-orange-600 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        Add Time Slot
      </button>
    </div>
  </div>
</template>

<style scoped>
.max-h-60 {
  max-height: 15rem;
}
</style>