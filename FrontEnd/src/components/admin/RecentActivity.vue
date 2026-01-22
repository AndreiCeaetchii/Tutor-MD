<script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import { getRecentActivities, type RecentActivityItem } from '../../services/adminService';

  const activities = ref<RecentActivityItem[]>([]);
  const loading = ref(false);

  const getBadgeColor = (type: string) => {
    switch (type) {
      case 'booking':
        return 'bg-blue-100 text-blue-800';
      case 'tutor':
        return 'bg-green-100 text-green-800';
      case 'student':
      case 'user':
        return 'bg-purple-200 text-purple-800';
      case 'review':
        return 'bg-orange-100 text-orange-800';
      case 'alert':
        return 'bg-yellow-100 text-yellow-800';
      default:
        return 'bg-gray-200 text-gray-800';
    }
  };

  const getDotColor = (type: string) => {
    switch (type) {
      case 'booking':
      case 'student':
      case 'user':
        return 'bg-blue-500';
      case 'tutor':
      case 'review':
        return 'bg-green-500';
      case 'alert':
        return 'bg-yellow-500';
      default:
        return 'bg-gray-500';
    }
  };

  const getTimeAgo = (timestamp: Date) => {
    const now = new Date();
    const diff = now.getTime() - new Date(timestamp).getTime();
    const minutes = Math.floor(diff / 60000);
    const hours = Math.floor(diff / 3600000);
    const days = Math.floor(diff / 86400000);

    if (minutes < 1) return 'Just now';
    if (minutes < 60) return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    if (hours < 24) return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    return `${days} day${days > 1 ? 's' : ''} ago`;
  };

  onMounted(async () => {
    loading.value = true;
    try {
      activities.value = await getRecentActivities();
    } catch (error) {
      console.error('Error loading recent activities:', error);
    } finally {
      loading.value = false;
    }
  });
</script>

<template>
  <div class="p-6 bg-white rounded-lg shadow-md">
    <h2 class="mb-4 text-xl font-bold text-gray-800">Recent Activity</h2>

    <div v-if="loading" class="flex justify-center py-8">
      <div
        class="w-8 h-8 border-4 border-purple-500 rounded-full border-t-transparent animate-spin"
      ></div>
    </div>

    <div v-else-if="activities.length === 0" class="py-8 text-center text-gray-500">
      No recent activities
    </div>

    <ul v-else class="space-y-4">
      <li v-for="activity in activities" :key="activity.id" class="flex items-start">
        <div :class="['w-2 h-2 rounded-full mt-2 mr-3', getDotColor(activity.type)]"></div>
        <div class="flex-1">
          <p class="text-gray-800" v-html="activity.message"></p>
          <p class="text-sm text-gray-500">{{ getTimeAgo(activity.timestamp) }}</p>
        </div>
        <span
          :class="[
            'ml-auto px-2 py-1 text-xs font-semibold rounded-full',
            getBadgeColor(activity.type),
          ]"
        >
          {{ activity.badge }}
        </span>
      </li>
    </ul>
  </div>
</template>
