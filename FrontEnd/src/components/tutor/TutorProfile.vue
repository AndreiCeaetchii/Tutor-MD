<script setup lang="ts">
import { ref, reactive, watch } from 'vue';

interface TutorProfile {
  id: string;
  name: string;
  email: string;
  phone: string;
  bio: string;
  subjects: string[];
  hourlyRate: number;
  experience: number;
  education: string;
  languages: string[];
  profileImage: string;
}

// State variables
const isEditing = ref(false);
const showSuccess = ref(false);
const newSubject = ref('');
const newLanguage = ref('');

const initialProfile: TutorProfile = {
  id: '1',
  name: 'Sarah Johnson',
  email: 'sarah.johnson@tutorconnect.com',
  phone: '+1 (555) 123-4567',
  bio: 'Passionate mathematics tutor with over 8 years of experience helping students excel in algebra, calculus, and statistics. I believe in making complex concepts accessible and engaging.',
  subjects: ['Mathematics', 'Calculus', 'Statistics', 'Algebra'],
  hourlyRate: 75,
  experience: 8,
  education: 'M.S. Mathematics, Stanford University',
  languages: ['English', 'Spanish', 'French'],
  profileImage: 'https://images.unsplash.com/photo-1494790108755-2616b612b786?w=400&h=400&fit=crop&crop=face',
};

const profile = reactive<TutorProfile>({ ...initialProfile });
let originalProfile: TutorProfile | null = null;

// Methods
const handleSave = () => {
  console.log('Profile saved:', profile);
  isEditing.value = false;
  showSuccess.value = true;
  setTimeout(() => (showSuccess.value = false), 3000);
};

const handleCancel = () => {
  if (originalProfile) {
    Object.assign(profile, originalProfile);
  }
  isEditing.value = false;
};

const addSubject = () => {
  if (newSubject.value.trim() && !profile.subjects.includes(newSubject.value.trim())) {
    profile.subjects.push(newSubject.value.trim());
    newSubject.value = '';
  }
};

const removeSubject = (index: number) => {
  profile.subjects.splice(index, 1);
};

const addLanguage = () => {
  if (newLanguage.value.trim() && !profile.languages.includes(newLanguage.value.trim())) {
    profile.languages.push(newLanguage.value.trim());
    newLanguage.value = '';
  }
};

const removeLanguage = (index: number) => {
  profile.languages.splice(index, 1);
};

// New image upload handler
const handleImageUpload = (event: Event) => {
  const input = event.target as HTMLInputElement;
  if (input.files && input.files[0]) {
    const file = input.files[0];
    const reader = new FileReader();
    
    reader.onload = (e) => {
      if (e.target && typeof e.target.result === 'string') {
        profile.profileImage = e.target.result;
      }
    };
    
    reader.readAsDataURL(file);
  }
};

watch(isEditing, (newValue) => {
  if (newValue) {
    originalProfile = { ...profile };
  } else {
    originalProfile = null;
  }
});
</script>

