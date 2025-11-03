import axios from 'axios';
import { useUserStore } from '../store/userStore';
import { setupTokenRefreshInterceptor } from '../store/tokenService';

export interface UploadPhotoResponse {
  message: string;
  photoUrl: string;
}

export interface MfaSetupResponse {
  qrCodeImage: string;
  secretKey: string;
  recoveryCodes: string[];
}

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api/users';
const userAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true,
});

userAxios.interceptors.request.use(async (config) => {
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

setupTokenRefreshInterceptor(userAxios);

export const uploadProfilePhoto = async (photoFile: File) => {
  try {
    const formData = new FormData();
    formData.append('file', photoFile);

    const response = await userAxios.post<UploadPhotoResponse>(`/users/add-photo`, formData);
    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || 'Failed to upload profile photo. Please try again.',
    );
  }
};

export const deleteProfilePhoto = async () => {
  try {
    const response = await userAxios.delete(`/users/delete-photo`);
    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || 'Failed to delete profile photo. Please try again.',
    );
  }
};

export const enableMfa = async () => {
  try {
    const response = await userAxios.put<MfaSetupResponse>(`/users/enable-mfa`, {});
    return response.data;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || 'Failed to enable MFA. Please try again.');
  }
};

export const verifyMfaSetup = async (code: string) => {
  try {
    const formattedCode = code.trim().padStart(6, '0').substring(0, 6);

    const response = await userAxios.put(
      `/users/verify-enable-mfa`,
      {
        code: formattedCode,
      },
      {
        headers: {
          'Content-Type': 'application/json',
        },
      },
    );

    return response.data;
  } catch (error: any) {
    console.error('MFA verification error:', error);
    if (error.response?.data?.detail) {
      throw new Error(error.response.data.detail);
    } else if (error.response?.data?.message) {
      throw new Error(error.response.data.message);
    } else {
      throw new Error('Failed to verify MFA code. Please try again.');
    }
  }
};

export const disableMfa = async () => {
  try {
    const response = await userAxios.put(`/users/disable-mfa`, {});
    return response.data;
  } catch (error: any) {
    console.error('Disable MFA error:', error);
    throw new Error(error.response?.data?.message || 'Failed to disable MFA. Please try again.');
  }
};
