// tutorService.ts
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
    profileStore.birthdate = profileData.createProfileDto.birthdate;

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

export const editTutorProfile = async (profileData: any) => {
  try {
    const store = useUserStore();
    const profileStore = useProfileStore();
    const token = store.accessToken;

    // Asigură-te că profileData are structura corectă înainte de a fi trimisă
    const response = await axios.put(`${API_URL}/update-tutor`, profileData, {
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
    console.error(
      'Eroare la editarea profilului de tutore:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};

export const addSubject = async (subjectData: Subject) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.post(`${API_URL}/add-subject`, subjectData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    console.error(
      'Eroare la adăugarea subiectului:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};

export const deleteSubject = async (subjectId: number) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.delete(`${API_URL}/delete-subject/${subjectId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    console.error(
      `Eroare la ștergerea subiectului cu id ${subjectId}:`,
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};

export const updateSubject = async (subjectData: UpdateSubject) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    const response = await axios.put(`${API_URL}/update-subject`, subjectData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    console.error(
      `Eroare la actualizarea subiectului cu id ${subjectData.subjectId}:`,
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};

export const getTutors = async () => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    if (!token) {
      throw new Error('User not authenticated. Access token is missing.');
    }

    const response = await axios.get(`${API_URL}/get-tutors`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error: any) {
    console.error(
      'Eroare la preluarea listei de tutori:',
      error.response ? error.response.data : error.message,
    );
    throw error;
  }
};
