<script setup lang="ts">
  import { defineProps, defineEmits, ref, onMounted, onUnmounted } from 'vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import {
    faEye,
    faEdit,
    faTrash,
    faGraduationCap,
    faChalkboardTeacher,
    faCheck,
    faTimes,
  } from '@fortawesome/free-solid-svg-icons';
  import DefaultImg from '../../assets/DefaultImg.png';

  library.add(faEye, faEdit, faTrash, faGraduationCap, faChalkboardTeacher, faCheck, faTimes);

  interface User {
    id: number;
    name: string;
    email: string;
    type: 'Student' | 'Tutor';
    status: 'Active' | 'Pending' | 'Suspended';
    joinDate: string;
    bookings: number;
  }

  const props = defineProps<{
    users: User[];
  }>();

  const emit = defineEmits(['view', 'edit', 'delete', 'activate', 'deactivate']);

  const isMobile = ref(window.innerWidth < 768);

  const handleResize = () => {
    isMobile.value = window.innerWidth < 768;
  };

  onMounted(() => {
    window.addEventListener('resize', handleResize);
  });

  onUnmounted(() => {
    window.removeEventListener('resize', handleResize);
  });

  const handleActivationToggle = (user: User) => {
    if (user.status === 'Active') {
      emit('deactivate', user);
      console.log(`Profile for ${user.name} has been deactivated.`);
    } else {
      emit('activate', user);
      console.log(`Profile for ${user.name} has been activated.`);
    }
  };

  const getStatusClasses = (status: User['status']) => {
    switch (status) {
      case 'Active':
        return 'bg-green-100 text-green-800';
      case 'Pending':
        return 'bg-yellow-100 text-yellow-800';
      case 'Suspended':
        return 'bg-red-100 text-red-800';
      default:
        return '';
    }
  };
</script>

<template>
  <div v-if="!isMobile" class="overflow-x-auto">
    <table class="min-w-full bg-white rounded-lg shadow-md">
      <thead>
        <tr class="w-full text-sm leading-normal text-left text-gray-500 uppercase bg-gray-50">
          <th class="px-6 py-3">User</th>
          <th class="px-6 py-3">Type</th>
          <th class="px-6 py-3">Status</th>
          <th class="px-6 py-3">Join Date</th>
          <th class="px-6 py-3">Reviews</th>
          <th class="px-6 py-3 text-center">Actions</th>
        </tr>
      </thead>
      <tbody class="text-sm font-light text-gray-600">
        <tr v-for="user in users" :key="user.id" class="border-b border-gray-200 hover:bg-gray-100">
          <td class="px-6 py-3 text-left whitespace-nowrap">
            <div class="flex items-center">
              <div class="mr-3">
                <img :src="DefaultImg" alt="Profile" class="w-10 h-10 rounded-full" />
              </div>
              <div>
                <p class="font-medium">{{ user.name }}</p>
                <p class="text-xs text-gray-500">{{ user.email }}</p>
              </div>
            </div>
          </td>
          <td class="px-6 py-3 text-left">
            <span class="flex items-center">
              <span class="mr-2">
                <font-awesome-icon
                  :icon="
                    user.type === 'Student'
                      ? 'fa-solid fa-graduation-cap'
                      : 'fa-solid fa-chalkboard-teacher'
                  "
                  class="text-purple-500"
                />
              </span>
              {{ user.type }}
            </span>
          </td>
          <td class="px-6 py-3 text-left">
            <span
              :class="[
                'py-1 px-3 rounded-full text-xs font-semibold',
                getStatusClasses(user.status),
              ]"
            >
              {{ user.status }}
            </span>
          </td>
          <td class="px-6 py-3 text-left">{{ user.joinDate }}</td>
          <td class="px-6 py-3 text-left">{{ user.bookings }}</td>
          <td class="px-6 py-3 text-center">
            <div class="flex justify-center space-x-2 item-center">
              <button
                @click="emit('view', user)"
                class="w-6 h-6 transform hover:text-purple-500 hover:scale-110"
              >
                <font-awesome-icon icon="fa-solid fa-eye" />
              </button>
              <div class="relative group">
                <button
                  @click="handleActivationToggle(user)"
                  :class="[
                    'w-6 h-6 transform hover:scale-110',
                    user.status === 'Active' ? 'text-red-500' : 'text-green-500',
                  ]"
                >
                  <font-awesome-icon
                    :icon="user.status === 'Active' ? 'fa-solid fa-times' : 'fa-solid fa-check'"
                  />
                </button>
                <span
                  class="absolute px-2 py-1 mb-2 text-xs text-white transition-transform duration-200 transform scale-0 -translate-x-1/2 bg-black rounded pointer-events-none bottom-full left-1/2 group-hover:scale-100 whitespace-nowrap"
                >
                  {{ user.status === 'Active' ? 'Restrict access' : 'Accept profile' }}
                </span>
              </div>
              <button
                @click="emit('delete', user)"
                class="w-6 h-6 transform hover:text-red-500 hover:scale-110"
              >
                <font-awesome-icon icon="fa-solid fa-trash" />
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>

  <div v-else class="space-y-4">
    <div
      v-for="user in users"
      :key="user.id"
      class="flex flex-col items-start justify-between p-4 bg-white rounded-lg shadow-md sm:flex-row sm:items-center"
    >
      <div class="flex items-center mb-4 sm:mb-0">
        <img :src="DefaultImg" alt="Profile" class="w-10 h-10 mr-4 rounded-full" />
        <div>
          <h3 class="text-lg font-bold">{{ user.name }}</h3>
          <p class="text-sm text-gray-500">{{ user.email }}</p>
        </div>
      </div>
      <div class="flex flex-col sm:flex-row sm:items-center sm:space-x-4">
        <div class="flex flex-col mb-2 text-sm text-gray-700 sm:mb-0">
          <p><span class="font-semibold">Type:</span> {{ user.type }}</p>
          <p><span class="font-semibold">Join Date:</span> {{ user.joinDate }}</p>
          <p><span class="font-semibold">Bookings:</span> {{ user.bookings }}</p>
        </div>
        <div class="flex flex-col items-start space-y-2 sm:items-center sm:space-y-0 sm:space-x-2">
          <span
            :class="['py-1 px-3 rounded-full text-xs font-semibold', getStatusClasses(user.status)]"
          >
            {{ user.status }}
          </span>
          <div class="flex mt-2 space-x-2 sm:mt-0">
            <button
              @click="emit('view', user)"
              class="w-6 h-6 transform hover:text-purple-500 hover:scale-110"
            >
              <font-awesome-icon icon="fa-solid fa-eye" />
            </button>
            <div class="relative group">
              <button
                @click="handleActivationToggle(user)"
                :class="[
                  'w-6 h-6 transform hover:scale-110',
                  user.status === 'Active' ? 'text-red-500' : 'text-green-500',
                ]"
              >
                <font-awesome-icon
                  :icon="user.status === 'Active' ? 'fa-solid fa-times' : 'fa-solid fa-check'"
                />
              </button>
              <span
                class="absolute px-2 py-1 mb-2 text-xs text-white transition-transform duration-200 transform scale-0 -translate-x-1/2 bg-black rounded pointer-events-none bottom-full left-1/2 group-hover:scale-100 whitespace-nowrap"
              >
                {{ user.status === 'Active' ? 'Restrict access' : 'Accept profile' }}
              </span>
            </div>
            <button
              @click="emit('delete', user)"
              class="w-6 h-6 transform hover:text-red-500 hover:scale-110"
            >
              <font-awesome-icon icon="fa-solid fa-trash" />
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
