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
  import { faMagnifyingGlass, faPlus, faCrown } from '@fortawesome/free-solid-svg-icons';

  library.add(
    faCrown,
    faUsers,
    faUserCheck,
    faUserClock,
    faUserTimes,
    faSort,
    faSortUp,
    faSortDown,
  );

  library.add(
    faUsers,
    faUserCheck,
    faUserClock,
    faUserTimes,
    faSort,
    faSortUp,
    faSortDown,
    faMagnifyingGlass,
    faPlus,
  );

  interface User {
    id: number;
    name: string;
    email: string;
    type: 'Tutor' | 'Student';
    status: 'Pending' | 'Suspended' | 'Active';
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
        let status: 'Pending' | 'Suspended' | 'Active' = 'Pending';
        if (!tutor.userProfile.isActive) {
          status = 'Suspended';
        } else if (tutor.verificationStatus === 'Verified') {
          status = 'Active';
        } else if (tutor.verificationStatus === 'Suspended') {
          status = 'Suspended';
        }

        return {
          id: tutor.userId,
          name: fullName,
          email: tutor.userProfile.email,
          type: 'Tutor' as const,
          status,
          joinDate: tutor.userProfile.birthdate || new Date().toISOString(),
          bookings: tutor.reviewCount || 0,
          isActive: tutor.userProfile.isActive,
        };
      });

      const studentUsers = studentsResponse.map((student: any) => {
        const fullName = `${student.userProfile.firstName.trim()} ${student.userProfile.lastName.trim()}`;

        let status: 'Pending' | 'Suspended' | 'Active' = 'Active';
        if (!student.userProfile.isActive) {
          status = 'Suspended';
        }

        return {
          id: student.userId,
          name: fullName,
          email: student.userProfile.email,
          type: 'Student' as const,
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
  const pendingUsers = computed(
    () => users.value.filter((u) => u.type === 'Tutor' && u.status === 'Pending').length,
  );
  const suspendedUsers = computed(() => users.value.filter((u) => u.status === 'Suspended').length);

  const sortedAndFilteredUsers = computed(() => {
    let filtered = users.value;
    if (searchQuery.value.trim()) {
      const query = searchQuery.value.toLowerCase();
      filtered = users.value.filter(
        (u) =>
          u.name.toLowerCase().includes(query) ||
          u.email.toLowerCase().includes(query) ||
          u.type.toLowerCase().includes(query) ||
          u.status.toLowerCase().includes(query),
      );
    }

    const sorted = [...filtered].sort((a, b) => {
      let aValue = a[sortBy.value as keyof User];
      let bValue = b[sortBy.value as keyof User];

      if (sortBy.value === 'joinDate') {
        aValue = new Date(aValue as string | number | Date).getTime() as any;
        bValue = new Date(bValue as string | number | Date).getTime() as any;
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
  };

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
    <div class="flex items-center justify-between mb-6">
      <h1 class="text-2xl font-bold">User Management</h1>
      <div class="flex items-center gap-4">
        <button
          @click="openModal"
          class="flex items-center gap-2 px-4 py-2 font-bold text-white transition duration-300 rounded-full shadow-md"
          style="background: linear-gradient(to right, #f97316, #f59e0b)"
        >
          <font-awesome-icon icon="fa-solid fa-crown" />
          Create Admin
        </button>
        <div class="relative w-64">
          <input
            type="text"
            v-model="searchQuery"
            placeholder="Search users..."
            class="w-full py-2 pl-10 pr-4 border border-gray-300 rounded-full focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-purple-500"
          />
          <font-awesome-icon
            icon="fa-solid fa-magnifying-glass"
            class="absolute text-gray-400 transform -translate-y-1/2 left-3 top-1/2"
          />
        </div>
      </div>
    </div>
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
          <p class="text-xs font-bold text-gray-500 uppercase">Suspended/Deactivated</p>
          <h3 class="text-3xl font-bold text-red-600">{{ suspendedUsers }}</h3>
        </div>
        <font-awesome-icon icon="fa-solid fa-user-times" class="text-4xl text-red-600" />
      </div>
    </div>
    <div class="p-6 overflow-x-auto bg-white rounded-lg shadow-md">
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
