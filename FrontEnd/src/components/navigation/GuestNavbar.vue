<script setup lang="ts">
  import { useRoute } from 'vue-router';
  import { computed } from 'vue';

  const route = useRoute();
  const tutorId = computed(() => (route.params.id as string) || '1');

  const studentTabs = computed(() => [
    { name: 'Profile', icon: 'person', path: `/tutor/${tutorId.value}/profile` },
    { name: 'Availability', icon: 'calendar_month', path: `/tutor/${tutorId.value}/availability` },
    { name: 'Reviews', icon: 'star', path: `/tutor/${tutorId.value}/reviews` },
  ]);

  const isActive = (path: string) => route.path.startsWith(path);
</script>

<template>
  <div class="max-w-3xl mx-auto mt-8">
    <!-- Informative title -->
    <div class="text-center mb-4">
      <h2 class="text-2xl font-semibold text-gray-800">
        Explore the details of the
        <span class="text-purple-600 font-bold">selected tutor</span>
      </h2>
      <p class="text-gray-500 text-sm">
        Navigate between Profile, Availability, and Reviews to learn more.
      </p>
    </div>

    <div class="flex p-2 mb-2 bg-gray-100 rounded-full overflow-x-auto">
      <router-link
        v-for="tab in studentTabs"
        :key="tab.name"
        :to="tab.path"
        class="flex items-center justify-center flex-1 px-4 py-2 text-center transition-colors rounded-full"
        :class="
          isActive(tab.path)
            ? 'bg-white shadow-sm text-purple-700'
            : 'text-gray-600 hover:bg-purple-50'
        "
      >
        <span class="material-icons mr-1 text-sm md:mr-2 md:text-base">
          {{ tab.icon }}
        </span>
        <span class="text-sm md:text-base">{{ tab.name }}</span>
      </router-link>
    </div>
  </div>
</template>
