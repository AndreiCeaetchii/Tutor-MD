import type { UserRole } from '../store/userStore';

export const localStorageService = {
  // setez rolul
  setRole(role: UserRole): void {
    localStorage.setItem('role', role);
  },

  // iau rolul
  getRole(): UserRole | null {
    const role = localStorage.getItem('role');
    return role as UserRole | null;
  },

  // setez statusul de autentificare
  setAuthenticated(isAuthenticated: boolean): void {
    localStorage.setItem('isAuthenticated', isAuthenticated.toString());
  },

  // verific statusul de autentificare
  getAuthenticated(): boolean {
    return localStorage.getItem('isAuthenticated') === 'true';
  },

  // curăț datele din localStorage
  clearUserData(): void {
    localStorage.removeItem('role');
    localStorage.removeItem('isAuthenticated');
  },
};
