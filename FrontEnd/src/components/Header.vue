<script setup lang="ts">
import { ref, computed as vueComputed } from 'vue';
import { Menu, X, User, Bell, LogOut } from 'lucide-vue-next';
import { useRouter } from 'vue-router';
import logo from '../assets/tutor2.png';
import { userStore } from '../store/userStore';

interface HeaderProps {
  userType?: 'student' | 'tutor';
}

withDefaults(defineProps<HeaderProps>(), {
  userType: 'student',
});

const router = useRouter();
const store = userStore();
const isMenuOpen = ref(false);
const showProfileMenu = ref(false);

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
  store.logout();
  router.push('/landing');
  showProfileMenu.value = false;
}

const userName = computed(() => {
  if (store.isAuthenticated && store.currentUser?.name) {
    return store.currentUser.name;
  }
  return store.userRole === 'tutor' ? 'Sarah Johnson' : 'Alex Thompson';
});

function computed(getter: () => string) {
  return vueComputed(getter);
}
</script>

<template>
  <header class="bg-white border-b border-gray-100 sticky top-0 z-50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between items-center h-16">
        <div class="flex items-center">
          <div class="flex-shrink-0">
            <router-link to="/">
              <img class="h-10 w-auto" :src="logo" alt="TutorConnect Logo" />
            </router-link>
          </div>
        </div>
        <nav class="hidden md:block">
          <div class="ml-10 flex items-baseline space-x-8">
            <a href="#dashboard" class="text-[#5f22d9] font-medium transition-colors">
              Dashboard
            </a>
            <a href="#students" class="text-gray-600 hover:text-violet-600 transition-colors">
              Students
            </a>
            <a href="#earnings" class="text-gray-600 hover:text-violet-600 transition-colors">
              Earnings
            </a>
            <a href="#help" class="text-gray-600 hover:text-violet-600 transition-colors">
              Help
            </a>
          </div>
        </nav>

        <div class="hidden md:flex items-center space-x-4">
          <Bell class="w-4 h-4 text-gray-600" />
          
          <div v-if="store.isAuthenticated" class="relative">
            <button
              @click="toggleProfileMenu"
              class="flex items-center px-4 py-2 text-gray-600 rounded-full transition-colors
              hover:bg-[#5f22d9] hover:text-white hover:border-[#5f22d9]"
            >
              <User class="w-4 h-4 mr-2" />
              {{ userName }}
            </button>
            
            <div 
              v-if="showProfileMenu" 
              class="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 z-50"
            >
              <a 
                href="#profile" 
                class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
              >
                Profile
              </a>
              <a 
                href="#settings" 
                class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
              >
                Settings
              </a>
              <button 
                @click="handleLogout"
                class="flex items-center w-full text-left px-4 py-2 text-sm text-red-600 hover:bg-gray-100"
              >
                <LogOut class="w-4 h-4 mr-2" />
                Logout
              </button>
            </div>
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
            <X v-if="isMenuOpen" class="h-6 w-6" />
            <Menu v-else class="h-6 w-6" />
          </button>
        </div>
      </div>

      <div v-if="isMenuOpen" class="md:hidden">
        <div class="px-2 pt-2 pb-3 space-y-1 sm:px-3 bg-white border-t border-gray-100">
          <a
            href="#dashboard"
            class="block px-3 py-2 text-base text-purple-500 font-medium"
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
          
          <div class="border-t border-gray-100 pt-4 pb-3">
            <div class="flex items-center justify-between px-3">
              <div class="flex items-center gap-3">
                <div class="w-8 h-8 bg-purple-100 rounded-full flex items-center justify-center">
                  <User class="w-4 h-4 text-purple-500" />
                </div>
                <span class="text-gray-900 font-medium">{{ userName }}</span>
              </div>
              <div class="flex items-center gap-3">
                <Bell class="w-6 h-6 text-gray-600" />
                <button 
                  v-if="store.isAuthenticated"
                  @click="handleLogout"
                  class="p-2 text-red-600 hover:bg-gray-100 rounded-full"
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