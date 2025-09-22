// store/findTutorStore.ts
import { defineStore } from 'pinia';
import debounce from 'lodash/debounce';
import { getTutors } from '../services/tutorService';

// Tipurile pentru tutor
interface Tutor {
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

// Funcție ajutătoare pentru căutare
function matchesSearchQuery(tutor: Tutor, query: string): boolean {
  return (
    tutor.name.toLowerCase().includes(query) ||
    tutor.location.toLowerCase().includes(query) ||
    tutor.categories.some((category) => category.toLowerCase().includes(query)) ||
    tutor.description.toLowerCase().includes(query)
  );
}

export const useFindTutorStore = defineStore('tutor', {
  state: () => ({
    searchQuery: '',
    selectedCategory: '',
    educationLevel: '',
    selectedSubjects: [] as string[],
    priceMin: 0,
    priceMax: 10000,
    selectedRatings: [] as number[],
    location: '',
    serviceTypes: [] as string[],
    workingLocation: null as number | null,
    currentPage: 1,
    tutorsPerPage: 3,

    tutors: [] as Tutor[],
    loading: false,
    error: null as string | null,
  }),

  getters: {
    filteredTutors(state): Tutor[] {
      if (!state.tutors || state.tutors.length === 0) return [];

      return state.tutors.filter((tutor) => {
        const query = state.searchQuery.toLowerCase();

        if (query && !matchesSearchQuery(tutor, query)) return false;

        if (state.selectedCategory && !tutor.categories.includes(state.selectedCategory))
          return false;

        if (
          state.selectedSubjects.length > 0 &&
          !state.selectedSubjects.some((subject) => tutor.categories.includes(subject))
        )
          return false;

        const price = parseFloat(tutor.hourlyRate);
        if (
          (state.priceMin > 0 && price < state.priceMin) ||
          (state.priceMax > 0 && price > state.priceMax)
        )
          return false;

        if (
          state.selectedRatings.length > 0 &&
          !state.selectedRatings.includes(Math.floor(tutor.rating))
        )
          return false;

        if (state.location && !tutor.location.toLowerCase().includes(state.location.toLowerCase()))
          return false;

        if (
          state.serviceTypes.length > 0 &&
          !state.serviceTypes.some((service) => tutor.services.includes(service))
        )
          return false;

        if (state.workingLocation !== null && tutor.workingLocation !== state.workingLocation) {
          return false;
        }

        return true;
      });
    },

    totalPages(): number {
      return Math.ceil(this.filteredTutors.length / this.tutorsPerPage);
    },

    paginatedTutors(): Tutor[] {
      const start = (this.currentPage - 1) * this.tutorsPerPage;
      const end = start + this.tutorsPerPage;
      return this.filteredTutors.slice(start, end);
    },

    hasActiveFilters(): boolean {
      return (
        this.searchQuery !== '' ||
        this.selectedCategory !== '' ||
        this.educationLevel !== '' ||
        this.selectedSubjects.length > 0 ||
        this.priceMin > 0 ||
        this.priceMax < 10000 ||
        this.selectedRatings.length > 0 ||
        this.location !== '' ||
        this.serviceTypes.length > 0
      );
    },
  },

  actions: {
    debouncedSearch: debounce(function (this: any) {
      this.currentPage = 1;
    }, 300),

    clearFilters() {
      this.searchQuery = '';
      this.selectedCategory = '';
      this.educationLevel = '';
      this.selectedSubjects = [];
      this.priceMin = 0;
      this.priceMax = 10000;
      this.selectedRatings = [];
      this.location = '';
      this.serviceTypes = [];
      this.currentPage = 1;
      this.workingLocation = null;
    },

    toggleSavedStatus(tutorId: number, isSaved: boolean) {
      const tutorIndex = this.tutors.findIndex((t) => t.id === tutorId);
      if (tutorIndex !== -1) {
        this.tutors[tutorIndex].saved = isSaved;
      }
    },

    changePage(page: number) {
      if (page >= 1 && page <= this.totalPages) {
        this.currentPage = page;
      }
    },

    async fetchTutors() {
      this.loading = true;
      this.error = null;

      try {
        const serverTutors = await getTutors();

        this.tutors = serverTutors.map((item: any) => ({
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
          saved: false,
          categories: item.tutorSubjects?.map((s: any) => s.subjectName) ?? [],
        }));
      } catch (err: any) {
        this.error = err.message || 'Eroare necunoscută la încărcarea tutorilor';
        console.error('fetchTutors error:', err);
      } finally {
        this.loading = false;
      }
    },
  },
});
