<script setup lang="ts">
  import { ref, onMounted, onUnmounted, computed } from 'vue';
  import AdminUserTable from './AdminUserTable.vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { getTutors } from '../../services/tutorService.ts';
  import {
    faUsers,
    faUserCheck,
    faUserClock,
    faUserTimes,
  } from '@fortawesome/free-solid-svg-icons';

  library.add(faUsers, faUserCheck, faUserClock, faUserTimes);

  interface User {
    id: number;
    name: string;
    email: string;
    type: string;
    status: string;
    joinDate: string;
    bookings: number;
  }

  const users = ref<User[]>([]);
  const isMobile = ref(window.innerWidth < 768);

  const fetchTutors = async () => {
    try {
      const response = await getTutors();

      users.value = response.map((tutor: any) => {
        const fullName = `${tutor.userProfile.firstName.trim()} ${tutor.userProfile.lastName.trim()}`;

        let status = 'Pending';
        if (tutor.verificationStatus === 'Verified') {
          status = 'Active';
        } else if (tutor.verificationStatus === 'Suspended') {
          status = 'Suspended';
        }

        return {
          id: tutor.userId,
          name: fullName,
          email: tutor.userProfile.email,
          type: 'Tutor',
          status,
          joinDate: tutor.userProfile.birthdate || new Date().toISOString(),
          bookings: tutor.reviewCount || 0,
        };
      });
    } catch (error) {
      console.error('Error loading tutors:', error);
    }
  };

  const handleResize = () => {
    isMobile.value = window.innerWidth < 768;
  };

  onMounted(() => {
    window.addEventListener('resize', handleResize);
    fetchTutors();
  });

  onUnmounted(() => {
    window.removeEventListener('resize', handleResize);
  });

  const totalUsers = computed(() => users.value.length);
  const activeUsers = computed(() => users.value.filter((u) => u.status === 'Active').length);
  const pendingUsers = computed(() => users.value.filter((u) => u.status === 'Pending').length);
  const suspendedUsers = computed(() => users.value.filter((u) => u.status === 'Suspended').length);

  const sortedUsers = computed(() =>
    [...users.value].sort(
      (a, b) => new Date(b.joinDate).getTime() - new Date(a.joinDate).getTime(),
    ),
  );

  const onView = (user: User) => console.log(`View user: ${user.name}`);
  const onEdit = (user: User) => console.log(`Edit user: ${user.name}`);
  const onDelete = (user: User) => console.log(`Delete user: ${user.name}`);
  const onActivate = (user: User) => console.log(`Activate user: ${user.name}`);
  const onDeactivate = (user: User) => console.log(`Deactivate user: ${user.name}`);
</script>

<template>
  <div class="p-6">
    <h1 class="mb-6 text-2xl font-bold">User Management</h1>
    <div class="grid grid-cols-1 gap-4 mb-6 sm:grid-cols-2 lg:grid-cols-4">
      <div class="flex items-center justify-between p-6 bg-white rounded-lg shadow-md">
        <div>
          <p class="text-xs font-bold text-gray-500 uppercase">Total Users</p>
          <h3 class="text-3xl font-bold text-purple-600">{{ totalUsers }}</h3>
        </div>
        <font-awesome-icon icon="fa-solid fa-users" class="text-4xl text-purple-600" />
      </div>
      <div class="flex items-center justify-between p-6 bg-white rounded-lg shadow-md">
        <div>
          <p class="text-xs font-bold text-gray-500 uppercase">Active Users</p>
          <h3 class="text-3xl font-bold text-green-600">{{ activeUsers }}</h3>
        </div>
        <font-awesome-icon icon="fa-solid fa-user-check" class="text-4xl text-green-600" />
      </div>
      <div class="flex items-center justify-between p-6 bg-white rounded-lg shadow-md">
        <div>
          <p class="text-xs font-bold text-gray-500 uppercase">Pending Approval</p>
          <h3 class="text-3xl font-bold text-yellow-600">{{ pendingUsers }}</h3>
        </div>
        <font-awesome-icon icon="fa-solid fa-user-clock" class="text-4xl text-yellow-600" />
      </div>
      <div class="flex items-center justify-between p-6 bg-white rounded-lg shadow-md">
        <div>
          <p class="text-xs font-bold text-gray-500 uppercase">Suspended</p>
          <h3 class="text-3xl font-bold text-red-600">{{ suspendedUsers }}</h3>
        </div>
        <font-awesome-icon icon="fa-solid fa-user-times" class="text-4xl text-red-600" />
      </div>
    </div>

    <!-- User table -->
    <div v-if="!isMobile" class="p-6 bg-white rounded-lg shadow-md">
      <AdminUserTable
        :users="sortedUsers"
        @view="onView"
        @edit="onEdit"
        @delete="onDelete"
        @activate="onActivate"
        @deactivate="onDeactivate"
      />
    </div>
  </div>
</template>
