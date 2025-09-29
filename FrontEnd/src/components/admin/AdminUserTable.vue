<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import {
  faEye,
  faEdit,
  faGraduationCap,
  faChalkboardTeacher,
  faCheck,
  faTimes,
  faBan,
  faUnlock,
  faSort,
  faSortUp,
  faSortDown,
} from '@fortawesome/free-solid-svg-icons';
import DefaultImg from '../../assets/DefaultImg.png';

library.add(faEye, faEdit, faGraduationCap, faChalkboardTeacher, faCheck, faTimes, faBan, faUnlock, faSort, faSortUp, faSortDown);

interface User {
  id: number;
  name: string;
  email: string;
  type: 'Student' | 'Tutor';
  status: 'Active' | 'Pending' | 'Suspended';
  joinDate: string;
  bookings: number;
  isActive: boolean;
}

const props = defineProps<{
  users: User[];
  sortBy: string;
  sortDirection: string;
}>();
const emit = defineEmits(['view', 'delete', 'activate', 'deactivate', 'restore', 'suspend', 'sort']);

const handleActivationToggle = (user: User) => {
  if (user.status === 'Active') {
    emit('deactivate', user);
  } else {
    emit('activate', user);
  }
};

const getDisplayStatus = (user: User): User['status'] => {
  if (!user.isActive) return 'Suspended';
  if (user.status === 'Pending') return 'Pending';
  if (user.status === 'Active') return 'Active';
  return user.status;
};


const getStatusClasses = (status: User['status']) => {
  switch (status) {
    case 'Active': return 'bg-green-100 text-green-800';
    case 'Pending': return 'bg-yellow-100 text-yellow-800';
    case 'Suspended': return 'bg-red-100 text-red-800';
    default: return '';
  }
};

const formattedJoinDate = (dateString: string) => {
  const date = new Date(dateString);
  const options: Intl.DateTimeFormatOptions = { day: '2-digit', month: '2-digit', year: 'numeric' };
  return new Intl.DateTimeFormat('ro-RO', options).format(date);
};

</script>

