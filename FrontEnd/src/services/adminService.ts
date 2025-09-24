import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

export interface TutorSubject {
  subjectId: number;
  subjectName: string;
  subjectSlug: string;
  price: number;
  currency: string;
}

export interface UserProfile {
  phone: string;
  username: string;
  firstName: string;
  lastName: string;
  bio: string;
  city: string;
  country: string;
  birthdate: string;
  isActive: boolean;
  email: string;
}

export interface TutorPhoto {
  id: number;
  publicId: string;
  url: string;
  mimeType: string;
  provider: string;
  width: number;
  height: number;
  bytes: number;
}

export interface TutorProfile {
  userId: number;
  verificationStatus: string;
  experienceYears: number;
  workingLocation: number;
  tutorSubjects: TutorSubject[];
  userProfile: UserProfile;
  photo: TutorPhoto | null;
  rating: number;
  reviewCount: number;
}

export interface StudentProfile {
  userId: number;
  userProfile: UserProfile;
  photo: TutorPhoto | null;
}

export const getTutorsForAdmin = async (): Promise<TutorProfile[]> => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token is not available. Please log in again.');
    }

    const response = await axios.get<TutorProfile[]>(`${API_URL}/admin/tutors`, {
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
        'Error while fetching tutors:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};

export const approveTutor = async (tutorId: number): Promise<TutorProfile> => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token is not available. Please log in again.');
    }

    const response = await axios.put<TutorProfile>(`${API_URL}/tutors/approve-tutor/${tutorId}`, null, {
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
        `Error while approving tutor with ID ${tutorId}:`,
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};


export const declineTutor = async (tutorId: number): Promise<TutorProfile> => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token is not available. Please log in again.');
    }

    const response = await axios.put<TutorProfile>(`${API_URL}/tutors/decline-tutor/${tutorId}`, null, {
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
        `Error while declining tutor with ID ${tutorId}:`,
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};

export const changeUserStatus = async (userId: number, isActive: boolean): Promise<any> => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token is not available. Please log in again.');
    }

    const payload = {
      userId,
      isActive,
    };

    const response = await axios.put(`${API_URL}/users/changestatus`, payload, {
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
        `Error while changing user status for user ID ${userId}:`,
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};


export const getAllStudents = async (): Promise<StudentProfile[]> => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token is not available. Please log in again.');
    }

    const response = await axios.get<StudentProfile[]>(`${API_URL}/students/all-students`, {
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
        'Error while fetching students:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};