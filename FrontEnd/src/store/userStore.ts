import { defineStore } from "pinia";
import { localStorageService } from "../services/localStorage";

export type UserRole = "tutor" | "student" | "admin";

export interface User {
  id: string;
  email: string;
  name?: string;
  role: UserRole;
  token?: string;
}

export const userStore = defineStore("user", {
  state: () => ({
    currentUser: null as User | null,
    isAuthenticated: false,
    selectedRole: null as UserRole | null,
  }),

  getters: {
    userRole: (state) => state.currentUser?.role,
    isTutor: (state) => state.currentUser?.role === "tutor",
    isStudent: (state) => state.currentUser?.role === "student",
    isAdmin: (state) => state.currentUser?.role === "admin",
  },

  actions: {
    setSelectedRole(role: UserRole) {
      this.selectedRole = role;
    },

    login(userData: User) {
      this.currentUser = userData;
      this.isAuthenticated = true;
      localStorageService.setUser(userData);
      localStorageService.setAuthenticated(true);
    },

    logout() {
      this.currentUser = null;
      this.isAuthenticated = false;
      this.selectedRole = null;
      localStorageService.clearUserData();
    },

    initializeFromStorage() {
      const storedUser = localStorageService.getUser();
      const isAuthenticated = localStorageService.getAuthenticated();

      if (storedUser && isAuthenticated) {
        this.currentUser = storedUser;
        this.isAuthenticated = true;
      }
    },
  },
});