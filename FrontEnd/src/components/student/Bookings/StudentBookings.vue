<script setup lang="ts">
import { ref, onMounted, onUnmounted, computed, watch } from 'vue';
import { useBookingStore } from '../../../store/bookingStore';
import { useUserStore } from '../../../store/userStore';
import { useFindTutorStore } from '../../../store/findTutorStore';

const bookingStore = useBookingStore();
const userStore = useUserStore();
const findTutorStore = useFindTutorStore();

// Form states
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

// Custom dropdown state
const isSubjectDropdownOpen = ref(false);
const isTutorDropdownOpen = ref(false);
const isTimeDropdownOpen = ref(false);

// Filter state
const statusFilter = ref('all');

// Computed properties
const availableTutors = computed(() => {
  if (!selectedSubject.value) return [];
  return [
    { id: 101, firstName: "Alexandru", lastName: "Munteanu", subjects: [{ name: "Matematică" }] },
    { id: 102, firstName: "Maria", lastName: "Popescu", subjects: [{ name: "Fizică" }, { name: "Matematică" }] },
    { id: 103, firstName: "Ion", lastName: "Ionescu", subjects: [{ name: "Informatică" }] },
    { id: 104, firstName: "Elena", lastName: "Codreanu", subjects: [{ name: "Chimie" }, { name: "Biologie" }] }
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
    { id: 1, name: "Matematică" },
    { id: 2, name: "Fizică" },
    { id: 3, name: "Chimie" },
    { id: 4, name: "Biologie" },
    { id: 5, name: "Informatică" },
    { id: 6, name: "Limba Română" },
    { id: 7, name: "Limba Engleză" }
  ];
});

const selectSubject = (subject: string) => {
  selectedSubject.value = subject;
  isSubjectDropdownOpen.value = false;
  selectedTutor.value = null;
  selectedDate.value = '';
  selectedTimeSlot.value = null;
};

const selectTutor = (tutor: Tutor) => {
  selectedTutor.value = tutor;
  isTutorDropdownOpen.value = false;
  selectedDate.value = '';
  selectedTimeSlot.value = null;
};

const selectTimeSlot = (timeSlot: any) => {
  selectedTimeSlot.value = timeSlot;
  isTimeDropdownOpen.value = false;
};

// Close dropdowns when clicking outside
const closeDropdowns = (e: MouseEvent) => {
  const target = e.target as HTMLElement;
  if (!target.closest('.subject-dropdown')) {
    isSubjectDropdownOpen.value = false;
  }
  if (!target.closest('.tutor-dropdown')) {
    isTutorDropdownOpen.value = false;
  }
  if (!target.closest('.time-dropdown')) {
    isTimeDropdownOpen.value = false;
  }
};

// Rest of the existing code remains the same
const createBooking = async () => {
  if (!selectedSubject.value || !selectedTutor.value || !selectedTimeSlot.value || !description.value) {
    return; // Validation failed
  }

  loading.value = true;

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

    selectedSubject.value = '';
    selectedTutor.value = null;
    selectedDate.value = '';
    selectedTimeSlot.value = '';
    description.value = '';
    showForm.value = false;
  } catch (error) {
    console.error("Failed to create booking:", error);
  } finally {
    loading.value = false;
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
  return date.toLocaleDateString('ro-RO', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
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
    'pending': 'În așteptare',
    'confirmed': 'Confirmat',
    'cancelled': 'Anulat',
    'completed': 'Finalizat'
  };
  return statusMap[status as keyof typeof statusMap] || status;
};

const loadTutorTimeSlots = async () => {
  if (selectedTutor.value && selectedDate.value) {
    await bookingStore.getAvailableTimeSlots(selectedTutor.value.id, selectedDate.value);
  }
};

// Watch for changes in date and tutor to load available time slots
watch([selectedDate, selectedTutor], () => {
  if (selectedDate.value && selectedTutor.value) {
    loadTutorTimeSlots();
  }
});

onMounted(async () => {
  try {
    // Fetch student's bookings using mock student ID for now
    await bookingStore.fetchStudentBookings(Number(userStore.userId) || 1);
    // Add click event listener to close dropdowns when clicking outside
    document.addEventListener('click', closeDropdowns);
  } catch (error) {
    console.error("Error loading bookings:", error);
  }
});

// Clean up event listener on component unmount
onUnmounted(() => {
  document.removeEventListener('click', closeDropdowns);
});
</script>

