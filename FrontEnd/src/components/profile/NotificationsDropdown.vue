<template>
  <div class="relative">
    <button @click="toggleNotificationsMenu" ref="notificationsButton" class="relative">
      <Bell class="w-5 h-5 text-gray-600 transition-colors hover:text-gray-900" />
      <span
        v-if="unreadCount > 0"
        class="absolute top-0 right-0 inline-flex items-center justify-center h-4 w-4 text-xs font-bold leading-none text-red-100 bg-red-600 rounded-full transform translate-x-1/2 -translate-y-1/2"
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
        class="absolute right-0 z-50 w-80 py-2 mt-2 bg-white shadow-2xl rounded-xl"
      >
        <div class="flex justify-between items-center px-4 py-2 border-b border-gray-100">
          <p class="text-sm font-semibold text-gray-900">Notificări ({{ notifications.length }})</p>
          <button
            @click="markAllAsRead"
            v-if="unreadCount > 0"
            class="text-xs text-violet-600 hover:text-violet-800"
          >
            Marchează tot ca citit
          </button>
        </div>

        <div v-if="isLoading" class="px-4 py-2 text-center text-sm text-gray-500">
          Se încarcă notificările...
        </div>

        <div v-else-if="notifications.length > 0" class="py-1 max-h-96 overflow-y-auto">
          <a
            v-for="notification in notifications"
            :key="notification.id"
            @click.prevent="markAsRead(notification.id)"
            href="#"
            class="block w-full text-sm transition-colors duration-200 group"
          >
            <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
              <font-awesome-icon
                :icon="['fas', 'bell']"
                :class="{
                  'text-violet-600': notification.status === 0,
                  'text-gray-400': notification.status === 1,
                }"
                class="w-4 h-4 mr-3 transition-colors duration-200 group-hover:text-violet-600"
              />
              <div class="flex-1">
                <p
                  :class="{ 'font-semibold text-gray-900': notification.status === 0 }"
                  class="font-normal flex items-center gap-2 transition-colors duration-200 group-hover:text-violet-600"
                >
                  {{ getNotificationTitle(notification) }}
                  <span
                    v-if="notification.status === 0"
                    class="h-2 w-2 bg-red-500 rounded-full"
                  ></span>
                </p>
                <p
                  class="text-xs text-gray-500 mt-1 transition-colors duration-200 group-hover:text-violet-600"
                >
                  {{ parseNotificationPayload(notification.payload) }}
                </p>
              </div>
            </div>
          </a>
        </div>
        <div v-else class="px-4 py-2 text-center text-sm text-gray-500">
          Nu ai nicio notificare nouă.
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, onBeforeUnmount, computed, watch } from 'vue';
  import { Bell } from 'lucide-vue-next';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { faBell } from '@fortawesome/free-solid-svg-icons';
  import {
    getUserNotifications,
    markNotificationAsRead,
    type Notification,
  } from '../../services/notificationService.ts';
  import { useUserStore } from '../../store/userStore.ts';

  library.add(faBell);

  const showNotificationsMenu = ref(false);
  const notificationsButton = ref<HTMLElement | null>(null);
  const notificationsMenu = ref<HTMLElement | null>(null);

  const notifications = ref<Notification[]>([]);
  const isLoading = ref(true);
  const userStore = useUserStore();

  const unreadCount = computed(() => notifications.value.filter((n) => n.status === 0).length);

  function toggleNotificationsMenu() {
    showNotificationsMenu.value = !showNotificationsMenu.value;
  }

  function parseNotificationPayload(payload: string) {
    try {
      const data = JSON.parse(payload);
      if (data.Message) return data.Message;
      return 'Detalii notificare indisponibile.';
    } catch (e) {
      return 'Formatul notificării este invalid.';
    }
  }

  function getNotificationTitle(notification: Notification) {
    try {
      const data = JSON.parse(notification.payload);
      switch (notification.type) {
        case 'NewReview':
          return `Recenzie de la ${data.StudentName}`;
        case 'NewMessage':
          return `Mesaj nou de la ${data.SenderName}`;
        case 'CourseUpdate':
          return `Curs actualizat: ${data.Title}`;
        default:
          return `Notificare de tip "${notification.type}"`;
      }
    } catch (e) {
      return `Notificare de tip "${notification.type}"`;
    }
  }

  async function markAsRead(id: number) {
    const notification = notifications.value.find((n) => n.id === id);
    if (notification && notification.status === 0) {
      try {
        await markNotificationAsRead(id);
        notification.status = 1;
      } catch (error) {
        console.error('Eroare la marcarea notificării ca citită:', error);
      }
    }
  }

  function markAllAsRead() {
    notifications.value.forEach((n) => (n.status = 1));
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

  async function fetchNotifications() {
    isLoading.value = true;
    try {
      const response = await getUserNotifications();
      notifications.value = response.notifications;
    } catch (error) {
      console.error('Eroare la preluarea notificărilor:', error);
    } finally {
      isLoading.value = false;
    }
  }

  onMounted(() => {
    document.addEventListener('click', handleClickOutside);
    // Încărcăm notificările la montarea inițială
    if (userStore.accessToken) {
      fetchNotifications();
    }
  });

  onBeforeUnmount(() => {
    document.removeEventListener('click', handleClickOutside);
  });

  // Watcher pentru a reacționa la schimbările de autentificare
  watch(
    () => userStore.accessToken,
    (newAccessToken, oldAccessToken) => {
      // Când un utilizator se deloghează, ștergem notificările vechi
      if (!newAccessToken) {
        notifications.value = [];
      }

      // Când un utilizator se loghează (detectăm un token nou), încărcăm noile notificări
      if (newAccessToken && newAccessToken !== oldAccessToken) {
        fetchNotifications();
      }
    },
    { immediate: true },
  );
</script>
