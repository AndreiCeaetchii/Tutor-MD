import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'https://localhost:8085/api';

const adminAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true,
});

adminAxios.interceptors.request.use(async (config) => {
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
    const response = await adminAxios.get<TutorProfile[]>(`/admin/tutors`, {
      headers: {
        'Content-Type': 'application/json',
      },
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
    const response = await adminAxios.put<TutorProfile>(`/tutors/approve-tutor/${tutorId}`, null, {
      headers: {
        'Content-Type': 'application/json',
      },
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
    const response = await adminAxios.put<TutorProfile>(`/tutors/decline-tutor/${tutorId}`, null, {
      headers: {
        'Content-Type': 'application/json',
      },
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
    const payload = {
      userId,
      isActive,
    };

    const response = await adminAxios.put(`/users/changestatus`, payload, {
      headers: {
        'Content-Type': 'application/json',
      },
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
    const response = await adminAxios.get<StudentProfile[]>(`/students/all-students`, {
      headers: {
        'Content-Type': 'application/json',
      },
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

export const createAdmin = async (userId: number): Promise<TutorProfile> => {
  try {
    const payload = { userId };

    const response = await adminAxios.post<TutorProfile>(`/users/admin/create`, payload, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        `Error while creating tutor with userId ${userId}:`,
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};