<template>
  <div class="container px-4 py-6 mx-auto">
    <!-- Header with Create Booking Button -->
    <div class="flex items-center justify-between mb-6">
      <div>
        <h2 class="text-2xl font-bold">Rezervările mele</h2>
        <p class="text-gray-600">Gestionează ședințele tale de meditație</p>
      </div>
      <button 
        @click="showForm = !showForm" 
        class="px-4 py-2 font-medium text-white transition-colors bg-purple-600 rounded-lg shadow-sm hover:bg-purple-700"
      >
        <span v-if="!showForm">Programează o ședință</span>
        <span v-else>Închide formularul</span>
      </button>
    </div>

    <!-- Booking Form -->
    <div v-if="showForm" class="p-6 mb-6 bg-white rounded-lg shadow-md">
      <h3 class="mb-4 text-lg font-semibold">Programează o ședință de meditație</h3>
      <div class="space-y-4">
        <!-- Subject Selection - Custom Dropdown -->
        <div class="relative subject-dropdown">
          <label class="block mb-1 text-gray-700">Selectează materia</label>
          <div 
            @click="isSubjectDropdownOpen = !isSubjectDropdownOpen" 
            class="flex items-center justify-between w-full px-4 py-3 border border-gray-300 rounded-lg cursor-pointer"
          >
            <span :class="{'text-gray-400': !selectedSubject}">
              {{ selectedSubject || 'Alege o materie' }}
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
          
          <!-- Dropdown Items -->
          <div 
            v-if="isSubjectDropdownOpen" 
            class="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded-md shadow-lg"
          >
            <div class="py-1 overflow-auto max-h-60">
              <div 
                v-for="subject in subjects" 
                :key="subject.id"
                @click="selectSubject(subject.name)" 
                class="flex items-center px-4 py-2 cursor-pointer hover:bg-gray-100"
              >
                <span v-if="subject.name === selectedSubject" class="mr-2 text-blue-500">✓</span>
                <span v-else class="invisible mr-2">✓</span>
                {{ subject.name }}
              </div>
            </div>
          </div>
        </div>

        <!-- Tutor Selection - Custom Dropdown -->
        <div v-if="selectedSubject" class="relative tutor-dropdown">
          <label class="block mb-1 text-gray-700">Selectează profesorul</label>
          <div 
            @click="isTutorDropdownOpen = !isTutorDropdownOpen" 
            class="flex items-center justify-between w-full px-4 py-3 border border-gray-300 rounded-lg cursor-pointer"
          >
            <span :class="{'text-gray-400': !selectedTutor}">
              {{ selectedTutor ? `${selectedTutor.firstName} ${selectedTutor.lastName}` : 'Alege un profesor' }}
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
          
          <!-- Dropdown Items -->
          <div 
            v-if="isTutorDropdownOpen" 
            class="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded-md shadow-lg"
          >
            <div v-if="availableTutors.length === 0" class="px-4 py-2 text-sm text-red-500">
              Nu există profesori disponibili pentru această materie.
            </div>
            <div v-else class="py-1 overflow-auto max-h-60">
              <div 
                v-for="tutor in availableTutors" 
                :key="tutor.id"
                @click="selectTutor(tutor)" 
                class="flex items-center px-4 py-2 cursor-pointer hover:bg-gray-100"
              >
                <span v-if="selectedTutor && tutor.id === selectedTutor.id" class="mr-2 text-blue-500">✓</span>
                <span v-else class="invisible mr-2">✓</span>
                {{ tutor.firstName }} {{ tutor.lastName }}
              </div>
            </div>
          </div>
        </div>

        <!-- Date Selection - Keep the native date picker -->
        <div v-if="selectedTutor">
          <label class="block mb-1 text-gray-700">Selectează data</label>
          <input 
            type="date" 
            v-model="selectedDate" 
            class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent"
            :min="new Date().toISOString().split('T')[0]"
          >
        </div>

        <!-- Time Slot Selection - Custom Dropdown -->
        <div v-if="selectedDate" class="relative time-dropdown">
          <label class="block mb-1 text-gray-700">Selectează ora</label>
          <div 
            @click="isTimeDropdownOpen = !isTimeDropdownOpen" 
            class="flex items-center justify-between w-full px-4 py-3 border border-gray-300 rounded-lg cursor-pointer"
          >
            <span :class="{'text-gray-400': !selectedTimeSlot}">
              {{ selectedTimeSlot ? `${selectedTimeSlot.startTime} - ${selectedTimeSlot.endTime}` : 'Alege o oră' }}
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
          
          <!-- Dropdown Items -->
          <div 
            v-if="isTimeDropdownOpen" 
            class="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded-md shadow-lg"
          >
            <div v-if="availableTimeSlots.length === 0" class="px-4 py-2 text-sm text-red-500">
              Nu există ore disponibile pentru această dată. Te rugăm să selectezi o altă dată.
            </div>
            <div v-else class="py-1 overflow-auto max-h-60">
              <div 
                v-for="slot in availableTimeSlots" 
                :key="slot.id"
                @click="selectTimeSlot(slot)" 
                class="flex items-center px-4 py-2 cursor-pointer hover:bg-gray-100"
              >
                <span v-if="selectedTimeSlot && slot.id === selectedTimeSlot.id" class="mr-2 text-blue-500">✓</span>
                <span v-else class="invisible mr-2">✓</span>
                {{ slot.startTime }} - {{ slot.endTime }}
              </div>
            </div>
          </div>
        </div>

        <!-- Description -->
        <div v-if="selectedTimeSlot">
          <label class="block mb-1 text-gray-700">Descriere</label>
          <textarea 
            v-model="description" 
            placeholder="Descrie cu ce ai nevoie de ajutor..." 
            class="w-full h-24 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent"
          ></textarea>
        </div>

        <!-- Submit Button -->
        <button 
          @click="createBooking" 
          class="w-full px-4 py-2 font-medium text-white transition-colors bg-purple-600 rounded-lg hover:bg-purple-700 disabled:bg-purple-300 disabled:cursor-not-allowed"
          :disabled="!selectedSubject || !selectedTutor || !selectedTimeSlot || !description || loading"
        >
          <span v-if="!loading">Programează</span>
          <span v-else>Se procesează...</span>
        </button>
      </div>
    </div>

    <!-- Filter Tabs -->
    <div class="mb-6 border-b border-gray-200">
      <div class="flex -mb-px space-x-8">
        <button 
          @click="statusFilter = 'all'" 
          :class="[
            'py-2 px-1 border-b-2 font-medium text-sm', 
            statusFilter === 'all' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          Toate
        </button>
        <button 
          @click="statusFilter = 'pending'" 
          :class="[
            'py-2 px-1 border-b-2 font-medium text-sm', 
            statusFilter === 'pending' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          În așteptare
        </button>
        <button 
          @click="statusFilter = 'confirmed'" 
          :class="[
            'py-2 px-1 border-b-2 font-medium text-sm', 
            statusFilter === 'confirmed' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          Confirmate
        </button>
        <button 
          @click="statusFilter = 'completed'" 
          :class="[
            'py-2 px-1 border-b-2 font-medium text-sm', 
            statusFilter === 'completed' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          Finalizate
        </button>
        <button 
          @click="statusFilter = 'cancelled'" 
          :class="[
            'py-2 px-1 border-b-2 font-medium text-sm', 
            statusFilter === 'cancelled' 
              ? 'border-purple-500 text-purple-600' 
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          Anulate
        </button>
      </div>
    </div>

    <!-- Bookings List (unchanged) -->
    <div class="space-y-4">
      <div v-if="filteredBookings.length === 0" class="py-8 text-center">
        <p class="text-gray-500">Nu ai nicio rezervare {{ statusFilter !== 'all' ? `cu statusul "${getStatusText(statusFilter)}"` : '' }}.</p>
      </div>
      
      <div v-for="booking in filteredBookings" :key="booking.id" class="overflow-hidden bg-white rounded-lg shadow-md">
        <div class="p-5 border-b border-gray-100">
          <div class="flex items-start justify-between">
            <div>
              <h3 class="text-lg font-semibold">{{ booking.subject }}</h3>
              <p class="text-gray-600">Cu: {{ booking.tutorName }}</p>
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
              <span :class="['px-3 py-1 rounded-full text-xs font-medium', getStatusClass(booking.status)]">
                {{ getStatusText(booking.status) }}
              </span>
            </div>
          </div>
          <p class="mt-4 text-gray-600 whitespace-pre-line">{{ booking.message }}</p>
          
          <div class="flex justify-end mt-4" v-if="booking.status === 'pending' || booking.status === 'confirmed'">
            <button 
              @click="cancelBooking(booking.id)" 
              class="font-medium text-red-600 transition-colors hover:text-red-800"
            >
              Anulează rezervarea
            </button>
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
</style>
