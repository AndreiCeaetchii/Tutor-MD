import { defineStore } from 'pinia';

interface Subject {
  name: string;
  price: number;
  currency: string;
}

interface ProfileState {
  firstName: string;
  lastName: string;
  bio: string;
  phone: string;
  email: string;
  experience: number;
  age: number;
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
    isEditing: false,
    firstName: 'Maria',
    lastName: 'Popescu',
    bio: 'Sunt un tutor pasionat de matematică și fizică cu peste 8 ani de experiență în predare. Îmi place să ajut studenții să înțeleagă conceptele complexe prin explicații clare și exemple practice.',
    phone: '+40 123 456 789',
    email: 'maria@email.com',
    experience: 8,
    age: 35,
    location: 'Bucharest, Romania',
    profileImage:
      'https://images.unsplash.com/photo-1544005313-94ddf0286df2?q=80&w=1976&auto=format&fit=crop',
    rating: 4.9,
    reviews: 147,
    students: 89,
    languages: ['Română', 'Engleză', 'Franceză'],
    subjects: [
      { name: 'Matematică', price: 50, currency: 'EUR' },
      { name: 'Fizică', price: 45, currency: 'MDL' },
      { name: 'Informatică', price: 60, currency: 'MDL' },
    ],
  }),

  getters: {
    getContactInfo: (state) => `${state.email} | ${state.phone}`,
    getName: (state) => `${state.firstName} ${state.lastName}`,
    getSubjectPrices: (state) =>
      state.subjects
        .map((subject) => `${subject.name}: ${subject.price} ${subject.currency}`)
        .join(', '),
  },

  actions: {
    setField<T extends keyof ProfileState>(field: T, value: ProfileState[T]) {
      this.$patch({ [field]: value });
    },

    addLanguage(language: string) {
      if (!this.languages.includes(language)) {
        this.languages.push(language);
      }
    },

    removeLanguage(language: string) {
      this.languages = this.languages.filter((l) => l !== language);
    },

    addSubject(newSubject: Subject) {
      this.subjects.push(newSubject);
    },

    removeSubject(subjectName: string) {
      this.subjects = this.subjects.filter((s) => s.name !== subjectName);
    },

    toggleEditing() {
      this.isEditing = !this.isEditing;
    },

    setProfileDetails(newDetails: Partial<ProfileState>) {
      Object.assign(this, newDetails);
    },
  },
});
