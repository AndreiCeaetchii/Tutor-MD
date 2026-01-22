<script setup lang="ts">
  import { ref, computed, onMounted } from 'vue';
  import { useBookingStore } from '../../../store/bookingStore';
  import {
    getTutorBookings,
    updateBookingStatus,
    addBookingToGoogleCalendar,
  } from '../../../services/tutorBookings';

  const bookingStore = useBookingStore();
  const statusFilter = ref('all');
  const loading = ref(false);
  const error = ref('');

  const showNotification = ref(false);
  const notificationType = ref('error');
  const notificationMessage = ref('');

  const fetchTutorBookings = async () => {
    loading.value = true;
    try {
      const apiBookings = await getTutorBookings();

      const sortedBookings = [...apiBookings].sort((a, b) => {
        const dateA = new Date(a.date);
        const dateB = new Date(b.date);

        if (dateB.getTime() !== dateA.getTime()) {
          return dateB.getTime() - dateA.getTime();
        }

        const timeA = a.startTime;
        const timeB = b.startTime;
        return timeB.localeCompare(timeA);
      });

      const transformedBookings = sortedBookings.map((booking) => ({
        id: booking.id,
        studentName: booking.studentName,
        subject: booking.subject || 'Subject not specified',
        subjectName: booking.subject || 'Subject not specified',
        status: mapStatusToString(booking.status),
        date: booking.date,
        startTime: booking.startTime.substring(0, 5),
        endTime: booking.endTime.substring(0, 5),
        time: booking.startTime.substring(0, 5),
        duration: calculateDuration(booking.startTime, booking.endTime),
        message: booking.description,
        studentImage: booking.studentPhoto,
        tutorName: booking.tutorName,
        tutorId: booking.tutorUserId,
        studentId: booking.studentUserId,
      }));

      bookingStore.bookings = transformedBookings;
    } catch (err) {
      console.error('Error fetching tutor bookings:', err);
    } finally {
      loading.value = false;
    }
  };

  const mapStatusToString = (status: number): string => {
    switch (status) {
      case 0:
        return 'pending';
      case 1:
        return 'confirmed';
      case 2:
        return 'cancelled';
      case 3:
        return 'completed';
      default:
        return 'unknown';
    }
  };

  const calculateDuration = (startTime: string, endTime: string): string => {
    if (!startTime || !endTime) return '';

    const [startHour, startMinute] = startTime.split(':').map(Number);
    const [endHour, endMinute] = endTime.split(':').map(Number);

    let durationMinutes = endHour * 60 + endMinute - (startHour * 60 + startMinute);
    if (durationMinutes < 0) durationMinutes += 24 * 60;

    return `${durationMinutes} min`;
  };

  onMounted(async () => {
    await fetchTutorBookings();
  });

  const filteredBookings = computed(() => {
    if (statusFilter.value === 'all') return bookingStore.bookings;
    return bookingStore.bookings.filter((b) => b.status === statusFilter.value);
  });

  const handleAccept = async (bookingId: number) => {
    try {
      loading.value = true;
      await updateBookingStatus(bookingId, 1);

      const booking = bookingStore.bookings.find((b) => b.id === bookingId);
      if (booking) booking.status = 'confirmed';

      notificationMessage.value = 'Booking accepted successfully';
      notificationType.value = 'success';
      showNotification.value = true;
    } catch (err) {
      console.error('Error accepting booking:', err);
      error.value = err instanceof Error ? err.message : 'Failed to accept booking';

      notificationMessage.value = 'Failed to accept booking';
      notificationType.value = 'error';
      showNotification.value = true;
    } finally {
      loading.value = false;
    }
  };

  const handleReject = async (bookingId: number) => {
    try {
      loading.value = true;
      await updateBookingStatus(bookingId, 2);

      const booking = bookingStore.bookings.find((b) => b.id === bookingId);
      if (booking) booking.status = 'cancelled';

      notificationMessage.value = 'Booking rejected successfully';
      notificationType.value = 'success';
      showNotification.value = true;
    } catch (err) {
      console.error('Error rejecting booking:', err);
      error.value = err instanceof Error ? err.message : 'Failed to reject booking';

      notificationMessage.value = 'Failed to reject booking';
      notificationType.value = 'error';
      showNotification.value = true;
    } finally {
      loading.value = false;
    }
  };

  const handleMarkComplete = async (bookingId: number) => {
    try {
      loading.value = true;
      await updateBookingStatus(bookingId, 3);

      const booking = bookingStore.bookings.find((b) => b.id === bookingId);
      if (booking) booking.status = 'completed';

      notificationMessage.value = 'Booking marked as completed';
      notificationType.value = 'success';
      showNotification.value = true;
    } catch (err) {
      console.error('Error completing booking:', err);
      error.value = err instanceof Error ? err.message : 'Failed to complete booking';

      notificationMessage.value = 'Failed to mark booking as complete';
      notificationType.value = 'error';
      showNotification.value = true;
    } finally {
      loading.value = false;
    }
  };

  const getStatusText = (status: string) => {
    const statusMap = {
      pending: 'Pending',
      confirmed: 'Confirmed',
      cancelled: 'Cancelled',
      completed: 'Completed',
      all: 'All',
    };
    return statusMap[status as keyof typeof statusMap] || status;
  };

  function formatDate(dateStr: string): string {
    if (/^\d{4}-\d{2}-\d{2}$/.test(dateStr)) {
      const [year, month, day] = dateStr.split('-').map(Number);
      const date = new Date(year, month - 1, day);
      if (!isNaN(date.getTime())) {
        return date.toLocaleDateString(undefined, {
          year: 'numeric',
          month: 'short',
          day: 'numeric',
        });
      }
    }

    const date = new Date(dateStr);
    if (isNaN(date.getTime())) return dateStr;
    return date.toLocaleDateString(undefined, { year: 'numeric', month: 'short', day: 'numeric' });
  }

  function formatTimeInterval(startTime?: string, endTime?: string, duration?: string): string {
    if (!startTime || !endTime) return 'Time not specified';

    const [startHour, startMinute] = startTime.split(':').map(Number);
    const [endHour, endMinute] = endTime.split(':').map(Number);

    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${pad(startHour)}:${pad(startMinute)} - ${pad(endHour)}:${pad(endMinute)} ${duration ? `(${duration})` : ''}`;
  }

  declare const google: any;

  const handleAddToGoogleCalendar = (bookingId: number): void => {
    const booking = bookingStore.bookings.find((b) => b.id === bookingId);
    if (!booking) {
      console.error('Booking not found');
      notificationMessage.value = 'Booking not found';
      notificationType.value = 'error';
      showNotification.value = true;
      return;
    }

    // Create Google Calendar URL with event details
    const startDateTime = new Date(`${booking.date}T${booking.startTime}:00`);
    const endDateTime = new Date(`${booking.date}T${booking.endTime}:00`);

    // Format dates to Google Calendar format (YYYYMMDDTHHmmss)
    const formatDateForGoogle = (date: Date) => {
      return date.toISOString().replace(/[-:]/g, '').split('.')[0] + 'Z';
    };

    const googleCalendarUrl = new URL('https://calendar.google.com/calendar/render');
    googleCalendarUrl.searchParams.append('action', 'TEMPLATE');
    googleCalendarUrl.searchParams.append('text', `Tutoring Session: ${booking.subject}`);
    googleCalendarUrl.searchParams.append(
      'details',
      `Session with ${booking.studentName}\n\n${booking.message || 'No additional details'}`,
    );
    googleCalendarUrl.searchParams.append(
      'dates',
      `${formatDateForGoogle(startDateTime)}/${formatDateForGoogle(endDateTime)}`,
    );
    googleCalendarUrl.searchParams.append('location', 'Online');

    // Open Google Calendar in a new tab
    window.open(googleCalendarUrl.toString(), '_blank');

    notificationMessage.value = 'Opening Google Calendar...';
    notificationType.value = 'success';
    showNotification.value = true;
  };
</script>

<template>
  <div class="container px-4 py-6 mx-auto tutor-bookings">
    <div class="mb-6 -mx-4 sm:mx-0">
      <div class="overflow-x-auto hide-scrollbar">
        <div class="flex px-4 border-b border-gray-200 min-w-max sm:px-0">
          <button
            @click="statusFilter = 'all'"
            :class="[
              'py-1 px-2 sm:py-2 sm:px-3 border-b-2 text-xs sm:text-sm whitespace-nowrap',
              statusFilter === 'all'
                ? 'border-purple-500 text-purple-600 font-medium'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
            ]"
          >
            All
          </button>
          <button
            @click="statusFilter = 'pending'"
            :class="[
              'py-1 px-2 sm:py-2 sm:px-3 border-b-2 text-xs sm:text-sm whitespace-nowrap',
              statusFilter === 'pending'
                ? 'border-purple-500 text-purple-600 font-medium'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
            ]"
          >
            Pending
          </button>
          <button
            @click="statusFilter = 'confirmed'"
            :class="[
              'py-1 px-2 sm:py-2 sm:px-3 border-b-2 text-xs sm:text-sm whitespace-nowrap',
              statusFilter === 'confirmed'
                ? 'border-purple-500 text-purple-600 font-medium'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
            ]"
          >
            Confirmed
          </button>
          <button
            @click="statusFilter = 'completed'"
            :class="[
              'py-1 px-2 sm:py-2 sm:px-3 border-b-2 text-xs sm:text-sm whitespace-nowrap',
              statusFilter === 'completed'
                ? 'border-purple-500 text-purple-600 font-medium'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
            ]"
          >
            Completed
          </button>
          <button
            @click="statusFilter = 'cancelled'"
            :class="[
              'py-1 px-2 sm:py-2 sm:px-3 border-b-2 text-xs sm:text-sm whitespace-nowrap',
              statusFilter === 'cancelled'
                ? 'border-purple-500 text-purple-600 font-medium'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
            ]"
          >
            Cancelled
          </button>
        </div>
      </div>
    </div>

    <div v-if="loading" class="flex justify-center mb-6">
      <div
        class="w-8 h-8 border-4 border-purple-500 rounded-full border-t-transparent animate-spin"
      ></div>
    </div>

    <div
      v-if="!loading && filteredBookings.length === 0"
      class="flex flex-col items-center justify-center py-12 text-gray-400"
    >
      <div
        class="flex items-center justify-center w-24 h-24 mb-4 border-4 border-gray-200 rounded-full"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="w-12 h-12 text-gray-300"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
          />
        </svg>
      </div>
      <p class="text-lg font-medium text-gray-500">No bookings found</p>
      <p v-if="statusFilter !== 'all'" class="mt-2 text-gray-400">Try changing the filter</p>
      <p v-else class="mt-2 text-gray-400">
        Your booking history will appear here once you schedule sessions with students
      </p>
    </div>

    <div v-else class="space-y-4">
      <div
        v-for="booking in filteredBookings"
        :key="booking.id"
        class="p-4 bg-white border border-gray-200 rounded-lg shadow-sm sm:p-6"
      >
        <div
          class="flex flex-col gap-2 mb-4 sm:flex-row sm:items-start sm:justify-between sm:gap-0"
        >
          <div class="flex items-center gap-3">
            <div
              v-if="booking.studentImage"
              class="flex-shrink-0 w-10 h-10 overflow-hidden rounded-full sm:w-12 sm:h-12"
            >
              <img :src="booking.studentImage" alt="Student" class="object-cover w-full h-full" />
            </div>
            <div
              v-else
              class="flex items-center justify-center flex-shrink-0 w-10 h-10 bg-gray-200 rounded-full sm:w-12 sm:h-12"
            >
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
                'bg-amber-100 text-amber-800': booking.status === 'pending',
                'bg-blue-100 text-blue-800': booking.status === 'confirmed',
                'bg-green-100 text-green-800': booking.status === 'completed',
                'bg-red-100 text-red-800': booking.status === 'cancelled',
              }"
            >
              {{ getStatusText(booking.status) }}
            </span>
          </div>
        </div>

        <div class="grid grid-cols-1 gap-2 mb-3 text-sm sm:gap-4 sm:mb-4 sm:text-base">
          <div class="flex items-center">
            <span class="mr-2 text-sm text-gray-400 material-icons sm:text-base"
              >calendar_today</span
            >
            <span>{{ formatDate(booking.date) }}</span>
          </div>
          <div class="flex items-center">
            <span class="mr-2 text-sm text-gray-400 material-icons sm:text-base">schedule</span>
            <span>{{
              formatTimeInterval(booking.startTime, booking.endTime, booking.duration)
            }}</span>
          </div>
        </div>

        <p class="py-2 mb-3 text-sm text-gray-700 border-gray-100 sm:mb-4 sm:text-base border-y">
          {{ booking.message }}
        </p>

        <div
          class="flex flex-col items-start justify-between gap-3 sm:flex-row sm:items-center sm:gap-0"
        >
          <div></div>
          <div class="flex flex-wrap justify-end w-full gap-2 sm:w-auto">
            <template v-if="booking.status === 'pending'">
              <button
                @click="handleAddToGoogleCalendar(booking.id)"
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm text-blue-700 bg-blue-100 border border-blue-300 rounded-full hover:bg-blue-200"
                :disabled="loading"
              >
                <span class="mr-1 text-xs material-icons sm:text-sm">event</span>
                Add to Calendar
              </button>
              <!-- <button
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm font-semibold text-purple-700 bg-purple-100 border border-purple-300 rounded-full hover:bg-purple-200 transition"
                @click="$emit('openChat', booking.studentName)"
              >
                <span class="mr-1 text-sm material-icons">chat</span>
                Chat
              </button> -->
              <button
                @click="handleAccept(booking.id)"
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm text-white bg-green-600 rounded-full hover:bg-green-700"
                :disabled="loading"
              >
                <span class="mr-1 text-xs material-icons sm:text-sm">check</span>
                Accept
              </button>
              <button
                @click="handleReject(booking.id)"
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm text-gray-700 border border-gray-300 rounded-full hover:bg-gray-100"
                :disabled="loading"
              >
                <span class="mr-1 text-xs text-red-500 material-icons sm:text-sm">close</span>
                Reject
              </button>
            </template>
            <template v-if="booking.status === 'confirmed'">
              <button
                @click="handleAddToGoogleCalendar(booking.id)"
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm text-blue-700 bg-blue-100 border border-blue-300 rounded-full hover:bg-blue-200"
                :disabled="loading"
              >
                <span class="mr-1 text-xs material-icons sm:text-sm">event</span>
                Add to Calendar
              </button>
              <!-- <button
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm font-semibold text-purple-700 bg-purple-100 border border-purple-300 rounded-full hover:bg-purple-200 transition"
                @click="$emit('openChat', booking.studentName)"
              >
                <span class="mr-1 text-sm material-icons">chat</span>
                Chat
              </button> -->
              <button
                @click="handleMarkComplete(booking.id)"
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm text-white bg-blue-600 rounded-full hover:bg-blue-700"
                :disabled="loading"
              >
                <span class="mr-1 text-xs material-icons sm:text-sm">task_alt</span>
                Mark Complete
              </button>
            </template>
            <!-- <template v-if="booking.status === 'completed' || booking.status === 'cancelled'">
              <button
                class="flex items-center px-3 py-1.5 sm:px-4 sm:py-2 text-xs sm:text-sm font-semibold text-purple-700 bg-purple-100 border border-purple-300 rounded-full hover:bg-purple-200 transition"
                @click="$emit('openChat', booking.studentName)"
              >
                <span class="mr-1 text-sm material-icons">chat</span>
                Chat
              </button>
            </template> -->
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

  .material-icons {
    font-size: 18px;
  }

  .hide-scrollbar {
    scrollbar-width: none;
    -ms-overflow-style: none;
  }

  .hide-scrollbar::-webkit-scrollbar {
    display: none;
  }
</style>
