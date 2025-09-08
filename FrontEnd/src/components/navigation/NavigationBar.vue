<script setup lang="ts">
import { ref, computed } from "vue";
import { userStore } from "../../store/userStore";

const store = userStore();
const activeTab = ref("");
const emit = defineEmits(['tabChange']);

const tutorTabs = [
  { name: "Availability", icon: "calendar_month" },
  { name: "Profile", icon: "person" },
  { name: "Bookings", icon: "book" },
  { name: "Reviews", icon: "star" },
  { name: "Messages", icon: "chat" },
];

const studentTabs = [
  { name: "Find Tutors", icon: "search" },
  { name: "My Bookings", icon: "book" },
  { name: "Reviews", icon: "star" },
  { name: "Messages", icon: "chat" },
  { name: "My Account", icon: "person" },
];

const tabs = computed(() => {
  return store.userRole === "tutor" ? tutorTabs : studentTabs;
});

const initializeActiveTab = () => {
  if (tabs.value.length > 0) {
    activeTab.value = tabs.value[0].name;
    emit('tabChange', activeTab.value);
  }
};

initializeActiveTab();

const setActiveTab = (tabName: string) => {
  activeTab.value = tabName;
  emit('tabChange', tabName);
};
</script>

<template>
  <div class="flex p-2 mb-6 bg-gray-100 rounded-full navigation-bar">
    <button 
      v-for="tab in tabs" 
      :key="tab.name"
      @click="setActiveTab(tab.name)"
      class="flex items-center justify-center flex-1 px-4 py-2 text-center transition-colors rounded-full"
      :class="activeTab === tab.name ? 'bg-white shadow-sm text-purple-700' : 'text-gray-600 hover:bg-purple-50'"
    >
      <span class="mr-1 text-sm material-icons md:text-base md:mr-2">{{ tab.icon }}</span>
      <span class="text-sm md:text-base">{{ tab.name }}</span>
    </button>
  </div>
</template>

<style scoped>
.navigation-bar {
  max-width: 1200px;
  margin: 0 auto;
  overflow-x: auto;
}

button {
  white-space: nowrap;
  transition: all 0.3s ease;
}

button:hover {
  color: #7e22ce; /* Purple-700 equivalent */
}

button.bg-white {
  color: #7e22ce;
}

@media (max-width: 640px) {
  .navigation-bar {
    justify-content: flex-start;
    border-radius: 1rem;
  }
  
  button {
    flex: 0 0 auto;
  }
}
</style>