import axios from 'axios';
import { useUserStore } from '../store/userStore';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

const reviewAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true,
});

reviewAxios.interceptors.request.use(async (config) => {
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
  bookingId: number;
  rating: number;
  description: string;
}

export const getTutorReviews = async (tutorId: number): Promise<TutorReviewsResponse> => {
  try {
    const userStore = useUserStore();
    const token = userStore.accessToken;
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
        'Error fetching tutor reviews:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
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
      console.error('Error creating review:', error.response ? error.response.data : error.message);
      throw new Error(
        error.response?.data?.message || 'An error occurred while creating the review.',
      );
    } else {
      console.error('Unknown error:', String(error));
      throw new Error('An unexpected error occurred.');
    }
  }
};
