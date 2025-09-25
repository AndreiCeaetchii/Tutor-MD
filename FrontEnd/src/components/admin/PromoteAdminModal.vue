<script setup lang="ts">
import { ref, computed, defineProps, defineEmits } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faTimes, faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import DefaultImg from '../../assets/DefaultImg.png';

library.add(faTimes, faMagnifyingGlass);

interface User {
  id: number;
  name: string;
  email: string;
  type: string;
  status: string;
}

const props = defineProps<{
  users: User[];
  isVisible: boolean;
}>();

const emit = defineEmits(['close', 'promote']);

const searchQuery = ref('');
const selectedUserId = ref<number | null>(null);

const filteredUsers = computed(() => {
  if (!searchQuery.value) {
    return props.users;
  }
  const query = searchQuery.value.toLowerCase();
  return props.users.filter(user => user.email.toLowerCase().includes(query));
});

const promoteUser = () => {
  if (selectedUserId.value) {
    const userToPromote = props.users.find(user => user.id === selectedUserId.value);
    if (userToPromote) {
      emit('promote', userToPromote);
      emit('close');
    }
  }
};
</script>

<template>
  <transition name="modal-fade">
    <div v-if="isVisible" class="fixed inset-0 z-50 overflow-y-auto bg-gray-900 bg-opacity-50 flex justify-center items-center p-4">
      <div class="bg-white rounded-lg shadow-xl w-full max-w-lg mx-auto p-6 relative">
        <button @click="emit('close')" class="absolute top-4 right-4 text-gray-400 hover:text-gray-600">
          <font-awesome-icon icon="fa-solid fa-times" class="h-6 w-6" />
        </button>

        <h2 class="text-xl font-bold mb-4">Promote User to Admin</h2>

        <div class="relative mb-4">
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Search users by email..."
            class="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
          />
          <font-awesome-icon icon="fa-solid fa-magnifying-glass" class="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
        </div>

        <div class="max-h-64 overflow-y-auto border border-gray-200 rounded-lg">
          <ul class="divide-y divide-gray-200">
            <li v-if="filteredUsers.length === 0" class="p-4 text-center text-gray-500">
              No users found.
            </li>
            <li v-for="user in filteredUsers" :key="user.id" class="p-4 flex items-center hover:bg-gray-50 cursor-pointer" @click="selectedUserId = user.id">
              <input type="radio" :value="user.id" v-model="selectedUserId" class="form-radio text-purple-600 mr-4" />
              <img :src="DefaultImg" alt="Profile" class="w-10 h-10 rounded-full mr-3" />
              <div>
                <p class="font-medium text-gray-800">{{ user.name }}</p>
                <p class="text-sm text-gray-500">{{ user.email }}</p>
              </div>
            </li>
          </ul>
        </div>

        <div class="mt-6 flex justify-end">
          <button
            @click="promoteUser"
            :disabled="!selectedUserId"
            class="py-2 px-6 rounded-lg shadow-md transition-all duration-300 transform hover:scale-105 flex items-center gap-2"
            :class="{ 'opacity-50 cursor-not-allowed': !selectedUserId }"
            style="background: linear-gradient(to right, #8b5cf6, #ec4899); color: white;"
          >
            <font-awesome-icon icon="fa-solid fa-crown" class="h-5 w-5" />
            <span>Promote to Admin</span>
          </button>
        </div>
      </div>
    </div>
  </transition>
</template>

<style scoped>
.modal-fade-enter-active, .modal-fade-leave-active {
  transition: opacity 0.3s ease;
}
.modal-fade-enter-from, .modal-fade-leave-to {
  opacity: 0;
}
</style>