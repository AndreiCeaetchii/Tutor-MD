<script setup lang="ts">
  import { ref, watch, computed } from 'vue';
  import { useFavouriteTutorStore } from '../../../store/favouriteTutorStore';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import DefaultProfileImage from '../../../assets/DefaultImg.png';
  import { useRouter } from 'vue-router';
  import {
    faHome,
    faMapMarkerAlt,
    faVideo,
    faCheckCircle,
    faStar,
    faBookmark,
    faHeart,
    faBook,
  } from '@fortawesome/free-solid-svg-icons';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  library.add(faHome, faMapMarkerAlt, faVideo, faCheckCircle, faStar, faBookmark, faHeart, faBook);

  const props = defineProps({
    id: { type: Number, required: true },
    name: { type: String, default: 'Mistie Monge' },
    location: { type: String, default: 'Phoenix, MN' },
    hourlyRate: { type: String, default: '712.93' },
    rating: { type: Number, default: 5.0 },
    reviews: { type: Array as () => any[], default: () => [] },
    profileImage: {
      type: String,
      default: () => DefaultProfileImage,
    },
    description: {
      type: String,
      default:
        "Hello there! My name is frederick and I'm currently a pre-medical student. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
    },
    services: {
      type: Array as () => string[],
      default: () => ['My home', "Student's home", 'Online'],
    },
    saved: { type: Boolean, default: false },
    workingLocation: { type: Number, required: true },
    showHourlyRate: { type: Boolean, default: true },
  });

  const emit = defineEmits(['save-toggled']);
  const router = useRouter();
  const isSaved = ref(props.saved);

  const favouriteTutorStore = useFavouriteTutorStore();

  watch(
    () => props.saved,
    (newValue) => {
      isSaved.value = newValue;
    },
  );

  async function toggleSave() {
    try {
      if (isSaved.value) {
        const success = await favouriteTutorStore.removeFromFavourites(props.id);
        if (success) {
          isSaved.value = false;
        }
      } else {
        const success = await favouriteTutorStore.addToFavourites(props.id);
        if (success) {
          isSaved.value = true;
        }
      }
      emit('save-toggled', isSaved.value);
    } catch (error) {
      console.error('Failed to toggle favorite status', error);
    }
  }

  function goToProfile() {
    router.push(`/tutor/${props.id}/profile`);
  }

  const visibleLocations = computed(() => {
    const locationId = props.workingLocation;
    return {
      home: locationId === 1 || locationId === 4 || locationId === 5 || locationId === 7,
      online: locationId === 2 || locationId === 4 || locationId === 6 || locationId === 7,
      studentHome: locationId === 3 || locationId === 5 || locationId === 6 || locationId === 7,
    };
  });

  const totalReviews = computed(() => {
    return props.reviews?.length || 0;
  });

  const actualAverageRating = computed(() => {
    return props.rating ?? 0;
  });

  const formattedRating = computed(() => {
    return actualAverageRating.value.toFixed(1);
  });
</script>

