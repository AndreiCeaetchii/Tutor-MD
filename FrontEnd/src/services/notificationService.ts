import axios from 'axios';
import { useUserStore } from '../store/userStore';
import { setupTokenRefreshInterceptor } from '../store/tokenService';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

const notificationAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true,
});

notificationAxios.interceptors.request.use(async (config) => {
  const store = useUserStore();
  const token = store.accessToken;
  const csrfToken = store.csrfToken;

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  if (csrfToken) {
    config.headers['X-CSRF-TOKEN'] = csrfToken;
  }

  return config;
});

setupTokenRefreshInterceptor(notificationAxios);

export type Notification = {
  id: number;
  type: string;
  payload: string;
  status: number;
  createdAt: string;
};

export interface NotificationsResponse {
  totalCount: number;
  notifications: Notification[];
}

export const getUserNotifications = async (): Promise<NotificationsResponse> => {
  try {
    const response = await notificationAxios.get<NotificationsResponse>(`/users/notifications`, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        'Error fetching notifications:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};

export const markNotificationAsRead = async (notificationId: number): Promise<void> => {
  try {
    await notificationAxios.put(
      `/users/notification/read/${notificationId}`,
      {},
      {
        headers: {
          'Content-Type': 'application/json',
        },
      },
    );
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        'Error marking notification as read:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};
