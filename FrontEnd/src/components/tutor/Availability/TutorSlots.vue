<script setup lang="ts">
import { ref, watch, onMounted, computed } from 'vue';
import { useCalendarStore } from '../../../store/calendarStore';
import { useUserStore } from '../../../store/userStore';
import { useProfileStore } from '../../../store/profileStore';
import { useRoute } from 'vue-router';
import TimeSlotModal from '../../modals/TutorTimeSlotModal.vue';
import StudentBookingModal from '../../modals/StudentBookingModal.vue';
import { createBooking as createBookingApi } from '../../../services/studentBookings';
import { updateAvailability } from '../../../services/tutorAvailability';

const props = defineProps<{
  date: Date;
}>();

const store = useCalendarStore();
const userStore = useUserStore();
const route = useRoute();
const profileStore = useProfileStore();

const isModalOpen = ref(false);
const isBookingModalOpen = ref(false);
const loading = ref(false);
const error = ref('');
const lastEndTime = ref('');
const bookingSuccess = ref(false);

const isViewingAsStudent = computed(() => {
  return !!route.params.id && Number(route.params.id) !== Number(userStore.userId);
});

const selectedSlotForBooking = ref<any>(null);

type Slot = {
  id: string;
  apiId?: number;
  startTime: string;
  endTime: string;
  date?: string;
  status?: string;
  studentName?: string;
};

const editingSlot = ref<Slot | null>(null);

const currentUserName = computed(() => {
  if (userStore.email) {
    return userStore.email.split('@')[0];
  }
  return profileStore.firstName ? 
    `${profileStore.firstName} ${profileStore.lastName || ''}`.trim() : 
    'Student';
});

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
    
    if (isViewingAsStudent.value) {
      try {
        await store.fetchAvailability();
        
        try {
          const bookedSlots = JSON.parse(localStorage.getItem('bookedSlots') || '[]');
          const currentDay = store.currentDay;
          const currentMonthKey = store.currentMonthKey;
          
          const userBookings = bookedSlots.filter((booking: any) => 
            booking.day === currentDay && 
            booking.monthKey === currentMonthKey &&
            booking.studentName === currentUserName.value
          );
          
          if (userBookings.length > 0 && 
              store.slotsByMonth[currentMonthKey] && 
              store.slotsByMonth[currentMonthKey].slotData[currentDay]) {
            
            userBookings.forEach((booking: any) => {
              const slotIndex = store.slotsByMonth[currentMonthKey].slotData[currentDay]
                .findIndex(slot => slot.apiId === booking.slotApiId);
              
              if (slotIndex >= 0) {
                store.slotsByMonth[currentMonthKey].slotData[currentDay][slotIndex].studentName = currentUserName.value;
              }
            });
          }
        } catch (e) {
          console.error('Error processing localStorage bookings:', e);
        }
        
        if (bookingSuccess.value) {
          await new Promise(resolve => setTimeout(resolve, 500));
          await store.fetchAvailability();
        }
      } catch (err) {
        throw err;
      }
    } else {
      await store.fetchAvailability();
    }
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

