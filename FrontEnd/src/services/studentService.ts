import axios from 'axios';
import { useUserStore } from '../store/userStore';

export interface CreateProfileDto {
  phone: string;
  username: string;
  firstName: string;
  lastName: string;
  bio: string;
  city: string;
  country: string;
  birthdate: string;
}

export interface StudentProfileData {
  grade: number;
  class: number;
  createProfileDto: CreateProfileDto;
}

export interface UpdateProfileDto {
  username: string;
  phone: string;
  firstName: string;
  lastName: string;
  bio: string;
  birthdate: string;
  grade: number;
  class: number;
  city: string;
  country: string;
}

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'https://localhost:8085/api';

export const createStudentProfile = async (profileData: StudentProfileData) => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token not available. Please log in.');
    }

    const response = await axios.post(`${API_URL}/students/create-student`, profileData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    console.error(
      'Error creating student profile:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};

export const updateStudentProfile = async (profileData: UpdateProfileDto) => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token not available. Please log in.');
    }

    const response = await axios.put(`${API_URL}/students/update-profile`, profileData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    console.error(
      'Error updating student profile:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};

export const getStudentProfile = async () => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token not available. Please log in.');
    }

    const response = await axios.get(`${API_URL}/students/student-profile`, {
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
        'Error retrieving student profile:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};
