import { defineStore } from 'pinia';

export type UserRole = 'tutor' | 'student' | 'admin';

interface UserState {
  accessToken: string | null;
  userId: string | null;
  role: string | null;
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
    setUser(token: string, id: string, role: string, email: string) {
      const isNew = this.accessToken !== token || this.userId !== id || this.role !== role;

      if (isNew) {
        console.log('[Store Debug] Setting new user:');
        console.log('AccessToken:', token);
        console.log('UserId:', id);
        console.log('Role:', role);
      } else {
        console.log('[Store Debug] Values are the same, nothing new to set.');
      }

      this.accessToken = token;
      this.userId = id;
      this.role = role;
      this.email = email;
    },
    clearUser() {
      console.log('[Store Debug] Clearing user...');
      this.accessToken = null;
      this.userId = null;
      this.role = null;
    },

    updateRole(newRole: UserRole) {
      console.log(`[Store Debug] Updating role from ${this.role} → ${newRole}`);
      this.role = newRole;
    },
  },
  persist: true, // activează persistența automată în localStorage
});