const closeBookingModal = () => {
  isBookingModalOpen.value = false;
  selectedSlotForBooking.value = null;
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

const createBooking = async (bookingData: any) => {
  try {
    loading.value = true;
    error.value = '';
    
    const subjectObj = selectedSlotForBooking.value.tutorSubjects.find(
      (s: any) => s.name === bookingData.subject
    );
    
    if (!subjectObj || !subjectObj.subjectId) {
      throw new Error('Subject not found');
    }
    
    const bookingRequest = {
      tutorUserId: Number(route.params.id),
      subjectId: subjectObj.subjectId,
      availabilityRuleId: selectedSlotForBooking.value.apiId,
      description: bookingData.description,
      status: 'Pending'
    };
    
    await createBookingApi(bookingRequest);

    try {
      await updateAvailability({
        id: selectedSlotForBooking.value.apiId,
        date: selectedSlotForBooking.value.date || store.formatDateForAPI(store.selectedDate),
        startTime: selectedSlotForBooking.value.startTime + ":00",
        endTime: selectedSlotForBooking.value.endTime + ":00",
        activeStatus: true,
      });
    } catch (availabilityErr: any) {
      if (availabilityErr.response && availabilityErr.response.status === 403) {
      } else {
        console.error('Error updating availability:', availabilityErr);
      }
    }
    
    const day = store.currentDay;
    const monthKey = store.currentMonthKey;
    
    if (store.slotsByMonth[monthKey] && store.slotsByMonth[monthKey].slotData[day]) {
      const slotIndex = store.slotsByMonth[monthKey].slotData[day].findIndex(
        slot => slot.apiId === selectedSlotForBooking.value.apiId
      );
      
      if (slotIndex >= 0) {
        store.slotsByMonth[monthKey].slotData[day][slotIndex].status = 'booked';
        store.slotsByMonth[monthKey].slotData[day][slotIndex].studentName = currentUserName.value;
        store.slotsByMonth[monthKey].slotData[day][slotIndex].activeStatus = true;
        
        const bookingInfo = {
          slotApiId: selectedSlotForBooking.value.apiId,
          day,
          monthKey,
          studentName: currentUserName.value,
          date: new Date().toISOString()
        };
        
        try {
          const bookedSlots = JSON.parse(localStorage.getItem('bookedSlots') || '[]');
          bookedSlots.push(bookingInfo);
          localStorage.setItem('bookedSlots', JSON.stringify(bookedSlots));
        } catch (e) {
          console.error('Error saving booking to localStorage:', e);
        }
      }
    }
    
    bookingSuccess.value = true;
    setTimeout(() => {
      bookingSuccess.value = false;
    }, 5000);
    
    await fetchAvailability();
    
    isBookingModalOpen.value = false;
    selectedSlotForBooking.value = null;
    loading.value = false;
  } catch (err) {
    loading.value = false;
    error.value = err instanceof Error ? err.message : 'Error booking lesson';
    console.error("Error booking lesson:", err);
  }
};

const openBookingModal = async (slot: any) => {
  selectedSlotForBooking.value = slot;
  
  const tutorId = Number(route.params.id);
  if (tutorId) {
    loading.value = true;
    try {
      const subjects = profileStore.subjects.map(s => ({
        name: s.name,
        subjectId: s.subjectId || 0
      }));
      
      selectedSlotForBooking.value = {
        ...slot,
        tutorSubjects: subjects
      };
      isBookingModalOpen.value = true;
    } catch (err) {
      console.error('Failed to load tutor subjects:', err);
      error.value = err instanceof Error ? err.message : 'Failed to load tutor subjects';
    } finally {
      loading.value = false;
    }
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
        v-if="!isViewingAsStudent"
        @click="openAddSlotModal"
        :disabled="loading"
        class="flex items-center px-4 py-2 text-white transition-colors bg-orange-500 rounded-lg hover:bg-orange-600 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <span class="mr-1">+</span>
        Add Time Slot
      </button>
    </div>

    <div v-if="bookingSuccess" class="p-3 mb-4 text-sm text-green-700 bg-green-100 rounded-md">
      Booking successful! The tutor will be notified of your request.
    </div>

    <div v-if="error" class="p-3 mb-4 text-sm text-red-600 bg-red-100 rounded-md">
      {{ error }}
    </div>

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
      <p v-if="!isViewingAsStudent" class="mt-2 text-gray-400">Click "Add Time Slot" to create availability</p>
    </div>
    
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
                slot.activeStatus === true ? 
                  (isViewingAsStudent && currentUserName === slot.studentName ? 'bg-green-100 text-green-800' : 
                  (slot.status === 'completed' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800')) 
                  : 'bg-gray-100 text-gray-800'
              ]"
            >
              <span v-if="slot.activeStatus === true">
                <span v-if="isViewingAsStudent && currentUserName === slot.studentName">
                  <template v-if="slot.status === 'completed'">Done</template>
                  <template v-else>Booked by You</template>
                </span>
                <span v-else-if="slot.status === 'completed'">
                  Done
                </span>
                <span v-else-if="isViewingAsStudent">
                  Reserved
                </span>
                <span v-else>
                  Booked by - {{ slot.studentName || 'Student' }}
                </span>
              </span>
              <span v-else>Available</span>
            </span>
            
            <button 
              v-if="isViewingAsStudent && slot.activeStatus === false"
              @click="openBookingModal(slot)"
              :disabled="loading"
              class="px-4 py-1 text-sm font-medium text-white bg-purple-600 rounded-md hover:bg-purple-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Book
            </button>

            <template v-if="!isViewingAsStudent">
              <button 
                v-if="slot.activeStatus === false || slot.status === 'completed'"
                @click="editSlot(slot)" 
                :disabled="loading || slot.status === 'completed'"
                class="flex items-center justify-center w-10 h-10 text-gray-600 rounded-full hover:bg-white hover:shadow disabled:opacity-50 disabled:cursor-not-allowed"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
                </svg>
              </button>
              
              <button 
                v-if="slot.activeStatus === false || slot.status === 'completed'"
                @click="deleteSlot(slot.id)" 
                :disabled="loading || slot.status === 'completed'"
                class="flex items-center justify-center w-10 h-10 text-gray-600 bg-red-100 rounded-full hover:bg-red-200 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                </svg>
              </button>
            </template>
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

    <StudentBookingModal
      v-if="selectedSlotForBooking"
      :is-open="isBookingModalOpen"
      :slot="selectedSlotForBooking"
      :tutor-id="Number(route.params.id)"
      @close="closeBookingModal"
      @book="createBooking"
    />
  </div>
</template>
