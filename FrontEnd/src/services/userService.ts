import axios from 'axios';
import { useUserStore } from '../store/userStore';

export interface UploadPhotoResponse {
  message: string;
  photoUrl: string;
}

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'https://localhost:7123/api/users';

export const uploadProfilePhoto = async (photoFile: File) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    if (!token) throw new Error('Access token not available. Please log in.');

    const formData = new FormData();
    formData.append('file', photoFile);

    const response = await axios.post<UploadPhotoResponse>(`${API_URL}/add-photo`, formData, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || 'Failed to upload profile photo. Please try again.',
    );
  }
};

export const deleteProfilePhoto = async () => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    if (!token) throw new Error('Access token not available. Please log in.');

    const response = await axios.delete(`${API_URL}/delete-photo`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || 'Failed to delete profile photo. Please try again.',
    );
  }
};
