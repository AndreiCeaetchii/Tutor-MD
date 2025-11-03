import axios from 'axios';
import type { AxiosInstance } from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

let isRefreshing = false;
let failedQueue: Array<{
  resolve: (value?: unknown) => void;
  reject: (reason?: any) => void;
}> = [];

const processQueue = (error: any = null, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

export const refreshAccessToken = async (): Promise<string | null> => {
  const store = useUserStore();

  try {
    const response = await axios.put(
      `${API_URL}/users/refresh`,
      {},
      { 
        withCredentials: true,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${store.accessToken}`
        }
      }
    );

    const { token } = response.data;
    
    if (!token) {
      throw new Error('No access token in refresh response');
    }

    store.updateAccessToken(token);
    
    return token;
  } catch (error: any) {
    store.clearUser();
    
    if (typeof window !== 'undefined') {
      window.location.href = '/login';
    }
    
    return null;
  }
};

export const setupTokenRefreshInterceptor = (axiosInstance: AxiosInstance) => {
  axiosInstance.interceptors.response.use(
    (response) => response,
    async (error) => {
      const originalRequest = error.config;

      if (error.response?.status === 401 && !originalRequest._retry) {
        if (isRefreshing) {
          return new Promise((resolve, reject) => {
            failedQueue.push({ resolve, reject });
          })
            .then((token) => {
              originalRequest.headers.Authorization = `Bearer ${token}`;
              return axiosInstance(originalRequest);
            })
            .catch((err) => Promise.reject(err));
        }

        originalRequest._retry = true;
        isRefreshing = true;

        try {
          const newToken = await refreshAccessToken();
          
          if (newToken) {
            processQueue(null, newToken);
            originalRequest.headers.Authorization = `Bearer ${newToken}`;
            return axiosInstance(originalRequest);
          } else {
            processQueue(new Error('Token refresh failed'), null);
            return Promise.reject(error);
          }
        } catch (refreshError) {
          processQueue(refreshError, null);
          return Promise.reject(refreshError);
        } finally {
          isRefreshing = false;
        }
      }

      return Promise.reject(error);
    }
  );
};
