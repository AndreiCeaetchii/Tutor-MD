import type { User } from "../store/userStore";

export const localStorageService = {

  setUser(userData: User): void {
    localStorage.setItem("user", JSON.stringify(userData));
  },

  getUser(): User | null {
    const userJson = localStorage.getItem("user");
    return userJson ? JSON.parse(userJson) : null;
  },

  setAuthenticated(isAuthenticated: boolean): void {
    localStorage.setItem("isAuthenticated", isAuthenticated.toString());
  },

  getAuthenticated(): boolean {
    return localStorage.getItem("isAuthenticated") === "true";
  },

  clearUserData(): void {
    localStorage.removeItem("user");
    localStorage.removeItem("isAuthenticated");
  }
};