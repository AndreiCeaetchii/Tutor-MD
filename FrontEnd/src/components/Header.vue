<script setup lang="ts">
import { ref } from 'vue';
import { Menu, X, User, Bell } from 'lucide-vue-next';
import logo from '../assets/tutor2.png';


interface HeaderProps {
  userType?: 'student' | 'tutor';
}

withDefaults(defineProps<HeaderProps>(), {
  userType: 'student',
});

const isMenuOpen = ref(false);

function toggleMenu() {
  isMenuOpen.value = !isMenuOpen.value;
}

function closeMenu() {
  isMenuOpen.value = false;
}
</script>

<template>
  <header class="bg-white border-b border-gray-100 sticky top-0 z-50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between items-center h-16">
        <!-- Logo -->
        <div class="flex items-center">
          <div class="flex-shrink-0">
            <img class="h-10 w-auto" :src="logo" alt="TutorConnect Logo" />
          </div>
        </div>
        <!-- Desktop Navigation -->
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

        <!-- Desktop User Actions -->
        <div class="hidden md:flex items-center space-x-4">
          <Bell class="w-4 h-4 text-gray-600" />
          <button
              class="flex items-center px-4 py-2  text-gray-600 rounded-full transition-colors
         hover:bg-[#5f22d9] hover:text-white hover:border-[#5f22d9]"
          >
            <User class="w-4 h-4 mr-2" />
            {{ userType === 'tutor' ? 'Sarah Johnson' : 'Alex Thompson' }}
          </button>

        </div>

        <!-- Mobile menu button -->
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

      <!-- Mobile Navigation -->
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
                <span class="text-gray-900 font-medium">{{ userType === 'tutor' ? 'Sarah Johnson' : 'Alex Thompson' }}</span>
              </div>
              <Bell class="w-6 h-6 text-gray-600" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>