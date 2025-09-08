<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  slotData?: Record<number, any[]>;
}>();

// Calculate statistics based on slot data
const statistics = computed(() => {
  if (!props.slotData) {
    return {
      availableSlots: 0,
      bookedSlots: 0,
      activeDays: 0
    };
  }

  let availableSlots = 0;
  let bookedSlots = 0;
  let activeDays = 0;

  // Count the number of active days (days with slots)
  activeDays = Object.keys(props.slotData).length;

  // Count available and booked slots
  Object.values(props.slotData).forEach(daySlots => {
    daySlots.forEach(slot => {
      if (slot.status === 'available') {
        availableSlots++;
      } else if (slot.status === 'booked') {
        bookedSlots++;
      }
    });
  });

  return {
    availableSlots,
    bookedSlots,
    activeDays
  };
});
</script>

<template>
  <div class="grid grid-cols-1 gap-4 mt-8 md:grid-cols-3">
    <!-- Available Slots Card -->
    <div class="p-6 bg-white shadow-lg rounded-2xl">
      <div class="flex flex-col items-center">
        <span class="text-4xl font-bold text-orange-500">{{ statistics.availableSlots }}</span>
        <span class="mt-2 text-lg text-gray-600">Available Slots</span>
      </div>
    </div>

    <!-- Booked Slots Card -->
    <div class="p-6 bg-white shadow-lg rounded-2xl">
      <div class="flex flex-col items-center">
        <span class="text-4xl font-bold text-teal-500">{{ statistics.bookedSlots }}</span>
        <span class="mt-2 text-lg text-gray-600">Booked Slots</span>
      </div>
    </div>

    <!-- Active Days Card -->
    <div class="p-6 bg-white shadow-lg rounded-2xl">
      <div class="flex flex-col items-center">
        <span class="text-4xl font-bold text-purple-500">{{ statistics.activeDays }}</span>
        <span class="mt-2 text-lg text-gray-600">Active Days</span>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Add any specific styles here if needed */
</style>