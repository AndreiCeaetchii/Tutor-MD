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
  <div
    class="navigation-bar bg-white rounded-full p-2 flex flex-wrap justify-center md:justify-between gap-2 shadow-sm"
  >
    <div
      v-for="tab in tabs"
      :key="tab.name"
      @click="setActiveTab(tab.name)"
      class="tab-item flex items-center px-3 md:px-4 py-2 rounded-full cursor-pointer text-sm md:text-base"
      :class="{
        'bg-purple-50 text-purple-700': activeTab === tab.name,
        'text-gray-600': activeTab !== tab.name,
      }"
    >
      <span class="material-icons text-sm md:text-base mr-1 md:mr-2">{{ tab.icon }}</span>
      <span>{{ tab.name }}</span>
    </div>
  </div>
</template>

<style scoped>
.navigation-bar {
  max-width: 1200px;
  margin: 0 auto;
  overflow-x: auto;
}

.tab-item {
  transition: all 0.3s ease;
  white-space: nowrap;
  flex-shrink: 0;
}

.tab-item:hover {
  background-color: #f9f5ff;
}

@media (max-width: 640px) {
  .navigation-bar {
    justify-content: flex-start;
    border-radius: 1rem;
  }
}
</style>