<template>
  <div class="hidden sm:block overflow-x-auto">
    <table class="min-w-full bg-white rounded-lg shadow-md">
      <thead>
      <tr class="w-full bg-gray-50 text-left text-gray-500 uppercase text-sm leading-normal">
        <th class="py-3 px-6">User</th>
        <th class="py-3 px-6 cursor-pointer hover:bg-gray-200 transition-colors duration-200" @click="emit('sort', 'type')">
          <div class="flex items-center">
            <span>Type</span>
            <font-awesome-icon
              :icon="props.sortBy === 'type' ? (props.sortDirection === 'asc' ? 'fa-solid fa-sort-up' : 'fa-solid fa-sort-down') : 'fa-solid fa-sort'"
              class="ml-1"
            />
          </div>
        </th>
        <th class="py-3 px-6 cursor-pointer hover:bg-gray-200 transition-colors duration-200" @click="emit('sort', 'status')">
          <div class="flex items-center">
            <span>Status</span>
            <font-awesome-icon
              :icon="props.sortBy === 'status' ? (props.sortDirection === 'asc' ? 'fa-solid fa-sort-up' : 'fa-solid fa-sort-down') : 'fa-solid fa-sort'"
              class="ml-1"
            />
          </div>
        </th>
        <th class="py-3 px-6 cursor-pointer hover:bg-gray-200 transition-colors duration-200" @click="emit('sort', 'joinDate')">
          <div class="flex items-center">
            <span>Join Date</span>
            <font-awesome-icon
              :icon="props.sortBy === 'joinDate' ? (props.sortDirection === 'asc' ? 'fa-solid fa-sort-up' : 'fa-solid fa-sort-down') : 'fa-solid fa-sort'"
              class="ml-1"
            />
          </div>
        </th>
        <th class="py-3 px-6 cursor-pointer hover:bg-gray-200 transition-colors duration-200" @click="emit('sort', 'bookings')">
          <div class="flex items-center">
            <span>Reviews</span>
            <font-awesome-icon
              :icon="props.sortBy === 'bookings' ? (props.sortDirection === 'asc' ? 'fa-solid fa-sort-up' : 'fa-solid fa-sort-down') : 'fa-solid fa-sort'"
              class="ml-1"
            />
          </div>
        </th>
        <th class="py-3 px-6 text-center">Actions</th>
      </tr>
      </thead>
      <tbody class="text-gray-600 text-sm font-light">
      <tr v-for="user in users" :key="user.id" class="border-b border-gray-200 hover:bg-gray-100">
        <td class="py-3 px-6 text-left whitespace-nowrap">
          <div class="flex items-center">
            <img :src="DefaultImg" alt="Profile" class="w-10 h-10 rounded-full mr-3" />
            <div>
              <p class="font-medium">{{ user.name }}</p>
              <p class="text-gray-500 text-xs">{{ user.email }}</p>
            </div>
          </div>
        </td>
        <td class="py-3 px-6 text-left">
            <span class="flex items-center">
              <font-awesome-icon
                :icon="user.type === 'Student' ? 'fa-solid fa-graduation-cap' : 'fa-solid fa-chalkboard-teacher'"
                class="text-purple-500 mr-2"
              />
              {{ user.type }}
            </span>
        </td>
        <td class="py-3 px-6 text-left">
            <span :class="['py-1 px-3 rounded-full text-xs font-semibold', getStatusClasses(getDisplayStatus(user))]">
              {{ getDisplayStatus(user) }}
            </span>

        </td>
        <td class="py-3 px-6 text-left">{{ formattedJoinDate(user.joinDate)  }}</td>
        <td class="py-3 px-6 text-left">{{ user.bookings }}</td>
        <td class="py-3 px-6 text-center">
          <div class="flex justify-center space-x-2">
            <div class="relative group">
              <button @click="emit('view', user)" class="w-6 h-6 hover:text-purple-500 hover:scale-110 transform">
                <font-awesome-icon icon="fa-solid fa-eye" />
              </button>
              <div class="absolute -top-6 left-1/2 transform -translate-x-1/2 z-10 opacity-0 group-hover:opacity-100 transition-opacity duration-0 bg-gray-700 text-white text-xs px-2 py-1 rounded-md whitespace-nowrap">
                View User
              </div>
            </div>
            <div class="relative group">
              <button
                @click="handleActivationToggle(user)"
                :class="['w-6 h-6 hover:scale-110 transform', user.status === 'Active' ? 'text-red-500' : 'text-green-500']"
              >
                <font-awesome-icon :icon="user.status === 'Active' ? 'fa-solid fa-times' : 'fa-solid fa-check'" />
              </button>
              <div class="absolute -top-6 left-1/2 transform -translate-x-1/2 z-10 opacity-0 group-hover:opacity-100 transition-opacity duration-0 bg-gray-700 text-white text-xs px-2 py-1 rounded-md whitespace-nowrap">
                {{ user.status === 'Active' ? 'Decline' : 'Approve' }}
              </div>
            </div>
            <div class="relative group">
              <button
                @click="user.isActive ? emit('suspend', user) : emit('restore', user)"
                :class="['w-6 h-6 hover:scale-110 transform', user.isActive ? 'text-red-500' : 'text-green-500']"
              >
                <font-awesome-icon :icon="user.isActive ? 'fa-solid fa-ban' : 'fa-solid fa-unlock'" />
              </button>
              <div class="absolute -top-6 left-1/2 transform -translate-x-1/2 z-10 opacity-0 group-hover:opacity-100 transition-opacity duration-0 bg-gray-700 text-white text-xs px-2 py-1 rounded-md whitespace-nowrap">
                {{ user.isActive ? 'Suspend' : 'Restore' }}
              </div>
            </div>
          </div>
        </td>
      </tr>
      </tbody>
    </table>
  </div>
  <div class="block sm:hidden space-y-4">
    <div
      v-for="user in users"
      :key="user.id"
      class="bg-white p-4 rounded-lg shadow-md flex flex-col space-y-3 max-w-sm mx-auto"
    >
      <div class="flex items-center">
        <img :src="DefaultImg" alt="Profile" class="w-10 h-10 rounded-full mr-4" />
        <div>
          <h3 class="font-bold text-lg">{{ user.name }}</h3>
          <p class="text-sm text-gray-500">{{ user.email }}</p>
        </div>
      </div>

      <div class="text-sm text-gray-700 space-y-1">
        <p><span class="font-semibold">Type:</span> {{ user.type }}</p>
        <p><span class="font-semibold">Join Date:</span> {{ formattedJoinDate(user.joinDate)  }}</p>
        <p><span class="font-semibold">Bookings:</span> {{ user.bookings }}</p>
      </div>

      <span
        :class="['py-1 px-3 rounded-full text-xs font-semibold self-start', getStatusClasses(user.status)]"
      >
      {{ user.status }}
    </span>

      <div class="flex justify-between pt-2 border-t">
        <div class="relative group">
          <button @click="emit('view', user)" class="w-6 h-6 hover:text-purple-500 hover:scale-110 transform">
            <font-awesome-icon icon="fa-solid fa-eye" />
          </button>
          <div class="absolute -top-6 left-1/2 transform -translate-x-1/2 z-10 opacity-0 group-hover:opacity-100 transition-opacity duration-0 bg-gray-700 text-white text-xs px-2 py-1 rounded-md whitespace-nowrap">
            View User
          </div>
        </div>
        <div class="relative group">
          <button
            @click="handleActivationToggle(user)"
            :class="['w-6 h-6 hover:scale-110 transform', user.status === 'Active' ? 'text-red-500' : 'text-green-500']"
          >
            <font-awesome-icon :icon="user.status === 'Active' ? 'fa-solid fa-times' : 'fa-solid fa-check'" />
          </button>
          <div class="absolute -top-6 left-1/2 transform -translate-x-1/2 z-10 opacity-0 group-hover:opacity-100 transition-opacity duration-0 bg-gray-700 text-white text-xs px-2 py-1 rounded-md whitespace-nowrap">
            {{ user.status === 'Active' ? 'Decline' : 'Approve' }}
          </div>
        </div>
        <div class="relative group">
          <button
            @click="user.isActive ? emit('suspend', user) : emit('restore', user)"
            :class="['w-6 h-6 hover:scale-110 transform', user.isActive ? 'text-red-500' : 'text-green-500']"
          >
            <font-awesome-icon :icon="user.isActive ? 'fa-solid fa-ban' : 'fa-solid fa-unlock'" />
          </button>
          <div class="absolute -top-6 left-1/2 transform -translate-x-1/2 z-10 opacity-0 group-hover:opacity-100 transition-opacity duration-0 bg-gray-700 text-white text-xs px-2 py-1 rounded-md whitespace-nowrap">
            {{ user.isActive ? 'Suspend' : 'Restore' }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>