<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import DropdownSelect from '../../../components/tutor/Profile/DropdownSelect.vue';
import { useRouter } from 'vue-router';
import { useStudentProfileStore } from '../../../store/studentProfileStore.ts';
import { createStudentProfile } from '../../../services/studentService.ts';
import AlertMessage from '../../../components/ui/AlertMessage.vue';

interface CreateProfileDto {
  phone: string;
  username: string;
  firstName: string;
  lastName: string;
  city: string;
  country: string;
  bio: string;
  birthdate: string;
}

interface StudentProfileData {
  grade: number;
  class: number;
  createProfileDto: CreateProfileDto;
}

const router = useRouter();
const studentProfileStore = useStudentProfileStore();

const showAlert = ref(false);
const alertMessage = ref('');
const alertType = ref('success');

const form = ref<StudentProfileData>({
  grade: null as unknown as number,
  class: null as unknown as number,
  createProfileDto: {
    phone: '',
    username: '',
    firstName: '',
    lastName: '',
    city: '',
    country: '',
    bio: '',
    birthdate: '',
  },
});

const birthDay = ref<number | null>(null);
const birthMonth = ref<number | null>(null);
const birthYear = ref<number | null>(null);

const currentYear = new Date().getFullYear();
const minYear = 1930;
const maxYear = currentYear - 5;

const birthDateError = ref('');
const phoneError = ref('');
const phonePattern = /^\+[0-9]{1,4}[0-9]{6,14}$/;

const validatePhone = () => {
  phoneError.value = '';
  
  if (!form.value.createProfileDto.phone) {
    phoneError.value = 'Phone number is required';
    return false;
  }
  
  if (!phonePattern.test(form.value.createProfileDto.phone)) {
    phoneError.value = 'Please enter a valid phone number (e.g., +37360000000)';
    return false;
  }
  
  return true;
};

const validateAge = () => {
  birthDateError.value = '';
  
  if (birthYear.value && birthMonth.value && birthDay.value) {
    const birthDate = new Date(birthYear.value, birthMonth.value - 1, birthDay.value);
    const today = new Date();
    const minAgeDate = new Date(today.getFullYear() - 6, today.getMonth(), today.getDate());
    
    if (birthDate > minAgeDate) {
      birthDateError.value = 'You must be at least 6 years old to register as a student';
      return false;
    }
    
    const isValidDate = birthDate.getFullYear() === birthYear.value &&
                        birthDate.getMonth() === birthMonth.value - 1 &&
                        birthDate.getDate() === birthDay.value;
                        
    if (!isValidDate) {
      birthDateError.value = 'Please enter a valid date';
      return false;
    }
  }
  
  return true;
};

watch([birthDay, birthMonth, birthYear], () => {
  validateAge();
}, { deep: true });

const cities = computed(() => {
  if (form.value.createProfileDto.country === 'Romania') {
    return ['Bucharest', 'Cluj-Napoca', 'Iași', 'Timișoara', 'Constanța', 'Craiova', 'Brașov'];
  } else if (form.value.createProfileDto.country === 'Moldova') {
    return ['Chișinău', 'Bălți', 'Tiraspol', 'Cahul', 'Ungheni', 'Orhei'];
  }
  return [];
});

watch(
  () => form.value.createProfileDto.country,
  () => {
    form.value.createProfileDto.city = '';
  },
);

const closeAlert = () => {
  showAlert.value = false;
};

const validatePhoneInput = () => {
  if (!form.value.createProfileDto.phone) {
    phoneError.value = 'Phone number is required';
  } else if (!phonePattern.test(form.value.createProfileDto.phone)) {
    phoneError.value = 'Please enter a valid phone number (e.g., +37360000000)';
  } else {
    phoneError.value = '';
  }
};

const clearPhoneError = () => {
  if (phoneError.value) phoneError.value = '';
};

const validateBirthdateInputs = () => {
  validateAge();
};

