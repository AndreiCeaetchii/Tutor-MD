<template>
  <div class="flex-shrink-0 w-full md:w-1/2 lg:w-1/3">
    <div class="max-w-xs sm:max-w-sm md:max-w-md lg:max-w-sm xl:max-w-sm mx-auto bg-white rounded-lg shadow-lg overflow-hidden ">
    <div class="w-full h-48 bg-gray-200">
      <img :src="props.imageUrl" alt="Building" class="w-full h-full object-cover" />
    </div>

    <div class="p-4 relative">
      <div class="absolute top-0 right-0 p-2 text-red-500">
        <font-awesome-icon :icon="['fas', 'bookmark']" class="w-6 h-6" />
      </div>

      <div class="flex items-center mb-4 mt-2">
        <div class="relative w-16 h-16 rounded-full overflow-hidden border-2 border-white -mt-10">
          <img :src="props.profileImage" alt="Profile" class="w-full h-full object-cover" />
        </div>
        <div class="ml-4">
          <div class="flex items-center">
            <h3 class="text-lg font-semibold">{{ props.name }}</h3>
            <font-awesome-icon :icon="['fas', 'check-circle']" class="w-5 h-5 ml-1 text-green-500" />
          </div>
          <p class="text-gray-500 text-sm">{{ props.location }}</p>
        </div>
      </div>

      <div class="border-t border-gray-200 pt-4">
        <p class="text-gray-500 text-sm">
          Starting from: <span class="font-bold text-lg text-blue-600">${{ props.hourlyRate }}/hr</span>
        </p>

        <p class="mt-4 text-gray-500 text-sm">You can get teaching service direct at</p>
        <div class="mt-2 flex justify-between space-x-2 text-center">
          <div class="flex-1 p-2 border border-gray-200 rounded-lg">
            <font-awesome-icon :icon="['fas', 'home']" class="w-6 h-6 mx-auto text-green-500" />
            <p class="text-xs text-gray-600 mt-1">My home</p>
          </div>
          <div class="flex-1 p-2 border border-gray-200 rounded-lg">
            <font-awesome-icon :icon="['fas', 'map-marker-alt']" class="w-6 h-6 mx-auto text-blue-500" />
            <p class="text-xs text-gray-600 mt-1">Student's home</p>
          </div>
          <div class="flex-1 p-2 border border-gray-200 rounded-lg">
            <font-awesome-icon :icon="['fas', 'video']" class="w-6 h-6 mx-auto text-orange-500" />
            <p class="text-xs text-gray-600 mt-1">Online</p>
          </div>
        </div>
      </div>

      <div class="border-t border-gray-200 mt-4 pt-4 flex items-center justify-center">
        <p class="text-gray-800 font-bold text-sm mr-2">{{ props.rating }}</p>
        <div class="flex text-yellow-400">
          <font-awesome-icon
              v-for="i in 5"
              :key="i"
              :icon="['fas', 'star']"
              :class="['w-4 h-4', i <= Math.floor(props.rating) ? 'text-yellow-400' : 'text-gray-300']"
          />
        </div>
        <p class="text-gray-500 text-sm ml-2">({{ props.reviews }})</p>
      </div>
    </div>
    </div>
  </div>
</template>

<script setup>
import { defineProps } from 'vue';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faHome, faMapMarkerAlt, faVideo, faCheckCircle, faStar, faBookmark } from '@fortawesome/free-solid-svg-icons';
library.add(faHome, faMapMarkerAlt, faVideo, faCheckCircle, faStar, faBookmark);


const props = defineProps({
  name: {
    type: String,
    default: 'Frederick Hicks'
  },
  location: {
    type: String,
    default: 'Fresno, OH'
  },
  hourlyRate: {
    type: String,
    default: '820.41'
  },
  rating: {
    type: Number,
    default: 5.0
  },
  reviews: {
    type: Number,
    default: 4448
  },
  profileImage: {
    type: String,
    default: 'https://images.unsplash.com/photo-1517841905240-472988babdf9?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=687&q=80'
  },
  imageUrl: {
    type: String,
    default: 'https://upload.wikimedia.org/wikipedia/commons/thumb/8/8e/Staples_High_School%2C_Westport%2C_CT.jpg/500px-Staples_High_School%2C_Westport%2C_CT.jpg'
  }
});
</script>