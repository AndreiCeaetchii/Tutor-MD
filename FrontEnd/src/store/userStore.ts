import { defineStore } from 'pinia';

export type UserRole = 'tutor' | 'student' | 'admin';

interface UserState {
  accessToken: string | null;
  userId: string | null;
  role: UserRole | null;
  email: string | null;
}

// @ts-ignore
export const useUserStore = defineStore('user', {
  state: (): UserState => ({
    accessToken: null,
    userId: null,
    role: null,
    email: null,
  }),
  getters: {
    userRole: (state) => state.role,
    isAuthenticated: (state) => !!state.accessToken,
  },
  actions: {
    setUser(token: string, id: string, role: UserRole, email: string) {

      this.accessToken = token;
      this.userId = id;
      this.role = role;
      this.email = email;
    },
    clearUser() {
      this.accessToken = null;
      this.userId = null;
      this.role = null;
      this.email = null;
    },

    updateRole(newRole: UserRole) {
      this.role = newRole;
    },
  },
  persist: true,
});
