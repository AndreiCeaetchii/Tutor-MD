<script setup lang="ts">
import { ref, computed } from 'vue';

const activeFilter = ref('All');

const bookings = [
  {
    id: 1,
    studentName: 'Alex Chen',
    subject: 'Calculus',
    status: 'Pending',
    date: '9/15/2025',
    time: '10:00 AM',
    duration: '60 min',
    message: 'Hi! I need help with integration by parts and related problems. I have an exam next week.',
    hourlyRate: 75,
    studentImage: 'https://i.pravatar.cc/150?img=11'
  },
  {
    id: 2,
    studentName: 'Maria Rodriguez',
    subject: 'Statistics',
    status: 'Accepted',
    date: '9/16/2025',
    time: '2:00 PM',
    duration: '90 min',
    message: 'Looking for help with hypothesis testing and p-values.',
    hourlyRate: 75,
    studentImage: null
  }
];

const pendingCount = bookings.filter(b => b.status === 'Pending').length;
const acceptedCount = bookings.filter(b => b.status === 'Accepted').length;
const completedCount = bookings.filter(b => b.status === 'Completed').length;
const draftCount = bookings.filter(b => b.status === 'Draft').length;

const filteredBookings = computed(() => {
  if (activeFilter.value === 'All') {
    return bookings;
  }
  return bookings.filter(booking => booking.status === activeFilter.value);
});

const setFilter = (filter: string) => {
  activeFilter.value = filter;
};

const handleAccept = (bookingId: number) => {
  const booking = bookings.find(b => b.id === bookingId);
  if (booking) {
    booking.status = 'Accepted';
  }
};

const handleReject = (bookingId: number) => {
  // In a real app, you might want to remove or archive the booking
  console.log(`Rejected booking ${bookingId}`);
};

const handleMarkComplete = (bookingId: number) => {
  const booking = bookings.find(b => b.id === bookingId);
  if (booking) {
    booking.status = 'Completed';
  }
};
</script>

