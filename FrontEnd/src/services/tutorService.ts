import axios from 'axios';
import { useUserStore } from '../store/userStore';
import { useProfileStore } from '../store/profileStore';

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

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

const tutorsAPI = `${API_URL}/tutors`;

export const createTutorProfile = async (profileData: TutorProfileData) => {
  try {
    const store = useUserStore();
    const profileStore = useProfileStore();
    const token = store.accessToken;

    const response = await axios.post(`${tutorsAPI}/create-tutor`, profileData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
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
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.get(`${tutorsAPI}/get-tutor/${userId}`, {
      headers: { Authorization: `Bearer ${token}` },
      withCredentials: true,
    });

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
    const store = useUserStore();
    const profileStore = useProfileStore();
    const token = store.accessToken;

    const response = await axios.put(`${tutorsAPI}/update-tutor`, profileData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
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
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.post(`${tutorsAPI}/add-subject`, subjectData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || error.message || 'Failed to add subject.');
  }
};

export const deleteSubject = async (subjectId: number) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.delete(`${tutorsAPI}/delete-subject/${subjectId}`, {
      headers: { Authorization: `Bearer ${token}` },
      withCredentials: true,
    });

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
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.put(`${tutorsAPI}/update-subject`, subjectData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
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
    const store = useUserStore();
    const token = store.accessToken;

    if (!token) {
      throw new Error('User not authenticated. Access token is missing.');
    }

    const response = await axios.get(`${tutorsAPI}/get-tutors`, {
      headers: { Authorization: `Bearer ${token}` },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to fetch tutors list.',
    );
  }
};
