<script setup lang="ts">
  import { useRoute } from 'vue-router';

  const props = defineProps({
    basePath: { type: String, default: '/admin-dashboard' },
  });

  interface Tab {
    name: string;
    icon: string;
    path: string;
  }

  const tabs: Tab[] = [
    { name: 'Overview', icon: 'bar_chart', path: 'overview' },
    { name: 'Users', icon: 'group', path: 'users' },
    { name: 'Notifications', icon: 'notifications', path: 'notifications' },
    { name: 'Analytics', icon: 'trending_up', path: 'analytics' },
  ];

  const route = useRoute();

  const fullPath = (p: string) => (p.startsWith('/') ? p : `${props.basePath}/${p}`);

  const isActive = (p: string) => {
    const fp = fullPath(p);
    return route.path === fp || route.path.startsWith(fp + '/');
  };
</script>

<template>
  <nav class="flex items-center p-1 bg-white rounded-full shadow-sm overflow-x-auto">
    <router-link
      v-for="tab in tabs"
      :key="tab.name"
      :to="fullPath(tab.path)"
      class="flex flex-1 items-center justify-center px-4 py-2 text-sm font-medium rounded-full transition-all"
      :class="isActive(tab.path) ? 'bg-purple-500 text-white' : 'text-gray-700 hover:bg-gray-100'"
    >
      <span class="mr-2 text-base material-icons">{{ tab.icon }}</span>
      <span>{{ tab.name }}</span>
    </router-link>
  </nav>
</template>
