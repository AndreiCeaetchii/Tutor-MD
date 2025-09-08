<script setup lang="ts">
import { ref, computed } from 'vue';

const emit = defineEmits(['dateSelected']);

const currentDate = new Date();
const currentMonth = ref(currentDate.getMonth());
const currentYear = ref(currentDate.getFullYear());
const selectedDate = ref(new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate()));

// Days with available slots (in this example, we're hardcoding some dates)
const daysWithSlots = ref([8, 15, 16, 17]);
const selectedDays = ref([currentDate.getDate()]); // Initial selection changed to 9 to match your example

const months = [
  'January', 'February', 'March', 'April', 'May', 'June', 
  'July', 'August', 'September', 'October', 'November', 'December'
];

const formattedMonth = computed(() => {
  return `${months[currentMonth.value]} ${currentYear.value}`;
});

const daysInMonth = computed(() => {
  return new Date(currentYear.value, currentMonth.value + 1, 0).getDate();
});

const startDay = computed(() => {
  return new Date(currentYear.value, currentMonth.value, 1).getDay();
});

const previousMonth = () => {
  if (currentMonth.value === 0) {
    currentMonth.value = 11;
    currentYear.value--;
  } else {
    currentMonth.value--;
  }
};

const nextMonth = () => {
  if (currentMonth.value === 11) {
    currentMonth.value = 0;
    currentYear.value++;
  } else {
    currentMonth.value++;
  }
};

const getPreviousMonthDays = () => {
  const lastDayOfPreviousMonth = new Date(currentYear.value, currentMonth.value, 0).getDate();
  const days = [];
  
  for (let i = startDay.value - 1; i >= 0; i--) {
    days.push(lastDayOfPreviousMonth - i);
  }
  
  return days;
};

const getNextMonthDays = () => {
  const days = [];
  const totalCells = Math.ceil((daysInMonth.value + startDay.value) / 7) * 7;
  const nextMonthDays = totalCells - (daysInMonth.value + startDay.value);
  
  for (let i = 1; i <= nextMonthDays; i++) {
    days.push(i);
  }
  
  return days;
};

const selectDate = (day: number) => {
  selectedDate.value = new Date(currentYear.value, currentMonth.value, day);
  selectedDays.value = [day]; // Update the selected day
  emit('dateSelected', selectedDate.value);
};

const isCurrentMonth = (day: number) => {
  const today = new Date();
  return (
    currentMonth.value === today.getMonth() &&
    currentYear.value === today.getFullYear() &&
    day === today.getDate()
  );
};

const isSelectedDate = (day: number) => {
  return selectedDays.value.includes(day);
};

const hasSlots = (day: number) => {
  return daysWithSlots.value.includes(day);
};

// Check if date is in the past
const isDateInPast = (day: number) => {
  const checkDate = new Date(currentYear.value, currentMonth.value, day);
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  return checkDate < today;
};
</script>

<template>
  <div class="p-6 bg-white shadow-lg rounded-2xl md:p-8">
    <div class="flex items-center mb-6">
      <span class="mr-2 text-gray-600 material-icons">schedule</span>
      <h2 class="text-xl font-semibold text-gray-800">Calendar</h2>
    </div>

    <div class="calendar">
      <div class="flex items-center justify-between mb-4">
        <button 
          @click="previousMonth" 
          class="flex items-center justify-center w-10 h-10 rounded-full hover:bg-gray-100 focus:outline-none"
        >
          <span class="text-gray-500 material-icons">chevron_left</span>
        </button>
        <h3 class="text-lg font-medium text-gray-800">{{ formattedMonth }}</h3>
        <button 
          @click="nextMonth" 
          class="flex items-center justify-center w-10 h-10 rounded-full hover:bg-gray-100 focus:outline-none"
        >
          <span class="text-gray-500 material-icons">chevron_right</span>
        </button>
      </div>
      
      <!-- Calendar with more compact design -->
      <div class="max-w-md mx-auto">
        <div class="grid grid-cols-7 gap-2 mb-2 text-center">
          <div v-for="day in ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa']" 
              :key="day" 
              class="py-1 text-sm font-medium text-gray-500">
            {{ day }}
          </div>
        </div>
        
        <div class="grid grid-cols-7 gap-2 text-center">
          <!-- Previous month days -->
          <div v-for="day in getPreviousMonthDays()" 
              :key="`prev-${day}`" 
              class="flex items-center justify-center h-10 text-sm text-gray-400">
            {{ day }}
          </div>
          
          <!-- Current month days -->
          <button v-for="day in daysInMonth" 
                  :key="`current-${day}`" 
                  @click="selectDate(day)"
                  :class="[
                    'h-10 w-10 rounded-full text-sm flex items-center justify-center relative mx-auto transition-colors',
                    isCurrentMonth(day) ? 'bg-gray-100' : '',
                    isSelectedDate(day) ? 'bg-indigo-500 text-white' : '',
                    hasSlots(day) && !isSelectedDate(day) ? 'bg-pink-100 text-gray-800' : '',
                    !hasSlots(day) && !isSelectedDate(day) ? 'hover:bg-gray-100' : ''
                  ]"
                  :disabled="isDateInPast(day)">
            {{ day }}
            <span v-if="hasSlots(day) && !isSelectedDate(day)" 
                  class="absolute bottom-0 left-1/2 transform -translate-x-1/2 w-1 h-1 -mb-0.5">
              <span v-if="day === 15 || day === 16 || day === 17" class="absolute w-1 h-1 bg-pink-400 rounded-full"></span>
              <span v-else-if="day === 8" class="absolute w-1 h-1 bg-blue-400 rounded-full"></span>
            </span>
          </button>
          
          <!-- Next month days -->
          <div v-for="day in getNextMonthDays()" 
              :key="`next-${day}`" 
              class="flex items-center justify-center h-10 text-sm text-gray-400">
            {{ day }}
          </div>
        </div>
      </div>
      
      <div class="flex items-center justify-center mt-4 text-xs text-gray-500">
        <span class="inline-block w-2 h-2 mr-1 bg-pink-200 rounded-full"></span>
        Days highlighted in pink have available time slots
      </div>
    </div>
  </div>
</template>

<style scoped>
button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

/* Add smooth transitions */
button {
  transition: background-color 0.2s ease, color 0.2s ease;
}
</style>