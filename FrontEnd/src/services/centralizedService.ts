import axios from 'axios';
import type { AxiosInstance } from 'axios';
import { useUserStore } from '../store/userStore';
import { fetchCsrfToken, getCsrfTokenFromCookie } from './csrfService';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'https://localhost:8085/api';

export const createApiClient = (baseURL?: string): AxiosInstance => {
  const apiClient = axios.create({
    baseURL: baseURL || API_URL,
    withCredentials: true,
  });

  apiClient.interceptors.request.use(
  async (config) => {
    const store = useUserStore();
    const token = store.accessToken;

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    let csrfToken = store.csrfToken;

    if (!csrfToken) {
      csrfToken = getCsrfTokenFromCookie();
      if (csrfToken) {
        store.setCsrfToken(csrfToken);
      }
    }

    if (!csrfToken && token) {
      try {
        csrfToken = await fetchCsrfToken();
      } catch (error) {
        console.error('Failed to fetch CSRF token', error);
      }
    }

    if (csrfToken) {
      config.headers['X-CSRF-TOKEN'] = csrfToken;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

  return apiClient;
};

export default createApiClient();
