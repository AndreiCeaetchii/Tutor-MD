<script setup lang="ts">
  import { ref } from 'vue';
  import TutorCalendar from './TutorCalendar.vue';
  import TutorSlots from './TutorSlots.vue';
  import TutorSlotCards from './TutorSlotCards.vue';

  const selectedDate = ref<Date>(new Date());

  const handleDateSelected = (date: Date) => {
    selectedDate.value = date;
  };
</script>

<template>
  <div class="grid grid-cols-1 gap-6 md:grid-cols-2">
    <TutorCalendar @dateSelected="handleDateSelected" />
    <TutorSlots :date="selectedDate" />
  </div>
  <TutorSlotCards />
</template>
