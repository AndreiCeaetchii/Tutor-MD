import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';
const studentAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true,
});

studentAxios.interceptors.request.use(async (config) => {
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

export const createStudentProfile = async (profileData: StudentProfileData) => {
  try {
    const response = await studentAxios.post(`/students/create-student`, profileData, {
      headers: {
        'Content-Type': 'application/json',
      },
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
    const response = await studentAxios.put(`/students/update-profile`, profileData, {
      headers: {
        'Content-Type': 'application/json',
      },
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

export const getStudentProfile = async (studentId: string) => {
  try {
    const response = await studentAxios.get(`/students/student-profile/${studentId}`, {
      headers: {
        'Content-Type': 'application/json',
      },
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
