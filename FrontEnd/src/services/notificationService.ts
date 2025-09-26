import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'https://localhost:8085/api';

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
        'Error marking notification as read:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};
