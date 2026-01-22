<script setup lang="ts">
  import { onMounted, ref } from 'vue';
  import GuestNavbar from '../components/navigation/GuestNavbar.vue';
  import { getStudentBookings } from '../services/studentBookings.ts';
  import { useRoute } from 'vue-router';
  import { getTutorReviews } from '../services/reviewService.ts';
  import { useUserStore } from '../store/userStore.ts';

  const router = useRoute();
  const activeTab = ref('find');
  const TutorIdFromParams = Number(router.params.id as string | undefined);
  const BookingIdForReview = ref<number | null>(0);
  const HaveToWriteReview = ref(false);
  const store = useUserStore();
  const userIdFromStore = store.userId;
  const tutorName = ref<string | null>(null);
  const subjectName = ref<string | null>(null);
  const tutorPhoto = ref<string | null>(null);
  const lessonDate = ref<string | null>(null);

  const handleTabChange = (tab: string) => {
    activeTab.value = tab;
  };

  const fetchBookings = async () => {
    try {
      const bookings = await getStudentBookings();
      if (!bookings.length) {
        return;
      }
      for (const booking of bookings) {
        if (booking.tutorUserId === TutorIdFromParams && Number(booking.status) === 3) {
          BookingIdForReview.value = booking.id;
          tutorName.value = booking.tutorName;
          subjectName.value = booking.subjectName;
          tutorPhoto.value = booking.tutorPhoto;
          lessonDate.value = booking.date;
          break;
        }
      }
    } catch (error) {
      console.error('Error fetching bookings:', error);
    }
  };

  const fetchReviews = async () => {
    if (!TutorIdFromParams) {
      console.error('Tutor ID is not available.');
      return;
    }

    try {
      const response = await getTutorReviews(TutorIdFromParams);
      const existingReview = response.reviews.find(
        (review) => review.studentUserId === Number(userIdFromStore),
      );
      HaveToWriteReview.value = BookingIdForReview.value !== 0 && !existingReview;
    } catch (err: any) {
      console.error('Failed to fetch reviews:', err);
    }
  };

  onMounted(async () => {
    await fetchBookings();
    await fetchReviews();
  });
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex flex-col">
    <GuestNavbar @tabChange="handleTabChange" :haveToWriteReview="HaveToWriteReview" />

    <main class="flex-grow md:p-8 max-w-7xl mx-auto w-full">
      <div class="mt-2">
        <router-view v-slot="{ Component }">
          <component
            :is="Component"
            :booking-id-for-review="BookingIdForReview"
            :haveToWriteReview="HaveToWriteReview"
            :tutor-name="tutorName"
            :subject-name="subjectName"
            :tutor-photo="tutorPhoto"
            :lessonDate="lessonDate"
          />
        </router-view>
      </div>
    </main>
  </div>
</template>
