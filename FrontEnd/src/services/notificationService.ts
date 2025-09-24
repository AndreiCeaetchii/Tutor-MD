import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

export interface Notification {
  id: number;
  type: string;
  payload: string;
  status: number;
}
export interface NotificationsResponse {
  totalCount: number;
  notifications: Notification[];
}

export const getUserNotifications = async (): Promise<NotificationsResponse> => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token not available. Please log in.');
    }

    const response = await axios.get<NotificationsResponse>(`${API_URL}/users/notifications`, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        'Eroare la preluarea notificărilor:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Eroare necunoscută:', String(error));
    }
    throw error;
  }
};

export const markNotificationAsRead = async (notificationId: number): Promise<void> => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token not available. Please log in.');
    }

    await axios.put(
      `${API_URL}/users/notification/read/${notificationId}`,
      {},
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        withCredentials: true,
      },
    );
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        'Eroare la marcarea notificării ca citită:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Eroare necunoscută:', String(error));
    }
    throw error;
  }
};
