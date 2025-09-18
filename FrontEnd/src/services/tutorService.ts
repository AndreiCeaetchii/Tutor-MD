import axios from 'axios';
import { useUserStore } from '../store/userStore';
import { useProfileStore } from '../store/profileStore';

export interface Subject {
  subjectName: string;
  subjectSlug: string;
  pricePerHour: number;
  currency: 'mdl';
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
  verificationStatus: 'verified';
  experienceYears: number;
  subjects: Subject[];
  createProfileDto: CreateProfileDto;
}

const API_URL = 'https://localhost:7123/api/tutors';

export const createTutorProfile = async (profileData: TutorProfileData) => {
  try {
    const store = useUserStore();
    const profileStore = useProfileStore();
    const token = store.accessToken;

    const response = await axios.post(`${API_URL}/create-tutor`, profileData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    profileStore.userName = profileData.createProfileDto.username;

    return response.data;
  } catch (error: any) {
    console.error(
      'Eroare la crearea profilului de tutore:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};

export const getTutorProfile = async (userId: number) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.get(`${API_URL}/get-tutor/${userId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    if (error.response && error.response.status === 404) {
      // Profilul nu există
      return null;
    }
    console.error('Eroare la preluarea profilului tutorului:', error.message);
    throw error;
  }
};
