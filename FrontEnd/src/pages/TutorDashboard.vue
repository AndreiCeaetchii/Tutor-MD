<script setup lang="ts">
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';

  import NavigationBar from '../components/navigation/NavigationBar.vue';
  import TutorProfile from '../components/tutor/TutorProfile.vue';
  import TutorBookings from '../components/tutor/TutorBookings.vue';
  import TutorReview from '../components/tutor/TutorReview.vue';
  import TutorCalendar from '../components/tutor/TutorCalendar.vue';
  import TutorSlots from '../components/tutor/TutorSlots.vue';
  import TutorSlotsCards from '../components/tutor/TutorSlotCards.vue';
  import TutorChat from '../components/tutor/TutorChat.vue';
  import Footer from '../components/Footer.vue';

  // Store
  import { useUserStore } from '../store/userStore';

  // Stores and router
  const store = useUserStore();
  const router = useRouter();

  // Refs
  const activeTab = ref('Availability');
  const selectedDate = ref(new Date());

  // Example slot data structure
  const slotData = ref({
    8: [
      { id: '1', startTime: '10:00', endTime: '11:00', status: 'booked', studentName: 'Alex Chen' },
      { id: '2', startTime: '14:00', endTime: '15:00', status: 'available' },
    ],
    15: [
      { id: '3', startTime: '09:00', endTime: '10:00', status: 'available' },
      {
        id: '4',
        startTime: '15:00',
        endTime: '16:30',
        status: 'booked',
        studentName: 'Maria Rodriguez',
      },
    ],
    16: [{ id: '5', startTime: '13:00', endTime: '14:00', status: 'available' }],
    17: [{ id: '6', startTime: '13:00', endTime: '14:00', status: 'available' }],
  });

  onMounted(() => {
    if (!store.isAuthenticated) {
      router.push('/login');
      return;
    }

    if (store.userRole !== 'tutor') {
      if (store.userRole === 'student') {
        router.push('/student-dashboard');
      } else if (store.userRole === 'admin') {
        router.push('/admin-dashboard');
      }
    }
  });

  const handleTabChange = (tabName: string) => {
    activeTab.value = tabName;
  };

  const handleDateSelected = (date: Date) => {
    selectedDate.value = date;
  };
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="p-4 mx-auto md:p-8 max-w-7xl">
      <div class="mb-6">
        <h1 class="mb-2 text-2xl font-bold">Tutor Dashboard</h1>
        <p class="text-gray-600">
          Welcome to your dashboard! Here you can manage your tutoring sessions, view student
          progress, and update your profile.
        </p>
      </div>

      <NavigationBar @tabChange="handleTabChange" />

      <div class="mt-8">
        <TutorProfile v-if="activeTab === 'Profile'" />
        <TutorReview v-else-if="activeTab === 'Reviews'" />
        <div v-else-if="activeTab === 'Availability'" class="content-container">
          <div class="grid grid-cols-1 gap-6 md:grid-cols-2">
            <TutorCalendar @dateSelected="handleDateSelected" />
            <TutorSlots :date="selectedDate" />
          </div>
          <TutorSlotsCards :slot-data="slotData" />
        </div>
        <TutorBookings v-else-if="activeTab === 'Bookings'" />
        <TutorChat v-else-if="activeTab === 'Messages'" />
      </div>
    </div>
  </div>
  <Footer />
</template>

<style scoped>
  .content-container {
    max-width: 1200px;
    width: 100%;
    margin: 0 auto;
  }

  @media (max-width: 640px) {
    .content-container {
      padding: 0 0.5rem;
    }
  }
</style>
