import { createApiClient } from './centralizedService';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'https://localhost:8085/api/users';

const userAxios = createApiClient(API_URL);

export interface UploadPhotoResponse {
  message: string;
  photoUrl: string;
}

export const uploadProfilePhoto = async (photoFile: File) => {
  try {
    const formData = new FormData();
    formData.append('file', photoFile);

    const response = await userAxios.post<UploadPhotoResponse>(`/add-photo`, formData);
    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || 'Failed to upload profile photo. Please try again.',
    );
  }
};

export const deleteProfilePhoto = async () => {
  try {
    const response = await userAxios.delete(`/delete-photo`);
    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || 'Failed to delete profile photo. Please try again.',
    );
  }
};
