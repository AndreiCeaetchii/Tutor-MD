<template>
  <div class="relative w-full rounded-t-2xl p-6 lg:p-8 overflow-hidden">
    <div class="absolute inset-0 bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] rounded-t-2xl"></div>

    <div class="relative flex flex-col lg:flex-row items-center justify-between z-10">
      <div class="flex flex-col items-center mb-6 lg:mb-0 lg:mr-8">
        <div
          class="relative w-32 h-32 rounded-full overflow-hidden border-4 border-white shadow-lg"
        >
          <img :src="profileImage" alt="Maria Popescu" class="w-full h-full object-cover" />
        </div>
        <div class="mt-2 text-white text-center">
          <div class="flex items-center justify-center space-x-1 text-yellow-400 text-lg">
            <font-awesome-icon :icon="['fas', 'star']" />
            <span class="font-bold text-white">{{ rating }}</span>
          </div>
          <span class="text-sm text-gray-200">({{ reviews }} reviews)</span>
        </div>
      </div>

      <div
        class="flex-1 flex flex-col items-center justify-center text-center lg:text-left text-white mt-4 lg:mt-0"
      >
        <h1 class="text-3xl sm:text-4xl font-bold">{{ name }}</h1>
        <p class="text-base font-medium text-gray-200 mt-1">{{ title }} • {{ age }} years old</p>

        <div
          class="flex flex-wrap justify-center lg:justify-start items-center space-x-4 mt-4 text-sm text-gray-200"
        >
          <div class="flex items-center space-x-2">
            <font-awesome-icon :icon="['fas', 'user-graduate']" />
            <span>{{ students }}+ students</span>
          </div>
          <div class="flex items-center space-x-2">
            <font-awesome-icon :icon="['fas', 'medal']" />
            <span>{{ experience }} years of experience</span>
          </div>
          <div class="flex items-center space-x-2">
            <font-awesome-icon :icon="['fas', 'map-marker-alt']" />
            <span>{{ location }}</span>
          </div>
        </div>
      </div>

      <div class="mt-6 lg:mt-0 lg:self-start">
        <button
          class="bg-white text-[#5f22d9] font-semibold py-2 px-6 rounded-full shadow hover:bg-gray-100 transition-colors"
        >
          Edit Profile
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { defineProps } from 'vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import {
    faStar,
    faUserGraduate,
    faMedal,
    faMapMarkerAlt,
  } from '@fortawesome/free-solid-svg-icons';

  library.add(faStar, faUserGraduate, faMedal, faMapMarkerAlt);

  const props = defineProps({
    name: {
      type: String,
      default: 'Maria Popescu',
    },
    title: {
      type: String,
      default: 'Professional Math & Physics Tutor',
    },
    age: {
      type: Number,
      default: 35,
    },
    students: {
      type: Number,
      default: 89,
    },
    experience: {
      type: Number,
      default: 8,
    },
    location: {
      type: String,
      default: 'Bucharest, Romania',
    },
    profileImage: {
      type: String,
      default:
        'https://images.unsplash.com/photo-1544005313-94ddf0286df2?q=80&w=1976&auto=format&fit=crop',
    },
    rating: {
      type: Number,
      default: 4.9,
    },
    reviews: {
      type: Number,
      default: 147,
    },
  });
</script>
