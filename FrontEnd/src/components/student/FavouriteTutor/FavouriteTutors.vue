<script>
import { mapActions, mapState } from 'vuex';

export default {
  name: 'FavouriteTutors',
  computed: {
    ...mapState('favouriteTutor', ['favouriteTutors', 'loading'])
  },
  mounted() {
    this.fetchFavouriteTutors();
  },
  methods: {
    ...mapActions('favouriteTutor', ['fetchFavouriteTutors', 'removeFromFavourites']),
    viewFullProfile(tutorId) {
      this.$router.push(`/tutor/${tutorId}`);
    },
    startChat(tutorId) {
      this.$router.push(`/chat/${tutorId}`);
    }
  }
}
</script>

<template>
  <div class="px-4 py-8 mx-auto max-w-7xl">
    <h1 class="mb-8 text-3xl font-bold text-center text-gray-800">My Favourite Tutors</h1>
    
    <div v-if="loading" class="flex flex-col items-center justify-center py-12">
      <div class="w-10 h-10 mb-4 border-4 border-blue-500 rounded-full border-t-transparent animate-spin"></div>
      <p class="text-gray-600">Loading your favourite tutors...</p>
    </div>
    
    <div v-else-if="favouriteTutors.length === 0" class="py-12 text-center">
      <p class="mb-4 text-gray-600">You don't have any favourite tutors yet.</p>
      <router-link to="/find-tutor" class="inline-block px-6 py-3 font-bold text-white transition-colors bg-blue-500 rounded hover:bg-blue-600">
        Find Tutors
      </router-link>
    </div>
    
    <div v-else class="grid grid-cols-1 gap-8 md:grid-cols-2 lg:grid-cols-3">
      <div v-for="tutor in favouriteTutors" :key="tutor.id" 
          class="p-6 transition-transform border border-gray-200 rounded-lg shadow-md hover:shadow-lg hover:-translate-y-1">
        <div class="relative flex mb-4">
          <div class="flex-shrink-0 w-20 h-20 mr-4 overflow-hidden rounded-full">
            <img :src="tutor.profileImage" :alt="tutor.name" class="object-cover w-full h-full">
          </div>
          <div class="flex-1">
            <h2 class="m-0 text-xl font-bold text-gray-800">{{ tutor.name }}</h2>
            <p class="mt-1 text-sm text-gray-600">{{ tutor.title }}</p>
            <div class="flex items-center mt-1">
              <span v-for="star in 5" :key="star" 
                    :class="{ 'text-yellow-400': star <= Math.round(tutor.rating), 'text-gray-300': star > Math.round(tutor.rating) }"
                    class="text-lg mr-0.5">
                ★
              </span>
              <span class="ml-1 text-sm text-gray-500">({{ tutor.rating }})</span>
            </div>
          </div>
          <button @click="removeFromFavourites(tutor.id)" 
                  class="absolute top-0 right-0 p-1 text-xl text-red-500 hover:text-red-600 focus:outline-none">
            <i class="fas fa-heart"></i>
          </button>
        </div>
        
        <div class="flex flex-wrap gap-2 mb-4">
          <span v-for="(subject, index) in tutor.subjects" :key="index" 
                class="px-3 py-1 text-xs text-gray-800 bg-gray-100 rounded-full">
            {{ subject }}
          </span>
        </div>
        
        <p class="mb-4 text-sm leading-relaxed text-gray-700">{{ tutor.description }}</p>
        
        <div class="flex gap-4 mt-4">
          <button @click="viewFullProfile(tutor.id)"
                  class="flex-1 px-4 py-2 font-bold text-gray-800 transition-colors bg-gray-100 rounded hover:bg-gray-200">
            View Full Profile
          </button>
          <button @click="startChat(tutor.id)"
                  class="flex-1 px-4 py-2 font-bold text-white transition-colors bg-blue-500 rounded hover:bg-blue-600">
            Let's Chat
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
