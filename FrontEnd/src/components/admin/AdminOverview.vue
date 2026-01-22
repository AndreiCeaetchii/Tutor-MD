<script setup lang="ts">
  import StatCard from './AdminStatCard.vue';
  import RecentActivity from './RecentActivity.vue';
  import TopTutors from './TopTutors.vue';
  import { getTutors } from '../../services/tutorService.ts';
  import { onMounted, ref, computed } from 'vue';

  interface Tutor {
    userId: string;
    userProfile: {
      firstName: string;
      lastName: string;
    };
    photo?: {
      url: string;
    };
    rating: number;
    reviewCount: number;
  }
  import { getPlatformStatistics, type PlatformStats } from '../../services/adminService.ts';
  import DefaultImage from '../../assets/DefaultImg.png';

  const topTutorsData = ref([]);
  const loading = ref(false);
  const platformStats = ref<PlatformStats>({
    totalTutors: 0,
    totalStudents: 0,
    totalUsers: 0,
    verifiedTutors: 0,
    pendingTutors: 0,
    activeTutors: 0,
    activeStudents: 0,
    averageRating: 0,
    totalReviews: 0,
  });

  const fetchTutors = async () => {
    try {
      const response = await getTutors();

      topTutorsData.value = response.map((tutor: Tutor) => ({
        id: tutor.userId,
        firstName: tutor.userProfile.firstName,
        lastName: tutor.userProfile.lastName,
        imageLink: tutor.photo ? tutor.photo.url : DefaultImage,
        rating: tutor.rating,
        totalReviews: tutor.reviewCount,
      }));
    } catch (error) {
      console.error('Error fetching tutors:', error);
    }
  };

  const fetchStatistics = async () => {
    try {
      platformStats.value = await getPlatformStatistics();
    } catch (error) {
      console.error('Error fetching statistics:', error);
    }
  };

  onMounted(async () => {
    loading.value = true;
    try {
      await Promise.all([fetchTutors(), fetchStatistics()]);
    } finally {
      loading.value = false;
    }
  });

  const stats = computed(() => [
    {
      title: 'Total Students',
      value: platformStats.value.totalStudents,
      icon: 'fa-graduation-cap',
      color: 'purple',
      trend: `${platformStats.value.activeStudents} active`,
    },
    {
      title: 'Total Tutors',
      value: platformStats.value.totalTutors,
      icon: 'fa-user-tie',
      color: 'green',
      trend: `${platformStats.value.verifiedTutors} verified, ${platformStats.value.pendingTutors} pending`,
    },
    {
      title: 'Total Users',
      value: platformStats.value.totalUsers,
      icon: 'fa-users',
      color: 'blue',
      trend: `${platformStats.value.activeTutors + platformStats.value.activeStudents} active`,
    },
  ]);
</script>

<template>
  <div v-if="loading" class="flex justify-center py-12">
    <div
      class="w-12 h-12 border-4 border-purple-500 rounded-full border-t-transparent animate-spin"
    ></div>
  </div>

  <div v-else>
    <div class="grid grid-cols-1 gap-6 mb-6 md:grid-cols-2 lg:grid-cols-3">
      <StatCard
        v-for="(stat, index) in stats"
        :key="index"
        :title="stat.title"
        :value="stat.value"
        :icon="stat.icon"
        :color="stat.color"
        :trend="stat.trend"
      />
    </div>

    <div class="grid grid-cols-1 gap-6 lg:grid-cols-2">
      <RecentActivity />
      <TopTutors :tutors="topTutorsData" />
    </div>
  </div>
</template>
