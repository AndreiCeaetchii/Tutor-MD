<template>
  <div class="profile-container">
    <transition name="fade">
      <div
        v-if="showSuccess"
        class="flex items-center gap-2 p-3 text-teal-800 border-l-4 border-teal-400 rounded-r-lg shadow-md success-toast bg-teal-50"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="w-5 h-5 text-teal-500"
          viewBox="0 0 20 20"
          fill="currentColor"
        >
          <path
            fill-rule="evenodd"
            d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
            clip-rule="evenodd"
          />
        </svg>
        <span>Profile updated successfully!</span>
      </div>
    </transition>

    <div
      class="p-4 font-sans bg-gradient-to-br from-indigo-50 via-purple-50 to-blue-50 shadow-lg rounded-2xl md:p-8"
    >
      <div class="flex flex-col items-center gap-4 mb-8 profile sm:flex-row sm:justify-between">
        <div class="flex flex-col items-center gap-4 sm:flex-row">
          <div class="flex flex-col items-center gap-4 sm:flex-row">
            <div class="relative w-32 h-32 flex-shrink-0">
              <div class="w-32 h-32 overflow-hidden rounded-full">
                <ProfilePhotoUpload v-if="isEditing" v-model="profile.profileImage" />
                <img
                  v-else
                  :src="profile.profileImage"
                  alt="Profile picture"
                  class="object-cover w-full h-full border-4 border-white rounded-full shadow-lg"
                />
              </div>

              <div
                v-if="isEditing"
                class="absolute bottom-3 right-0 p-1 bg-white rounded-full shadow-md z-10"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="w-6 h-6 text-purple-500"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path
                    d="M17.414 2.586a2 2 0 00-2.828 0L7 10.172V13h2.828l7.586-7.586a2 2 0 000-2.828z"
                  />
                  <path
                    fill-rule="evenodd"
                    d="M2 6a2 2 0 012-2h4a1 1 0 010 2H4v10h10v-4a1 1 0 112 0v4a2 2 0 01-2 2H4a2 2 0 01-2-2V6z"
                    clip-rule="evenodd"
                  />
                </svg>
              </div>
            </div>
            <div class="text-center sm:text-left sm:ml-4">
              <h3 class="text-xl font-semibold text-gray-800">
                {{ profile.firstName }} {{ profile.lastName }}
              </h3>
              <p v-if="!isEditing" class="text-gray-600">
                Class: {{ profile.class }}, Grade: {{ profile.grade }}
              </p>
            </div>
          </div>
        </div>

        <div class="flex gap-2 mt-4 sm:mt-0">
          <button
            v-if="isEditing"
            @click="handleSave"
            class="flex items-center gap-2 px-5 py-2 bg-purple-600 text-white rounded-full font-medium transition-all hover:-translate-y-0.5 hover:shadow-md"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-5 h-5"
              viewBox="0 0 20 20"
              fill="currentColor"
            >
              <path
                fill-rule="evenodd"
                d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                clip-rule="evenodd"
              />
            </svg>
            Save
          </button>
          <button
            v-if="isEditing"
            @click="handleCancel"
            class="flex items-center gap-2 px-5 py-2 font-medium text-gray-600 transition-all border border-gray-300 rounded-full"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-5 h-5"
              viewBox="0 0 20 20"
              fill="currentColor"
            >
              <path
                fill-rule="evenodd"
                d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                clip-rule="evenodd"
              />
            </svg>
            Cancel
          </button>
          <button
            v-else
            @click="isEditing = true"
            class="flex items-center gap-2 px-5 py-2 bg-purple-600 text-white rounded-full font-medium transition-all hover:-translate-y-0.5 hover:shadow-md"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-5 h-5"
              viewBox="0 0 20 20"
              fill="currentColor"
            >
              <path
                d="M17.414 2.586a2 2 0 00-2.828 0L7 10.172V13h2.828l7.586-7.586a2 2 0 000-2.828z"
              />
              <path
                fill-rule="evenodd"
                d="M2 6a2 2 0 012-2h4a1 1 0 010 2H4v10h10v-4a1 1 0 112 0v4a2 2 0 01-2 2H4a2 2 0 01-2-2V6z"
                clip-rule="evenodd"
              />
            </svg>
            Edit Profile
          </button>
        </div>
      </div>

      <div class="space-y-6">
        <div class="grid grid-cols-1 gap-4 md:grid-cols-2 md:gap-6">
          <template v-if="isEditing">
            <div class="field-group">
              <label class="label">Username</label>
              <div class="input-container group">
                <input v-model="profile.username" type="text" class="input-field" />
              </div>
            </div>
            <div class="field-group">
              <label class="label">First Name</label>
              <div class="input-container group">
                <input v-model="profile.firstName" type="text" class="input-field" />
              </div>
            </div>
            <div class="field-group">
              <label class="label">Last Name</label>
              <div class="input-container group">
                <input v-model="profile.lastName" type="text" class="input-field" />
              </div>
            </div>
          </template>
          <template v-else>
            <div class="field-group md:col-span-2">
              <label class="label">Full Name</label>
              <div class="input-container">
                <p class="text-display">{{ profile.firstName }} {{ profile.lastName }}</p>
              </div>
            </div>
          </template>

          <div class="field-group">
            <label class="label">Phone</label>
            <div class="input-container group">
              <input v-if="isEditing" v-model="profile.phone" type="tel" class="input-field" />
              <p v-else class="text-display">{{ profile.phone }}</p>
            </div>
          </div>
          <div class="field-group">
            <label class="label">City</label>
            <div class="input-container group">
              <input v-if="isEditing" v-model="profile.city" type="text" class="input-field" />
              <p v-else class="text-display">{{ profile.city }}</p>
            </div>
          </div>
          <div class="field-group">
            <label class="label">Country</label>
            <div class="input-container group">
              <input v-if="isEditing" v-model="profile.country" type="text" class="input-field" />
              <p v-else class="text-display">{{ profile.country }}</p>
            </div>
          </div>
          <template v-if="isEditing">
            <div class="field-group">
              <label class="label">Grade</label>
              <div class="input-container group">
                <input v-model="profile.grade" type="text" class="input-field" />
              </div>
            </div>
            <div class="field-group">
              <label class="label">Class</label>
              <div class="input-container group">
                <input v-model="profile.class" type="text" class="input-field" />
              </div>
            </div>
          </template>
        </div>
        <div class="field-group">
          <label class="label">Bio</label>
          <div class="input-container group">
            <textarea
              v-if="isEditing"
              v-model="profile.bio"
              class="input-field min-h-[100px] resize-y"
            ></textarea>
            <p v-else class="leading-relaxed text-display">{{ profile.bio }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, watch, reactive, onMounted } from 'vue';
  import { storeToRefs } from 'pinia';
  import { useStudentProfileStore } from '../../../store/studentProfileStore.ts';
  import { updateStudentProfile, getStudentProfile } from '../../../services/studentService.ts';
  import ProfilePhotoUpload from '../../../components/profile/ProfileImageUploader.vue';
  import defaultProfileImage from '../../../assets/DefaultImg.png';
  import { useRouter } from 'vue-router';

  const studentStore = useStudentProfileStore();
  const router = useRouter();
  const { grade, class: studentClass, userProfile, photo } = storeToRefs(studentStore);

  const isEditing = ref(false);
  const showSuccess = ref(false);

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
    if (!userProfile.value?.firstName) {
      try {
        const serverProfile = await getStudentProfile();

        studentStore.updateUserProfile(serverProfile.userProfile);
        studentStore.updateGradeAndClass(serverProfile.grade, serverProfile.class);
        studentStore.setPhoto(serverProfile.photo.url);

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
    const payload = {
      username: profile.username,
      phone: profile.phone,
      firstName: profile.firstName || '',
      lastName: profile.lastName || '',
      bio: profile.bio,
      birthdate: userProfile.value.birthdate,
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
        city: profile.city,
        country: profile.country,
        username: profile.username,
      });
      studentStore.updateGradeAndClass(profile.grade, profile.class);
      studentStore.setPhoto(response.photo?.url || defaultProfileImage); // Salvează URL-ul primit

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

<style scoped>
  .profile-container {
    max-width: 1200px;
    width: 100%;
    margin: 0 auto;
    position: relative;
  }

  .success-toast {
    position: fixed;
    top: 24px;
    right: 24px;
    z-index: 50;
    max-width: 300px;
  }

  .fade-enter-active,
  .fade-leave-active {
    transition:
      opacity 0.3s,
      transform 0.3s;
  }

  .fade-enter-from,
  .fade-leave-to {
    opacity: 0;
    transform: translateY(-10px);
  }

  .field-group {
    display: flex;
    flex-direction: column;
    gap: 0.25rem;
  }

  .label {
    font-size: 0.875rem;
    color: #4b5563;
    font-weight: 500;
  }

  .input-container {
    background-color: #fff;
    border-radius: 0.6rem;
    padding: 0.5rem;
    transition:
      box-shadow 0.2s ease-in-out,
      border-color 0.2s ease-in-out;
    border: 1px solid #e5e7eb;
  }

  .input-container:has(input:focus),
  .input-container:has(textarea:focus) {
    border-color: #9333ea;
    box-shadow: 0 0 0 3px rgba(147, 51, 234, 0.1);
  }

  .input-field {
    width: 100%;
    background-color: transparent;
    border: 0;
    border-radius: 0.5rem;
  }

  .input-field:focus {
    outline: none;
    box-shadow: none;
    border: transparent;
  }

  .text-display {
    padding-top: 0.125rem;
    padding-bottom: 0.125rem;
    color: #1f2937;
  }

  @media (max-width: 640px) {
    .input-container {
      padding: 0.5rem;
    }

    .success-toast {
      left: 16px;
      right: 16px;
      max-width: calc(100% - 32px);
    }
  }
</style>
