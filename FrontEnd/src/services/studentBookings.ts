import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL = 'https://localhost:7123/api';

const bookingAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true
});

export interface BookingRequest {
  tutorUserId: number;
  subjectId: number;
  availabilityRuleId: number;
  description: string;
  status: string;
}

export const createBooking = async (bookingData: BookingRequest) => {
  try {
    const store = useUserStore();
    const token = store.accessToken;

    console.log('Sending booking request:', bookingData);
    
    // API call to create a booking
    const response = await bookingAxios.post('/students/booking/create', bookingData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      }
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

export const formatTimeForBooking = (date: string, time: string): string => {
  const [day, month, year] = date.split('.');
  return `${year}-${month}-${day}T${time}:00Z`;
};

export const fetchTutorSubjects = async (tutorId: number) => {
  try {
    const response = await axios.get(`${API_URL}/tutors/${tutorId}/subjects`);
    return response.data.map((subject: any) => ({
      name: subject.name,
      subjectId: subject.id
    }));
  } catch (error) {
    console.error('Error fetching tutor subjects:', error);
    return [];
  }
};
