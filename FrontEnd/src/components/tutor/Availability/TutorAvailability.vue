<script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import TutorCalendar from './TutorCalendar.vue';
  import TutorSlots from './TutorSlots.vue';
  import TutorSlotCards from './TutorSlotCards.vue';
  import { useCalendarStore } from '../../../store/calendarStore';

  const selectedDate = ref<Date>(new Date());
  const store = useCalendarStore();
  const loading = ref(false);
  const error = ref('');

  onMounted(async () => {
    try {
      loading.value = true;
      await store.fetchAvailability();
      loading.value = false;
    } catch (err) {
      loading.value = false;
      error.value = err instanceof Error ? err.message : 'Error loading availability data';
      console.error("Error loading availability data:", err);
    }
  });

  const handleDateSelected = (date: Date) => {
    selectedDate.value = date;
  };
</script>

<template>
  <div v-if="error" class="p-3 mb-4 text-sm text-red-600 bg-red-100 rounded-md">
    {{ error }}
  </div>
  
  <div v-if="loading" class="flex justify-center py-8">
    <div class="w-10 h-10 border-4 border-orange-500 rounded-full border-t-transparent animate-spin"></div>
  </div>
  
  <div v-else class="grid grid-cols-1 gap-6 md:grid-cols-2">
    <TutorCalendar @dateSelected="handleDateSelected" />
    <TutorSlots :date="selectedDate" />
  </div>
  <TutorSlotCards />
</template>
