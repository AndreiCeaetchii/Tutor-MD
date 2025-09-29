import { defineStore } from 'pinia';
import { useNotificationStore } from './notificationStore';

export type UserRole = 'tutor' | 'student' | 'admin';

interface UserState {
  accessToken: string | null;
  userId: string | null;
  role: UserRole | null;
  email: string | null;
  hasMfa: boolean;
  csrfToken: string | null;
}

// @ts-ignore
export const useUserStore = defineStore('user', {
  state: (): UserState => ({
    accessToken: null,
    userId: null,
    role: null,
    email: null,
    hasMfa: false,
    csrfToken: null
  }),
  getters: {
    userRole: (state) => state.role,
    isAuthenticated: (state) => !!state.accessToken,
  },
  actions: {
    setUser(token: string, id: string, role: UserRole, email: string, hasMfa: boolean = false) {

      this.accessToken = token;
      this.userId = id;
      this.role = role;
      this.email = email;
      this.hasMfa = hasMfa;
    },

    setCsrfToken(token: string) {
      this.csrfToken = token;
    },

    clearUser() {
      this.accessToken = null;
      this.userId = null;
      this.role = null;
      this.email = null;
      const notificationStore = useNotificationStore();
      notificationStore.notifications = [];
      notificationStore.locallyReadIds = [];
      notificationStore.loading = false;
      notificationStore.error = null;
      notificationStore.lastFetchTime = 0;

      this.csrfToken = null;

    },

    updateRole(newRole: UserRole) {
      this.role = newRole;
    },

    updateUserMfaStatus(status: boolean) {
    this.hasMfa = status;
  },
  },
  persist: true,
});
