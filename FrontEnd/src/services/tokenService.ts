import axios from 'axios';
import type { AxiosError, InternalAxiosRequestConfig, AxiosInstance } from 'axios';
import { useUserStore } from '../store/userStore';
import { useRouter } from 'vue-router';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

interface CustomAxiosRequestConfig extends InternalAxiosRequestConfig {
  _retry?: boolean;
}

interface QueueItem {
  resolve: (value?: unknown) => void;
  reject: (reason?: any) => void;
}

const createRefreshManager = () => {
  let isRefreshing = false;
  let failedQueue: QueueItem[] = [];

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

  const refreshAccessToken = async (): Promise<string | null> => {
    const store = useUserStore();

    try {
      const response = await axios.put(
        `${API_URL}/users/refresh`,
        {},
        {
          withCredentials: true,
          headers: {
            'Content-Type': 'application/json',
          },
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
        const router = useRouter();
        router.push('/login');
      }

      return null;
    }
  };

  const addToQueue = (): Promise<unknown> => {
    return new Promise((resolve, reject) => {
      failedQueue.push({ resolve, reject });
    });
  };

  return {
    isRefreshing,
    setRefreshing: (value: boolean) => {
      isRefreshing = value;
    },
    getRefreshing: () => isRefreshing,
    processQueue,
    refreshAccessToken,
    addToQueue,
  };
};

const refreshManager = createRefreshManager();

export const setupTokenRefreshInterceptor = (axiosInstance: AxiosInstance) => {
  axiosInstance.interceptors.response.use(
    (response) => response,
    async (error: AxiosError) => {
      const originalRequest = error.config as CustomAxiosRequestConfig;

      if (!originalRequest) {
        return Promise.reject(error);
      }

      if (error.response?.status === 401 && !originalRequest._retry) {
        if (refreshManager.getRefreshing()) {
          return refreshManager
            .addToQueue()
            .then((token) => {
              if (!originalRequest.headers) {
                originalRequest.headers = {} as any;
              }
              originalRequest.headers.Authorization = `Bearer ${token}`;
              return axiosInstance(originalRequest);
            })
            .catch((err) => Promise.reject(err));
        }

        originalRequest._retry = true;
        refreshManager.setRefreshing(true);

        try {
          const newToken = await refreshManager.refreshAccessToken();

          if (newToken) {
            refreshManager.processQueue(null, newToken);
            
            if (!originalRequest.headers) {
              originalRequest.headers = {} as any;
            }
            originalRequest.headers.Authorization = `Bearer ${newToken}`;
            
            return axiosInstance(originalRequest);
          } else {
            refreshManager.processQueue(new Error('Token refresh failed'), null);
            return Promise.reject(error);
          }
        } catch (refreshError) {
          refreshManager.processQueue(refreshError, null);
          return Promise.reject(refreshError);
        } finally {
          refreshManager.setRefreshing(false);
        }
      }

      return Promise.reject(error);
    }
  );
};
