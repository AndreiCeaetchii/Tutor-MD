<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import AdminUserTable from './AdminUserTable.vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import PromoteAdminModal from './PromoteAdminModal.vue';
import { useRouter } from 'vue-router';
import {
  getTutorsForAdmin,
  approveTutor,
  declineTutor,
  changeUserStatus,
  getAllStudents,
  createAdmin,
} from '../../services/adminService.ts';
import {
  faUsers,
  faUserCheck,
  faUserClock,
  faUserTimes,
  faSort,
  faSortUp,
  faSortDown,
} from '@fortawesome/free-solid-svg-icons';
import { faMagnifyingGlass, faPlus, faCrown  } from '@fortawesome/free-solid-svg-icons';

library.add(faCrown, faUsers, faUserCheck, faUserClock, faUserTimes, faSort, faSortUp, faSortDown);

library.add(faUsers, faUserCheck, faUserClock, faUserTimes, faSort, faSortUp, faSortDown, faMagnifyingGlass, faPlus);

interface User {
  id: number;
  name: string;
  email: string;
  type: string;
  status: string;
  joinDate: string;
  bookings: number;
  isActive: boolean;
}

const searchQuery = ref<string>('');
const users = ref<User[]>([]);
const sortBy = ref<string>('joinDate');
const sortDirection = ref<string>('desc');
const router = useRouter();

const fetchAllUsers = async () => {
  try {
    const [tutorsResponse, studentsResponse] = await Promise.all([
      getTutorsForAdmin(),
      getAllStudents(),
    ]);

    const tutorUsers = tutorsResponse.map((tutor: any) => {
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
        isActive: tutor.userProfile.isActive,
      };
    });

    const studentUsers = studentsResponse.map((student: any) => {
      const fullName = `${student.userProfile.firstName.trim()} ${student.userProfile.lastName.trim()}`;

      let status = 'Active';

      return {
        id: student.userId,
        name: fullName,
        email: student.userProfile.email,
        type: 'Student',
        status,
        joinDate: student.userProfile.birthdate || new Date().toISOString(),
        bookings: 0,
        isActive: student.userProfile.isActive,
      };
    });

    users.value = [...tutorUsers, ...studentUsers];

  } catch (error) {
    console.error('Error loading all users:', error);
  }
};

onMounted(() => {
  fetchAllUsers();
});

const handleSort = (column: string) => {
  if (sortBy.value === column) {
    sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
  } else {
    sortBy.value = column;
    sortDirection.value = 'asc';
  }
};

const totalUsers = computed(() => users.value.length);
const activeUsers = computed(() => users.value.filter((u) => u.isActive).length);
const pendingUsers = computed(() => users.value.filter((u) => u.type === 'Tutor' && u.status === 'Pending').length);
const suspendedUsers = computed(() => users.value.filter((u) => u.status === 'Suspended' || u.status === 'Deactivated').length);

const sortedAndFilteredUsers = computed(() => {
  const sorted = [...users.value].sort((a, b) => {
    let aValue = a[sortBy.value as keyof User];
    let bValue = b[sortBy.value as keyof User];

    if (sortBy.value === 'joinDate') {
      aValue = new Date(aValue).getTime() as any;
      bValue = new Date(bValue).getTime() as any;
    }

    if (aValue < bValue) return sortDirection.value === 'asc' ? -1 : 1;
    if (aValue > bValue) return sortDirection.value === 'asc' ? 1 : -1;
    return 0;
  });
  return sorted;
});

const onView = (user: User) => {
  if (user.type === 'Student') {
    router.push(`/student/${user.id}`);
    return;
  }
  router.push(`/tutor/${user.id}/profile`);
}

const onDecline = async (user: User) => {
  if (user.type === 'Student') {
    return;
  }
  try {
    if (user.type === 'Tutor') {
      await declineTutor(user.id);
    } else {
      await changeUserStatus(user.id, false);
    }
    await fetchAllUsers();
  } catch (error) {
    console.error(`Failed to decline/deactivate user ${user.name}:`, error);
  }
};

