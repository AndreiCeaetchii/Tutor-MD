<script setup lang="ts">
import StatCard from './AdminStatCard.vue';
import RecentActivity from './RecentActivity.vue';
import TopTutors from './TopTutors.vue';
import { getTutors } from "../../services/tutorService.ts";
import { onMounted, ref } from "vue";
import { getAllStudents } from "../../services/adminService.ts";
import { computed } from "vue";
import DefaultImage from '../../assets/DefaultImg.png';

const NumberOfTutors = ref(0);
const NumberOfStudents = ref(0);
const topTutorsData = ref([]);

const fetchTutors = async () => {
  try {
    const response = await getTutors();
    NumberOfTutors.value = response.length;

    const sortedTutors = response.sort((a, b) => b.rating - a.rating);

    topTutorsData.value = sortedTutors.slice(0, 5).map(tutor => ({
      id: tutor.userId,
      firstName: tutor.userProfile.firstName,
      lastName: tutor.userProfile.lastName,
      imageLink: tutor.photo ? tutor.photo.url : DefaultImage,
      rating: tutor.rating,
      totalReviews: tutor.reviewCount,
    }));

  } catch (error) {
    console.error("Error fetching tutors:", error);
  }
};

const fetchStudents = async () => {
  try {
    const response = await getAllStudents();
    NumberOfStudents.value = response.length;
  } catch (error) {
    console.error("Error fetching students:", error);
  }
};

onMounted(() => {
  fetchTutors();
  fetchStudents();
})

const stats = computed(() => [
  {
    title: 'Total Students',
    value: NumberOfStudents.value,
    icon: 'fa-graduation-cap',
    color: 'purple',
    trend: '+12% this month',
  },
  {
    title: 'Total Tutors',
    value: NumberOfTutors.value,
    icon: 'fa-user-tie',
    color: 'green',
    trend: '+8% this month',
  },
  {
    title: 'Total Users',
    value: NumberOfStudents.value + NumberOfTutors.value,
    icon: 'fa-users',
    color: 'blue',
    trend: '+10% this month',
  },
]);
</script>

<template>
  <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-6">
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

  <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
    <RecentActivity />
    <TopTutors :tutors="topTutorsData" />
  </div>
</template>