<template>
  <div class="p-4 tutor-bookings">
    <!-- Booking status cards - make responsive -->
    <div class="grid grid-cols-2 gap-3 mb-6 sm:grid-cols-4 sm:gap-4 sm:mb-8">
      <div class="p-3 text-center bg-white rounded-lg shadow-sm sm:p-4">
        <div class="text-xl font-bold sm:text-2xl text-amber-500">{{ pendingCount }}</div>
        <div class="text-sm text-gray-600 sm:text-base">Pending</div>
      </div>
      <div class="p-3 text-center bg-white rounded-lg shadow-sm sm:p-4">
        <div class="text-xl font-bold text-blue-500 sm:text-2xl">{{ acceptedCount }}</div>
        <div class="text-sm text-gray-600 sm:text-base">Accepted</div>
      </div>
      <div class="p-3 text-center bg-white rounded-lg shadow-sm sm:p-4">
        <div class="text-xl font-bold text-green-500 sm:text-2xl">{{ completedCount }}</div>
        <div class="text-sm text-gray-600 sm:text-base">Completed</div>
      </div>
      <div class="p-3 text-center bg-white rounded-lg shadow-sm sm:p-4">
        <div class="text-xl font-bold text-gray-500 sm:text-2xl">{{ draftCount }}</div>
        <div class="text-sm text-gray-600 sm:text-base">Draft</div>
      </div>
    </div>

    <!-- Booking requests section -->
    <h2 class="mb-4 text-lg font-semibold sm:text-xl">Booking Requests</h2>

    <!-- Filter tabs - improve mobile display -->
    <div class="mb-6 overflow-x-auto">
      <div class="flex p-1 mb-2 bg-gray-100 rounded-full sm:p-2 sm:mb-6 min-w-max">
        <button 
          v-for="filter in ['All', 'Pending', 'Accepted', 'Completed', 'Draft']" 
          :key="filter"
          @click="setFilter(filter)"
          class="flex-1 px-3 py-1.5 sm:px-4 sm:py-2 text-sm sm:text-base text-center transition-colors rounded-full whitespace-nowrap"
          :class="activeFilter === filter ? 'bg-white shadow-sm' : 'text-gray-600 hover:bg-gray-200'"
        >
          {{ filter }}
        </button>
      </div>
    </div>

    <!-- Booking list -->
    <div class="space-y-4">
      <div 
        v-for="booking in filteredBookings" 
        :key="booking.id"
        class="p-4 bg-white border border-gray-200 rounded-lg sm:p-6"
      >
        <!-- Student info and status - improved for mobile -->
        <div class="flex flex-col gap-2 mb-4 sm:flex-row sm:items-start sm:justify-between sm:gap-0">
          <div class="flex items-center gap-3">
            <div v-if="booking.studentImage" class="flex-shrink-0 w-10 h-10 overflow-hidden rounded-full sm:w-12 sm:h-12">
              <img :src="booking.studentImage" alt="Student" class="object-cover w-full h-full" />
            </div>
            <div v-else class="flex items-center justify-center flex-shrink-0 w-10 h-10 bg-gray-200 rounded-full sm:w-12 sm:h-12">
              <span class="text-sm text-gray-400 material-icons sm:text-base">person</span>
            </div>
            <div>
              <h3 class="text-base font-semibold sm:text-lg">{{ booking.studentName }}</h3>
              <p class="text-sm text-gray-600 sm:text-base">{{ booking.subject }}</p>
            </div>
          </div>
          <div class="self-start sm:self-auto">
            <span 
              class="px-2 py-1 text-xs rounded-full sm:text-sm"
              :class="{
                'bg-amber-100 text-amber-800': booking.status === 'Pending',
                'bg-blue-100 text-blue-800': booking.status === 'Accepted',
                'bg-green-100 text-green-800': booking.status === 'Completed',
                'bg-gray-100 text-gray-800': booking.status === 'Draft'
              }"
            >
              {{ booking.status }}
            </span>
          </div>
        </div>

        <!-- Booking details -->
        <div class="grid grid-cols-1 gap-2 mb-3 text-sm sm:gap-4 sm:mb-4 sm:text-base">
          <div class="flex items-center">
            <span class="mr-2 text-sm text-gray-400 material-icons sm:text-base">calendar_today</span>
            <span>{{ booking.date }}</span>
          </div>
          <div class="flex items-center">
            <span class="mr-2 text-sm text-gray-400 material-icons sm:text-base">schedule</span>
            <span>{{ booking.time }} ({{ booking.duration }})</span>
          </div>
        </div>

        <!-- Booking message -->
        <p class="py-2 mb-3 text-sm text-gray-700 border-gray-100 sm:mb-4 sm:text-base border-y">
          {{ booking.message }}
        </p>

        <!-- Pricing and actions -->
        <div class="flex flex-col items-start justify-between gap-3 sm:flex-row sm:items-center sm:gap-0">
          <div class="text-sm font-semibold text-green-600 sm:text-base">${{ booking.hourlyRate }}/hour</div>
          <div class="flex flex-wrap justify-end w-full gap-2 sm:w-auto">
            <!-- Conditional rendering based on booking status -->
            <template v-if="booking.status === 'Pending'">
              <button 
                @click="handleReject(booking.id)" 
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm text-gray-700 border border-gray-300 rounded-full hover:bg-gray-100"
              >
                <span class="mr-1 text-xs text-red-500 material-icons sm:text-sm">close</span>
                Reject
              </button>
              <button 
                @click="handleAccept(booking.id)" 
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm text-white bg-green-600 rounded-full hover:bg-green-700"
              >
                <span class="mr-1 text-xs material-icons sm:text-sm">check</span>
                Accept
              </button>
            </template>
            
            <template v-if="booking.status === 'Accepted'">
              <button 
                @click="handleMarkComplete(booking.id)" 
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm text-white bg-blue-600 rounded-full hover:bg-blue-700"
              >
                <span class="mr-1 text-xs material-icons sm:text-sm">task_alt</span>
                Mark Complete
              </button>
            </template>
            
            <button class="p-1.5 sm:p-2 border border-gray-300 rounded-full hover:bg-gray-100">
              <span class="text-xs text-gray-600 material-icons sm:text-sm">chat</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.tutor-bookings {
  max-width: 1200px;
  width: 100%;
  margin: 0 auto;
}
</style>
