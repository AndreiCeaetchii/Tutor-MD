<script setup lang="ts">
import { ref, computed } from 'vue';

// Booking status filter
const activeFilter = ref('All');

// Mock data for bookings
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

// Count bookings by status
const pendingCount = bookings.filter(b => b.status === 'Pending').length;
const acceptedCount = bookings.filter(b => b.status === 'Accepted').length;
const completedCount = bookings.filter(b => b.status === 'Completed').length;
const draftCount = bookings.filter(b => b.status === 'Draft').length;

// Filtered bookings based on active filter
const filteredBookings = computed(() => {
  if (activeFilter.value === 'All') {
    return bookings;
  }
  return bookings.filter(booking => booking.status === activeFilter.value);
});

// Methods
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
  <div class="tutor-bookings">
    <!-- Booking status cards -->
    <div class="grid grid-cols-4 gap-4 mb-8">
      <div class="p-4 text-center bg-white rounded-lg shadow-sm">
        <div class="text-2xl font-bold text-amber-500">{{ pendingCount }}</div>
        <div class="text-gray-600">Pending</div>
      </div>
      <div class="p-4 text-center bg-white rounded-lg shadow-sm">
        <div class="text-2xl font-bold text-blue-500">{{ acceptedCount }}</div>
        <div class="text-gray-600">Accepted</div>
      </div>
      <div class="p-4 text-center bg-white rounded-lg shadow-sm">
        <div class="text-2xl font-bold text-green-500">{{ completedCount }}</div>
        <div class="text-gray-600">Completed</div>
      </div>
      <div class="p-4 text-center bg-white rounded-lg shadow-sm">
        <div class="text-2xl font-bold text-gray-500">{{ draftCount }}</div>
        <div class="text-gray-600">Draft</div>
      </div>
    </div>

    <!-- Booking requests section -->
    <h2 class="mb-4 text-xl font-semibold">Booking Requests</h2>

    <!-- Filter tabs -->
    <div class="flex p-2 mb-6 bg-gray-100 rounded-full">
      <button 
        v-for="filter in ['All', 'Pending', 'Accepted', 'Completed', 'Draft']" 
        :key="filter"
        @click="setFilter(filter)"
        class="flex-1 px-4 py-2 text-center transition-colors rounded-full"
        :class="activeFilter === filter ? 'bg-white shadow-sm' : 'text-gray-600 hover:bg-gray-200'"
      >
        {{ filter }}
      </button>
    </div>

    <!-- Booking list -->
    <div class="space-y-4">
      <div 
        v-for="booking in filteredBookings" 
        :key="booking.id"
        class="p-6 bg-white border border-gray-200 rounded-lg"
      >
        <!-- Student info and status -->
        <div class="flex items-start justify-between mb-4">
          <div class="flex items-center gap-3">
            <div v-if="booking.studentImage" class="w-12 h-12 overflow-hidden rounded-full">
              <img :src="booking.studentImage" alt="Student" class="object-cover w-full h-full" />
            </div>
            <div v-else class="flex items-center justify-center w-12 h-12 bg-gray-200 rounded-full">
              <span class="text-gray-400 material-icons">person</span>
            </div>
            <div>
              <h3 class="text-lg font-semibold">{{ booking.studentName }}</h3>
              <p class="text-gray-600">{{ booking.subject }}</p>
            </div>
          </div>
          <div>
            <span 
              class="px-3 py-1 text-sm rounded-full"
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
        <div class="grid grid-cols-1 gap-4 mb-4 md:grid-cols-2">
          <div class="flex items-center">
            <span class="mr-2 text-gray-400 material-icons">calendar_today</span>
            <span>{{ booking.date }}</span>
          </div>
          <div class="flex items-center">
            <span class="mr-2 text-gray-400 material-icons">schedule</span>
            <span>{{ booking.time }} ({{ booking.duration }})</span>
          </div>
        </div>

        <!-- Booking message -->
        <p class="py-2 mb-4 text-gray-700 border-gray-100 border-y">
          {{ booking.message }}
        </p>

        <!-- Pricing and actions -->
        <div class="flex items-center justify-between">
          <div class="font-semibold text-green-600">${{ booking.hourlyRate }}/hour</div>
          <div class="flex gap-2">
            <!-- Conditional rendering based on booking status -->
            <template v-if="booking.status === 'Pending'">
              <button 
                @click="handleReject(booking.id)" 
                class="flex items-center px-4 py-2 text-gray-700 border border-gray-300 rounded-full hover:bg-gray-100"
              >
                <span class="mr-1 text-red-500 material-icons">close</span>
                Reject
              </button>
              <button 
                @click="handleAccept(booking.id)" 
                class="flex items-center px-4 py-2 text-white bg-green-600 rounded-full hover:bg-green-700"
              >
                <span class="mr-1 material-icons">check</span>
                Accept
              </button>
            </template>
            
            <template v-if="booking.status === 'Accepted'">
              <button 
                @click="handleMarkComplete(booking.id)" 
                class="flex items-center px-4 py-2 text-white bg-blue-600 rounded-full hover:bg-blue-700"
              >
                <span class="mr-1 material-icons">task_alt</span>
                Mark Complete
              </button>
            </template>
            
            <button class="p-2 border border-gray-300 rounded-full hover:bg-gray-100">
              <span class="text-gray-600 material-icons">chat</span>
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