<template>
  <div class="p-4 mb-5 bg-white border border-gray-100 rounded-lg shadow">
    <div class="relative">
      <div v-if="showHourlyRate" class="absolute top-0 right-0 text-right">
        <div class="text-xs text-gray-500">Starting from:</div>
        <div class="text-xl font-bold text-blue-500">{{ hourlyRate }} MDL</div>
      </div>

      <div
        class="flex flex-col gap-3"
        :class="
          showHourlyRate
            ? 'pr-32 sm:flex-row sm:items-center sm:pr-0'
            : 'sm:flex-row sm:items-center'
        "
      >
        <img
          :src="profileImage && profileImage.trim() !== '' ? profileImage : DefaultProfileImage"
          alt="Profile"
          class="object-cover rounded-lg w-14 h-14"
        />
        <div class="flex-1">
          <div class="flex items-center gap-1">
            <h3 class="text-lg font-semibold">{{ name }}</h3>
            <font-awesome-icon :icon="['fas', 'check-circle']" class="w-4 h-4 text-green-500" />
          </div>
          <div class="flex flex-wrap items-center gap-2">
            <span class="flex items-center text-sm font-semibold">
              {{ formattedRating }}
              <font-awesome-icon :icon="['fas', 'star']" class="w-3.5 h-3.5 ml-1 text-yellow-400" />
              <span class="ml-1 font-normal text-gray-500">({{ totalReviews }})</span>
            </span>
            <span class="flex items-center text-sm text-gray-500">
              <font-awesome-icon :icon="['fas', 'map-marker-alt']" class="w-3.5 h-3.5 mr-1" />
              {{ location }}
            </span>
          </div>
          <div
            v-if="services && services.length > 0"
            class="flex flex-wrap items-center gap-1.5 mt-2"
          >
            <font-awesome-icon :icon="['fas', 'book']" class="w-3.5 h-3.5 text-purple-600" />
            <span
              v-for="(subject, index) in services.slice(0, 4)"
              :key="index"
              class="px-2 py-0.5 text-xs font-medium text-purple-700 bg-purple-100 rounded-full"
            >
              {{ subject }}
            </span>
            <span
              v-if="services.length > 4"
              class="px-2 py-0.5 text-xs font-medium text-gray-600 bg-gray-100 rounded-full"
            >
              +{{ services.length - 4 }} more
            </span>
          </div>
        </div>

        <div class="hidden sm:block" v-if="showHourlyRate">
          <div class="invisible text-xs text-gray-500">Starting from:</div>
          <div class="invisible text-xl font-bold text-blue-500">{{ hourlyRate }} MDL</div>
        </div>
      </div>
    </div>

    <div class="mt-3 text-sm text-gray-700">
      {{ description }}
    </div>

    <div>
      <div class="mt-3 mb-1.5 text-sm font-bold text-gray-800">
        You can get teaching service direct at
      </div>
      <div class="flex justify-center gap-1 sm:justify-start sm:gap-4">
        <div
          v-if="visibleLocations.home"
          class="flex flex-col items-center p-1 sm:p-2 transition-colors rounded cursor-pointer w-[30%] sm:w-28 bg-gray-50 hover:bg-gray-200"
        >
          <font-awesome-icon
            :icon="['fas', 'home']"
            class="w-5 h-5 mb-0.5 sm:mb-1 text-green-500"
          />
          <span class="text-xs font-medium text-center text-gray-700 sm:text-sm">My home</span>
        </div>

        <div
          v-if="visibleLocations.studentHome"
          class="flex flex-col items-center p-1 sm:p-2 transition-colors rounded cursor-pointer w-[40%] sm:w-36 bg-gray-50 hover:bg-gray-200"
        >
          <font-awesome-icon
            :icon="['fas', 'map-marker-alt']"
            class="w-5 h-5 mb-0.5 sm:mb-1 text-blue-500"
          />
          <span
            class="text-xs font-medium text-center text-gray-700 sm:text-sm sm:whitespace-nowrap"
            >Student's home</span
          >
        </div>

        <div
          v-if="visibleLocations.online"
          class="flex flex-col items-center p-1 sm:p-2 transition-colors rounded cursor-pointer w-[30%] sm:w-28 bg-gray-50 hover:bg-gray-200"
        >
          <font-awesome-icon
            :icon="['fas', 'video']"
            class="w-5 h-5 mb-0.5 sm:mb-1 text-orange-500"
          />
          <span class="text-xs font-medium text-center text-gray-700 sm:text-sm">Online</span>
        </div>
      </div>
    </div>

    <div
      class="flex flex-col pt-3 mt-4 border-t border-gray-200 sm:flex-row sm:items-center sm:justify-between"
    >
      <div
        class="flex items-center gap-1.5 cursor-pointer mb-3 sm:mb-0"
        :class="isSaved ? 'text-red-500' : 'text-gray-400'"
        @click="toggleSave"
      >
        <font-awesome-icon :icon="['fas', 'heart']" class="w-4 h-4" />
        <span class="text-sm">{{ isSaved ? 'Favourite Tutor' : 'Add to favourite' }}</span>
      </div>
      <div class="flex gap-2">
        <button
          class="w-auto px-3 py-1.5 text-xs text-gray-500 transition bg-white border border-gray-300 rounded-md sm:px-5 sm:text-sm hover:bg-gray-100"
        >
          Let's chat
        </button>
        <button
          class="w-auto px-3 py-1.5 text-xs text-white transition bg-purple-700 rounded-md sm:px-5 sm:text-sm hover:bg-purple-800"
          @click="goToProfile"
        >
          View full profile
        </button>
      </div>
    </div>
  </div>
</template>