const clearBirthdateError = () => {
  if (birthDateError.value) birthDateError.value = '';
};

const bioCharacterCount = computed(() => {
  return form.value.createProfileDto.bio.length;
});

const bioLimitReached = computed(() => {
  return bioCharacterCount.value >= 200;
});

const limitBioText = () => {
  if (form.value.createProfileDto.bio.length > 200) {
    form.value.createProfileDto.bio = form.value.createProfileDto.bio.substring(0, 200);
  }
};

const handleSubmit = async () => {
  const isPhoneValid = validatePhone();
  const isAgeValid = validateAge();
  
  if (!isPhoneValid || !isAgeValid) {
    if (!isAgeValid) {
      document.getElementById('birthdate-section')?.scrollIntoView({ behavior: 'smooth' });
    }
    return;
  }
  
  const pad = (num: number | null) => (num !== null ? String(num).padStart(2, '0') : '');

  const yyyy = birthYear.value;
  const mm = pad(birthMonth.value);
  const dd = pad(birthDay.value);

  if (!yyyy || !mm || !dd) {
    console.error('Birthdate is incomplete.');
    return;
  }

  const finalBirthdate = `${yyyy}-${mm}-${dd}`;
  const birthdateFormatted = finalBirthdate ? `${finalBirthdate}T00:00:00` : '';

  const payload: StudentProfileData = {
    grade: form.value.grade,
    class: form.value.class,
    createProfileDto: {
      ...form.value.createProfileDto,
      birthdate: birthdateFormatted,
    },
  };

  try {
    await createStudentProfile(payload);

    studentProfileStore.updateGradeAndClass(form.value.grade, form.value.class);
    studentProfileStore.updateUserProfile(payload.createProfileDto);

    alertMessage.value = 'Your student profile has been successfully created!';
    alertType.value = 'success';
    showAlert.value = true;
    
    setTimeout(() => {
      router.push('/student-dashboard');
    }, 2000);
  } catch (error) {
    console.error('Error creating student profile:', error);
    
    alertMessage.value = 'An error occurred while creating your profile. Please try again.';
    alertType.value = 'error';
    showAlert.value = true;
  }
};
</script>

