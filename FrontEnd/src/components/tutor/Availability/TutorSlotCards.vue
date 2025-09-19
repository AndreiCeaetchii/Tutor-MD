<script setup lang="ts">
  import { computed, watch } from 'vue';
  import { useCalendarStore } from '../../../store/calendarStore';

  const store = useCalendarStore();

  // Folosim statisticile specifice lunii curente
  const statistics = computed(() => {
    return store.getCurrentMonthStatistics();
  });

  // Re-calculăm statisticile când se schimbă luna selectată
  watch(() => store.currentMonthKey, () => {
    // Statisticile se vor actualiza automat prin computed
  });
</script>

<template>
  <div class="grid grid-cols-1 gap-4 mt-8 md:grid-cols-3">
    <div class="p-6 bg-white shadow-lg rounded-2xl">
      <div class="flex flex-col items-center">
        <span class="text-4xl font-bold text-orange-500">{{ statistics.availableSlots }}</span>
        <span class="mt-2 text-lg text-gray-600">Available Slots</span>
      </div>
    </div>

    <div class="p-6 bg-white shadow-lg rounded-2xl">
      <div class="flex flex-col items-center">
        <span class="text-4xl font-bold text-teal-500">{{ statistics.bookedSlots }}</span>
        <span class="mt-2 text-lg text-gray-600">Booked Slots</span>
      </div>
    </div>

    <div class="p-6 bg-white shadow-lg rounded-2xl">
      <div class="flex flex-col items-center">
        <span class="text-4xl font-bold text-purple-500">{{ statistics.activeDays }}</span>
        <span class="mt-2 text-lg text-gray-600">Active Days</span>
      </div>
    </div>
  </div>
</template>
