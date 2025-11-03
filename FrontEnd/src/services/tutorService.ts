import axios from 'axios';
import { useUserStore } from '../store/userStore';
import { useProfileStore } from '../store/profileStore';
import { setupTokenRefreshInterceptor } from './tokenService';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';
const tutorAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true,
});

tutorAxios.interceptors.request.use(async (config) => {
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

setupTokenRefreshInterceptor(tutorAxios);

export interface Subject {
  subjectName: string;
  subjectSlug: string;
  pricePerHour: number;
  currency: string;
}

export interface UpdateSubject {
  subjectId: number;
  subjectName: string;
  subjectSlug: string;
  price: number;
  currency: string;
}

export interface CreateProfileDto {
  phone: string;
  username: string;
  firstName: string;
  lastName: string;
  bio: string;
  birthdate: string;
  country: string;
  city: string;
}

export interface TutorProfileData {
  verificationStatus: 'Pending';
  experienceYears: number;
  subjects: Subject[];
  createProfileDto: CreateProfileDto;
  workingLocation: number;
}

const tutorsAPI = `${API_URL}/tutors`;
const favouriteTutorAPI = `${API_URL}/students`;

export const createTutorProfile = async (profileData: TutorProfileData) => {
  try {
    const profileStore = useProfileStore();

    const response = await tutorAxios.post(`${tutorsAPI}/create-tutor`, profileData, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    profileStore.userName = profileData.createProfileDto.username;
    profileStore.birthdate = profileData.createProfileDto.birthdate;

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to create tutor profile.',
    );
  }
};

export const getTutorProfile = async (userId: number) => {
  try {
    const response = await tutorAxios.get(`${tutorsAPI}/get-tutor/${userId}`);
    return response.data;
  } catch (error: any) {
    if (error.response?.status === 404) {
      return null;
    }
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to fetch tutor profile.',
    );
  }
};

export const editTutorProfile = async (profileData: any) => {
  try {
    const profileStore = useProfileStore();

    const response = await tutorAxios.put(`${tutorsAPI}/update-tutor`, profileData, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (response.data.userProfile?.username) {
      profileStore.userName = response.data.userProfile.username;
    }

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to edit tutor profile.',
    );
  }
};

export const addSubject = async (subjectData: Subject) => {
  try {
    const response = await tutorAxios.post(`${tutorsAPI}/add-subject`, subjectData, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || error.message || 'Failed to add subject.');
  }
};

export const deleteSubject = async (subjectId: number) => {
  try {
    const response = await tutorAxios.delete(`${tutorsAPI}/delete-subject/${subjectId}`);
    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
        error.message ||
        `Failed to delete subject with id ${subjectId}.`,
    );
  }
};

export const updateSubject = async (subjectData: UpdateSubject) => {
  try {
    const response = await tutorAxios.put(`${tutorsAPI}/update-subject`, subjectData, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
        error.message ||
        `Failed to update subject with id ${subjectData.subjectId}.`,
    );
  }
};

export const getTutors = async () => {
  try {
    const response = await tutorAxios.get(`${tutorsAPI}/get-tutors`);
    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to fetch tutors list.',
    );
  }
};

export const addToFavourites = async (tutorId: number) => {
  try {
    const response = await tutorAxios.post(
      `${favouriteTutorAPI}/favorites/${tutorId}`,
      {},
      {
        headers: {
          'Content-Type': 'application/json',
        },
      },
    );

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to add tutor to favourites.',
    );
  }
};

export const removeFromFavourites = async (tutorId: number) => {
  try {
    const response = await tutorAxios.delete(`${favouriteTutorAPI}/favorites/${tutorId}`);
    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to remove tutor from favourites.',
    );
  }
};
