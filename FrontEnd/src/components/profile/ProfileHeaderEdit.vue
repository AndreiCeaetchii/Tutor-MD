<template>
  <div class="relative w-full rounded-t-2xl p-6 lg:p-8 overflow-hidden">
    <div class="absolute inset-0 bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] rounded-t-2xl"></div>

    <form @submit.prevent="$emit('save-profile', editedProfile)" class="relative z-10">
      <div class="relative flex flex-col lg:flex-row items-center justify-between z-10">
        <div class="relative flex flex-col items-center mb-6 lg:mb-0 lg:mr-8">
          <ProfileImageUploader v-model="editedProfile.profileImage" />
          <button
            @click.prevent=""
            class="absolute bottom-[4rem] right-2 flex justify-center items-center rounded-full w-8 h-8 shadow-lg cursor-pointer text-purple-600 bg-white"
          >
            <font-awesome-icon :icon="['fas', 'pencil-alt']" />
          </button>
          <div class="mt-2 text-white text-center">
            <div class="flex items-center justify-center space-x-1 text-yellow-400 text-lg">
              <font-awesome-icon :icon="['fas', 'star']" />
              <span class="font-bold text-white">{{ profileStore.rating }}</span>
            </div>
            <span class="text-sm text-gray-200">({{ profileStore.reviews }} reviews)</span>
          </div>
        </div>

        <div
          class="flex-1 flex flex-col items-center lg:items-start justify-center text-white mt-4 lg:mt-0"
        >
          <div class="text-center lg:text-left w-full">
            <div class="flex flex-col sm:flex-row space-x-0 sm:space-x-4 mb-4 lg:mb-0">
              <div class="flex-1">
                <span class="text-xs text-gray-300">First Name</span>
                <input
                  v-model="editedProfile.firstName"
                  type="text"
                  class="w-full text-lg sm:text-xl font-medium bg-gradient-to-r from-[#5f5ce1] to-[#4a3de1] text-white rounded-lg px-4 py-2 border border-white/70 focus:outline-none focus:ring-1 focus:ring-white"
                />
              </div>
              <div class="flex-1 mt-2 sm:mt-0">
                <span class="text-xs text-gray-300">Last Name</span>
                <input
                  v-model="editedProfile.lastName"
                  type="text"
                  class="w-full text-lg sm:text-xl font-medium bg-gradient-to-r from-[#5f5ce1] to-[#4a3de1] text-white rounded-lg px-4 py-2 border border-white/70 focus:outline-none focus:ring-1 focus:ring-white"
                />
              </div>
            </div>

            <div class="mt-4 text-sm text-gray-200 grid grid-cols-1 lg:grid-cols-2 gap-4">
              <div class="flex items-center space-x-2">
                <div class="flex-1">
                  <span class="text-xs text-gray-300">Years of experience</span>
                  <input
                    v-model.number="editedProfile.experience"
                    type="number"
                    min="0"
                    class="w-full text-base font-bold text-left bg-gradient-to-r from-[#5f5ce1] to-[#4a3de1] text-white rounded-lg px-4 py-2 border border-white/70 focus:outline-none focus:ring-1 focus:ring-white"
                  />
                </div>
              </div>

              <div class="flex items-center space-x-2">
                <div class="flex-1">
                  <span class="text-xs text-gray-300">Location</span>
                  <input
                    v-model="editedProfile.location"
                    type="text"
                    class="w-full bg-gradient-to-r from-[#5f5ce1] to-[#4a3de1] text-white rounded-lg px-4 py-2 border border-white/70 focus:outline-none focus:ring-1 focus:ring-white"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="mt-6 lg:mt-0 lg:self-start">
          <button
            type="submit"
            class="bg-white text-[#5f22d9] font-semibold py-2 px-6 rounded-full shadow hover:bg-gray-100 transition-colors"
          >
            Save Changes
          </button>
        </div>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
  import { defineProps, defineEmits } from 'vue';
  import { useProfileStore } from '../../store/profileStore.ts';
  import { faStar, faTrophy, faMapMarkerAlt, faPencilAlt } from '@fortawesome/free-solid-svg-icons';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import ProfileImageUploader from './ProfileImageUploader.vue';

  library.add(faStar, faTrophy, faMapMarkerAlt, faPencilAlt);

  const profileStore = useProfileStore();

  const props = defineProps({
    editedProfile: {
      type: Object,
      required: true,
    },
  });

  const emits = defineEmits(['save-profile']);
</script>