<template>
  <div class="min-h-screen p-6 bg-gradient-to-br from-indigo-50 via-purple-50 to-blue-50 lg:p-12">
    <AlertMessage 
      :show="showAlert"
      :message="alertMessage"
      :type="alertType"
      position="top-center"
      :duration="2000"
      @close="closeAlert"
    />
    
    <div class="max-w-4xl mx-auto">
      <form @submit.prevent="handleSubmit" class="space-y-8">
        <div
          class="p-8 text-center shadow-2xl bg-gradient-to-r from-orange-500 to-yellow-500 rounded-2xl"
        >
          <h1 class="mb-2 text-4xl font-bold text-white">Complete Your Student Profile</h1>
          <p class="text-purple-100">Let's create your academic profile together</p>
        </div>

        <section
          class="p-8 transition-all duration-300 border shadow-xl bg-white/80 backdrop-blur-sm border-white/50 rounded-2xl hover:shadow-2xl"
        >
          <div class="flex items-center mb-6">
            <div
              class="w-8 h-8 bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-sm font-bold text-white">1</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] bg-clip-text text-transparent"
            >
              Personal Details
            </h2>
          </div>
          <div class="grid grid-cols-1 gap-6 md:grid-cols-2">
            <div class="flex flex-col">
              <label for="firstName" class="mb-2 text-sm font-semibold text-gray-700"
              >First Name <span class="text-red-500">*</span></label
              >
              <input
                id="firstName"
                v-model="form.createProfileDto.firstName"
                type="text"
                placeholder="e.g., John"
                required
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="lastName" class="mb-2 text-sm font-semibold text-gray-700"
              >Last Name <span class="text-red-500">*</span></label
              >
              <input
                id="lastName"
                v-model="form.createProfileDto.lastName"
                type="text"
                placeholder="e.g., Doe"
                required
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="username" class="mb-2 text-sm font-semibold text-gray-700"
              >Username <span class="text-red-500">*</span></label
              >
              <input
                id="username"
                v-model="form.createProfileDto.username"
                type="text"
                required
                placeholder="e.g., john_student"
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="phone" class="mb-2 text-sm font-semibold text-gray-700"
              >Phone <span class="text-red-500">*</span></label
              >
              <input
                id="phone"
                v-model="form.createProfileDto.phone"
                type="tel"
                required
                placeholder="+37360000000"
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
                :class="{'border-red-300': phoneError}"
                @blur="validatePhoneInput"
                @input="clearPhoneError"
              />
              <p v-if="phoneError" class="mt-1 text-sm text-red-500">
                {{ phoneError }}
              </p>
            </div>
            <div class="flex flex-col md:col-span-2" id="birthdate-section">
              <label class="mb-2 text-sm font-semibold text-gray-700"
              >Birthdate <span class="text-red-500">*</span></label
              >
              <div class="grid grid-cols-3 gap-4">
                <input
                  id="birthDay"
                  v-model.number="birthDay"
                  type="number"
                  placeholder="Day (DD)"
                  min="1"
                  max="31"
                  required
                  class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
                  :class="{'border-red-300': birthDateError}"
                  maxlength="2"
                  oninput="if(this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);"
                  @blur="validateBirthdateInputs"
                  @input="clearBirthdateError"
                />
                <input
                  id="birthMonth"
                  v-model.number="birthMonth"
                  type="number"
                  placeholder="Month (MM)"
                  min="1"
                  max="12"
                  required
                  class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
                  :class="{'border-red-300': birthDateError}"
                  maxlength="2"
                  oninput="if(this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);"
                  @blur="validateBirthdateInputs"
                  @input="clearBirthdateError"
                />
                <input
                  id="birthYear"
                  v-model.number="birthYear"
                  type="number"
                  placeholder="Year (YYYY)"
                  :min="minYear"
                  :max="maxYear"
                  required
                  class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
                  :class="{'border-red-300': birthDateError}"
                  maxlength="4"
                  oninput="if(this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);"
                  @blur="validateBirthdateInputs"
                  @input="clearBirthdateError"
                />
              </div>
              <p v-if="birthDateError" class="mt-2 text-sm font-medium text-red-500">
                {{ birthDateError }}
              </p>
              <p class="mt-1 text-xs text-gray-500" v-else>You must be at least 6 years old to register as a student</p>
            </div>
          </div>
        </section>

        <section
          class="p-8 transition-all duration-300 border shadow-xl bg-white/80 backdrop-blur-sm border-white/50 rounded-2xl hover:shadow-2xl"
        >
          <div class="flex items-center mb-6">
            <div
              class="flex items-center justify-center w-8 h-8 mr-3 rounded-full bg-gradient-to-r from-emerald-500 to-teal-500"
            >
              <span class="text-sm font-bold text-white">2</span>
            </div>
            <h2
              class="text-2xl font-bold text-transparent bg-gradient-to-r from-emerald-500 to-teal-500 bg-clip-text"
            >
              Academic Details
            </h2>
          </div>
          <div class="grid grid-cols-1 gap-6 md:grid-cols-2">
            <div class="flex flex-col">
              <label for="grade" class="mb-2 text-sm font-semibold text-gray-700"
              >Current GPA</label
              >
              <input
                id="grade"
                v-model.number="form.grade"
                type="number"
                min="1"
                max="12"
                placeholder="e.g., 10"
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="class" class="mb-2 text-sm font-semibold text-gray-700"
              >Current Grade</label
              >
              <input
                id="class"
                v-model.number="form.class"
                type="number"
                placeholder="e.g., 11"
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
          </div>
        </section>

        <section
          class="relative z-40 p-8 transition-all duration-300 border shadow-xl bg-white/80 backdrop-blur-sm border-white/50 rounded-2xl hover:shadow-2xl"
        >
          <div class="flex items-center mb-6">
            <div
              class="flex items-center justify-center w-8 h-8 mr-3 rounded-full bg-gradient-to-r from-yellow-500 to-orange-500"
            >
              <span class="text-sm font-bold text-white">3</span>
            </div>
            <h2
              class="text-2xl font-bold text-transparent bg-gradient-to-r from-yellow-500 to-orange-500 bg-clip-text"
            >
              Location Information
            </h2>
          </div>
          <div class="grid grid-cols-1 gap-6 md:grid-cols-2">
            <div class="flex flex-col">
              <label for="country" class="mb-2 text-sm font-semibold text-gray-700"
              >Country <span class="text-red-500">*</span></label
              >
              <DropdownSelect
                :options="['Romania', 'Moldova']"
                placeholder="Select country from list"
                @update:modelValue="form.createProfileDto.country = $event"
              />
              <div v-if="form.createProfileDto.country" class="mt-4">
                <span
                  class="px-5 py-2 bg-gradient-to-r from-purple-100 to-indigo-100 border border-purple-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm font-medium transition-all duration-300"
                >
                  {{ form.createProfileDto.country }}
                </span>
              </div>
            </div>

            <div class="flex flex-col">
              <label for="city" class="mb-2 text-sm font-semibold text-gray-700"
              >City <span class="text-red-500">*</span></label
              >
              <DropdownSelect
                :options="cities"
                placeholder="Select city from list"
                @update:modelValue="form.createProfileDto.city = $event"
              />
              <div v-if="form.createProfileDto.city" class="mt-4">
                <span
                  class="px-5 py-2 bg-gradient-to-r from-purple-100 to-indigo-100 border border-purple-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm font-medium transition-all duration-300"
                >
                  {{ form.createProfileDto.city }}
                </span>
              </div>
            </div>
          </div>
        </section>

        <section
          class="p-8 transition-all duration-300 border shadow-xl bg-white/80 backdrop-blur-sm border-white/50 rounded-2xl hover:shadow-2xl"
        >
          <div class="flex items-center mb-6">
            <div
              class="flex items-center justify-center w-8 h-8 mr-3 rounded-full bg-gradient-to-r from-pink-500 to-rose-500"
            >
              <span class="text-sm font-bold text-white">4</span>
            </div>
            <h2
              class="text-2xl font-bold text-transparent bg-gradient-to-r from-pink-500 to-rose-500 bg-clip-text"
            >
              A brief introduction
            </h2>
          </div>
          <textarea
            v-model="form.createProfileDto.bio"
            rows="6"
            maxlength="200"
            class="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md resize-none"
            placeholder="Tell us about your academic interests, goals, and what you're looking for in a tutor..."
            @input="limitBioText"
            :class="{'border-red-300': bioLimitReached}"
          ></textarea>
          <div class="mt-2 text-sm text-right" :class="{
            'text-gray-500': bioCharacterCount < 150,
            'text-orange-500': bioCharacterCount >= 150 && bioCharacterCount < 200,
            'text-red-500': bioCharacterCount >= 200
          }">
            {{ bioCharacterCount }}/200 characters
          </div>
        </section>

        <div class="flex justify-center pt-4">
          <button
            type="submit"
            class="relative bg-gradient-to-r from-[#5f22d9] via-[#4a1fb8] to-[#3a22d9] text-white font-bold py-4 px-12 rounded-2xl shadow-2xl hover:shadow-3xl transform hover:scale-105 transition-all duration-300 text-lg overflow-hidden group"
          >
            <span
              class="absolute inset-0 transition-opacity duration-300 opacity-0 bg-gradient-to-r from-white/20 to-transparent group-hover:opacity-100"
            ></span>
            <span class="relative">Create Profile</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
