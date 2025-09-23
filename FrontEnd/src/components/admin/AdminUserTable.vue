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
        <tr class="w-full bg-gray-50 text-left text-gray-500 uppercase text-sm leading-normal">
          <th class="py-3 px-6">User</th>
          <th class="py-3 px-6">Type</th>
          <th class="py-3 px-6">Status</th>
          <th class="py-3 px-6">Join Date</th>
          <th class="py-3 px-6">Bookings</th>
          <th class="py-3 px-6 text-center">Actions</th>
        </tr>
      </thead>
      <tbody class="text-gray-600 text-sm font-light">
        <tr v-for="user in users" :key="user.id" class="border-b border-gray-200 hover:bg-gray-100">
          <td class="py-3 px-6 text-left whitespace-nowrap">
            <div class="flex items-center">
              <div class="mr-3">
                <img :src="DefaultImg" alt="Profile" class="w-10 h-10 rounded-full" />
              </div>
              <div>
                <p class="font-medium">{{ user.name }}</p>
                <p class="text-gray-500 text-xs">{{ user.email }}</p>
              </div>
            </div>
          </td>
          <td class="py-3 px-6 text-left">
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
          <td class="py-3 px-6 text-left">
            <span
              :class="[
                'py-1 px-3 rounded-full text-xs font-semibold',
                getStatusClasses(user.status),
              ]"
            >
              {{ user.status }}
            </span>
          </td>
          <td class="py-3 px-6 text-left">{{ user.joinDate }}</td>
          <td class="py-3 px-6 text-left">{{ user.bookings }}</td>
          <td class="py-3 px-6 text-center">
            <div class="flex item-center justify-center space-x-2">
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
                  class="absolute bottom-full mb-2 left-1/2 -translate-x-1/2 transform scale-0 group-hover:scale-100 transition-transform duration-200 bg-black text-white text-xs rounded py-1 px-2 pointer-events-none whitespace-nowrap"
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
      class="bg-white p-4 rounded-lg shadow-md flex flex-col sm:flex-row justify-between items-start sm:items-center"
    >
      <div class="flex items-center mb-4 sm:mb-0">
        <img :src="DefaultImg" alt="Profile" class="w-10 h-10 rounded-full mr-4" />
        <div>
          <h3 class="font-bold text-lg">{{ user.name }}</h3>
          <p class="text-sm text-gray-500">{{ user.email }}</p>
        </div>
      </div>
      <div class="flex flex-col sm:flex-row sm:items-center sm:space-x-4">
        <div class="flex flex-col text-sm text-gray-700 mb-2 sm:mb-0">
          <p><span class="font-semibold">Type:</span> {{ user.type }}</p>
          <p><span class="font-semibold">Join Date:</span> {{ user.joinDate }}</p>
          <p><span class="font-semibold">Bookings:</span> {{ user.bookings }}</p>
        </div>
        <div class="flex flex-col items-start sm:items-center space-y-2 sm:space-y-0 sm:space-x-2">
          <span
            :class="['py-1 px-3 rounded-full text-xs font-semibold', getStatusClasses(user.status)]"
          >
            {{ user.status }}
          </span>
          <div class="flex space-x-2 mt-2 sm:mt-0">
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
                class="absolute bottom-full mb-2 left-1/2 -translate-x-1/2 transform scale-0 group-hover:scale-100 transition-transform duration-200 bg-black text-white text-xs rounded py-1 px-2 pointer-events-none whitespace-nowrap"
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
