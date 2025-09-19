<script setup lang="ts">
import { ref, onMounted, onUnmounted, computed, watch } from 'vue';
import { useBookingStore } from '../../../store/bookingStore';
import { useUserStore } from '../../../store/userStore';
// import { useFindTutorStore } from '../../../store/findTutorStore';

const bookingStore = useBookingStore();
const userStore = useUserStore();
// const findTutorStore = useFindTutorStore();

const selectedSubject = ref('');
interface Tutor {
  id: number;
  firstName: string;
  lastName: string;
  subjects: { name: string }[];
}
const selectedTutor = ref<Tutor | null>(null);
const selectedDate = ref('');
const selectedTimeSlot = ref<any | null>(null);
const description = ref('');
const showForm = ref(false);
const loading = ref(false);
const formSubmitting = ref(false);

const isSubjectDropdownOpen = ref(false);
const isTutorDropdownOpen = ref(false);
const isTimeDropdownOpen = ref(false);

const statusFilter = ref('all');

const availableTutors = computed(() => {
  if (!selectedSubject.value) return [];
  return [
    { id: 101, firstName: "Alexandru", lastName: "Munteanu", subjects: [{ name: "Mathematics" }] },
    { id: 102, firstName: "Maria", lastName: "Popescu", subjects: [{ name: "Physics" }, { name: "Mathematics" }] },
    { id: 103, firstName: "Ion", lastName: "Ionescu", subjects: [{ name: "Computer Science" }] },
    { id: 104, firstName: "Elena", lastName: "Codreanu", subjects: [{ name: "Chemistry" }, { name: "Biology" }] }
  ].filter(tutor => tutor.subjects.some(s => s.name === selectedSubject.value));
});

const availableTimeSlots = computed(() => {
  if (!selectedTutor.value || !selectedDate.value) return [];
  return bookingStore.availableTimeSlots;
});

const filteredBookings = computed(() => {
  if (statusFilter.value === 'all') {
    return bookingStore.studentBookings;
  }
  return bookingStore.studentBookings.filter(booking => booking.status === statusFilter.value);
});

const subjects = computed(() => {
  return [
    { id: 1, name: "Mathematics" },
    { id: 2, name: "Physics" },
    { id: 3, name: "Chemistry" },
    { id: 4, name: "Biology" },
    { id: 5, name: "Computer Science" },
    { id: 6, name: "English Language" },
    { id: 7, name: "History" }
  ];
});

const resetForm = () => {
  selectedSubject.value = '';
  selectedTutor.value = null;
  selectedDate.value = '';
  selectedTimeSlot.value = null;
  description.value = '';
  isSubjectDropdownOpen.value = false;
  isTutorDropdownOpen.value = false;
  isTimeDropdownOpen.value = false;
};

const openForm = () => {
  resetForm();
  showForm.value = true;
  document.body.style.overflow = 'hidden';
};

const closeForm = (force = false) => {
  if (formSubmitting.value && !force) return;
  
  showForm.value = false;
  document.body.style.overflow = '';
};

const selectSubject = (subject: string, event?: Event) => {
  if (event) {
    event.stopPropagation();
  }
  selectedSubject.value = subject;
  isSubjectDropdownOpen.value = false;
  selectedTutor.value = null;
  selectedDate.value = '';
  selectedTimeSlot.value = null;
};

const selectTutor = (tutor: Tutor, event?: Event) => {
  if (event) {
    event.stopPropagation();
  }
  selectedTutor.value = tutor;
  isTutorDropdownOpen.value = false;
  selectedDate.value = '';
  selectedTimeSlot.value = null;
};

const selectTimeSlot = (timeSlot: any, event?: Event) => {
  if (event) {
    event.stopPropagation();
  }
  selectedTimeSlot.value = timeSlot;
  isTimeDropdownOpen.value = false;
};

