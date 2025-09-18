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
        <nav class="hidden md:block">
          <div class="flex items-baseline ml-10 space-x-8">
            <a href="#dashboard" class="text-[#5f22d9] font-medium transition-colors">
              Dashboard
            </a>
            <a href="#students" class="text-gray-600 transition-colors hover:text-violet-600">
              Students
            </a>
            <a href="#earnings" class="text-gray-600 transition-colors hover:text-violet-600">
              Earnings
            </a>
            <a href="#help" class="text-gray-600 transition-colors hover:text-violet-600">Help</a>
          </div>
        </nav>

        <div class="items-center hidden space-x-4 md:flex">
          <Bell class="w-5 h-5 text-gray-600" />

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
                class="absolute right-0 mt-2 w-64 bg-white rounded-xl shadow-2xl py-2 z-50"
              >
                <div class="px-4 py-2 border-b border-gray-100">
                  <p class="text-sm font-semibold text-gray-900">{{ userName }}</p>
                  <p class="mt-1 text-xs text-gray-500 truncate">{{ email }}</p>
                </div>
                <div class="py-1">
                  <a
                    href="#profile"
                    class="block text-sm text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <font-awesome-icon
                        :icon="['fas', 'user']"
                        class="w-4 h-4 mr-3 text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                      />
                      Profile
                    </div>
                  </a>
                  <a
                    href="#settings"
                    class="block text-sm text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <font-awesome-icon
                        :icon="['fas', 'cog']"
                        class="w-4 h-4 mr-3 text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                      />

                      Settings
                    </div>
                  </a>
                  <a
                    href="#my-courses"
                    class="block text-sm text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <font-awesome-icon
                        :icon="['fas', 'book']"
                        class="w-4 h-4 mr-3 text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                      />

                      My Courses
                    </div>
                  </a>
                  <a
                    href="#billing"
                    class="block text-sm text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <font-awesome-icon
                        :icon="['fas', 'credit-card']"
                        class="w-4 h-4 mr-3 text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                      />

                      Billing & Payments
                    </div>
                  </a>
                  <a
                    href="#help"
                    class="block text-sm text-gray-700 transition-colors duration-200 hover:text-violet-600 group"
                  >
                    <div class="flex items-center px-4 py-2.5 mx-2 rounded-lg hover:bg-violet-50">
                      <font-awesome-icon
                        :icon="['fas', 'circle-question']"
                        class="w-4 h-4 mr-3 text-gray-500 transition-colors duration-200 group-hover:text-violet-600"
                      />
                      Help & Support
                    </div>
                  </a>
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
              class="text-purple-600 hover:text-purple-800 px-4 py-1 rounded-full text-sm border border-purple-600"
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

        <div class="md:hidden">
          <button
            @click="toggleMenu"
            aria-label="Toggle menu"
            class="p-2 rounded-md text-gray-600 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-purple-500"
          >
            <X v-if="isMenuOpen" class="w-6 h-6" />
            <Menu v-else class="w-6 h-6" />
          </button>
        </div>
      </div>

      <div v-if="isMenuOpen" class="md:hidden">
        <div class="px-2 pt-2 pb-3 space-y-1 bg-white border-t border-gray-100 sm:px-3">
          <a
            href="#dashboard"
            class="block px-3 py-2 text-base font-medium text-purple-500"
            @click="closeMenu"
          >
            Dashboard
          </a>
          <a
            href="#students"
            class="block px-3 py-2 text-base text-gray-600 hover:text-purple-500"
            @click="closeMenu"
          >
            Students
          </a>
          <a
            href="#earnings"
            class="block px-3 py-2 text-base text-gray-600 hover:text-purple-500"
            @click="closeMenu"
          >
            Earnings
          </a>
          <a
            href="#help"
            class="block px-3 py-2 text-base text-gray-600 hover:text-purple-500"
            @click="closeMenu"
          >
            Help
          </a>

          <div class="pt-4 pb-3 border-t border-gray-100">
            <div class="flex items-center justify-between px-3">
              <div class="flex items-center gap-3">
                <div class="flex items-center justify-center w-8 h-8 bg-purple-100 rounded-full">
                  <User class="w-4 h-4 text-purple-500" />
                </div>
                <span class="font-medium text-gray-900">{{ userName }}</span>
              </div>
              <div class="flex items-center gap-3">
                <Bell class="w-6 h-6 text-gray-600" />
                <button
                  v-if="store.isAuthenticated"
                  @click="handleLogout"
                  class="p-2 text-red-600 rounded-full hover:bg-gray-100"
                >
                  <LogOut class="w-5 h-5" />
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>

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
    faBook,
    faCreditCard,
    faCircleQuestion,
    faRightFromBracket,
  } from '@fortawesome/free-solid-svg-icons';
  import { useUserStore } from '../store/userStore.ts';
  import { useProfileStore } from '../store/profileStore.ts';
  import { computed } from 'vue';

  library.add(faUser, faCog, faBook, faCreditCard, faCircleQuestion, faRightFromBracket);

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

  // Adaugă referințe la elementele DOM
  const profileButton = ref<HTMLElement | null>(null);
  const profileMenu = ref<HTMLElement | null>(null);

  function toggleMenu() {
    isMenuOpen.value = !isMenuOpen.value;
  }

  function closeMenu() {
    isMenuOpen.value = false;
  }

  function toggleProfileMenu() {
    showProfileMenu.value = !showProfileMenu.value;
  }

  function handleLogout() {
    store.clearUser();
    profileStore.clearProfile();
    router.push('/landing');
    showProfileMenu.value = false;
  }

  const userName = computed(() => profileStore.userName);
  const email = computed(() => profileStore.email);

  // Funcția care verifică dacă click-ul a avut loc în afara meniului
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

  // Adaugă și elimină event listener-ul în momentele corespunzătoare
  onMounted(() => {
    document.addEventListener('click', handleClickOutside);
  });

  onBeforeUnmount(() => {
    document.removeEventListener('click', handleClickOutside);
  });
</script>
