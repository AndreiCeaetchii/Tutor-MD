import { defineStore } from 'pinia';
import debounce from 'lodash/debounce';

// Interfaces
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
}

// Helper function pentru căutare (mutată în afara store-ului)
function matchesSearchQuery(tutor: Tutor, query: string): boolean {
  return tutor.name.toLowerCase().includes(query) || 
         tutor.location.toLowerCase().includes(query) ||
         tutor.categories.some(category => category.toLowerCase().includes(query)) ||
         tutor.description.toLowerCase().includes(query);
}

export const useTutorStore = defineStore('tutor', {
  state: () => ({
    // Search & Filter State
    searchQuery: '',
    selectedCategory: '',
    educationLevel: '',
    selectedSubjects: [] as string[],
    priceMin: 0,
    priceMax: 10000,
    selectedRatings: [] as number[],
    location: '',
    serviceTypes: [] as string[],
    currentPage: 1,
    tutorsPerPage: 3,

    // Data
    allTutors: [
      {
        id: 1,
        name: 'Mistie Monge',
        location: 'Phoenix, MN',
        hourlyRate: '712.93',
        rating: 5.0,
        reviews: 4448,
        profileImage: 'https://randomuser.me/api/portraits/women/44.jpg',
        description: "Hello there! My name is Mistie and I'm currently a pre-medical student. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
        services: ['My home', "Student's home", 'Online'],
        saved: true,
        categories: ['Mathematics', 'Science']
      },
      {
        id: 2,
        name: 'Jamie Armstrong',
        location: 'New Orleans, OR',
        hourlyRate: '610.29',
        rating: 5.0,
        reviews: 4448,
        profileImage: 'https://randomuser.me/api/portraits/women/25.jpg',
        description: "Hello there! My name is Jamie and I'm currently a pre-medical student. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
        services: ['My home', "Student's home", 'Online'],
        saved: false,
        categories: ['English', 'Literature']
      },
      {
        id: 3,
        name: 'Nathaniel Fletcher',
        location: 'San Francisco, CA',
        hourlyRate: '481.80',
        rating: 4.5,
        reviews: 2356,
        profileImage: 'https://randomuser.me/api/portraits/men/11.jpg',
        description: "Hello there! My name is Nathaniel and I'm currently a computer science graduate. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
        services: ['My home', 'Online'],
        saved: false,
        categories: ['Computer Science', 'Informatics']
      },
      {
        id: 4,
        name: 'Elizabeth Leonard',
        location: 'Tampa, LA',
        hourlyRate: '475.60',
        rating: 4.8,
        reviews: 3189,
        profileImage: 'https://randomuser.me/api/portraits/women/33.jpg',
        description: "Hello there! My name is Elizabeth and I'm currently an English literature major. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
        services: ['My home', "Student's home", 'Online'],
        saved: true,
        categories: ['English', 'Social studies']
      },
      {
        id: 5,
        name: 'Bernard Ketler',
        location: 'Austin, AZ',
        hourlyRate: '505.80',
        rating: 4.2,
        reviews: 1246,
        profileImage: 'https://randomuser.me/api/portraits/men/45.jpg',
        description: "Hello there! My name is Bernard and I'm currently a mathematics professor. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
        services: ["Student's home", 'Online'],
        saved: false,
        categories: ['Mathematics', 'Physics']
      },
      {
        id: 6,
        name: 'Maria Lopez',
        location: 'Denver, CO',
        hourlyRate: '550.25',
        rating: 4.9,
        reviews: 2789,
        profileImage: 'https://randomuser.me/api/portraits/women/28.jpg',
        description: "I am a passionate educator with 10 years of experience. I specialize in teaching Biology and Chemistry to students of all ages.",
        services: ['My home', "Student's home", 'Online'],
        saved: false,
        categories: ['Biology', 'Chemistry', 'Science']
      },
      {
        id: 7,
        name: 'Robert Chen',
        location: 'Seattle, WA',
        hourlyRate: '625.75',
        rating: 4.7,
        reviews: 1856,
        profileImage: 'https://randomuser.me/api/portraits/men/32.jpg',
        description: "Mathematics and Physics tutor with a PhD in Theoretical Physics. I make complex concepts easy to understand.",
        services: ['Online'],
        saved: false,
        categories: ['Mathematics', 'Physics']
      }
    ] as Tutor[],
    
    tutors: [] as Tutor[],
    loading: false,
    error: null as string | null
  }),

  getters: {
    filteredTutors(): Tutor[] {
      // Dacă nu sunt tutori, returnează array gol
      if (!this.tutors || this.tutors.length === 0) {
        return [];
      }
      
      return this.tutors.filter(tutor => {
        // Apply search query filter (global search)
        const query = this.searchQuery.toLowerCase();
        if (query && !matchesSearchQuery(tutor, query)) {
          return false;
        }
        
        // Apply category filter
        if (this.selectedCategory && !tutor.categories.includes(this.selectedCategory)) {
          return false;
        }
        
        // Apply subject filters
        if (this.selectedSubjects.length > 0 && !this.selectedSubjects.some(subject => 
          tutor.categories.includes(subject))) {
          return false;
        }
        
        // Apply price range filter
        const price = parseFloat(tutor.hourlyRate);
        if ((this.priceMin > 0 && price < this.priceMin) || 
            (this.priceMax > 0 && price > this.priceMax)) {
          return false;
        }
        
        // Apply rating filter
        if (this.selectedRatings.length > 0 && !this.selectedRatings.includes(Math.floor(tutor.rating))) {
          return false;
        }
        
        // Apply location filter
        if (this.location && !tutor.location.toLowerCase().includes(this.location.toLowerCase())) {
          return false;
        }
        
        // Apply service type filter
        if (this.serviceTypes.length > 0 && !this.serviceTypes.some(service => 
          tutor.services.includes(service))) {
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
    
    // Getter pentru verificarea dacă sunt filtre aplicate
    hasActiveFilters(): boolean {
      return this.searchQuery !== '' || 
             this.selectedCategory !== '' ||
             this.educationLevel !== '' ||
             this.selectedSubjects.length > 0 ||
             this.priceMin > 0 ||
             this.priceMax < 10000 ||
             this.selectedRatings.length > 0 ||
             this.location !== '' ||
             this.serviceTypes.length > 0;
    }
  },

  actions: {
    // Apply all filters
    applyFilters() {
      // Aici vei face un API call pentru date reale
      // Pentru moment, folosim datele mock
      this.tutors = [...this.allTutors];
    },
    
    // Search with debounce
    debouncedSearch: debounce(function(this: any) {
      this.currentPage = 1; // Reset to first page on new search
      this.applyFilters();
    }, 300),
    
    // Reset all filters
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
      this.applyFilters();
    },
    
    // Toggle saved status
    toggleSavedStatus(tutorId: number, isSaved: boolean) {
      const tutorIndex = this.tutors.findIndex(t => t.id === tutorId);
      if (tutorIndex !== -1) {
        this.tutors[tutorIndex].saved = isSaved;
      }
    },
    
    // Change page
    changePage(page: number) {
      if (page >= 1 && page <= this.totalPages) {
        this.currentPage = page;
      }
    },
    
    // Initialize store
    initialize() {
      this.applyFilters();
    }
  }
});
