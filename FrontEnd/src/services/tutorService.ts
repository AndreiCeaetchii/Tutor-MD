import axios from 'axios';

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

const API_URL = 'http://localhost:7122/api/tutors';

export const createTutorProfile = async (profileData: TutorProfileData) => {
  try {
    const response = await axios.post(`${API_URL}/create-tutor`, profileData, {
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return response.data;
  } catch (error) {
    console.error(
      'Eroare la crearea profilului de tutore:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};
