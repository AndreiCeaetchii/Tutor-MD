<script setup lang="ts">
  import { ref, computed, watch } from 'vue';
  import { useCalendarStore } from '../../../store/calendarStore';

  const store = useCalendarStore();
  const emit = defineEmits(['dateSelected']);

  const currentDate = new Date();
  const currentMonth = ref(currentDate.getMonth());
  const currentYear = ref(currentDate.getFullYear());

  const months = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December',
  ];

  watch([currentMonth, currentYear], () => {
    const selectedDay = store.currentDay;
    const newDate = new Date(currentYear.value, currentMonth.value, selectedDay);
    store.setSelectedDate(newDate);
  });

  const formattedMonth = computed(() => {
    return `${months[currentMonth.value]} ${currentYear.value}`;
  });

  const daysInMonth = computed(() => {
    return new Date(currentYear.value, currentMonth.value + 1, 0).getDate();
  });

  const startDay = computed(() => {
    const day = new Date(currentYear.value, currentMonth.value, 1).getDay();
    return day === 0 ? 6 : day - 1;
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
    const newDate = new Date(currentYear.value, currentMonth.value, day);
    store.setSelectedDate(newDate);
    emit('dateSelected', newDate);
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
    const selectedDate = store.selectedDate instanceof Date ? 
      store.selectedDate : new Date(store.selectedDate);
    return (
      day === selectedDate.getDate() &&
      currentMonth.value === selectedDate.getMonth() &&
      currentYear.value === selectedDate.getFullYear()
    );
  };

  const hasSlots = (day: number) => {
    return store.daysWithSlots.includes(day);
  };

  const getSlotCount = (day: number) => {
    return store.slotData[day]?.length || 0;
  };

  const isEditedPastSlotDay = (day: number) => {
    return store.daysWithEditedSlots?.includes(day);
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

      <div class="max-w-md mx-auto">
        <div class="grid grid-cols-7 gap-2 mb-2 text-center">
          <div
            v-for="day in ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su']"
            :key="day"
            class="py-1 text-sm font-medium text-gray-500"
          >
            {{ day }}
          </div>
        </div>

        <div class="grid grid-cols-7 gap-2 text-center">
          <div
            v-for="day in getPreviousMonthDays()"
            :key="`prev-${day}`"
            class="flex items-center justify-center h-10 text-sm text-gray-400"
          >
            {{ day }}
          </div>

          <button
            v-for="day in daysInMonth"
            :key="`current-${day}`"
            @click="selectDate(day)"
            :class="[
              'h-10 w-10 rounded-full text-sm flex items-center justify-center relative mx-auto transition-colors',
              isCurrentMonth(day) ? 'bg-gray-100' : '',
              isSelectedDate(day) ? 'bg-indigo-500 text-white' : '',
              isEditedPastSlotDay(day) && !isSelectedDate(day) ? 'bg-red-100 text-gray-800' : '',
              hasSlots(day) && !isSelectedDate(day) && !isEditedPastSlotDay(day) ? 'bg-orange-100 text-gray-800' : '',
              !hasSlots(day) && !isSelectedDate(day) && !isEditedPastSlotDay(day) ? 'hover:bg-gray-100' : '',
            ]"
          >
            {{ day }}
            <span 
              v-if="getSlotCount(day) > 0 && !isSelectedDate(day)"
              :class="[
                'absolute flex items-center justify-center w-5 h-5 text-xs text-white rounded-full -top-1 -right-1',
                isEditedPastSlotDay(day) ? 'bg-red-500' : 'bg-orange-500'
              ]"
            >
              {{ getSlotCount(day) }}
            </span>
          </button>

          <div
            v-for="day in getNextMonthDays()"
            :key="`next-${day}`"
            class="flex items-center justify-center h-10 text-sm text-gray-400"
          >
            {{ day }}
          </div>
        </div>
      </div>

      <div class="flex flex-col items-center justify-center mt-6 space-y-2 text-xs text-gray-500">
        <div class="flex items-center">
          <span class="inline-block w-3 h-3 mr-2 bg-orange-200 rounded-full"></span>
          Days highlighted in orange have available time slots
        </div>
        <div class="flex items-center">
          <span class="inline-block w-3 h-3 mr-2 bg-red-200 rounded-full"></span>
          Days highlighted in red have edited past slots
        </div>
      </div>
    </div>
  </div>
</template>
