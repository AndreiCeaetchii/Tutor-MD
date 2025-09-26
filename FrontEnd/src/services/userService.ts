import axios from 'axios';
import { useUserStore } from '../store/userStore';

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
  'https://localhost:8085/api/users';

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

export const enableMfa = async () => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    if (!token) throw new Error('Access token not available. Please log in.');

    const response = await axios.put<MfaSetupResponse>(`${API_URL}/enable-mfa`, {}, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || 'Failed to enable MFA. Please try again.',
    );
  }
};

export const verifyMfaSetup = async (code: string) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    if (!token) throw new Error('Access token not available. Please log in.');

    const formattedCode = code.trim().padStart(6, '0').substring(0, 6);

    const response = await axios.put(`${API_URL}/verify-enable-mfa`, { code: formattedCode }, {
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      withCredentials: true,
    });

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
    const store = useUserStore();
    const token = store.accessToken;

    if (!token) throw new Error('Access token not available. Please log in.');

    const response = await axios.put(
      `${API_URL}/disable-mfa`, 
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        withCredentials: true,
      }
    );

    return response.data;
  } catch (error: any) {
    console.error('Disable MFA error:', error);
    throw new Error(
      error.response?.data?.message || 'Failed to disable MFA. Please try again.',
    );
  }
};