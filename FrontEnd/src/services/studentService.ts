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
  birthdate: string; // ISO string: e.g., "1995-05-08T00:00:00"
  grade: number;
  class: number;
  city: string;
  country: string;
}

const API_URL = 'https://localhost:7123/api/students';

export const createStudentProfile = async (profileData: StudentProfileData) => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token not available. Please log in.');
    }

    const response = await axios.post(`${API_URL}/create-student`, profileData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    console.error(
      'Eroare la crearea profilului de student:',
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

    const response = await axios.put(`${API_URL}/update-profile`, profileData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true, // Keeps cookies if needed
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

    const response = await axios.get(`${API_URL}/student-profile`, {
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
        'Eroare la preluarea profilului de student:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Eroare necunoscută:', String(error));
    }
    throw error;
  }
};
