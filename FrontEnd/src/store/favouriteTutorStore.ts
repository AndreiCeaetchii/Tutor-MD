import { defineStore } from 'pinia';
import { ref } from 'vue';
import { addToFavourites as apiAddToFavourites, removeFromFavourites as apiRemoveFromFavourites, getTutors } from '../services/tutorService';

interface FavouriteTutor {
  id: number;
  name: string;
  location: string;
  hourlyRate: string;
  rating: number;
  reviews: number;
  profileImage: string;
  description: string;
  services: string[];
  saved: boolean;
  categories: string[];
  workingLocation: number;
}

export const useFavouriteTutorStore = defineStore('favouriteTutor', () => {
  const favouriteTutors = ref<FavouriteTutor[]>([]);
  const favouriteTutorIds = ref<number[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);

  function hasFavourites() {
    return favouriteTutors.value.length > 0;
  }

  async function fetchFavouriteTutors() {
    loading.value = true;
    error.value = null;
    
    try {
      // Get all tutors and filter by favorites
      const allTutors = await getTutors();
      
      favouriteTutors.value = allTutors
        .filter((tutor: any) => favouriteTutorIds.value.includes(tutor.userId))
        .map((item: any) => ({
          id: item.userId,
          name: `${item.userProfile.firstName} ${item.userProfile.lastName}`,
          location: `${item.userProfile.city}, ${item.userProfile.country}`,
          hourlyRate: item.tutorSubjects?.[0]?.price?.toString() ?? '0',
          rating: item.rating ?? 0,
          reviews: item.reviews ?? 0,
          profileImage: item.photo?.url ?? '',
          description: item.userProfile.bio ?? '',
          services: item.tutorSubjects?.map((s: any) => s.subjectName) ?? [],
          workingLocation: item.workingLocation ?? 0,
          saved: true,
          categories: item.tutorSubjects?.map((s: any) => s.subjectName) ?? [],
        }));
    } catch (err: any) {
      error.value = err.message || 'Failed to fetch favourite tutors';
      console.error('fetchFavouriteTutors error:', err);
    } finally {
      loading.value = false;
    }
  }

  async function addToFavourites(tutorId: number) {
    try {
      await apiAddToFavourites(tutorId);
      
      // Add to local state if not already there
      if (!favouriteTutorIds.value.includes(tutorId)) {
        favouriteTutorIds.value.push(tutorId);
      }
      
      await fetchFavouriteTutors();
      return true;
    } catch (err: any) {
      error.value = err.message || 'Failed to add tutor to favourites';
      console.error('addToFavourites error:', err);
      return false;
    }
  }

  async function removeFromFavourites(tutorId: number) {
    try {
      await apiRemoveFromFavourites(tutorId);
      
      favouriteTutorIds.value = favouriteTutorIds.value.filter(id => id !== tutorId);
      favouriteTutors.value = favouriteTutors.value.filter(tutor => tutor.id !== tutorId);
      
      return true;
    } catch (err: any) {
      error.value = err.message || 'Failed to remove tutor from favourites';
      console.error('removeFromFavourites error:', err);
      return false;
    }
  }

  function isFavourite(tutorId: number): boolean {
    return favouriteTutorIds.value.includes(tutorId);
  }

  return {
    favouriteTutors,
    favouriteTutorIds,
    loading,
    error,
    hasFavourites,
    fetchFavouriteTutors,
    addToFavourites,
    removeFromFavourites,
    isFavourite
  };
}, {
  persist: true
});
