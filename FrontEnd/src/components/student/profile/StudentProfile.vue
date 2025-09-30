<script setup lang="ts">
import { ref, watch, reactive, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useStudentProfileStore } from '../../../store/studentProfileStore.ts';
import { updateStudentProfile, getStudentProfile } from '../../../services/studentService.ts';
import ProfilePhotoUpload from '../../../components/tutor/Profile/ProfileImageUploader.vue';
import defaultProfileImage from '../../../assets/DefaultImg.png';
import { useRouter } from 'vue-router';
import { useUserStore } from '../../../store/userStore.ts';
  

const studentStore = useStudentProfileStore();

const { grade, class: studentClass, userProfile, photo } = storeToRefs(studentStore);

const isEditing = ref(false);
const showSuccess = ref(false);

const router = useRouter();

const profile = reactive({
  firstName: userProfile.value?.firstName || '',
  lastName: userProfile.value?.lastName || '',
  phone: userProfile.value?.phone || '',
  bio: userProfile.value?.bio || '',
  city: userProfile.value?.city || '',
  country: userProfile.value?.country || '',
  grade: grade.value || 0,
  class: studentClass.value || 0,
  profileImage: photo.value || defaultProfileImage,
  username: userProfile.value?.username || '',
});

let originalProfile: any = null;

const initializeProfile = async () => {
  if (!userProfile.value?.firstName || !userProfile.value?.birthdate) {
    try {
      const userIdFromUrl = router.currentRoute.value.params.id ? Number(router.currentRoute.value.params.id) : Number(useUserStore().userId);
      const serverProfile = await getStudentProfile(String(userIdFromUrl));

      studentStore.updateUserProfile(serverProfile.userProfile);
      studentStore.updateGradeAndClass(serverProfile.grade, serverProfile.class);
      studentStore.setPhoto(serverProfile.photo.url || '');

      syncProfileWithStore();
    } catch (error) {
      console.error('Failed to load student profile from server:', error);
    }
  } else {
    syncProfileWithStore();
  }
};

const syncProfileWithStore = () => {
  profile.firstName = userProfile.value?.firstName || '';
  profile.lastName = userProfile.value?.lastName || '';
  profile.phone = userProfile.value?.phone || '';
  profile.bio = userProfile.value?.bio || '';
  profile.city = userProfile.value?.city || '';
  profile.country = userProfile.value?.country || '';
  profile.grade = grade.value || 0;
  profile.class = studentClass.value || 0;
  profile.username = userProfile.value?.username || '';
  profile.profileImage = photo.value || defaultProfileImage;
};

  onMounted(initializeProfile);

  const handleSave = async () => {
  // Format the birthdate properly for the backend
  let formattedBirthdate = userProfile.value.birthdate;
  
  // If the birthdate is not in the correct format, format it
  if (formattedBirthdate && !formattedBirthdate.includes('T00:00:00')) {
    // Ensure we have a valid date format before processing
    const parsedDate = new Date(formattedBirthdate);
    if (!isNaN(parsedDate.getTime())) {
      const yyyy = parsedDate.getFullYear();
      const mm = String(parsedDate.getMonth() + 1).padStart(2, '0');
      const dd = String(parsedDate.getDate()).padStart(2, '0');
      formattedBirthdate = `${yyyy}-${mm}-${dd}T00:00:00`;
    }
  }
    const payload = {
      username: profile.username,
      phone: profile.phone,
      firstName: profile.firstName || '',
      lastName: profile.lastName || '',
      bio: profile.bio,
      birthdate: formattedBirthdate,
      grade: profile.grade,
      class: profile.class,
      city: profile.city,
      country: profile.country,
    };

    try {
      const response = await updateStudentProfile(payload);

      studentStore.updateUserProfile({
        firstName: profile.firstName || '',
        lastName: profile.lastName || '',
        phone: profile.phone,
        bio: profile.bio,
        birthdate: formattedBirthdate,
        city: profile.city,
        country: profile.country,
        username: profile.username,
      });
      studentStore.updateGradeAndClass(profile.grade, profile.class);
      studentStore.setPhoto(response.photo?.url || defaultProfileImage); 

      profile.profileImage = response.photo?.url || defaultProfileImage;
      isEditing.value = false;
      showSuccess.value = true;
      setTimeout(() => (showSuccess.value = false), 3000);
    } catch (error) {
      console.error('Error updating profile:', error);
    }
  };

  const handleCancel = () => {
    if (originalProfile) Object.assign(profile, originalProfile);
    isEditing.value = false;
  };

  watch(isEditing, (val) => {
    if (val) {
      originalProfile = { ...profile };
    } else {
      originalProfile = null;
    }
  });

  watch(
    () => [userProfile.value, grade.value, studentClass.value, photo.value],
    () => {
      syncProfileWithStore();
    },
    { deep: true },
  );
</script>

