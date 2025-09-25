<script setup lang="ts">
  import { onMounted } from 'vue';
  import TutorCard from './TutorCards.vue';
  import TutorPagination from './TutorPagination.vue';
  import { useFindTutorStore } from '../../../store/findTutorStore';

  const tutorStore = useFindTutorStore();

  onMounted(() => {
    tutorStore.fetchTutors();
  });

  const handleTutorSaveChange = (tutorId: number, isSaved: boolean) => {
    tutorStore.toggleSavedStatus(tutorId, isSaved);
  };
</script>

<template>
  <div class="min-h-[500px]">
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
        :workingLocation="tutor.workingLocation"
        @save-toggled="handleTutorSaveChange(tutor.id, $event)"
      />
    </div>
    
    <!-- Empty state when filters are applied but no matches -->
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
    
    <!-- NEW: Empty state when no tutors exist at all -->
    <div
      class="flex flex-col items-center justify-center py-16 text-center rounded-lg bg-gray-50"
      v-if="!tutorStore.hasActiveFilters && tutorStore.paginatedTutors.length === 0"
    >
      <svg 
        class="w-24 h-24 mb-6 text-gray-300" 
        fill="none" 
        stroke="currentColor" 
        viewBox="0 0 24 24" 
        xmlns="http://www.w3.org/2000/svg"
      >
        <path 
          stroke-linecap="round" 
          stroke-linejoin="round" 
          stroke-width="1.5" 
          d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"
        ></path>
      </svg>
      <h2 class="mb-2 text-2xl font-semibold text-gray-700">No tutors available yet</h2>
      <p class="max-w-md mb-6 text-gray-500">
        We're working on expanding our tutor community. Check back soon to discover expert tutors in your desired field.
      </p>
    </div>

    <TutorPagination
      v-if="tutorStore.totalPages > 1"
      :currentPage="tutorStore.currentPage"
      :totalPages="tutorStore.totalPages"
      @page-changed="tutorStore.changePage"
    />
  </div>
</template>
