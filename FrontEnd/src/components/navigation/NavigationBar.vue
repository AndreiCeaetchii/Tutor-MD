<script setup lang="ts">
  import { computed } from 'vue';
  import { useUserStore } from '../../store/userStore.ts';
  import { useRoute } from 'vue-router';

  const store = useUserStore();
  const route = useRoute();

  // Definim tab-urile pentru tutor și student, dar acum includem și ruta
  const tutorTabs = [
    { name: 'Availability', icon: 'calendar_month', path: '/tutor-dashboard/availability' },
    { name: 'Profile', icon: 'person', path: '/tutor-dashboard/profile' },
    { name: 'Bookings', icon: 'book', path: '/tutor-dashboard/bookings' },
    { name: 'Reviews', icon: 'star', path: '/tutor-dashboard/reviews' },
    { name: 'Messages', icon: 'chat', path: '/tutor-dashboard/messages' },
  ];

  const studentTabs = [
    { name: 'Find Tutors', icon: 'search', path: '/student-dashboard/find' },
    { name: 'My Bookings', icon: 'book', path: '/student-dashboard/bookings' },
    { name: 'Reviews', icon: 'star', path: '/student-dashboard/reviews' },
    { name: 'Messages', icon: 'chat', path: '/student-dashboard/messages' },
    { name: 'My Account', icon: 'person', path: '/student-dashboard/account' },
  ];

  // Alegem tab-urile în funcție de rol
  const tabs = computed(() => (store.userRole === 'tutor' ? tutorTabs : studentTabs));

  // Verificăm care tab este activ în funcție de ruta curentă
  const isActive = (path: string) => route.path === path;
</script>

<template>
  <div class="flex p-2 mb-6 bg-gray-100 rounded-full navigation-bar">
    <router-link
      v-for="tab in tabs"
      :key="tab.name"
      :to="tab.path"
      class="flex items-center justify-center flex-1 px-4 py-2 text-center transition-colors rounded-full"
      :class="
        isActive(tab.path)
          ? 'bg-white shadow-sm text-purple-700'
          : 'text-gray-600 hover:bg-purple-50'
      "
    >
      <span class="mr-1 text-sm material-icons md:text-base md:mr-2">{{ tab.icon }}</span>
      <span class="text-sm md:text-base">{{ tab.name }}</span>
    </router-link>
  </div>
</template>

<style scoped>
  .navigation-bar {
    max-width: 1200px;
    margin: 0 auto;
    overflow-x: auto;
  }

  button,
  .router-link-active {
    white-space: nowrap;
    transition: all 0.3s ease;
  }

  .router-link-active {
    color: #7e22ce; /* Purple-700 */
  }

  button:hover,
  a:hover {
    color: #7e22ce;
  }

  button.bg-white,
  a.bg-white {
    color: #7e22ce;
  }

  @media (max-width: 640px) {
    .navigation-bar {
      justify-content: flex-start;
      border-radius: 1rem;
    }

    a {
      flex: 0 0 auto;
    }
  }
</style>
