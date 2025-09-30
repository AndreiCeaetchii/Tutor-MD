import { defineStore } from 'pinia';
import defaultProfileImage from '../assets/DefaultImg.png';
import { useUserStore } from './userStore.ts';
import { getStudentProfile } from '../services/studentService.ts';

interface UserProfile {
  phone: string;
  username: string;
  firstName: string;
  lastName: string;
  bio: string;
  city: string;
  country: string;
  birthdate: string;
}

export interface StudentProfileState {
  grade: number;
  class: number;
  userProfile: UserProfile;
  photo: string | null;
  isEditing: boolean;
  isLoading: boolean;
}

export const useStudentProfileStore = defineStore('studentProfile', {
  state: (): StudentProfileState => ({
    grade: 8,
    class: 11,
    userProfile: {
      phone: '',
      username: '',
      firstName: '',
      lastName: '',
      bio: '',
      city: '',
      country: '',
      birthdate: '',
    },
    photo: defaultProfileImage,
    isEditing: false,
    isLoading: false,
  }),

  getters: {
    fullName: (state) => `${state.userProfile.firstName} ${state.userProfile.lastName}`,
    location: (state) =>
      state.userProfile.city && state.userProfile.country
        ? `${state.userProfile.city}, ${state.userProfile.country}`
        : '',
    contactInfo: (state) => `${state.userProfile.username} | ${state.userProfile.phone}`,
  },

  actions: {
    toggleEditing() {
      this.isEditing = !this.isEditing;
    },

    setPhoto(imageUrl: string | null) {
      this.photo = imageUrl ?? defaultProfileImage;
    },

    updateUserProfile(newData: Partial<UserProfile>) {
      Object.assign(this.userProfile, newData);
    },

    updateGradeAndClass(newGrade: number, newClass: number) {
      this.grade = newGrade;
      this.class = newClass;
    },

    updateUsername(newUsername: string) {
      if (this.userProfile) {
        this.userProfile.username = newUsername;
      }
    },

    async loadProfile() {
      const userStore = useUserStore();
      if (!userStore.userId) return;

      this.isLoading = true;

      try {
        const studentProfile = await getStudentProfile(userStore.userId);

        if (studentProfile && studentProfile.userProfile) {
          this.userProfile.firstName = studentProfile.userProfile.firstName || '';
          this.userProfile.lastName = studentProfile.userProfile.lastName || '';
        }
      } catch (error) {
        console.error('Failed to load student profile.', error);
      } finally {
        this.isLoading = false;
      }
    },

    clearProfile() {
      this.grade = 0;
      this.class = 0;
      this.userProfile = {
        phone: '',
        username: '',
        firstName: '',
        lastName: '',
        bio: '',
        city: '',
        country: '',
        birthdate: '',
      };
      this.photo = defaultProfileImage;
      this.isEditing = false;
      this.isLoading = false;
    },
  },

  persist: true,
});