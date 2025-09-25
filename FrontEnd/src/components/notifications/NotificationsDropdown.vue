<script setup lang="ts">
  import { ref, onMounted, onBeforeUnmount, computed, watch } from 'vue';
  import { Bell } from 'lucide-vue-next';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { faBell } from '@fortawesome/free-solid-svg-icons';
  import { type Notification } from '../../services/notificationService.ts';
  import { useUserStore } from '../../store/userStore.ts';
  import { useNotificationStore } from '../../store/notificationStore.ts';

  library.add(faBell);

  const showNotificationsMenu = ref(false);
  const notificationsButton = ref<HTMLElement | null>(null);
  const notificationsMenu = ref<HTMLElement | null>(null);
  const userStore = useUserStore();
  const notificationStore = useNotificationStore();

  const isLoading = computed(() => notificationStore.loading);
  const notifications = computed(() => notificationStore.notifications);
  const unreadCount = computed(() => notificationStore.unreadCount);

  const badgeSizeClass = computed(() => {
    const count = unreadCount.value;
    if (count >= 100) return 'w-6 h-6 text-xxs';
    if (count >= 10) return 'w-5 h-5 text-xs';
    return 'w-4 h-4';
  });

  function isNotificationRead(notificationId: number): boolean {
    return notificationStore.locallyReadIds.includes(notificationId);
  }

  function toggleNotificationsMenu() {
    showNotificationsMenu.value = !showNotificationsMenu.value;
  }

  function handleNotificationClick(notification: Notification) {
    notificationStore.markAsRead(notification.id);
  }

  function parseNotificationPayload(payload: string, type: string) {
    try {
      const data = JSON.parse(payload);

      switch (type) {
        case 'NewBooking':
          return `${data.StudentName} booked a ${data.Subject} session on ${formatDate(data.Date)} from ${formatTime(data.StartTime)} to ${formatTime(data.EndTime)}`;

        case 'BookingUpdatedStatus':
          const statusText = getStatusText(data.Status);
          return `Your booking for ${data.Subject} on ${formatDate(data.Date)} was ${statusText}`;

        case 'NewReview':
          return `"${data.Message}" - ${data.Rating}/5 stars`;

        default:
          if (data.Message) return data.Message;
          return JSON.stringify(data).substring(0, 100);
      }
    } catch (e) {
      console.error('Error parsing notification payload:', e);
      return 'Unable to display notification details.';
    }
  }

  function getStatusText(status: number): string {
    switch (status) {
      case 0:
        return 'pending';
      case 1:
        return 'accepted';
      case 2:
        return 'completed';
      case 3:
        return 'cancelled';
      default:
        return 'updated';
    }
  }

  function formatDate(dateStr: string): string {
    try {
      if (typeof dateStr === 'string' && dateStr.match(/^\w{3} \d{1,2}, \d{4}$/)) {
        return dateStr;
      }

      const date = new Date(dateStr);

      if (isNaN(date.getTime())) {
        return 'Upcoming';
      }

      return new Intl.DateTimeFormat('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric',
      }).format(date);
    } catch (e) {
      return 'Upcoming';
    }
  }

  function formatTime(timeStr: string): string {
    if (!timeStr) return '';
    if (timeStr.includes('T')) {
      const date = new Date(timeStr);
      return date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' });
    }
    return timeStr.substring(0, 5);
  }

  function formatNotificationTime(timestamp: string | undefined): string {
    if (timestamp === undefined || timestamp === null) return 'recently';
    
    try {
      if (timestamp === '') return 'recently';
      
      let date: Date;
      
      if (!isNaN(Number(timestamp))) {
        const timestampNum = Number(timestamp);
        date = new Date(timestampNum < 10000000000 ? timestampNum * 1000 : timestampNum);
      } else {
        date = new Date(timestamp);
      }
      
      if (isNaN(date.getTime())) {
        return 'recently';
      }
      
      const now = new Date();
      const diffMs = now.getTime() - date.getTime();
      const diffMins = Math.floor(diffMs / 60000);
      const diffHours = Math.floor(diffMins / 60);
      const diffDays = Math.floor(diffHours / 24);
      
      if (diffMins < 1) return 'just now';
      if (diffMins < 60) return diffMins === 1 ? '1 minute ago' : `${diffMins} minutes ago`;
      if (diffHours < 24) return diffHours === 1 ? '1 hour ago' : `${diffHours} hours ago`;
      if (diffDays < 7) return diffDays === 1 ? '1 day ago' : `${diffDays} days ago`;
      
      return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
    } catch (e) {
      return 'recently';
    }
  }

  function getNotificationTitle(notification: Notification) {
    try {
      const data = JSON.parse(notification.payload);
      switch (notification.type) {
        case 'NewBooking':
          return `New Booking Request`;
        case 'BookingUpdatedStatus':
          return `Booking Status Update`;
        case 'NewReview':
          return `New Review from ${data.StudentName}`;
        case 'NewMessage':
          return `New Message from ${data.SenderName}`;
        case 'CourseUpdate':
          return `Course Updated: ${data.Title}`;
        default:
          return `${notification.type} Notification`;
      }
    } catch (e) {
      return `${notification.type} Notification`;
    }
  }

  function handleClickOutside(event: MouseEvent) {
    if (
      showNotificationsMenu.value &&
      notificationsButton.value &&
      notificationsMenu.value &&
      !notificationsButton.value.contains(event.target as Node) &&
      !notificationsMenu.value.contains(event.target as Node)
    ) {
      showNotificationsMenu.value = false;
    }
  }

  onMounted(() => {
    document.addEventListener('click', handleClickOutside);
    notificationStore.initStore();

    if (userStore.accessToken) {
      notificationStore.fetchNotifications();
    }
  });

  onBeforeUnmount(() => {
    document.removeEventListener('click', handleClickOutside);
  });

  watch(
    () => userStore.accessToken,
    (newAccessToken, oldAccessToken) => {
      if (newAccessToken && newAccessToken !== oldAccessToken) {
        notificationStore.fetchNotifications();
      }
    },
    { immediate: true },
  );
