<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useRoute } from 'vue-router';

  import GuestNavbar from '../components/navigation/GuestNavbar.vue';
  import Footer from '../components/Footer.vue';

  const route = useRoute();

  const activeTab = ref('find');

  const handleTabChange = (tab: string) => {
    activeTab.value = tab;
  };
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex flex-col">
    <GuestNavbar @tabChange="handleTabChange" />

    <main class="flex-grow md:p-8 max-w-7xl mx-auto w-full">
      <div class="mt-2">
        <router-view />
      </div>
    </main>

    <Footer />
  </div>
</template>
