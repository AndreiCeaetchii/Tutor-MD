import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'https://localhost:8085/api';

const bookingAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true,
});

bookingAxios.interceptors.request.use(async (config) => {
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

export interface BookingRequest {
  tutorUserId: number;
  subjectId: number;
  availabilityRuleId: number;
  description: string;
  status: string;
}

export interface StudentBooking {
  id: number;
  tutorUserId: number;
  tutorName: string;
  studentUserId: number;
  subjectName: string;
  price: number;
  date: string;
  startTime: string;
  endTime: string;
  description: string;
  status: number;
  tutorPhoto: string | null;
}

export const createBooking = async (bookingData: BookingRequest) => {
  try {
    const response = await bookingAxios.post('/students/booking/create', bookingData, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error: any) {
    console.error('Booking error response:', error.response?.data);
    if (error.response && error.response.data) {
      throw new Error(error.response.data);
    }
    throw error;
  }
};

export const getStudentBookings = async (): Promise<StudentBooking[]> => {
  try {
    const response = await bookingAxios.get(`/students/bookings`);
    return response.data;
  } catch (error: any) {
    console.error('Error fetching student bookings:', error.response?.data || error);
    throw new Error(error.response?.data || 'Failed to fetch student bookings');
  }
};

export const cancelStudentBooking = async (bookingId: number) => {
  try {
    const response = await bookingAxios.put(`/students/booking/update/${bookingId}`, 2, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error: any) {
    console.error('Error canceling booking:', error.response?.data || error);
    throw new Error(error.response?.data || 'Failed to cancel booking');
  }
};

export const formatTimeForBooking = (date: string, time: string): string => {
  const [day, month, year] = date.split('.');
  return `${year}-${month}-${day}T${time}:00Z`;
};

export const fetchTutorSubjects = async (tutorId: number) => {
  try {
    const response = await bookingAxios.get(`/tutors/${tutorId}/subjects`);
    return response.data.map((subject: any) => ({
      name: subject.name,
      subjectId: subject.id,
    }));
  } catch (error) {
    console.error('Error fetching tutor subjects:', error);
    return [];
  }
};

export const addBookingToGoogleCalendar = async (
  bookingId: number,
  googleAccessToken: string
): Promise<any> => {
  try {
    const response = await bookingAxios.post(`/students/booking/add-calendar/${bookingId}`, 
      { accessToken: googleAccessToken },
      {
        headers: {
          'Content-Type': 'application/json',
        },
      }
    );
    return response.data;
  } catch (error: any) {
    console.error('Error adding booking to Google Calendar:', error.response?.data || error);
    throw new Error(error.response?.data || 'Failed to add booking to Google Calendar');
  }
};