<template>
  <div class="relative w-full max-w-6xl px-4 py-4 mx-auto">
    <!-- Success Notification -->
    <transition 
      enter-active-class="transition duration-300 ease-out" 
      enter-from-class="transform -translate-y-3 opacity-0" 
      leave-active-class="transition duration-200 ease-in" 
      leave-to-class="transform -translate-y-3 opacity-0"
    >
      <div
        v-if="showSuccess"
        class="fixed top-6 right-6 z-50 max-w-[300px] bg-green-50 border-l-4 border-green-500 px-4 py-3 rounded-lg shadow-lg flex items-center gap-2"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="w-5 h-5 text-green-600"
          viewBox="0 0 20 20"
          fill="currentColor"
        >
          <path
            fill-rule="evenodd"
            d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
            clip-rule="evenodd"
          />
        </svg>
        <span class="font-medium text-green-800">Profile updated successfully!</span>
      </div>
    </transition>

    <!-- Main Profile Card -->
    <div class="overflow-hidden bg-white shadow-md rounded-2xl">
      <!-- Profile Header Section -->
      <div class="p-6 border-b border-gray-100 bg-gradient-to-r from-indigo-50 to-blue-50">
        <div class="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
          <!-- Left Side: Photo and Name -->
          <div class="flex flex-col items-center gap-4 sm:flex-row">
            <!-- Profile Photo -->
            <div class="relative flex-shrink-0">
              <div class="w-32 h-32 overflow-hidden border-4 border-white rounded-full shadow-md">
                <ProfilePhotoUpload
                  v-if="isEditing"
                  v-model="profile.profileImage"
                  class="absolute w-24 h-24 -top-1 -left-1"
                />
                <img
                  v-else
                  :src="profile.profileImage"
                  alt="Profile"
                  class="object-cover w-full h-full"
                />
              </div>

              <div
                v-if="isEditing"
                class="absolute right-0 p-1 bg-white rounded-full shadow-sm bottom-1"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="w-6 h-6 text-purple-600"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path d="M17.414 2.586a2 2 0 00-2.828 0L7 10.172V13h2.828l7.586-7.586a2 2 0 000-2.828z" />
                  <path fill-rule="evenodd" d="M2 6a2 2 0 012-2h4a1 1 0 010 2H4v10h10v-4a1 1 0 112 0v4a2 2 0 01-2 2H4a2 2 0 01-2-2V6z" clip-rule="evenodd" />
                </svg>
              </div>
            </div>
            
            <!-- Name and Academic Info -->
            <div class="text-center sm:text-left">
              <h1 class="text-2xl font-bold text-gray-800">{{ profile.firstName }} {{ profile.lastName }}</h1>
              
              <div v-if="!isEditing" class="flex flex-wrap gap-2 mt-2">
                <span class="px-3 py-1 text-sm font-semibold text-indigo-700 bg-indigo-100 rounded-full">
                  Grade: {{ profile.class }}
                </span>
                <span class="px-3 py-1 text-sm font-semibold text-blue-700 bg-blue-100 rounded-full">
                  GPA: {{ profile.grade }}
                </span>
              </div>
            </div>
          </div>
          
          <!-- Right Side: Action Buttons -->
          <div class="flex gap-3">
            <!-- Editing Mode Buttons -->
            <div v-if="isEditing" class="flex gap-2">
              <button
                @click="handleSave"
                class="flex items-center justify-center gap-1 px-6 py-2 text-sm font-medium text-white transition-colors bg-purple-600 rounded-full hover:bg-purple-700"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
                </svg>
                Save
              </button>
              
              <button
                @click="handleCancel"
                class="flex items-center justify-center gap-1 px-6 py-2 text-sm font-medium text-gray-600 transition-colors border border-gray-300 rounded-full hover:bg-gray-50"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
                </svg>
                Cancel
              </button>
            </div>
            
            <!-- View Mode Buttons -->
            <template v-else>
              <button
                @click="isEditing = true"
                class="flex items-center justify-center gap-1 px-6 py-2 text-sm font-medium text-white transition-colors bg-purple-600 rounded-full hover:bg-purple-700"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" viewBox="0 0 20 20" fill="currentColor">
                  <path d="M17.414 2.586a2 2 0 00-2.828 0L7 10.172V13h2.828l7.586-7.586a2 2 0 000-2.828z" />
                  <path fill-rule="evenodd" d="M2 6a2 2 0 012-2h4a1 1 0 010 2H4v10h10v-4a1 1 0 112 0v4a2 2 0 01-2 2H4a2 2 0 01-2-2V6z" clip-rule="evenodd" />
                </svg>
                Edit Profile
              </button>
              
              <button
                @click="router.push('/mfa-setup')"
                class="flex items-center justify-center gap-1 px-6 py-2 text-sm font-medium text-white transition-colors bg-indigo-600 rounded-full hover:bg-indigo-700"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M5 9V7a5 5 0 0110 0v2a2 2 0 012 2v5a2 2 0 01-2 2H5a2 2 0 01-2-2v-5a2 2 0 012-2zm8-2v2H7V7a3 3 0 016 0z" clip-rule="evenodd" />
                </svg>
                Enable MFA
              </button>
            </template>
          </div>
        </div>
      </div>

      <!-- Profile Content -->
      <div class="p-5">
        <!-- Personal Information Section -->
        <div class="mb-6">
          <h2 class="mb-4 text-lg font-medium text-gray-700">Personal Information</h2>
          
          <div class="grid grid-cols-1 gap-5 sm:grid-cols-2">
            <template v-if="isEditing">
              <!-- Username Field -->
              <div>
                <label class="block mb-1 text-sm text-gray-500">Username</label>
                <div class="border border-gray-200 rounded-lg bg-gray-50">
                  <input v-model="profile.username" type="text" class="w-full px-3 py-2 bg-transparent" />
                </div>
              </div>
              
              <!-- First Name Field -->
              <div>
                <label class="block mb-1 text-sm text-gray-500">First Name</label>
                <div class="border border-gray-200 rounded-lg bg-gray-50">
                  <input v-model="profile.firstName" type="text" class="w-full px-3 py-2 bg-transparent" />
                </div>
              </div>
              
              <!-- Last Name Field -->
              <div>
                <label class="block mb-1 text-sm text-gray-500">Last Name</label>
                <div class="border border-gray-200 rounded-lg bg-gray-50">
                  <input v-model="profile.lastName" type="text" class="w-full px-3 py-2 bg-transparent" />
                </div>
              </div>
            </template>
            <template v-else>
              <!-- Full Name Display -->
              <div class="sm:col-span-2">
                <label class="block mb-1 text-sm text-gray-500">Full Name</label>
                <div class="px-3 py-2 border border-gray-200 rounded-lg bg-gray-50">
                  {{ profile.firstName }} {{ profile.lastName }}
                </div>
              </div>
            </template>

            <!-- Phone Field -->
            <div>
              <label class="block mb-1 text-sm text-gray-500">Phone</label>
              <div class="border border-gray-200 rounded-lg bg-gray-50">
                <input v-if="isEditing" v-model="profile.phone" type="tel" class="w-full px-3 py-2 bg-transparent" />
                <div v-else class="px-3 py-2">{{ profile.phone }}</div>
              </div>
            </div>

            <!-- City & Country Fields -->
            <template v-if="isEditing">
              <!-- City Field (Edit Mode) -->
              <div>
                <label class="block mb-1 text-sm text-gray-500">City</label>
                <div class="border border-gray-200 rounded-lg bg-gray-50">
                  <input v-model="profile.city" type="text" class="w-full px-3 py-2 bg-transparent" />
                </div>
              </div>
              
              <!-- Country Field (Edit Mode) -->
              <div>
                <label class="block mb-1 text-sm text-gray-500">Country</label>
                <div class="border border-gray-200 rounded-lg bg-gray-50">
                  <input v-model="profile.country" type="text" class="w-full px-3 py-2 bg-transparent" />
                </div>
              </div>
            </template>
            
            <!-- Combined Location Field (View Mode) -->
            <div v-if="!isEditing" class="sm:col-span-1">
              <label class="block mb-1 text-sm text-gray-500">Location</label>
              <div class="px-3 py-2 border border-gray-200 rounded-lg bg-gray-50">
                {{ profile.city }}, {{ profile.country }}
              </div>
            </div>
            
            <!-- Edit Mode Academic Fields -->
            <template v-if="isEditing">
              <div>
                <label class="block mb-1 text-sm text-gray-500">Grade</label>
                <div class="border border-gray-200 rounded-lg bg-gray-50">
                  <input v-model="profile.grade" type="text" class="w-full px-3 py-2 bg-transparent" />
                </div>
              </div>
              
              <div>
                <label class="block mb-1 text-sm text-gray-500">Class</label>
                <div class="border border-gray-200 rounded-lg bg-gray-50">
                  <input v-model="profile.class" type="text" class="w-full px-3 py-2 bg-transparent" />
                </div>
              </div>
            </template>
          </div>
        </div>
        
        <!-- Bio Section -->
        <div>
          <h2 class="mb-3 text-lg font-medium text-gray-700">About Me</h2>
          <div>
            <div class="border border-gray-200 rounded-lg bg-gray-50">
              <textarea
                v-if="isEditing"
                v-model="profile.bio"
                class="w-full px-3 py-2 bg-transparent resize-y min-h-[100px]"
                placeholder="Tell us about yourself..."
              ></textarea>
              <div v-else class="px-3 py-2 min-h-[50px] whitespace-pre-line">{{ profile.bio || "No bio provided." }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
