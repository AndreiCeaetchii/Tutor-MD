<script setup lang="ts">
import { ref, computed } from 'vue';

const props = defineProps({
  userRole: {
    type: String,
    default: 'student', // sau 'tutor'
    validator: (value: string) => ['student', 'tutor'].includes(value)
  }
});

const activeTab = ref('');

const tutorTabs = [
  { name: 'Availability', icon: 'calendar_month' },
  { name: 'Profile', icon: 'person' },
  { name: 'Bookings', icon: 'book' },
  { name: 'Reviews', icon: 'star' },
  { name: 'Messages', icon: 'chat' }
];

const studentTabs = [
  { name: 'Find Tutors', icon: 'search' },
  { name: 'My Bookings', icon: 'book' },
  { name: 'Reviews', icon: 'star' },
  { name: 'Messages', icon: 'chat' },
  { name: 'My Account', icon: 'person' }
];

const tabs = computed(() => {
  return props.userRole === 'tutor' ? tutorTabs : studentTabs;
});

const initializeActiveTab = () => {
  if (tabs.value.length > 0) {
    activeTab.value = tabs.value[0].name;
  }
};

// Inițializăm tabul activ
initializeActiveTab();

const setActiveTab = (tabName: string) => {
  activeTab.value = tabName;
};
</script>

<template>
  <div class="navigation-bar bg-white rounded-full p-2 flex justify-between shadow-sm">
    <div 
      v-for="tab in tabs" 
      :key="tab.name"
      @click="setActiveTab(tab.name)"
      class="tab-item flex items-center px-4 py-2 rounded-full cursor-pointer"
      :class="{ 'bg-purple-50 text-purple-700': activeTab === tab.name, 'text-gray-600': activeTab !== tab.name }"
    >
      <span class="material-icons mr-2">{{ tab.icon }}</span>
      <span>{{ tab.name }}</span>
    </div>
  </div>
</template>

<style scoped>
.navigation-bar {
  max-width: 800px;
  margin: 0 auto;
}

.tab-item {
  transition: all 0.3s ease;
}

.tab-item:hover {
  background-color: #f9f5ff;
}
</style>