import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

const tutorsWithNoAvailability: number[] = [];

const hasTutorWithNoAvailability = (tutorId: number): boolean =>
  tutorsWithNoAvailability.includes(tutorId);

const addTutorWithNoAvailability = (tutorId: number): void => {
  if (!tutorsWithNoAvailability.includes(tutorId)) {
    tutorsWithNoAvailability.push(tutorId);
  }
};

const removeTutorWithNoAvailability = (tutorId: number): void => {
  const index = tutorsWithNoAvailability.indexOf(tutorId);
  if (index !== -1) tutorsWithNoAvailability.splice(index, 1);
};

const requestStorageAccess = async () => {
  try {
    // Request storage access if supported
    if (document.requestStorageAccess) {
      await document.requestStorageAccess();
      console.log('Storage access granted');
    }
  } catch (error) {
    console.error('Storage access request failed:', error);
  }
};

const availabilityAxios = axios.create({
  baseURL: `${API_URL}/tutors`,
  withCredentials: true,
});

availabilityAxios.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 400 && error.response.data) {
      const errorData = error.response.data;
      const errorMessage = Array.isArray(errorData) ? errorData[0] : String(errorData);

      if (errorMessage.includes('does not have any availability')) {
        return { data: [] };
      }
    }
    return Promise.reject(error);
  },
);

availabilityAxios.interceptors.request.use(async (config) => {
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

export interface AvailabilitySlot {
  id?: number;
  date: string;
  startTime: string;
  endTime: string;
  activeStatus: boolean;
}

export const getTutorAvailability = async (tutorId: number) => {
  if (hasTutorWithNoAvailability(tutorId)) return [];

  try {
    const response = await availabilityAxios.get(`/availability/${tutorId}`);

    if (hasTutorWithNoAvailability(tutorId)) {
      removeTutorWithNoAvailability(tutorId);
    }

    return response.data;
  } catch (error: any) {
    if (error.response?.status === 400 && error.response.data) {
      const errorData = error.response.data;
      const errorMessage = Array.isArray(errorData) ? errorData[0] : String(errorData);

      if (errorMessage.includes('does not have any availability')) {
        addTutorWithNoAvailability(tutorId);
        return [];
      }
    }
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to fetch tutor availability.',
    );
  }
};

export const createAvailability = async (slot: AvailabilitySlot) => {
  try {
    await requestStorageAccess();

    const store = useUserStore();
    const tutorId = Number(store.userId);

    const simplifiedSlot = {
      date: slot.date,
      startTime: slot.startTime,
      endTime: slot.endTime,
      activeStatus: slot.activeStatus,
    };

    const response = await availabilityAxios.post(`/availability/create`, simplifiedSlot, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (hasTutorWithNoAvailability(tutorId)) {
      removeTutorWithNoAvailability(tutorId);
    }

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to create availability slot.',
    );
  }
};

export const updateAvailability = async (slot: AvailabilitySlot) => {
  try {
    const response = await availabilityAxios.put(`/availability/update`, slot, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to update availability slot.',
    );
  }
};

export const deleteAvailability = async (id: number) => {
  try {
    const response = await availabilityAxios.delete(`/availability/delete/${id}`);

    return response.data;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || error.message || 'Failed to delete availability slot.',
    );
  }
};
