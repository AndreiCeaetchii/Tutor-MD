// userService.ts
import axios from 'axios';
import { useUserStore } from '../store/userStore';

export interface UploadPhotoResponse {
  message: string;
  photoUrl: string;
}

const API_URL = 'https://localhost:7123/api/users';

export const uploadProfilePhoto = async (photoFile: File) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

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
    console.error(
      'Eroare la încărcarea fotografiei de profil:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};

export const deleteProfilePhoto = async () => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.delete(`${API_URL}/delete-photo`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    console.error(
      'Eroare la ștergerea fotografiei de profil:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};
