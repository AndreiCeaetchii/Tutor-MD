<script setup lang="ts">
  import { onMounted } from 'vue';
  import TutorCard from './TutorCards.vue';
  import TutorPagination from './TutorPagination.vue';
  import { useTutorStore } from '../../../store/findTutorStore';

  const tutorStore = useTutorStore();

  onMounted(() => {
    tutorStore.fetchTutors();
    console.log('TutorList component mounted, fetching tutors...', tutorStore.paginatedTutors);
  });

  const handleTutorSaveChange = (tutorId: number, isSaved: boolean) => {
    tutorStore.toggleSavedStatus(tutorId, isSaved);
  };
</script>

<template>
  <div>
    <div v-for="tutor in tutorStore.paginatedTutors" :key="tutor.id">
      <TutorCard
        :id="tutor.id"
        :name="tutor.name"
        :location="tutor.location"
        :hourlyRate="tutor.hourlyRate"
        :rating="tutor.rating"
        :reviews="tutor.reviews"
        :profileImage="tutor.profileImage"
        :description="tutor.description"
        :services="tutor.services"
        :saved="tutor.saved"
        @save-toggled="handleTutorSaveChange(tutor.id, $event)"
      />
    </div>
    <div
      class="py-12 text-center text-gray-500"
      v-if="tutorStore.hasActiveFilters && tutorStore.paginatedTutors.length === 0"
    >
      <svg
        class="w-16 h-16 mx-auto mb-4 text-gray-400"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
        />
      </svg>
      <p class="text-xl font-medium">No tutors found matching your criteria.</p>
      <button
        @click="tutorStore.clearFilters()"
        class="px-4 py-2 mt-4 text-white transition bg-purple-700 rounded hover:bg-purple-800"
      >
        Clear all filters
      </button>
    </div>

    <TutorPagination
      v-if="tutorStore.totalPages > 1"
      :currentPage="tutorStore.currentPage"
      :totalPages="tutorStore.totalPages"
      @page-changed="tutorStore.changePage"
    />
  </div>
</template>
