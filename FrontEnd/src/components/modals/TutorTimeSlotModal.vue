<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, watch } from 'vue';
import { useCalendarStore } from '../../store/calendarStore';

const props = defineProps<{
  isOpen: boolean;
  editingSlot?: {
    id: string;
    startTime: string;
    endTime: string;
  } | null;
  lastEndTime?: string;
}>();

const emit = defineEmits(['close', 'save']);
const store = useCalendarStore();

const generateTimeOptions = () => {
  const options = [];
  for (let hour = 8; hour < 24; hour++) {
    for (let minute = 0; minute < 60; minute += 30) {
      if (hour === 22 && minute > 0) continue;
      const formattedHour = hour.toString().padStart(2, '0');
      const formattedMinute = minute.toString().padStart(2, '0');
      options.push(`${formattedHour}:${formattedMinute}`);
    }
  }
  return options;
};

const timeOptions = generateTimeOptions();

const showStartTimeDropdown = ref(false);
const showEndTimeDropdown = ref(false);
const startTime = ref('');
const endTime = ref('');
const startTimeText = ref('Select start time');
const endTimeText = ref('Select end time');
const errorMessage = ref('');

watch(() => props.isOpen, (isOpen) => {
  if (isOpen) {
    errorMessage.value = '';
    
    if (props.editingSlot) {
      startTime.value = props.editingSlot.startTime;
      startTimeText.value = props.editingSlot.startTime;
      endTime.value = props.editingSlot.endTime;
      endTimeText.value = props.editingSlot.endTime;
    } else if (props.lastEndTime) {
      startTime.value = props.lastEndTime;
      startTimeText.value = props.lastEndTime;
      
      const startIndex = timeOptions.findIndex(t => t === props.lastEndTime);
      if (startIndex !== -1 && startIndex < timeOptions.length - 1) {
        endTime.value = timeOptions[startIndex + 1];
      } else {
        endTime.value = '09:00';
      }
      endTimeText.value = endTime.value;
    } else {
      startTime.value = '08:00';
      startTimeText.value = '08:00';
      endTime.value = '09:00';
      endTimeText.value = '09:00';
    }
  }
}, { immediate: true });

const closeModal = () => {
  emit('close');
  resetForm();
};

const saveTimeSlot = () => {
  errorMessage.value = '';
  
  if (!startTime.value || !endTime.value) {
    errorMessage.value = 'Please select both start and end times';
    return;
  }
  
  const startMinutes = store.timeToMinutes(startTime.value);
  const endMinutes = store.timeToMinutes(endTime.value);
  
  if (endMinutes <= startMinutes) {
    errorMessage.value = 'End time must be after start time';
    return;
  }
  
  try {
    const slotData = { startTime: startTime.value, endTime: endTime.value };
    
    if (props.editingSlot) {
      if (store.checkOverlap(slotData, props.editingSlot.id)) {
        errorMessage.value = 'This time slot would overlap with another existing slot';
        return;
      }
    } else {
      if (store.checkOverlap(slotData)) {
        errorMessage.value = 'This time slot overlaps with an existing slot';
        return;
      }
    }
    
    emit('save', slotData);
    resetForm();
  } catch (error) {
    if (error instanceof Error) {
      errorMessage.value = error.message;
    } else {
      errorMessage.value = 'An error occurred while saving the time slot';
    }
  }
};

const resetForm = () => {
  startTime.value = '08:00';
  endTime.value = '09:00';
  startTimeText.value = '08:00';
  endTimeText.value = '09:00';
  showStartTimeDropdown.value = false;
  showEndTimeDropdown.value = false;
  errorMessage.value = '';
};

const selectStartTime = (time: string) => {
  startTime.value = time;
  startTimeText.value = time;
  showStartTimeDropdown.value = false;
  
  if (!endTime.value || store.timeToMinutes(endTime.value) <= store.timeToMinutes(time)) {
    const startIndex = timeOptions.findIndex(t => t === time);
    if (startIndex < timeOptions.length - 1) {
      endTime.value = timeOptions[startIndex + 1];
      endTimeText.value = endTime.value;
    }
  }
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

const filteredEndTimes = computed(() => {
  if (!startTime.value) return timeOptions;
  
  const startIndex = timeOptions.findIndex(time => time === startTime.value);
  if (startIndex === -1) return timeOptions;
  
  return timeOptions.slice(startIndex + 1);
});

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

const wouldCauseOverlap = (time: string, isStartTime: boolean) => {
  if (props.editingSlot) {
    return false;
  }
  
  const tempSlot = isStartTime
    ? { startTime: time, endTime: endTime.value || timeOptions[timeOptions.indexOf(time) + 1] }
    : { startTime: startTime.value, endTime: time };
    
  return store.checkOverlap(tempSlot);
};
</script>

<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="relative w-full max-w-md p-6 mx-auto bg-white rounded-lg shadow-xl">
      <button @click="closeModal" class="absolute text-gray-500 top-4 right-4 hover:text-gray-700">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
      
      <h2 class="mb-6 text-xl font-semibold text-gray-800">
        {{ props.editingSlot ? 'Edit Time Slot' : 'Add Time Slot' }}
      </h2>
      
      <div v-if="errorMessage" class="p-3 mb-4 text-sm text-red-600 bg-red-100 rounded-md">
        {{ errorMessage }}
      </div>
      
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
                :class="[
                  'px-4 py-2 text-sm cursor-pointer flex items-center',
                  time === startTime ? 'bg-gray-100 font-medium text-gray-800' : 'text-gray-600 hover:bg-gray-50',
                  wouldCauseOverlap(time, true) ? 'opacity-50 cursor-not-allowed' : ''
                ]"
                :style="{ pointerEvents: wouldCauseOverlap(time, true) ? 'none' : 'auto' }"
              >
                <span class="w-4 mr-2">
                  <svg v-if="time === startTime" xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 text-blue-500" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
                  </svg>
                </span>
                {{ time }}
              </li>
            </ul>
          </div>
        </div>
      </div>
      
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
                :class="[
                  'px-4 py-2 text-sm cursor-pointer flex items-center',
                  time === endTime ? 'bg-gray-100 font-medium text-gray-800' : 'text-gray-600 hover:bg-gray-50',
                  wouldCauseOverlap(time, false) ? 'opacity-50 cursor-not-allowed' : ''
                ]"
                :style="{ pointerEvents: wouldCauseOverlap(time, false) ? 'none' : 'auto' }"
              >
                <span class="w-4 mr-2">
                  <svg v-if="time === endTime" xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 text-blue-500" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
                  </svg>
                </span>
                {{ time }}
              </li>
            </ul>
          </div>
        </div>
      </div>
      
      <button 
        @click="saveTimeSlot"
        :disabled="!startTime || !endTime"
        class="w-full py-3 text-white transition-colors bg-orange-500 rounded-md hover:bg-orange-600 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        {{ props.editingSlot ? 'Save Changes' : 'Add Time Slot' }}
      </button>
    </div>
  </div>
</template>

<style scoped>
.max-h-60 {
  max-height: 15rem;
}

.overflow-y-auto {
  scrollbar-width: thin;
  scrollbar-color: rgba(0, 0, 0, 0.2) transparent;
}
.overflow-y-auto::-webkit-scrollbar {
  width: 6px;
}
.overflow-y-auto::-webkit-scrollbar-track {
  background: transparent;
}
.overflow-y-auto::-webkit-scrollbar-thumb {
  background-color: rgba(0, 0, 0, 0.2);
  border-radius: 3px;
}
</style>