const onApprove = async (user: User) => {
  if (user.type === 'Student') {
    return;
  }

  try {
    if (user.type === 'Tutor') {
      await approveTutor(user.id);
    }
    await fetchAllUsers();
  } catch (error) {
    console.error(`Failed to approve user ${user.name}:`, error);
  }
};

const onActivate = async (user: User) => {
  try {
    await changeUserStatus(user.id, true);
    await fetchAllUsers();
  } catch (error) {
    console.error(`Failed to activate user ${user.name}:`, error);
  }
};

const onDeactivate = async (user: User) => {
  try {
    await changeUserStatus(user.id, false);
    await fetchAllUsers();
  } catch (error) {
    console.error(`Failed to deactivate user ${user.name}:`, error);
  }
};

const isModalVisible = ref(false);

const openModal = () => {
  isModalVisible.value = true;
};

const closeModal = () => {
  isModalVisible.value = false;
};

const handlePromote = async (user: any) => {
  try {
    await createAdmin(user.id);
    await fetchAllUsers();
  } catch (error) {
    console.error(`Failed to promote user ${user.name} to Admin:`, error);
  }
};


</script>

<template>
  <div class="p-6">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">User Management</h1>
      <div class="flex items-center gap-4">
        <button
          @click="openModal"
          class="text-white font-bold py-2 px-4 rounded-full shadow-md transition duration-300 flex items-center gap-2"
          style="background: linear-gradient(to right, #f97316, #f59e0b);"
        >
          <font-awesome-icon icon="fa-solid fa-crown" />
          Create Admin
        </button>
        <div class="relative w-64">
          <input
            type="text"
            v-model="searchQuery"
            placeholder="Search users..."
            class="pl-10 pr-4 py-2 w-full border border-gray-300 rounded-full focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-purple-500"
          />
          <font-awesome-icon icon="fa-solid fa-magnifying-glass" class="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
        </div>
      </div>
    </div>
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
      <div class="bg-white p-6 rounded-lg shadow-md flex justify-between items-center">
        <div>
          <p class="text-gray-500 uppercase text-xs font-bold">Total Users</p>
          <h3 class="text-3xl font-bold text-purple-600">{{ totalUsers }}</h3>
        </div>
        <font-awesome-icon icon="fa-solid fa-users" class="text-4xl text-purple-600" />
      </div>
      <div class="bg-white p-6 rounded-lg shadow-md flex justify-between items-center">
        <div>
          <p class="text-gray-500 uppercase text-xs font-bold">Active Users</p>
          <h3 class="text-3xl font-bold text-green-600">{{ activeUsers }}</h3>
        </div>
        <font-awesome-icon icon="fa-solid fa-user-check" class="text-4xl text-green-600" />
      </div>
      <div class="bg-white p-6 rounded-lg shadow-md flex justify-between items-center">
        <div>
          <p class="text-gray-500 uppercase text-xs font-bold">Pending Approval</p>
          <h3 class="text-3xl font-bold text-yellow-600">{{ pendingUsers }}</h3>
        </div>
        <font-awesome-icon icon="fa-solid fa-user-clock" class="text-4xl text-yellow-600" />
      </div>
      <div class="bg-white p-6 rounded-lg shadow-md flex justify-between items-center">
        <div>
          <p class="text-gray-500 uppercase text-xs font-bold">Suspended/Deactivated</p>
          <h3 class="text-3xl font-bold text-red-600">{{ suspendedUsers }}</h3>
        </div>
        <font-awesome-icon icon="fa-solid fa-user-times" class="text-4xl text-red-600" />
      </div>
    </div>
    <div class="bg-white p-6 rounded-lg shadow-md overflow-x-auto">
      <div class="min-w-full md:min-w-[800px]">
        <AdminUserTable
          :users="sortedAndFilteredUsers"
          :sort-by="sortBy"
          :sort-direction="sortDirection"
          @sort="handleSort"
          @view="onView"
          @deactivate="onDecline"
          @activate="onApprove"
          @suspend="onDeactivate"
          @restore="onActivate"
        />
      </div>
    </div>
  </div>
  <PromoteAdminModal
    :users="users"
    :isVisible="isModalVisible"
    @close="closeModal"
    @promote="handlePromote"
  />
</template>