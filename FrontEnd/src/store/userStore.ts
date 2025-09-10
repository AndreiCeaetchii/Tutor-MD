import { defineStore } from 'pinia';
import { localStorageService } from '../services/localStorage';

export type UserRole = 'tutor' | 'student' | 'admin';

export const userStore = defineStore('user', {
  state: () => ({
    role: null as UserRole | null,
    isAuthenticated: false,
  }),

  getters: {
    isTutor: (state) => state.role === 'tutor',
    isStudent: (state) => state.role === 'student',
    isAdmin: (state) => state.role === 'admin',
  },

  actions: {
    login(role: UserRole) {
      this.role = role;
      this.isAuthenticated = true;
      localStorageService.setRole(role);
      localStorageService.setAuthenticated(true);
    },

    logout() {
      this.role = null;
      this.isAuthenticated = false;
      localStorageService.clearUserData();
    },

    initializeFromStorage() {
      const storedRole = localStorageService.getRole();
      const isAuthenticated = localStorageService.getAuthenticated();

      if (storedRole && isAuthenticated) {
        this.role = storedRole;
        this.isAuthenticated = true;
      }
    },
  },
});
