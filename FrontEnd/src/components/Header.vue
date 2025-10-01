<script setup lang="ts">
  import { ref, onMounted, onBeforeUnmount } from 'vue';
  import { Menu, X, User, Bell, LogOut } from 'lucide-vue-next';
  import { useRouter } from 'vue-router';
  import logo from '../assets/tutor2.png';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import {
    faUser,
    faCog,
    faCalendarDays,
    faMessage,
    faUserGear,
    faRightFromBracket,
    faShieldHalved,
    faCheckCircle,
  } from '@fortawesome/free-solid-svg-icons';
  import { useUserStore } from '../store/userStore.ts';
  import { useProfileStore } from '../store/profileStore.ts';
  import { computed } from 'vue';
  import { useStudentProfileStore } from '../store/studentProfileStore.ts';
  import NotificationsDropdown from '../components/notifications/NotificationsDropdown.vue';

  library.add(
    faUser, 
    faCog, 
    faCalendarDays,
    faMessage,
    faUserGear,
    faRightFromBracket, 
    faShieldHalved,
    faCheckCircle
  );

  interface HeaderProps {
    userType?: 'student' | 'tutor';
  }

  withDefaults(defineProps<HeaderProps>(), {
    userType: 'student',
  });

  const router = useRouter();
  const store = useUserStore();
  const profileStore = useProfileStore();
  const isMenuOpen = ref(false);
  const showProfileMenu = ref(false);
  const studentProfileStore = useStudentProfileStore();

  const profileButton = ref<HTMLElement | null>(null);
  const profileMenu = ref<HTMLElement | null>(null);

  const hasMfa = computed(() => store.mfaEnabled);

  function toggleMenu() {
    isMenuOpen.value = !isMenuOpen.value;
  }

  function toggleProfileMenu() {
    showProfileMenu.value = !showProfileMenu.value;
  }

  function closeProfileMenu() {
    showProfileMenu.value = false;
  }

  function handleLogout() {
    store.clearUser();
    profileStore.clearProfile();
    studentProfileStore.clearProfile();
    router.push('/landing');
    showProfileMenu.value = false;
  }

  function navigateToMfaSetup() {
    router.push('/mfa-setup');
    showProfileMenu.value = false;
  }

  const userRole = computed(() => store.userRole);

  function navigateBasedOnRole(path: string) {
    const role = userRole.value;
    if (role === 'tutor') {
      router.push(`/tutor-dashboard${path}`);
    } else if (role === 'student') {
      router.push(`/student-dashboard${path}`);
    } else if (role === 'admin') {
      router.push(`/admin-dashboard${path}`);
    } else {
      router.push('/landing');
    }
    showProfileMenu.value = false;
  }

  const userName = computed(() => {
  if (userRole.value === 'student') {
    if (studentProfileStore.userProfile?.username) {
      return studentProfileStore.userProfile.username;
    }
    
    if (store.email) {
      const emailPart = store.email.split('@')[0];
      const cleanEmailPart = emailPart.replace(/[^a-zA-Z0-9_]/g, '_');
      studentProfileStore.updateUsername(cleanEmailPart);
      return cleanEmailPart;
    }
    
    return 'user';
  } else if (userRole.value === 'admin') {
    if (store.email) {
      return `Admin`;
    }
    return 'Administrator';
  } else {
    return profileStore.userName || '';
  }
});

  const email = computed(() => store.email);

  function handleClickOutside(event: MouseEvent) {
    if (
      showProfileMenu.value &&
      profileButton.value &&
      profileMenu.value &&
      !profileButton.value.contains(event.target as Node) &&
      !profileMenu.value.contains(event.target as Node)
    ) {
      showProfileMenu.value = false;
    }
  }

  onMounted(() => {
    document.addEventListener('click', handleClickOutside);
  });

  onBeforeUnmount(() => {
    document.removeEventListener('click', handleClickOutside);
  });
</script>

