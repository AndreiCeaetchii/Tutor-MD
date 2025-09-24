import { defineStore } from 'pinia';
import defaultProfileImage from '../assets/DefaultImg.png';

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

    clearProfile() {
      this.grade = 0;
      this.class = 0;
      this.userProfile = {
        phone: '',
        // username: this.userProfile.username,
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
    },
  },

  persist: true,
});
