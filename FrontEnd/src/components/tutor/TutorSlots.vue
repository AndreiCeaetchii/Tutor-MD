<script setup lang="ts">
import { ref, computed } from 'vue';
import TimeSlotModal from '../modals/TutorTimeSlotModal.vue';

const props = defineProps<{
  date: Date;
}>();

// Slot interface
interface TimeSlot {
  id: string;
  startTime: string;
  endTime: string;
  status: 'available' | 'booked';
  studentName?: string;
}

// Define which days have slots - must match with TutorCalendar
const daysWithSlots = ref([8, 15, 16, 17]);

// Get the day of the month from the date prop
const currentDay = computed(() => props.date.getDate());

// Store the actual slot data
const slotData = ref<Record<number, TimeSlot[]>>({
  8: [
    {
      id: '1',
      startTime: '10:00',
      endTime: '11:00',
      status: 'booked' as const,
      studentName: 'Alex Chen'
    },
    {
      id: '2',
      startTime: '14:00',
      endTime: '15:00',
      status: 'available' as const
    }
  ],
  15: [
    {
      id: '3',
      startTime: '09:00',
      endTime: '10:00',
      status: 'available' as const
    },
    {
      id: '4',
      startTime: '15:00',
      endTime: '16:30',
      status: 'booked' as const,
      studentName: 'Maria Rodriguez'
    }
  ],
  16: [
    {
      id: '5',
      startTime: '13:00',
      endTime: '14:00',
      status: 'available' as const
    }
  ],
  17: [
    {
      id: '6',
      startTime: '13:00',
      endTime: '14:00',
      status: 'available' as const
    }
  ]
});

// Check if the selected date has any slots
const hasSlots = computed(() => {
  return daysWithSlots.value.includes(currentDay.value);
});

// Dynamic slots based on the selected date
const slots = computed<TimeSlot[]>(() => {
  if (!hasSlots.value || !slotData.value[currentDay.value]) {
    return [];
  }
  
  return slotData.value[currentDay.value];
});

// Modal control
const isModalOpen = ref(false);

// Format date for display (e.g., 9/15/2025)
const formattedDate = computed(() => {
  const month = props.date.getMonth() + 1;
  const day = props.date.getDate();
  const year = props.date.getFullYear();
  return `${month}/${day}/${year}`;
});

// Open the modal for adding a new slot
const openAddSlotModal = () => {
  isModalOpen.value = true;
};

// Close the modal
const closeModal = () => {
  isModalOpen.value = false;
};

// Save a new time slot
const saveNewTimeSlot = (newSlotData: { startTime: string, endTime: string }) => {
  const day = currentDay.value;
  
  // Create a new slot object
  const newSlot: TimeSlot = {
    id: `${Date.now()}`,
    startTime: newSlotData.startTime,
    endTime: newSlotData.endTime,
    status: 'available' as const
  };
  
  // If this day doesn't exist in our slots yet, initialize it
  if (!daysWithSlots.value.includes(day)) {
    daysWithSlots.value.push(day);
  }
  
  // Initialize the array for this day if it doesn't exist
  if (!slotData.value[day]) {
    slotData.value[day] = [];
  }
  
  // Add the new slot to the day's slots
  slotData.value[day].push(newSlot);
  
  console.log(`Added new slot: ${newSlot.startTime}-${newSlot.endTime} on day ${day}`);
  isModalOpen.value = false;
};

// Delete time slot
const deleteSlot = (id: string) => {
  const day = currentDay.value;
  
  // Find the index of the slot to delete
  const slotIndex = slotData.value[day].findIndex(slot => slot.id === id);
  
  if (slotIndex !== -1) {
    // Remove the slot from the array
    slotData.value[day].splice(slotIndex, 1);
    
    // If this was the last slot for this day, remove the day from daysWithSlots
    if (slotData.value[day].length === 0) {
      const dayIndex = daysWithSlots.value.indexOf(day);
      if (dayIndex !== -1) {
        daysWithSlots.value.splice(dayIndex, 1);
      }
      delete slotData.value[day];
    }
  }
};

// Edit time slot
const editSlot = (slot: TimeSlot) => {
  console.log("Edit slot:", slot);
  // Implement edit functionality here
};
</script>

<template>
  <div class="p-6 bg-white shadow-lg rounded-2xl">
    <div class="flex items-center justify-between mb-6">
      <div class="flex items-center">
        <h2 class="text-xl font-semibold text-gray-800">
          Slots for {{ formattedDate }}
        </h2>
      </div>
      <button 
        @click="openAddSlotModal"
        class="flex items-center px-4 py-2 text-white transition-colors bg-orange-500 rounded-full hover:bg-orange-600"
      >
        <span class="mr-1">+</span>
        Add Slot
      </button>
    </div>

    <!-- Empty state - No slots scheduled -->
    <div v-if="slots.length === 0" class="flex flex-col items-center justify-center py-12 text-gray-400">
      <div class="flex items-center justify-center w-24 h-24 mb-4 border-4 border-gray-200 rounded-full">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-12 h-12 text-gray-300" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
        </svg>
      </div>
      <p class="text-lg font-medium text-gray-500">No time slots scheduled for this date</p>
      <p class="mt-2 text-gray-400">Click "Add Slot" to create availability</p>
    </div>
    
    <!-- Slot list -->
    <div v-else class="space-y-4">
      <div 
        v-for="slot in slots" 
        :key="slot.id"
        class="flex items-center justify-between p-4 transition-colors rounded-lg bg-gray-50 hover:bg-gray-100"
      >
        <div class="flex items-center">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 mr-3 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          <span class="text-lg font-medium">{{ slot.startTime }} - {{ slot.endTime }}</span>
        </div>
        
        <div class="flex items-center space-x-2">
          <span 
            :class="[
              'px-3 py-1 text-sm font-medium rounded-full',
              slot.status === 'booked' ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'
            ]"
          >
            <span v-if="slot.status === 'booked'">Booked - {{ slot.studentName }}</span>
            <span v-else>Available</span>
          </span>
          
          <button 
            v-if="slot.status === 'available'"
            @click="editSlot(slot)" 
            class="flex items-center justify-center w-10 h-10 text-gray-600 rounded-full hover:bg-white hover:shadow"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
            </svg>
          </button>
          
          <button 
            v-if="slot.status === 'available'"
            @click="deleteSlot(slot.id)" 
            class="flex items-center justify-center w-10 h-10 text-gray-600 bg-red-100 rounded-full hover:bg-red-200"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
            </svg>
          </button>
        </div>
      </div>
    </div>
    
    <!-- Time Slot Modal -->
    <TimeSlotModal 
      :is-open="isModalOpen"
      @close="closeModal"
      @save="saveNewTimeSlot"
    />
  </div>
</template>

<style scoped>
button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>