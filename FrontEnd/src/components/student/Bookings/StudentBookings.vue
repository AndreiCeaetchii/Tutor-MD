<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useBookingStore } from '../../../store/bookingStore';
import { getStudentBookings as getStudentBookingsAPI, cancelStudentBooking } from '../../../services/studentBookings';

const bookingStore = useBookingStore();

const loading = ref(false);
const statusFilter = ref('all');

const filteredBookings = computed(() => {
  if (statusFilter.value === 'all') {
    return bookingStore.studentBookings;
  }
  return bookingStore.studentBookings.filter(booking => booking.status.toLowerCase() === statusFilter.value);
});

const fetchStudentBookingsFromAPI = async () => {
  loading.value = true;
  try {
    const apiBookings = await getStudentBookingsAPI();
    
    const transformedBookings = apiBookings.map(booking => {
      return {
        id: booking.id,
        studentName: "Current Student",
        tutorName: booking.tutorName,
        subject: booking.subjectName || 'Subject not specified',
        status: mapStatusToString(booking.status),
        date: booking.date,
        startTime: booking.startTime.substring(0, 5),
        endTime: booking.endTime.substring(0, 5),
        time: booking.startTime.substring(0, 5),
        duration: calculateDuration(booking.startTime.substring(0, 5), booking.endTime.substring(0, 5)),
        message: booking.description,
        tutorImage: booking.tutorPhoto,
        tutorId: booking.tutorUserId,
      };
    });
    
    bookingStore.studentBookings = transformedBookings;
  } catch (error) {
    console.error('Error fetching student bookings:', error);
  } finally {
    loading.value = false;
  }
};

const cancelBooking = async (bookingId: number) => {
  try {
    loading.value = true;
    await cancelStudentBooking(bookingId);
    
    const booking = bookingStore.studentBookings.find(b => b.id === bookingId);
    if (booking) {
      booking.status = 'cancelled';
    }
    
    await fetchStudentBookingsFromAPI();
  } catch (error) {
    console.error("Failed to cancel booking:", error);
    alert(`Cancellation failed: ${error instanceof Error ? error.message : 'Unknown error'}`);
  } finally {
    loading.value = false;
  }
};

const mapStatusToString = (status: number): string => {
  switch (status) {
    case 0: return 'pending';
    case 1: return 'confirmed';
    case 2: return 'cancelled';
    case 3: return 'completed';
    default: return 'unknown';
  }
};

const calculateDuration = (startTime: string, endTime: string): string => {
  if (!startTime || !endTime) return '';
  
  const [startHour, startMinute] = startTime.split(':').map(Number);
  const [endHour, endMinute] = endTime.split(':').map(Number);
  
  let durationMinutes = (endHour * 60 + endMinute) - (startHour * 60 + startMinute);
  if (durationMinutes < 0) durationMinutes += 24 * 60;
  
  return `${durationMinutes} min`;
};

const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleDateString(navigator.language || 'en-US', { 
    weekday: 'long', 
    year: 'numeric', 
    month: 'long', 
    day: 'numeric' 
  });
};

const getStatusClass = (status: string) => {
  const statusClasses = {
    'pending': 'bg-yellow-100 text-yellow-800',
    'confirmed': 'bg-green-100 text-green-800',
    'cancelled': 'bg-red-100 text-red-800',
    'completed': 'bg-blue-100 text-blue-800'
  };
  return statusClasses[status.toLowerCase() as keyof typeof statusClasses] || 'bg-gray-100 text-gray-800';
};

const getStatusText = (status: string) => {
  const statusMap = {
    'pending': 'Pending',
    'confirmed': 'Confirmed',
    'cancelled': 'Cancelled',
    'completed': 'Completed',
    'all': 'All'
  };
  return statusMap[status as keyof typeof statusMap] || status;
};

onMounted(async () => {
  try {
    await fetchStudentBookingsFromAPI();
  } catch (error) {
    console.error("Error loading bookings:", error);
  }
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
    </div>

    <div v-if="loading" class="flex justify-center mb-6">
      <div class="w-8 h-8 border-4 border-purple-500 rounded-full border-t-transparent animate-spin"></div>
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

.btn-chat {
  color: #7e22ce;
  background-color: #f3e8ff;
}

.btn-chat:hover {
  background-color: #e9d5ff;
}
</style>
