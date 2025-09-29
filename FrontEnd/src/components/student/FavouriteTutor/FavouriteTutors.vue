<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useFavouriteTutorStore } from '../../../store/favouriteTutorStore';
import TutorCards from '../FindTutor/TutorCards.vue';

const favouriteTutorStore = useFavouriteTutorStore();
const loading = ref(true);
const errorMessage = ref('');

onMounted(async () => {
  try {
    loading.value = true;
    await favouriteTutorStore.fetchFavouriteTutors();
  } catch (error: any) {
    errorMessage.value = error.message || 'An error occurred while fetching favourite tutors.';
  } finally {
    loading.value = false;
  }
});

const handleSaveToggled = async (isSaved: boolean, tutorId: number) => {
  if (!isSaved) {
    await favouriteTutorStore.removeFromFavourites(tutorId);
  }
};
</script>

<template>
  <div class="favourite-tutors">
    <div class="container px-4 py-6 mx-auto">
      <h1 class="mb-6 text-2xl font-bold">My Favourite Tutors</h1>
      
      <div v-if="loading" class="flex justify-center my-8">
        <div class="w-12 h-12 border-t-2 border-b-2 border-purple-500 rounded-full animate-spin"></div>
      </div>
      
      <div v-else-if="errorMessage" class="px-4 py-3 mb-4 text-red-700 bg-red-100 border border-red-400 rounded">
        {{ errorMessage }}
      </div>
      
      <div v-else-if="favouriteTutorStore.favouriteTutors.length === 0" class="py-12 text-center">
        <div class="mb-4 text-gray-500">
          <svg class="w-16 h-16 mx-auto text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"></path>
          </svg>
        </div>
        <h2 class="mb-2 text-xl font-medium">No favourite tutors yet</h2>
        <p class="text-gray-500">
          You haven't added any tutors to your favourites list yet.
          <br>
          Browse tutors and click the heart icon to add them here.
        </p>
      </div>
      
      <div v-else class="grid grid-cols-1 gap-4 md:grid-cols-2">
        <div v-for="tutor in favouriteTutorStore.favouriteTutors" :key="tutor.id">
          <TutorCards 
            :id="tutor.id"
            :name="tutor.name"
            :location="tutor.location"
            :hourlyRate="tutor.hourlyRate"
            :rating="tutor.rating"
            :reviews="tutor.reviews"
            :profileImage="tutor.profileImage"
            :description="tutor.description"
            :services="tutor.services"
            :saved="true"
            :workingLocation="tutor.workingLocation"
            :showHourlyRate="false"
            @save-toggled="(isSaved) => handleSaveToggled(isSaved, tutor.id)"
          />
        </div>
      </div>
    </div>
  </div>
</template>
