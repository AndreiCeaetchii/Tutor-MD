<script setup lang="ts">
import { ref, watch, onMounted } from 'vue';
import { useCalendarStore } from '../../../store/calendarStore';
import TimeSlotModal from '../../modals/TutorTimeSlotModal.vue';

const props = defineProps<{
  date: Date;
}>();

const store = useCalendarStore();
const isModalOpen = ref(false);
const loading = ref(false);
const error = ref('');
const lastEndTime = ref('');

type Slot = {
  id: string;
  apiId?: number;
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

onMounted(async () => {
  await fetchAvailability();
});

const fetchAvailability = async () => {
  try {
    loading.value = true;
    error.value = '';
    await store.fetchAvailability();
    loading.value = false;
  } catch (err) {
    loading.value = false;
    error.value = err instanceof Error ? err.message : 'Error loading availability';
    console.error('Error loading availability:', err);
  }
};

const openAddSlotModal = () => {
  editingSlot.value = null;
  isModalOpen.value = true;
};

const closeModal = () => {
  isModalOpen.value = false;
  editingSlot.value = null;
};

const saveNewTimeSlot = async (newSlotData: { startTime: string, endTime: string }) => {
  try {
    loading.value = true;
    error.value = '';
    
    if (editingSlot.value) {
      await store.editSlot(editingSlot.value.id, newSlotData);
    } else {
      await store.addSlot(newSlotData);
      lastEndTime.value = newSlotData.endTime;
    }
    
    isModalOpen.value = false;
    editingSlot.value = null;
    loading.value = false;
  } catch (err) {
    loading.value = false;
    error.value = err instanceof Error ? err.message : 'Error saving time slot';
    console.error("Error saving slot:", err);
  }
};

const deleteSlot = async (id: string) => {
  try {
    loading.value = true;
    error.value = '';
    await store.deleteSlot(id);
    loading.value = false;
  } catch (err) {
    loading.value = false;
    error.value = err instanceof Error ? err.message : 'Error deleting time slot';
    console.error("Error deleting slot:", err);
  }
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
          Time slots for {{ store.formattedDate }}
        </h2>
      </div>
      <button 
        @click="openAddSlotModal"
        :disabled="loading"
        class="flex items-center px-4 py-2 text-white transition-colors bg-orange-500 rounded-lg hover:bg-orange-600 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <span class="mr-1">+</span>
        Add Time Slot
      </button>
    </div>

    <!-- Error message -->
    <div v-if="error" class="p-3 mb-4 text-sm text-red-600 bg-red-100 rounded-md">
      {{ error }}
    </div>

    <!-- Loading state -->
    <div v-if="loading" class="flex justify-center py-8">
      <div class="w-8 h-8 border-4 border-orange-500 rounded-full border-t-transparent animate-spin"></div>
    </div>

    <div v-else-if="store.slots.length === 0" class="flex flex-col items-center justify-center py-12 text-gray-400">
      <div class="flex items-center justify-center w-24 h-24 mb-4 border-4 border-gray-200 rounded-full">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-12 h-12 text-gray-300" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
        </svg>
      </div>
      <p class="text-lg font-medium text-gray-500">No time slots scheduled for this date</p>
      <p class="mt-2 text-gray-400">Click "Add Time Slot" to create availability</p>
    </div>
    
    <!-- Adăugăm containerul de scroll cu înălțime maximă fixă -->
    <div v-else class="max-h-[400px] overflow-y-auto pr-1">
      <div class="space-y-4">
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
              :disabled="loading"
              class="flex items-center justify-center w-10 h-10 text-gray-600 rounded-full hover:bg-white hover:shadow disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
              </svg>
            </button>
            
            <button 
              v-if="slot.status === 'available'"
              @click="deleteSlot(slot.id)" 
              :disabled="loading"
              class="flex items-center justify-center w-10 h-10 text-gray-600 bg-red-100 rounded-full hover:bg-red-200 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
              </svg>
            </button>
          </div>
        </div>
      </div>
    </div>
    
    <TimeSlotModal 
      :is-open="isModalOpen"
      :editing-slot="editingSlot"
      :last-end-time="lastEndTime"
      @close="closeModal"
      @save="saveNewTimeSlot"
    />
  </div>
</template>

<style scoped>
.overflow-y-auto {
  scrollbar-width: thin;
  scrollbar-color: rgba(156, 163, 175, 0.5) transparent;
}

.overflow-y-auto::-webkit-scrollbar {
  width: 6px;
}

.overflow-y-auto::-webkit-scrollbar-track {
  background: transparent;
}

.overflow-y-auto::-webkit-scrollbar-thumb {
  background-color: rgba(156, 163, 175, 0.5);
  border-radius: 3px;
}

.overflow-y-auto::-webkit-scrollbar-thumb:hover {
  background-color: rgba(156, 163, 175, 0.8);
}
</style>
