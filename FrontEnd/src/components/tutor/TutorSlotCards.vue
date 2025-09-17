<script setup lang="ts">
  import { computed, ref } from 'vue';

  const slotData = ref({
    8: [
      { id: '1', startTime: '10:00', endTime: '11:00', status: 'booked', studentName: 'Alex Chen' },
      { id: '2', startTime: '14:00', endTime: '15:00', status: 'available' },
    ],
    15: [
      { id: '3', startTime: '09:00', endTime: '10:00', status: 'available' },
      {
        id: '4',
        startTime: '15:00',
        endTime: '16:30',
        status: 'booked',
        studentName: 'Maria Rodriguez',
      },
    ],
    16: [{ id: '5', startTime: '13:00', endTime: '14:00', status: 'available' }],
    17: [{ id: '6', startTime: '13:00', endTime: '14:00', status: 'available' }],
  });

  const statistics = computed(() => {
    const data = slotData.value;

    if (!data || Object.keys(data).length === 0) {
      return {
        availableSlots: 0,
        bookedSlots: 0,
        activeDays: 0,
      };
    }

    let availableSlots = 0;
    let bookedSlots = 0;

    Object.values(data).forEach((daySlots) => {
      daySlots.forEach((slot) => {
        if (slot.status === 'available') {
          availableSlots++;
        } else if (slot.status === 'booked') {
          bookedSlots++;
        }
      });
    });

    const activeDays = Object.keys(data).length;

    return {
      availableSlots,
      bookedSlots,
      activeDays,
    };
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