const toggleSubjectDropdown = (event: Event) => {
  event.stopPropagation();
  isSubjectDropdownOpen.value = !isSubjectDropdownOpen.value;
  isTutorDropdownOpen.value = false;
  isTimeDropdownOpen.value = false;
};

const toggleTutorDropdown = (event: Event) => {
  event.stopPropagation();
  isTutorDropdownOpen.value = !isTutorDropdownOpen.value;
  isSubjectDropdownOpen.value = false;
  isTimeDropdownOpen.value = false;
};

const toggleTimeDropdown = (event: Event) => {
  event.stopPropagation();
  isTimeDropdownOpen.value = !isTimeDropdownOpen.value;
  isSubjectDropdownOpen.value = false;
  isTutorDropdownOpen.value = false;
};

const handleBackdropClick = (event: MouseEvent) => {
  const target = event.target as HTMLElement;
  
  if (target.classList.contains('modal-backdrop') && !formSubmitting.value) {
    closeForm();
  }
};

const handleGlobalClick = (event: MouseEvent) => {
  const target = event.target as HTMLElement;
  
  if (!target.closest('.dropdown-toggle') && !target.closest('.dropdown-menu')) {
    isSubjectDropdownOpen.value = false;
    isTutorDropdownOpen.value = false;
    isTimeDropdownOpen.value = false;
  }
};

const createBooking = async () => {
  if (!selectedSubject.value || !selectedTutor.value || !selectedTimeSlot.value || !description.value) {
    return;
  }
  
  if (formSubmitting.value) {
    return;
  }

  loading.value = true;
  formSubmitting.value = true;

  try {
    await bookingStore.createBooking({
      tutorId: selectedTutor.value.id,
      studentId: Number(userStore.userId) || 1,
      tutorName: `${selectedTutor.value.firstName} ${selectedTutor.value.lastName}`,
      subject: selectedSubject.value,
      date: selectedDate.value,
      startTime: selectedTimeSlot.value.startTime,
      endTime: selectedTimeSlot.value.endTime,
      message: description.value,
    });

    closeForm(true);
    
    resetForm();
  } catch (error) {
    console.error("Failed to create booking:", error);
  } finally {
    loading.value = false;
    formSubmitting.value = false;
  }
};

const cancelBooking = async (bookingId: number) => {
  try {
    await bookingStore.cancelBooking(bookingId);
  } catch (error) {
    console.error("Failed to cancel booking:", error);
  }
};

const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
};

const getStatusClass = (status: string) => {
  const statusClasses = {
    'pending': 'bg-yellow-100 text-yellow-800',
    'confirmed': 'bg-green-100 text-green-800',
    'cancelled': 'bg-red-100 text-red-800',
    'completed': 'bg-blue-100 text-blue-800'
  };
  return statusClasses[status as keyof typeof statusClasses] || 'bg-gray-100 text-gray-800';
};

const getStatusText = (status: string) => {
  const statusMap = {
    'pending': 'Pending',
    'confirmed': 'Confirmed',
    'cancelled': 'Cancelled',
    'completed': 'Completed'
  };
  return statusMap[status as keyof typeof statusMap] || status;
};

const loadTutorTimeSlots = async () => {
  if (selectedTutor.value && selectedDate.value) {
    await bookingStore.getAvailableTimeSlots(selectedTutor.value.id, selectedDate.value);
  }
};

watch([selectedDate, selectedTutor], () => {
  if (selectedDate.value && selectedTutor.value) {
    loadTutorTimeSlots();
  }
});

onMounted(async () => {
  try {
    await bookingStore.fetchStudentBookings(Number(userStore.userId) || 1);
    
    document.addEventListener('click', handleGlobalClick);
  } catch (error) {
    console.error("Error loading bookings:", error);
  }
});

onUnmounted(() => {
  document.removeEventListener('click', handleGlobalClick);
  document.body.style.overflow = '';
});
</script>