<template>
  <div class="profile-container">
    <!-- Success notification as a toast -->
    <transition name="fade">
      <div 
        v-if="showSuccess" 
        class="success-toast flex items-center gap-2 bg-teal-50 border-l-4 border-teal-400 text-teal-800 p-3 rounded-r-lg shadow-md"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-teal-500" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
        </svg>
        <span>Profile updated successfully!</span>
      </div>
    </transition>
    
    <div class="bg-white rounded-2xl shadow-lg p-4 md:p-8 font-sans">
      <div class="profile flex flex-col sm:flex-row items-center sm:justify-between mb-8 gap-4">
        <div class="flex flex-col sm:flex-row items-center gap-4">
          <div class="relative w-24 h-24 rounded-full overflow-hidden flex-shrink-0">
            <img
              :src="profile.profileImage"
              alt="Profile picture"
              class="w-full h-full object-cover"
            />
            <label v-if="isEditing" class="absolute inset-0 cursor-pointer">
              <input 
                type="file" 
                class="hidden" 
                accept="image/*" 
                @change="handleImageUpload"
              />
              <div class="absolute bottom-0 right-0 bg-purple-600 text-white rounded-full p-1.5 shadow-lg hover:bg-purple-700 transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-3.5 w-3.5" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M4 3a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V5a2 2 0 00-2-2H4zm12 12H4l4-8 3 6 2-4 3 6z" clip-rule="evenodd" />
                </svg>
              </div>
            </label>
          </div>
          <div class="text-center sm:text-left sm:ml-4">
            <h3 class="text-xl font-semibold text-gray-800">{{ profile.name }}</h3>
            <p class="text-gray-600">{{ profile.subjects.join(', ') }} Tutor</p>
          </div>
        </div>
        
        <!-- Action buttons -->
        <div class="flex gap-2 mt-4 sm:mt-0">
          <button
            v-if="isEditing"
            @click="handleSave"
            class="flex items-center gap-2 px-5 py-2 bg-purple-600 text-white rounded-full font-medium transition-all hover:-translate-y-0.5 hover:shadow-md"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
            </svg>
            Save
          </button>
          <button
            v-if="isEditing"
            @click="handleCancel"
            class="flex items-center gap-2 px-5 py-2 border border-gray-300 text-gray-600 rounded-full font-medium transition-all"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
            </svg>
            Cancel
          </button>
          <button
            v-else
            @click="isEditing = true"
            class="flex items-center gap-2 px-5 py-2 bg-purple-600 text-white rounded-full font-medium transition-all hover:-translate-y-0.5 hover:shadow-md"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path d="M17.414 2.586a2 2 0 00-2.828 0L7 10.172V13h2.828l7.586-7.586a2 2 0 000-2.828z" />
              <path fill-rule="evenodd" d="M2 6a2 2 0 012-2h4a1 1 0 010 2H4v10h10v-4a1 1 0 112 0v4a2 2 0 01-2 2H4a2 2 0 01-2-2V6z" clip-rule="evenodd" />
            </svg>
            Edit Profile
          </button>
        </div>
      </div>

      <!-- Form fields in a responsive grid layout -->
      <div class="space-y-6">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4 md:gap-6">
          <div class="field-group">
            <label class="label">Full Name</label>
            <div class="input-container">
              <input
                v-if="isEditing"
                v-model="profile.name"
                type="text"
                class="input-field"
              />
              <p v-else class="text-display">{{ profile.name }}</p>
            </div>
          </div>

          <div class="field-group">
            <label class="label">Email</label>
            <div class="input-container">
              <input
                v-if="isEditing"
                v-model="profile.email"
                type="email"
                class="input-field"
              />
              <p v-else class="text-display">{{ profile.email }}</p>
            </div>
          </div>

          <div class="field-group">
            <label class="label">Phone</label>
            <div class="input-container">
              <input
                v-if="isEditing"
                v-model="profile.phone"
                type="tel"
                class="input-field"
              />
              <p v-else class="text-display">{{ profile.phone }}</p>
            </div>
          </div>

          <div class="field-group">
            <label class="label">Hourly Rate ($)</label>
            <div class="input-container">
              <input
                v-if="isEditing"
                v-model.number="profile.hourlyRate"
                type="number"
                class="input-field"
              />
              <p v-else class="text-display">{{ profile.hourlyRate }}</p>
            </div>
          </div>
        </div>

        <div class="field-group">
          <label class="label">Bio</label>
          <div class="input-container">
            <textarea
              v-if="isEditing"
              v-model="profile.bio"
              class="input-field min-h-[100px] resize-y"
            ></textarea>
            <p v-else class="text-display leading-relaxed">{{ profile.bio }}</p>
          </div>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4 md:gap-6">
          <div class="field-group">
            <label class="label">Education</label>
            <div class="input-container">
              <input
                v-if="isEditing"
                v-model="profile.education"
                type="text"
                class="input-field"
              />
              <p v-else class="text-display">{{ profile.education }}</p>
            </div>
          </div>

          <div class="field-group">
            <label class="label">Years of Experience</label>
            <div class="input-container">
              <input
                v-if="isEditing"
                v-model.number="profile.experience"
                type="number"
                class="input-field"
              />
              <p v-else class="text-display">{{ profile.experience }}</p>
            </div>
          </div>
        </div>

        <div class="field-group">
          <label class="label">Subjects</label>
          <div class="flex flex-wrap gap-2 items-center">
            <div
              v-for="(subject, index) in profile.subjects"
              :key="`subject-${index}`"
              class="bg-purple-600 text-white py-1.5 px-4 rounded-full text-sm flex items-center gap-1 font-medium"
            >
              {{ subject }}
              <button
                v-if="isEditing"
                @click="removeSubject(index)"
                class="bg-white bg-opacity-30 text-white rounded-full w-5 h-5 flex items-center justify-center text-xs ml-1 hover:bg-opacity-50 transition-colors"
              >
                ×
              </button>
            </div>
            <div v-if="isEditing" class="flex items-center gap-2 mt-2 sm:mt-0 w-full sm:w-auto">
              <input
                v-model="newSubject"
                @keyup.enter="addSubject"
                placeholder="Add subject"
                class="px-4 py-1.5 border border-dashed border-gray-300 rounded-full text-sm w-full sm:min-w-[150px] focus:outline-none focus:ring-2 focus:ring-purple-600 focus:border-transparent"
              />
              <button
                @click="addSubject"
                class="w-8 h-8 rounded-full bg-gray-100 hover:bg-gray-200 flex items-center justify-center transition-colors text-purple-600 flex-shrink-0"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z" clip-rule="evenodd" />
                </svg>
              </button>
            </div>
          </div>
        </div>

        <div class="field-group">
          <label class="label">Languages</label>
          <div class="flex flex-wrap gap-2 items-center">
            <div
              v-for="(language, index) in profile.languages"
              :key="`language-${index}`"
              class="bg-gray-200 text-gray-800 py-1.5 px-4 rounded-full text-sm flex items-center gap-1 font-medium"
            >
              {{ language }}
              <button
                v-if="isEditing"
                @click="removeLanguage(index)"
                class="bg-gray-300 text-gray-600 rounded-full w-5 h-5 flex items-center justify-center text-xs ml-1 hover:bg-gray-400 transition-colors"
              >
                ×
              </button>
            </div>
            <div v-if="isEditing" class="flex items-center gap-2 mt-2 sm:mt-0 w-full sm:w-auto">
              <input
                v-model="newLanguage"
                @keyup.enter="addLanguage"
                placeholder="Add language"
                class="px-4 py-1.5 border border-dashed border-gray-300 rounded-full text-sm w-full sm:min-w-[150px] focus:outline-none focus:ring-2 focus:ring-purple-600 focus:border-transparent"
              />
              <button
                @click="addLanguage"
                class="w-8 h-8 rounded-full bg-gray-100 hover:bg-gray-200 flex items-center justify-center transition-colors text-purple-600 flex-shrink-0"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z" clip-rule="evenodd" />
                </svg>
              </button>
            </div>
          </div>
        </div>
      </div>
      <!-- Removed the success message from here -->
    </div>
  </div>
</template>

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

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s, transform 0.3s;
}

.fade-enter-from, .fade-leave-to {
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
  background-color: #f3f4f6;
  border-radius: 0.5rem;
  padding: 0.75rem;
  transition-property: color, background-color, border-color, text-decoration-color, fill, stroke;
  transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  transition-duration: 200ms;
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
