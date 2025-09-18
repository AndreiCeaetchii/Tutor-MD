import { defineStore } from 'pinia';

interface Subject {
  name: string;
  price: number;
  currency: string;
  subjectId?: number;
  isNew?: boolean;
  isModified?: boolean;
}

export interface ProfileState {
  birthdate?: string;
  userName?: string;
  firstName: string;
  lastName: string;
  bio: string;
  phone: string;
  email: string;
  experience: number;
  age: number;
  country?: string;
  city?: string;
  location: string;
  profileImage: string;
  rating: number;
  reviews: number;
  students: number;
  languages: string[];
  subjects: Subject[];
  isEditing: boolean;
}

export const useProfileStore = defineStore('profile', {
  state: (): ProfileState => ({
    birthdate: '',
    userName: '',
    firstName: '',
    lastName: '',
    bio: '',
    phone: '',
    email: '',
    experience: 0,
    age: 0,
    country: '',
    city: '',
    location: '',
    profileImage: '',
    rating: 0,
    reviews: 0,
    students: 0,
    languages: [],
    subjects: [],
    isEditing: false,
  }),

  getters: {
    getFullLocation: (state) =>
      state.city && state.country ? `${state.city}, ${state.country}` : state.location,
    getContactInfo: (state) => `${state.email} | ${state.phone}`,
    getName: (state) => `${state.firstName} ${state.lastName}`,
    getSubjectPrices: (state) =>
      state.subjects
        .map((subject) => `${subject.name}: ${subject.price} ${subject.currency}`)
        .join(', '),
  },

  actions: {
    toggleEditing() {
      this.isEditing = !this.isEditing;
    },

    setProfileDetails(newDetails: Partial<ProfileState>) {
      const { getFullLocation, getContactInfo, getName, getSubjectPrices, ...stateData } =
        newDetails as any;
      Object.assign(this, stateData);
    },

    clearProfile() {
      this.userName = '';
      this.firstName = '';
      this.lastName = '';
      this.bio = '';
      this.phone = '';
      this.email = '';
      this.experience = 0;
      this.age = 0;
      this.country = '';
      this.city = '';
      this.location = '';
      this.profileImage = '';
      this.rating = 0;
      this.reviews = 0;
      this.students = 0;
      this.languages = [];
      this.subjects = [];
      this.isEditing = false;

      this.$reset();
    },
  },

  persist: true,
});
