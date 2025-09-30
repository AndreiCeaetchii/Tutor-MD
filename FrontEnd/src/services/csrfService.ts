import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

export const fetchCsrfToken = async (): Promise<string | null> => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    if (!token) {
      console.error('No access token available');
      return null;
    }

    const response = await axios.get(`${API_URL}/users/csrf-token`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });
        console.log('document.cookie (accessible cookies):', document.cookie);

    if (response.data && response.data.csrfToken) {
      const csrfToken = response.data.csrfToken;
      store.setCsrfToken(csrfToken);
      return csrfToken;
    }

    return null;
  } catch (error) {
    console.error('Failed to fetch CSRF token:', error);
    return null;
  }
};

// Helper to extract CSRF token from cookie
export const getCsrfTokenFromCookie = (): string | null => {
  const cookieName = '.AspNetCore.Antiforgery.En7hbRc_cP8';
  const cookies = document.cookie.split(';');
  for (const cookie of cookies) {
    const [name, value] = cookie.trim().split('=');
    if (name === cookieName) {
      return value;
    }
  }
  return null;
};
