import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'https://localhost:7123/api';

export interface TutorReview {
  id: number;
  studentUserId: number;
  userName: string;
  rating: number;
  description: string;
  createdAt: string;
}

export interface TutorReviewsResponse {
  reviews: TutorReview[];
  averageRating: number;
}

export interface CreateReviewDto {
  BookingId: number;
  Rating: number;
  Description: string;
}

export const getTutorReviews = async (tutorId: number): Promise<TutorReviewsResponse> => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;
    console.log(token);
    if (!token) {
      throw new Error('Access token not available. Please log in.');
    }

    const response = await axios.get<TutorReviewsResponse>(
      `${API_URL}/students/reviews/${tutorId}`,
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        withCredentials: true,
      },
    );

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        'Eroare la preluarea recenziilor tutorelui:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Eroare necunoscută:', String(error));
    }
    throw error;
  }
};

export const createReview = async (reviewData: CreateReviewDto) => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;

    if (!token) {
      throw new Error('Access token not available. Please log in.');
    }

    const response = await axios.post(`${API_URL}/students/reviews/create`, reviewData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        'Eroare la crearea recenziei:',
        error.response ? error.response.data : error.message,
      );
      throw new Error(error.response?.data?.message || 'A apărut o eroare la crearea recenziei.');
    } else {
      console.error('Eroare necunoscută:', String(error));
      throw new Error('O eroare neașteptată a avut loc.');
    }
  }
};
