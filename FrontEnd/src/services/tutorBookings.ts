import axios from 'axios';
import { useUserStore } from '../store/userStore';
import { setupTokenRefreshInterceptor } from './tokenService';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

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

setupTokenRefreshInterceptor(bookingAxios);

export interface TutorBooking {
  id: number;
  tutorUserId: number;
  tutorName: string;
  studentUserId: number;
  studentName: string;
  price: number;
  date: string;
  startTime: string;
  endTime: string;
  description: string;
  status: number;
  studentPhoto: string | null;
  tutorPhoto: string | null;
  subject: string;
}

export const getBookingById = async (bookingId: number): Promise<TutorBooking> => {
  try {
    const response = await bookingAxios.get(`/students/booking/${bookingId}`);
    return response.data;
  } catch (error: any) {
    console.error('Error fetching booking details:', error.response?.data || error);
    throw new Error(error.response?.data || 'Failed to fetch booking details');
  }
};

export const getTutorBookings = async (): Promise<TutorBooking[]> => {
  try {
    const response = await bookingAxios.get(`/students/bookings`);
    return response.data;
  } catch (error: any) {
    console.error('Error fetching tutor bookings:', error.response?.data || error);
    throw new Error(error.response?.data || 'Failed to fetch tutor bookings');
  }
};

export const updateBookingStatus = async (
  bookingId: number,
  status: number,
): Promise<TutorBooking> => {
  try {
    const response = await bookingAxios.put(`/students/booking/update/${bookingId}`, status, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error: any) {
    console.error('Error updating booking status:', error.response?.data || error);
    throw new Error(error.response?.data || 'Failed to update booking status');
  }
};

export const addBookingToGoogleCalendar = async (
  bookingId: number,
  googleAccessToken: string,
): Promise<any> => {
  try {
    const response = await bookingAxios.put(
      `/students/booking/add-calendar/${bookingId}`,
      JSON.stringify(googleAccessToken),
      {
        headers: {
          'Content-Type': 'application/json',
        },
      },
    );
    return response.data;
  } catch (error: any) {
    console.error('Error adding booking to Google Calendar:', error.response?.data || error);
    throw new Error(error.response?.data || 'Failed to add booking to Google Calendar');
  }
};
