import axios from 'axios';
import { useUserStore } from '../store/userStore';

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
  verificationStatus: 'Pending';
  experienceYears: number;
  subjects: Subject[];
  createProfileDto: CreateProfileDto;
}

const API_URL = 'https://localhost:7123/api/tutors';

export const createTutorProfile = async (profileData: TutorProfileData) => {
  try {
    // 📌 Apelează store-ul doar când funcția este folosită
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.post(`${API_URL}/create-tutor`, profileData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    console.error(
      'Eroare la crearea profilului de tutore:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};
