<script setup lang="ts">
import { ref, watch } from 'vue';
import { library } from '@fortawesome/fontawesome-svg-core';
import {
  faHome,
  faMapMarkerAlt,
  faVideo,
  faCheckCircle,
  faStar,
  faBookmark,
  faHeart,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
library.add(faHome, faMapMarkerAlt, faVideo, faCheckCircle, faStar, faBookmark, faHeart);

const props = defineProps({
  name: { type: String, default: 'Mistie Monge' },
  location: { type: String, default: 'Phoenix, MN' },
  hourlyRate: { type: String, default: '712.93' },
  rating: { type: Number, default: 5.0 },
  reviews: { type: Number, default: 4448 },
  profileImage: { type: String, default: 'https://randomuser.me/api/portraits/women/44.jpg' },
  description: {
    type: String,
    default: "Hello there! My name is frederick and I'm currently a pre-medical student. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!"
  },
  services: {
    type: Array as () => string[],
    default: () => ['My home', "Student's home", 'Online']
  },
  saved: { type: Boolean, default: false }
});

const emit = defineEmits(['save-toggled']);

const isSaved = ref(props.saved);

watch(() => props.saved, (newValue) => {
  isSaved.value = newValue;
});

function toggleSave() {
  isSaved.value = !isSaved.value;
  emit('save-toggled', isSaved.value);
}
</script>

<template>
  <div class="p-4 mb-5 bg-white border border-gray-100 rounded-lg shadow">
    <!-- Header -->
    <div class="flex items-center gap-3">
      <img :src="profileImage" alt="Profile" class="object-cover rounded-lg w-14 h-14" />
      <div class="flex-1">
        <div class="flex items-center gap-1">
          <h3 class="text-lg ">{{ name }}</h3>
          <font-awesome-icon :icon="['fas', 'check-circle']" class="w-4 h-4 text-green-500" />
        </div>
        <div class="flex items-center gap-2">
          <span class="flex items-center text-sm ">
            {{ rating }}
            <font-awesome-icon :icon="['fas', 'star']" class="w-3.5 h-3.5 ml-1 text-yellow-400" />
            <span class="ml-1 font-normal text-gray-500">({{ reviews }})</span>
          </span>
          <span class="flex items-center text-sm text-gray-500">
            <font-awesome-icon :icon="['fas', 'map-marker-alt']" class="w-3.5 h-3.5 mr-1" />
            {{ location }}
          </span>
        </div>
      </div>
      <div class="text-right">
        <div class="text-xs text-gray-500">Starting from:</div>
        <div class="text-xl font-bold text-blue-500">${{ hourlyRate }}/hr</div>
      </div>
    </div>

    <!-- Description -->
    <div class="mt-3 text-sm text-gray-700">
      {{ description }}
    </div>

    <!-- Services -->
    <div>
      <div class="mt-3 mb-1.5 text-sm font-bold text-gray-800">
        You can get teaching service direct at
      </div>
      <div class="flex gap-4 ustify-start">
        <div
          v-if="services.includes('My home')"
          class="flex flex-col items-center p-3 transition-colors rounded cursor-pointer w-36 bg-gray-50 hover:bg-gray-200"
        >
          <font-awesome-icon :icon="['fas', 'home']" class="w-5 h-5 mb-1 text-green-500" />
          <span class="text-sm font-medium text-gray-700">My home</span>
        </div>

        <div
          v-if="services.includes('Student\'s home')"
          class="flex flex-col items-center p-3 transition-colors rounded cursor-pointer w-36 bg-gray-50 hover:bg-gray-200"
        >
          <font-awesome-icon :icon="['fas', 'map-marker-alt']" class="w-5 h-5 mb-1 text-blue-500" />
          <span class="text-sm font-medium text-gray-700">Student's home</span>
        </div>

        <div
          v-if="services.includes('Online')"
          class="flex flex-col items-center p-3 transition-colors rounded cursor-pointer w-36 bg-gray-50 hover:bg-gray-200"
        >
          <font-awesome-icon :icon="['fas', 'video']" class="w-5 h-5 mb-1 text-orange-500" />
          <span class="text-sm font-medium text-gray-700">Online</span>
        </div>
      </div>
    </div>

    <!-- Footer -->
    <div class="flex items-center justify-between pt-3 mt-4 border-t border-gray-200">
      <div
        class="flex items-center gap-1.5  cursor-pointer"
        :class="isSaved ? 'text-red-500' : 'text-gray-400'"
        @click="toggleSave"
      >
        <font-awesome-icon :icon="['fas', 'heart']" class="w-4 h-4" />
        <span class="text-sm">{{ isSaved ? 'Saved' : 'Add to save' }}</span>
      </div>
      <div class="flex gap-2">
        <button
          class="px-5 py-1.5 text-sm  text-gray-500 transition bg-white border border-gray-300 rounded-md hover:bg-gray-100"
        >
          Let's chat
        </button>
        <button
          class="px-5 py-1.5 text-sm  text-white transition bg-purple-700 rounded-md hover:bg-purple-800"
        >
          View full profile
        </button>
      </div>
    </div>
  </div>
</template>
