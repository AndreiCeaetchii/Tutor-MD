<template>
  <div class="relative w-full rounded-t-2xl p-6 lg:p-8 overflow-hidden">
    <div class="absolute inset-0 bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] rounded-t-2xl"></div>

    <div class="relative flex flex-col lg:flex-row items-center justify-between z-10">
      <div class="flex flex-col items-center mb-6 lg:mb-0 lg:mr-8">
        <div
          class="relative w-32 h-32 rounded-full overflow-hidden border-4 border-white shadow-lg"
        >
          <img
            :src="profileStore.profileImage"
            :alt="profileStore.getName"
            class="w-full h-full object-cover"
          />
        </div>
        <div class="mt-2 text-white text-center">
          <div class="flex items-center justify-center space-x-1 text-yellow-400 text-lg">
            <font-awesome-icon :icon="['fas', 'star']" />
            <span class="font-bold text-white">{{ profileStore.rating }}</span>
          </div>
          <span class="text-sm text-gray-200">({{ profileStore.reviews }} reviews)</span>
        </div>
      </div>

      <div class="flex-1 flex flex-col items-center justify-center text-white mt-4 lg:mt-0">
        <div class="text-center lg:text-left">
          <h1 class="text-3xl sm:text-4xl font-bold">{{ profileStore.getName }}</h1>
          <p class="text-base font-medium text-gray-200 mt-1">
            {{ getSubjectNames }} Tutor • {{ profileStore.age }} years old
          </p>

          <div
            class="flex flex-wrap justify-center lg:justify-start items-center space-x-4 mt-4 text-sm text-gray-200"
          >
            <div class="flex items-center space-x-2">
              <font-awesome-icon :icon="['fas', 'chalkboard-teacher']" />
              <span>Online & In-person</span>
            </div>

            <div class="flex items-center space-x-2">
              <font-awesome-icon :icon="['fas', 'medal']" />
              <span>{{ profileStore.experience }} years of experience</span>
            </div>
            <div class="flex items-center space-x-2">
              <font-awesome-icon :icon="['fas', 'map-marker-alt']" />
              <span>{{ profileStore.getFullLocation }}</span>
            </div>
          </div>
        </div>
      </div>

      <div class="mt-6 lg:mt-0 lg:self-start">
        <button
          @click="profileStore.toggleEditing"
          class="bg-white text-[#5f22d9] font-semibold py-2 px-6 rounded-full shadow hover:bg-gray-100 transition-colors"
        >
          Edit Profile
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { useProfileStore } from '../../store/profileStore.ts';
  import { computed } from 'vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import {
    faStar,
    faUserGraduate,
    faMedal,
    faMapMarkerAlt,
    faChalkboardTeacher,
  } from '@fortawesome/free-solid-svg-icons';

  library.add(faStar, faUserGraduate, faMedal, faMapMarkerAlt, faChalkboardTeacher);

  const profileStore = useProfileStore();

  const getSubjectNames = computed(() => {
    return profileStore.subjects.map((subject) => subject.name).join(' & ');
  });
</script>
