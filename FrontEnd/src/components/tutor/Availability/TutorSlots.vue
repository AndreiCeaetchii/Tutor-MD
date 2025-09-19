<script setup lang="ts">
import { ref, watch } from 'vue';
import { useCalendarStore } from '../../../store/calendarStore';
import TimeSlotModal from '../../modals/TutorTimeSlotModal.vue';

const props = defineProps<{
  date: Date;
}>();

const store = useCalendarStore();
const isModalOpen = ref(false);
type Slot = {
  id: string;
  startTime: string;
  endTime: string;
  status?: string;
  studentName?: string;
};

const editingSlot = ref<Slot | null>(null);

watch(() => props.date, (newDate) => {
  if (newDate) {
    store.setSelectedDate(newDate);
  }
}, { immediate: true });

const openAddSlotModal = () => {
  editingSlot.value = null;
  isModalOpen.value = true;
};

const closeModal = () => {
  isModalOpen.value = false;
  editingSlot.value = null;
};

const saveNewTimeSlot = (newSlotData: { startTime: string, endTime: string }) => {
  try {
    if (editingSlot.value) {
      // If editing an existing slot
      store.editSlot(editingSlot.value.id, newSlotData);
    } else {
      // If creating a new slot
      store.addSlot(newSlotData);
    }
    isModalOpen.value = false;
    editingSlot.value = null;
  } catch (error) {
    console.error("Error saving slot:", error);
    // Error will be handled in the modal
  }
};

const deleteSlot = (id: string) => {
  store.deleteSlot(id);
};

const editSlot = (slot: any) => {
  editingSlot.value = slot;
  isModalOpen.value = true;
};
</script>

<template>
  <div class="p-6 bg-white shadow-lg rounded-2xl">
    <div class="flex items-center justify-between mb-6">
      <div class="flex items-center">
        <h2 class="text-xl font-semibold text-gray-800">
          Slots for {{ store.formattedDate }}
        </h2>
      </div>
      <button 
        @click="openAddSlotModal"
        class="flex items-center px-4 py-2 text-white transition-colors bg-orange-500 rounded-lg hover:bg-orange-600"
      >
        <span class="mr-1">+</span>
        Add Slot
      </button>
    </div>

    <div v-if="store.slots.length === 0" class="flex flex-col items-center justify-center py-12 text-gray-400">
      <div class="flex items-center justify-center w-24 h-24 mb-4 border-4 border-gray-200 rounded-full">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-12 h-12 text-gray-300" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
        </svg>
      </div>
      <p class="text-lg font-medium text-gray-500">No time slots scheduled for this date</p>
      <p class="mt-2 text-gray-400">Click "Add Slot" to create availability</p>
    </div>
    
    <div v-else class="space-y-4">
      <div 
        v-for="slot in store.slots" 
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
    
    <TimeSlotModal 
      :is-open="isModalOpen"
      :editing-slot="editingSlot"
      @close="closeModal"
      @save="saveNewTimeSlot"
    />
  </div>
</template>
