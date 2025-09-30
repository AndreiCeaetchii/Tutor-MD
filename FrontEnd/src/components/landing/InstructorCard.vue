<script setup>
import { library } from '@fortawesome/fontawesome-svg-core';
import { faHome, faMapMarkerAlt, faVideo, faCheckCircle, faStar, faBookmark } from '@fortawesome/free-solid-svg-icons';
library.add(faHome, faMapMarkerAlt, faVideo, faCheckCircle, faStar, faBookmark);

const instructorProfiles = [
  {
    name: 'Alexandru Popescu',
    location: 'Bucharest, Romania',
    hourlyRate: '45.00',
    rating: 4.9,
    reviews: 237,
    profileImage: 'https://images.unsplash.com/photo-1564564321837-a57b7070ac4f?ixlib=rb-1.2.1&auto=format&fit=crop&w=687&q=80',
    imageUrl: 'https://images.unsplash.com/photo-1589391886645-d51941baf7fb?ixlib=rb-1.2.1&auto=format&fit=crop&w=1170&q=80'
  },
  {
    name: 'Petru Ionescu',
    location: 'Cluj-Napoca, Romania',
    hourlyRate: '43.50',
    rating: 4.8,
    reviews: 189,
    profileImage: 'https://images.unsplash.com/photo-1534030347209-467a5b0ad3e6?ixlib=rb-1.2.1&auto=format&fit=crop&w=687&q=80',
    imageUrl: 'https://images.unsplash.com/photo-1546410531-bb4caa6b424d?ixlib=rb-1.2.1&auto=format&fit=crop&w=1171&q=80'
  },
  {
    name: 'Vasile Rusu',
    location: 'Balti, Moldova',
    hourlyRate: '36.00',
    rating: 4.8,
    reviews: 98,
    profileImage: 'https://images.unsplash.com/photo-1539571696357-5a69c17a67c6?ixlib=rb-1.2.1&auto=format&fit=crop&w=687&q=80',
    imageUrl: 'https://images.unsplash.com/photo-1541339907198-e08756dedf3f?ixlib=rb-1.2.1&auto=format&fit=crop&w=1170&q=80'
  },
  {
    name: 'Bogdan Stancu',
    location: 'Cluj-Napoca, Romania',
    hourlyRate: '42.00',
    rating: 4.7,
    reviews: 156,
    profileImage: 'https://images.unsplash.com/photo-1599566150163-29194dcaad36?ixlib=rb-1.2.1&auto=format&fit=crop&w=687&q=80',
    imageUrl: 'https://images.unsplash.com/photo-1504384308090-c894fdcc538d?ixlib=rb-1.2.1&auto=format&fit=crop&w=1170&q=80'
  },
  {
    name: 'Diana Enache',
    location: 'Timisoara, Romania',
    hourlyRate: '40.00',
    rating: 4.6,
    reviews: 129,
    profileImage: 'https://images.unsplash.com/photo-1580489944761-15a19d654956?ixlib=rb-1.2.1&auto=format&fit=crop&w=687&q=80',
    imageUrl: 'https://images.unsplash.com/photo-1577896851231-70ef18881754?ixlib=rb-1.2.1&auto=format&fit=crop&w=1170&q=80'
  },
];

const props = defineProps({
  instructorIndex: {
    type: Number,
    default: 0
  }
});

const instructorName = instructorProfiles[props.instructorIndex].name;
const instructorLocation = instructorProfiles[props.instructorIndex].location;
const instructorHourlyRate = instructorProfiles[props.instructorIndex].hourlyRate;
const instructorRating = instructorProfiles[props.instructorIndex].rating;
const instructorReviews = instructorProfiles[props.instructorIndex].reviews;
const instructorProfileImage = instructorProfiles[props.instructorIndex].profileImage;
const instructorImageUrl = instructorProfiles[props.instructorIndex].imageUrl;
</script>

<template>
  <div class="flex-shrink-0 w-full md:w-1/2 lg:w-1/3">
    <div class="max-w-xs mx-auto overflow-hidden bg-white rounded-lg shadow-lg sm:max-w-sm md:max-w-md lg:max-w-sm xl:max-w-sm">
      <div class="w-full h-48 bg-gray-200">
        <img :src="instructorImageUrl" alt="Building" class="object-cover w-full h-full" />
      </div>

      <div class="relative p-4">
        <div class="absolute top-0 right-0 p-2 text-red-500">
          <font-awesome-icon :icon="['fas', 'bookmark']" class="w-6 h-6" />
        </div>

        <div class="flex items-center mt-2 mb-4">
          <div class="relative w-16 h-16 -mt-10 overflow-hidden border-2 border-white rounded-full">
            <img :src="instructorProfileImage" alt="Profile" class="object-cover w-full h-full" />
          </div>
          <div class="ml-4">
            <div class="flex items-center">
              <h3 class="text-lg font-semibold">{{ instructorName }}</h3>
              <font-awesome-icon :icon="['fas', 'check-circle']" class="w-5 h-5 ml-1 text-green-500" />
            </div>
            <p class="text-sm text-gray-500">{{ instructorLocation }}</p>
          </div>
        </div>

        <div class="pt-4 border-t border-gray-200">
          <p class="text-sm text-gray-500">
            Starting from: <span class="text-lg font-bold text-blue-600">${{ instructorHourlyRate }}/hr</span>
          </p>

          <p class="mt-4 text-sm text-gray-500">You can get teaching service direct at</p>
          <div class="flex justify-between mt-2 space-x-2 text-center">
            <div class="flex-1 p-2 border border-gray-200 rounded-lg">
              <font-awesome-icon :icon="['fas', 'home']" class="w-6 h-6 mx-auto text-green-500" />
              <p class="mt-1 text-xs text-gray-600">My home</p>
            </div>
            <div class="flex-1 p-2 border border-gray-200 rounded-lg">
              <font-awesome-icon :icon="['fas', 'map-marker-alt']" class="w-6 h-6 mx-auto text-blue-500" />
              <p class="mt-1 text-xs text-gray-600">Student's home</p>
            </div>
            <div class="flex-1 p-2 border border-gray-200 rounded-lg">
              <font-awesome-icon :icon="['fas', 'video']" class="w-6 h-6 mx-auto text-orange-500" />
              <p class="mt-1 text-xs text-gray-600">Online</p>
            </div>
          </div>
        </div>

        <div class="flex items-center justify-center pt-4 mt-4 border-t border-gray-200">
          <p class="mr-2 text-sm font-bold text-gray-800">{{ instructorRating }}</p>
          <div class="flex text-yellow-400">
            <font-awesome-icon
                v-for="i in 5"
                :key="i"
                :icon="['fas', 'star']"
                :class="['w-4 h-4', i <= Math.floor(instructorRating) ? 'text-yellow-400' : 'text-gray-300']"
            />
          </div>
          <p class="ml-2 text-sm text-gray-500">({{ instructorReviews }})</p>
        </div>
      </div>
    </div>
  </div>
</template>