<template>
  <div class="container px-4 py-6 mx-auto">
    <div class="flex items-center justify-between mb-6">
      <div class="flex border-b border-gray-200">
        <button 
          @click="statusFilter = 'all'" 
          :class="[
            'py-2 px-3 border-b-2', 
            statusFilter === 'all' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          All
        </button>
        <button 
          @click="statusFilter = 'pending'" 
          :class="[
            'py-2 px-3 border-b-2', 
            statusFilter === 'pending' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          Pending
        </button>
        <button 
          @click="statusFilter = 'confirmed'" 
          :class="[
            'py-2 px-3 border-b-2', 
            statusFilter === 'confirmed' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          Confirmed
        </button>
        <button 
          @click="statusFilter = 'completed'" 
          :class="[
            'py-2 px-3 border-b-2', 
            statusFilter === 'completed' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          Completed
        </button>
        <button 
          @click="statusFilter = 'cancelled'" 
          :class="[
            'py-2 px-3 border-b-2', 
            statusFilter === 'cancelled' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          Cancelled
        </button>
      </div>
      <button 
        @click="openForm" 
        class="px-4 py-2 text-white transition-colors bg-purple-600 rounded-full shadow-sm hover:bg-purple-700"
      >
        Schedule a session
      </button>
    </div>

    <div 
      v-if="showForm" 
      class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50 modal-backdrop"
      @click="handleBackdropClick"
    >
      <div 
        class="w-full max-w-md p-6 bg-white rounded shadow-lg"
        @click.stop
      >
        <div class="flex justify-between mb-4">
          <h3 class="text-lg font-semibold">Schedule a tutoring session</h3>
          <button 
            @click="closeForm()" 
            type="button" 
            :disabled="formSubmitting"
            class="text-gray-500 hover:text-gray-700"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
            </svg>
          </button>
        </div>
        
        <div class="space-y-4">
          <div class="relative">
            <label class="block mb-1 text-gray-700">Select subject</label>
            <div 
              @click="toggleSubjectDropdown"
              class="flex items-center justify-between w-full px-4 py-3 border border-gray-300 rounded cursor-pointer dropdown-toggle"
              :class="{'opacity-50 cursor-not-allowed': formSubmitting}"
            >
              <span :class="{'text-gray-400': !selectedSubject}">
                {{ selectedSubject || 'Choose a subject' }}
              </span>
              <svg 
                class="w-5 h-5 ml-2 text-gray-400" 
                :class="{'transform rotate-180': isSubjectDropdownOpen}"
                fill="none" 
                stroke="currentColor" 
                viewBox="0 0 24 24"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path>
              </svg>
            </div>
            
            <div 
              v-if="isSubjectDropdownOpen && !formSubmitting" 
              class="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded shadow-lg dropdown-menu"
              @click.stop
            >
              <div class="py-1 overflow-auto max-h-60">
                <div 
                  v-for="subject in subjects" 
                  :key="subject.id"
                  @click="(e) => selectSubject(subject.name, e)" 
                  class="flex items-center px-4 py-2 cursor-pointer hover:bg-gray-100"
                >
                  <span v-if="subject.name === selectedSubject" class="mr-2 text-blue-500">✓</span>
                  <span v-else class="invisible mr-2">✓</span>
                  {{ subject.name }}
                </div>
              </div>
            </div>
          </div>

          <div v-if="selectedSubject" class="relative">
            <label class="block mb-1 text-gray-700">Select tutor</label>
            <div 
              @click="toggleTutorDropdown" 
              class="flex items-center justify-between w-full px-4 py-3 border border-gray-300 rounded cursor-pointer dropdown-toggle"
              :class="{'opacity-50 cursor-not-allowed': formSubmitting}"
            >
              <span :class="{'text-gray-400': !selectedTutor}">
                {{ selectedTutor ? `${selectedTutor.firstName} ${selectedTutor.lastName}` : 'Choose a tutor' }}
              </span>
              <svg 
                class="w-5 h-5 ml-2 text-gray-400" 
                :class="{'transform rotate-180': isTutorDropdownOpen}"
                fill="none" 
                stroke="currentColor" 
                viewBox="0 0 24 24"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path>
              </svg>
            </div>
            
            <div 
              v-if="isTutorDropdownOpen && !formSubmitting" 
              class="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded shadow-lg dropdown-menu"
              @click.stop
            >
              <div v-if="availableTutors.length === 0" class="px-4 py-2 text-sm text-red-500">
                No tutors available for this subject.
              </div>
              <div v-else class="py-1 overflow-auto max-h-60">
                <div 
                  v-for="tutor in availableTutors" 
                  :key="tutor.id"
                  @click="(e) => selectTutor(tutor, e)" 
                  class="flex items-center px-4 py-2 cursor-pointer hover:bg-gray-100"
                >
                  <span v-if="selectedTutor && tutor.id === selectedTutor.id" class="mr-2 text-blue-500">✓</span>
                  <span v-else class="invisible mr-2">✓</span>
                  {{ tutor.firstName }} {{ tutor.lastName }}
                </div>
              </div>
            </div>
          </div>

          <div v-if="selectedTutor">
            <label class="block mb-1 text-gray-700">Select date</label>
            <input 
              type="date" 
              v-model="selectedDate" 
              class="w-full px-4 py-3 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent"
              :min="new Date().toISOString().split('T')[0]"
              :disabled="formSubmitting"
              @click.stop
            >
          </div>

          <div v-if="selectedDate" class="relative">
            <label class="block mb-1 text-gray-700">Select time</label>
            <div 
              @click="toggleTimeDropdown" 
              class="flex items-center justify-between w-full px-4 py-3 border border-gray-300 rounded cursor-pointer dropdown-toggle"
              :class="{'opacity-50 cursor-not-allowed': formSubmitting}"
            >
              <span :class="{'text-gray-400': !selectedTimeSlot}">
                {{ selectedTimeSlot ? `${selectedTimeSlot.startTime} - ${selectedTimeSlot.endTime}` : 'Choose a time' }}
              </span>
              <svg 
                class="w-5 h-5 ml-2 text-gray-400" 
                :class="{'transform rotate-180': isTimeDropdownOpen}"
                fill="none" 
                stroke="currentColor" 
                viewBox="0 0 24 24"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path>
              </svg>
            </div>
            
            <div 
              v-if="isTimeDropdownOpen && !formSubmitting" 
              class="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded shadow-lg dropdown-menu"
              @click.stop
            >
              <div v-if="availableTimeSlots.length === 0" class="px-4 py-2 text-sm text-red-500">
                No available times for this date. Please select another date.
              </div>
              <div v-else class="py-1 overflow-auto max-h-60">
                <div 
                  v-for="slot in availableTimeSlots" 
                  :key="slot.id"
                  @click="(e) => selectTimeSlot(slot, e)" 
                  class="flex items-center px-4 py-2 cursor-pointer hover:bg-gray-100"
                >
                  <span v-if="selectedTimeSlot && slot.id === selectedTimeSlot.id" class="mr-2 text-blue-500">✓</span>
                  <span v-else class="invisible mr-2">✓</span>
                  {{ slot.startTime }} - {{ slot.endTime }}
                </div>
              </div>
            </div>
          </div>

          <div v-if="selectedTimeSlot">
            <label class="block mb-1 text-gray-700">Description</label>
            <textarea 
              v-model="description" 
              placeholder="Describe what you need help with..." 
              class="w-full h-24 px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent"
              :disabled="formSubmitting"
              @click.stop
            ></textarea>
          </div>

          <div class="flex gap-2">
            <button 
                @click="closeForm()" 
                type="button"
                :disabled="formSubmitting"
                class="flex items-center justify-center px-4 py-2 text-red-700 transition-colors bg-white border border-gray-300 rounded-full shadow-sm hover:bg-gray-100 disabled:opacity-50"
            >
                <svg class="w-5 h-5 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                </svg>
                Cancel
            </button>
            
            <button 
                @click="createBooking" 
                type="button"
                class="flex items-center justify-center px-4 py-2 text-white transition-colors bg-purple-600 rounded-full shadow-sm hover:bg-purple-700 disabled:bg-purple-300 disabled:cursor-not-allowed"
                :disabled="!selectedSubject || !selectedTutor || !selectedTimeSlot || !description || formSubmitting"
            >
                <svg class="w-5 h-5 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
                </svg>
                Accept
            </button>
          </div>
        </div>
      </div>
    </div>

    <div class="space-y-4">
      <div v-if="filteredBookings.length === 0" class="py-8 text-center">
        <p class="text-gray-500">You have no bookings {{ statusFilter !== 'all' ? `with "${getStatusText(statusFilter)}" status` : '' }}.</p>
      </div>
      
      <div v-for="booking in filteredBookings" :key="booking.id" class="overflow-hidden bg-white rounded shadow-md">
        <div class="p-5 border-b border-gray-100">
          <div class="flex items-start justify-between">
            <div>
              <h3 class="text-lg font-semibold">{{ booking.subject }}</h3>
              <p class="text-gray-600">With: {{ booking.tutorName }}</p>
              <div class="flex items-center mt-2 text-gray-500">
                <span class="mr-1 text-sm text-purple-600 material-icons">calendar_today</span>
                <span>{{ formatDate(booking.date) }}</span>
              </div>
              <div class="flex items-center mt-1 text-gray-500">
                <span class="mr-1 text-sm text-purple-600 material-icons">schedule</span>
                <span>{{ booking.startTime }} - {{ booking.endTime }} ({{ booking.duration }})</span>
              </div>
            </div>
            <div class="flex flex-col items-end">
              <span :class="['px-3 py-1 rounded-full text-xs', getStatusClass(booking.status)]">
                {{ getStatusText(booking.status) }}
              </span>
            </div>
          </div>
          <p class="mt-4 text-gray-600 whitespace-pre-line">{{ booking.message }}</p>
          
          <div class="flex justify-end mt-4" v-if="booking.status === 'pending' || booking.status === 'confirmed'">
            <div class="flex gap-2">
              <button 
                @click="cancelBooking(booking.id)" 
                class="flex items-center px-4 py-2 text-red-700 transition-colors bg-white border border-gray-300 rounded-full shadow-sm hover:bg-gray-100"
              >
                <svg class="w-5 h-5 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                </svg>
                Cancel
              </button>
              
              <button 
                class="flex items-center px-4 py-2 text-purple-700 transition-colors bg-purple-100 rounded-full shadow-sm hover:bg-purple-200"
              >
                <svg class="w-5 h-5 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 12h.01M12 12h.01M16 12h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z"></path>
                </svg>
                Chat
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.material-icons {
  font-size: 18px;
}

.btn {
  display: flex;
  align-items: center;
  justify-content: center;
  padding-left: 1rem;
  padding-right: 1rem;
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
  border-radius: 9999px;
  box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
  transition-property: color, background-color, border-color;
  transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  transition-duration: 150ms;
}

.btn-cancel {
  color: #b91c1c;
  background-color: #ffffff;
  border: 1px solid #d1d5db;
}

.btn-cancel:hover {
  background-color: #f3f4f6;
}

.btn-accept {
  color: #ffffff;
  background-color: #9333ea;
}

.btn-accept:hover {
  background-color: #7e22ce;
}

.btn-accept:disabled {
  background-color: #d8b4fe;
  cursor: not-allowed;
}

.btn-chat {
  color: #7e22ce;
  background-color: #f3e8ff;
}

.btn-chat:hover {
  background-color: #e9d5ff;
}
</style>
