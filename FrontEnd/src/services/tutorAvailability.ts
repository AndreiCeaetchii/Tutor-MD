import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL = 'https://localhost:7123/api/tutors';

const tutorsWithNoAvailability: number[] = [];

const hasTutorWithNoAvailability = (tutorId: number): boolean => {
  return tutorsWithNoAvailability.includes(tutorId);
};

const addTutorWithNoAvailability = (tutorId: number): void => {
  if (!tutorsWithNoAvailability.includes(tutorId)) {
    tutorsWithNoAvailability.push(tutorId);
  }
};

const removeTutorWithNoAvailability = (tutorId: number): void => {
  const index = tutorsWithNoAvailability.indexOf(tutorId);
  if (index !== -1) {
    tutorsWithNoAvailability.splice(index, 1);
  }
};

const availabilityAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true
});

availabilityAxios.interceptors.response.use(
  response => response,
  error => {
    if (error.response && 
        error.response.status === 400 && 
        error.response.data) {
      
      const errorData = error.response.data;
      const errorMessage = Array.isArray(errorData) ? errorData[0] : String(errorData);
      
      if (errorMessage.includes("does not have any availability")) {
        console.log("Tutor has no availability yet, returning empty array");
        return { data: [] };
      }
    }
    
    return Promise.reject(error);
  }
);

export interface AvailabilitySlot {
  id?: number;
  date: string;
  startTime: string;
  endTime: string;
  activeStatus: boolean;
}

export const getTutorAvailability = async (tutorId: number) => {
  if (hasTutorWithNoAvailability(tutorId)) {
    console.log(`Tutor ${tutorId} is known to have no availability, returning empty array without API call`);
    return [];
  }
  
  try {
    const store = useUserStore();
    const token = store.accessToken;

    const response = await availabilityAxios.get(`/availability/${tutorId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      }
    });

    if (hasTutorWithNoAvailability(tutorId)) {
      removeTutorWithNoAvailability(tutorId);
    }
    
    return response.data;
  } catch (error: any) {
    if (error.response && error.response.status === 400 && error.response.data) {
      const errorData = error.response.data;
      const errorMessage = Array.isArray(errorData) ? errorData[0] : String(errorData);
      
      if (errorMessage.includes("does not have any availability")) {
        addTutorWithNoAvailability(tutorId);
        return [];
      }
    }
    
    throw error;
  }
};

export const createAvailability = async (slot: AvailabilitySlot) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;
    const tutorId = Number(store.userId);
    
    const simplifiedSlot = {
      date: slot.date,
      startTime: slot.startTime,
      endTime: slot.endTime,
      activeStatus: slot.activeStatus
    };

    const response = await availabilityAxios.post(`/availability/create`, simplifiedSlot, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      }
    });

    if (hasTutorWithNoAvailability(tutorId)) {
      removeTutorWithNoAvailability(tutorId);
    }
    
    return response.data;
  } catch (error: any) {
    throw error;
  }
};

export const updateAvailability = async (slot: AvailabilitySlot) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    const response = await availabilityAxios.put(`/availability/update`, slot, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      }
    });

    return response.data;
  } catch (error: any) {
    throw error;
  }
};

export const deleteAvailability = async (id: number) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    const response = await availabilityAxios.delete(`/availability/delete/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      }
    });

    return response.data;
  } catch (error: any) {
    throw error;
  }
};