</script>

<template>
  <div class="relative">
    <button @click="toggleNotificationsMenu" ref="notificationsButton" class="relative">
      <Bell class="w-5 h-5 text-gray-600 transition-colors hover:text-gray-900" />
      <span
        v-if="unreadCount > 0"
        :class="[
          'absolute top-0 right-0 inline-flex items-center justify-center text-xs font-bold leading-none text-red-100 transform translate-x-1/2 -translate-y-1/2 bg-red-600 rounded-full',
          badgeSizeClass,
        ]"
        >{{ unreadCount }}</span
      >
    </button>
    <transition
      enter-active-class="transition duration-200 ease-out"
      enter-from-class="transform scale-95 opacity-0"
      enter-to-class="transform scale-100 opacity-100"
      leave-active-class="transition duration-75 ease-in"
      leave-from-class="transform scale-100 opacity-100"
      leave-to-class="transform scale-95 opacity-0"
    >
      <div
        v-if="showNotificationsMenu"
        ref="notificationsMenu"
        class="absolute right-0 z-50 py-2 mt-2 bg-white shadow-2xl w-80 rounded-xl"
      >
        <div class="flex items-center justify-between px-4 py-2 border-b border-gray-100">
          <p class="text-sm font-semibold text-gray-900">Notifications ({{ notifications.length }})</p>
        </div>

        <div v-if="isLoading" class="px-4 py-2 text-sm text-center text-gray-500">
          Loading notifications...
        </div>

        <div v-else-if="notifications.length > 0" class="py-1 overflow-y-auto max-h-96">
          <div
            v-for="notification in notifications"
            :key="notification.id"
            @click="handleNotificationClick(notification)"
            class="block w-full text-sm transition-colors duration-200 cursor-pointer group"
          >
            <div
              class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50 cursor-pointer"
            >
              <font-awesome-icon
                :icon="['fas', 'bell']"
                :class="{
                  'text-violet-600':
                    notification.status === 0 && !isNotificationRead(notification.id),
                  'text-gray-400': notification.status === 1 || isNotificationRead(notification.id),
                }"
                class="w-4 h-4 mr-3 transition-colors duration-200 group-hover:text-violet-600"
              />
              <div class="flex-1">
                <p
                  :class="{
                    'font-semibold text-gray-900':
                      notification.status === 0 && !isNotificationRead(notification.id),
                  }"
                  class="flex items-center gap-2 font-normal transition-colors duration-200 group-hover:text-violet-600"
                >
                  {{ getNotificationTitle(notification) }}
                  <span
                    v-if="notification.status === 0 && !isNotificationRead(notification.id)"
                    class="w-2 h-2 bg-red-500 rounded-full"
                  ></span>
                </p>
                <p
                  class="mt-1 text-xs text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                >
                  {{ parseNotificationPayload(notification.payload, notification.type) }}
                </p>
                <p class="mt-1 text-xs text-gray-400">
                  {{ formatNotificationTime(notification.createdAt) }}
                </p>
              </div>
            </div>
          </div>
        </div>
        <div v-else class="px-4 py-2 text-sm text-center text-gray-500">
          You have no notifications.
        </div>
      </div>
    </transition>
  </div>
</template>

<style>
  .text-xxs {
    font-size: 0.65rem;
  }
</style>