<template>
  <header class="sticky top-0 z-50 bg-white border-b border-gray-100">
    <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
      <div class="flex items-center justify-between h-16">
        <div class="flex items-center">
          <div class="flex-shrink-0">
            <router-link to="/">
              <img class="w-auto h-10" :src="logo" alt="TutorConnect Logo" />
            </router-link>
          </div>
        </div>
        <div class="items-center hidden space-x-4 md:flex">
          <NotificationsDropdown v-if="store.isAuthenticated" @close-other-menus="closeProfileMenu" />

          <div v-if="store.isAuthenticated" class="relative">
            <button
              @click="toggleProfileMenu"
              ref="profileButton"
              class="flex items-center px-4 py-2 text-gray-600 rounded-full transition-colors hover:bg-[#5f22d9] hover:text-white hover:border-[#5f22d9]"
            >
              <User class="w-5 h-5 mr-2" />
              {{ userName }}
            </button>

            <transition
              enter-active-class="transition duration-200 ease-out"
              enter-from-class="transform scale-95 opacity-0"
              enter-to-class="transform scale-100 opacity-100"
              leave-active-class="transition duration-75 ease-in"
              leave-from-class="transform scale-100 opacity-100"
              leave-to-class="transform scale-95 opacity-0"
            >
              <div
                v-if="showProfileMenu"
                ref="profileMenu"
                class="absolute right-0 z-50 w-64 py-2 mt-2 bg-white shadow-2xl rounded-xl"
              >
                <div class="px-4 py-2 border-b border-gray-100">
                  <p class="text-sm font-semibold text-gray-900">{{ userName }}</p>
                  <p class="mt-1 text-xs text-gray-500 truncate">{{ email }}</p>
                </div>
                <div class="py-1">
                  <button
                    @click="navigateToMfaSetup"
                    class="block w-full text-left text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <div class="relative w-4 h-4 mr-3">
                        <font-awesome-icon 
                          :icon="['fas', 'shield-halved']" 
                          class="w-4 h-4 text-gray-500 transition-colors duration-200 group-hover:text-violet-600" 
                        />
                        <font-awesome-icon 
                          v-if="hasMfa"
                          :icon="['fas', 'check-circle']" 
                          class="absolute -top-1 -right-1.5 w-3 h-3 text-green-500" 
                        />
                      </div>
                      {{ hasMfa ? 'Manage 2FA Security' : 'Enable 2FA Security' }}
                    </div>
                  </button>

                  <!-- Tutor: Availability Link -->
                  <button
                    v-if="userRole === 'tutor'"
                    @click="navigateBasedOnRole('/availability')"
                    class="block w-full text-sm text-left text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <font-awesome-icon
                        :icon="['fas', 'calendar-days']"
                        class="w-4 h-4 mr-3 text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                      />
                      Availability
                    </div>
                  </button>

                  <!-- Profile Link with better icon -->
                  <button
                    @click="navigateBasedOnRole(userRole === 'student' ? '/account' : '/profile')"
                    class="block w-full text-sm text-left text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <font-awesome-icon
                        :icon="['fas', 'user-gear']"
                        class="w-4 h-4 mr-3 text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                      />
                      Profile Settings
                    </div>
                  </button>
                  
                  <!-- Bookings Link with better icon -->
                  <button
                    @click="navigateBasedOnRole('/bookings')"
                    class="block w-full text-sm text-left text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <font-awesome-icon
                        :icon="['fas', 'calendar-days']"
                        class="w-4 h-4 mr-3 text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                      />
                      Bookings
                    </div>
                  </button>
                  
                  <!-- Messages Link with better icon -->
                  <button
                    @click="navigateBasedOnRole('/messages')"
                    class="block w-full text-sm text-left text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <font-awesome-icon
                        :icon="['fas', 'message']"
                        class="w-4 h-4 mr-3 text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                      />
                      Messages
                    </div>
                  </button>
                </div>
                <div class="pt-2 mt-2 border-t border-gray-100">
                  <button
                    @click="handleLogout"
                    class="w-full text-sm text-left text-red-600 transition-colors duration-200 hover:text-red-700 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-red-50">
                      <font-awesome-icon
                        :icon="['fas', 'right-from-bracket']"
                        class="w-4 h-4 mr-3 text-red-600 transition-colors duration-200 group-hover:text-red-700"
                      />
                      Logout
                    </div>
                  </button>
                </div>
              </div>
            </transition>
          </div>

          <div v-else class="flex space-x-2">
            <router-link
              to="/login"
              class="px-4 py-1 text-sm text-[#5f22d9] border border-[#5d15ec] rounded-full hover:text-purple-800"
            >
              Login
            </router-link>
            <router-link
              to="/signup"
              class="bg-[#5f22d9] hover:bg-[#5d15ec] text-white px-4 py-1 rounded-full text-sm"
            >
              Sign Up
            </router-link>
          </div>
        </div>

        <div class="md:hidden">
          <button
            v-if="store.isAuthenticated"
            @click="toggleMenu"
            aria-label="Toggle menu"
            class="p-2 text-gray-600 rounded-md hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-purple-500"
          >
            <X v-if="isMenuOpen" class="w-6 h-6" />
            <Menu v-else class="w-6 h-6" />
          </button>
        </div>
      </div>

      <div v-if="isMenuOpen && store.isAuthenticated" class="md:hidden">
        <div class="px-2 pt-2 pb-3 space-y-1 bg-white border-t border-gray-100 sm:px-3">
          <div class="py-2 space-y-1">
            <button
              @click="navigateBasedOnRole(userRole === 'student' ? '/account' : '/profile')"
              class="flex items-center w-full px-3 py-2 text-left text-gray-700 rounded-md hover:bg-gray-100 hover:text-purple-600"
            >
              <font-awesome-icon :icon="['fas', 'user-gear']" class="w-5 h-5 mr-3 text-gray-500" />
              Profile
            </button>
            
            <!-- 2FA link with check mark for mobile -->
            <button
              @click="navigateToMfaSetup"
              class="flex items-center w-full px-3 py-2 text-left text-gray-700 rounded-md hover:bg-gray-100 hover:text-purple-600"
            >
              <div class="relative">
                <font-awesome-icon :icon="['fas', 'shield-halved']" class="w-5 h-5 mr-3 text-gray-500" />
                <font-awesome-icon 
                  v-if="hasMfa"
                  :icon="['fas', 'check-circle']" 
                  class="absolute -top-1 -right-1.5 w-3 h-3 text-green-500" 
                />
              </div>
              Security Settings
            </button>
            
            <button
              v-if="userRole === 'tutor'"
              @click="navigateBasedOnRole('/availability')"
              class="flex items-center w-full px-3 py-2 text-left text-gray-700 rounded-md hover:bg-gray-100 hover:text-purple-600"
            >
              <font-awesome-icon :icon="['fas', 'calendar-days']" class="w-5 h-5 mr-3 text-gray-500" />
              Availability
            </button>
            
            <button
              @click="navigateBasedOnRole('/bookings')"
              class="flex items-center w-full px-3 py-2 text-left text-gray-700 rounded-md hover:bg-gray-100 hover:text-purple-600"
            >
              <font-awesome-icon
                :icon="['fas', 'calendar-days']"
                class="w-5 h-5 mr-3 text-gray-500"
              />
              Bookings
            </button>
            
            <button
              @click="navigateBasedOnRole('/messages')"
              class="flex items-center w-full px-3 py-2 text-left text-gray-700 rounded-md hover:bg-gray-100 hover:text-purple-600"
            >
              <font-awesome-icon
                :icon="['fas', 'message']"
                class="w-5 h-5 mr-3 text-gray-500"
              />
              Messages
            </button>
          </div>

          <div class="pt-4 pb-3 border-t border-gray-100">
            <div v-if="store.isAuthenticated" class="flex items-center justify-between px-3">
              <div class="flex items-center gap-3">
                <div class="flex items-center justify-center w-8 h-8 bg-purple-100 rounded-full">
                  <User class="w-4 h-4 text-purple-500" />
                </div>
                <span class="font-medium text-gray-900">{{ userName }}</span>
              </div>
              <div class="flex items-center gap-3">
                <Bell class="w-6 h-6 text-gray-600" />
                <button
                  @click="handleLogout"
                  class="p-2 text-red-600 rounded-full hover:bg-gray-100"
                >
                  <LogOut class="w-5 h-5" />
                </button>
              </div>
            </div>

            <div v-else class="flex space-x-2">
              <router-link
                to="/login"
                class="px-4 py-1 text-sm text-purple-600 border border-purple-600 rounded-full hover:text-purple-800"
              >
                Login
              </router-link>
              <router-link
                to="/signup"
                class="bg-[#5f22d9] hover:bg-purple-700 text-white px-4 py-1 rounded-full text-sm"
              >
                Sign Up
              </router-link>
            </div>

          </div>
        </div>
      </div>
    </div>
  </header>
</template>
