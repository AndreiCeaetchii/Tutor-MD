<template>
  <div class="relative w-full p-6 overflow-hidden rounded-t-2xl lg:p-8">
    <div class="absolute inset-0 bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] rounded-t-2xl"></div>

    <form @submit.prevent="$emit('save-profile', editedProfile)" class="relative z-10">
      <div class="relative z-10 flex flex-col items-center justify-between lg:flex-row">
        <div class="relative flex flex-col items-center mb-6 lg:mb-0 lg:mr-8">
          <ProfileImageUploader v-model="editedProfile.profileImage" />
          <button
            @click.prevent=""
            class="absolute bottom-[4rem] right-2 flex justify-center items-center rounded-full w-8 h-8 shadow-lg cursor-pointer text-purple-600 bg-white"
          >
            <font-awesome-icon :icon="['fas', 'pencil-alt']" />
          </button>
          <div class="mt-2 text-center text-white">
            <div class="flex items-center justify-center space-x-1 text-lg text-yellow-400">
              <font-awesome-icon :icon="['fas', 'star']" />
              <span class="font-bold text-white">{{ actualAverageRating.toFixed(1) }}</span>
            </div>
            <span class="text-sm text-gray-200">({{ totalReviews }} reviews)</span>
          </div>
        </div>

        <div
          class="flex flex-col items-center justify-center flex-1 mt-4 text-white lg:items-start lg:mt-0"
        >
          <div class="w-full text-center lg:text-left">
            <div class="flex flex-col mb-4 space-x-0 sm:flex-row sm:space-x-4 lg:mb-0">
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

            <div class="grid grid-cols-1 gap-4 mt-4 text-sm text-gray-200 lg:grid-cols-3">
              <div class="flex flex-col">
                <span class="text-xs text-gray-300">Years of experience</span>
                <input
                  v-model.number="editedProfile.experience"
                  type="number"
                  min="0"
                  class="w-full text-base font-bold text-left bg-gradient-to-r from-[#5f5ce1] to-[#4a3de1] text-white rounded-lg px-4 py-2 border border-white/70 focus:outline-none focus:ring-1 focus:ring-white"
                />
              </div>

              <div class="flex flex-col">
                <span class="text-xs text-gray-300">City</span>
                <input
                  v-model="editedProfile.city"
                  type="text"
                  class="text-base w-full bg-gradient-to-r from-[#5f5ce1] to-[#4a3de1] text-white rounded-lg px-4 py-2 border border-white/70 focus:outline-none focus:ring-1 focus:ring-white"
                />
              </div>

              <div class="flex flex-col">
                <span class="text-xs text-gray-300">Country</span>
                <input
                  v-model="editedProfile.country"
                  type="text"
                  class="text-base w-full bg-gradient-to-r from-[#5f5ce1] to-[#4a3de1] text-white rounded-lg px-4 py-2 border border-white/70 focus:outline-none focus:ring-1 focus:ring-white"
                />
              </div>
            </div>
          </div>
        </div>

        <div class="flex flex-col mt-6 space-y-4 lg:mt-0 lg:ml-4 lg:self-start">
          <button
            type="submit"
            class="bg-white text-[#5f22d9] font-semibold py-2 px-6 rounded-full shadow hover:bg-gray-100 transition-colors"
          >
            Save Changes
          </button>
          <button
            type="button"
            @click="$emit('cancel-edit')"
            class="border border-white text-white font-semibold py-2 px-6 rounded-full shadow hover:bg-[#4a3de1] transition-colors"
          >
            Cancel
          </button>
        </div>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
  import { defineProps, defineEmits } from 'vue';
  import { faStar, faTrophy, faMapMarkerAlt, faPencilAlt } from '@fortawesome/free-solid-svg-icons';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import ProfileImageUploader from './ProfileImageUploader.vue';
  import { computed } from 'vue';

  library.add(faStar, faTrophy, faMapMarkerAlt, faPencilAlt);

  interface ReviewItem {
    id: number;
    rating: number;
  }

  const props = defineProps<{
    editedProfile: {
      firstName?: string;
      lastName?: string;
      experience?: number;
      city?: string;
      country?: string;
      profileImage?: any;
    };
    reviews: ReviewItem[];
  }>();

  const totalReviews = computed(() => {
    return props.reviews?.length || 0;
  });

  const actualAverageRating = computed(() => {
    if (totalReviews.value === 0) return 0;
    const sum = props.reviews.reduce((acc, r) => acc + r.rating, 0);
    return parseFloat((sum / totalReviews.value).toFixed(1));
  });

  const emits = defineEmits(['save-profile', 'cancel-edit']);
</script>
