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
  grade: number | null;
  class: number | null;
  createProfileDto: CreateProfileDto;
}

const router = useRouter();
const studentProfileStore = useStudentProfileStore();

const showAlert = ref(false);
const alertMessage = ref('');
const alertType = ref('success');

const form = ref<StudentProfileData>({
  grade: null,
  class: null,
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

const attemptedSubmit = ref(false);

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

const isCountryInvalid = computed(() => attemptedSubmit.value && !form.value.createProfileDto.country);
const isCityInvalid = computed(() => attemptedSubmit.value && !form.value.createProfileDto.city);

// --- Funcție de Validare Reutilizabilă (doar Max) ---
// Aceasta corectează valoarea doar dacă depășește limita maximă.
// Permite null/0/valori mici pentru o editare fluentă.
const enforceMax = (val: number | null, maxLimit: number, targetRef: any) => {
  // Permite ștergerea (null)
  if (val === null) return;

  // Corectează doar dacă valoarea depășește limita maximă.
  if (val > maxLimit) {
    targetRef.value = maxLimit;
  }
};

// --- Aplicarea Logicii de Validare ---

// Birthdate Limits
watch(birthDay, (newVal) => enforceMax(newVal, 31, birthDay));
watch(birthMonth, (newVal) => enforceMax(newVal, 12, birthMonth));


// Academic Limits
const MAX_GPA = 10;    // Limita noua pentru GPA (Nota)
const MAX_CLASS = 12;  // Limita noua pentru Clasa

// 1. Validare Current GPA (Grade) - Max 10
watch(() => form.value.grade, (newVal) => enforceMax(newVal, MAX_GPA, form.value.grade));

// 2. Validare Current School Year (Class) - Max 12
watch(() => form.value.class, (newVal) => enforceMax(newVal, MAX_CLASS, form.value.class));


// ---------------------------------------------


const handleSubmit = async () => {
  attemptedSubmit.value = true;

  const pad = (num: number | null) => (num !== null ? String(num).padStart(2, '0') : '');

  const yyyy = birthYear.value;
  const mm = pad(birthMonth.value);
  const dd = pad(birthDay.value);

  // Verifică datele de naștere
  if (!yyyy || !mm || !dd) {
    console.error('Birthdate is incomplete.');
    return;
  }

  // Verifică GPA și Clasa la trimitere
  const gradeValue = form.value.grade;
  const classValue = form.value.class;

  if (
      gradeValue === null || classValue === null ||
      gradeValue < 1 || gradeValue > MAX_GPA ||
      classValue < 1 || classValue > MAX_CLASS
  ) {
    console.error('Academic details are incomplete or contain invalid values.');
    return;
  }

  if (isCountryInvalid.value || isCityInvalid.value) {
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
            class="bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] p-8 rounded-2xl shadow-2xl text-center"
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
              >First name</label
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
              >Last name</label
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
              >Username</label
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
              >Phone</label
              >
              <input
                  id="phone"
                  v-model="form.createProfileDto.phone"
                  type="tel"
                  required
                  placeholder="+37360000000"
                  class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
              <p v-if="phoneError" class="mt-1 text-sm text-red-500">
                {{ phoneError }}
              </p>
            </div>
            <div class="flex flex-col md:col-span-2" id="birthdate-section">
              <label class="mb-2 text-sm font-semibold text-gray-700"
              >Birthdate</label
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
                    maxlength="2"
                    oninput="if(this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);"
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
                    maxlength="2"
                    oninput="if(this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);"
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
                    maxlength="4"
                    oninput="if(this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);"
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
              >Current GPA (Max 10)</label
              >
              <input
                  id="grade"
                  v-model.number="form.grade"
                  type="number"
                  min="1"
                  :max="MAX_GPA"
                  placeholder="e.g. 9"
                  required
                  class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="class" class="mb-2 text-sm font-semibold text-gray-700"
              >Current School Year (Max 12)</label
              >
              <input
                  id="class"
                  v-model.number="form.class"
                  type="number"
                  placeholder="e.g. 10"
                  min="1"
                  :max="MAX_CLASS"
                  required
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
              >Country</label
              >
              <DropdownSelect
                  :options="['Romania', 'Moldova']"
                  placeholder="Select country from list"
                  @update:modelValue="form.createProfileDto.country = $event"
                  :class="{'border-red-500': isCountryInvalid}"
              />
              <p v-if="isCountryInvalid" class="mt-1 text-xs text-red-500">Please select a country.</p>

              <div v-if="form.createProfileDto.country" class="mt-4">
                <span
                    class="px-5 py-2 bg-gradient-to-r from-emerald-100 to-teal-100 border border-emerald-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm font-medium transition-all duration-300"
                >
                  {{ form.createProfileDto.country }}
                </span>
              </div>
            </div>

            <div class="flex flex-col">
              <label for="city" class="mb-2 text-sm font-semibold text-gray-700"
              >City</label
              >
              <DropdownSelect
                  :options="cities"
                  placeholder="Select city from list"
                  @update:modelValue="form.createProfileDto.city = $event"
                  :class="{'border-red-500': isCityInvalid}"
              />
              <p v-if="isCityInvalid" class="mt-1 text-xs text-red-500">Please select a city.</p>

              <div v-if="form.createProfileDto.city" class="mt-4">
                <span
                    class="px-5 py-2 bg-gradient-to-r from-emerald-100 to-teal-100 border border-emerald-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm font-medium transition-all duration-300"
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
              maxlength="400"
              required
              class="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md resize-none"
              placeholder="Tell us about your academic interests, goals, and what you're looking for in a tutor..."
          ></textarea>

          <div class="mt-2 text-sm text-right">
            <p
                v-if="form.createProfileDto.bio && form.createProfileDto.bio.length >= 400"
                class="font-semibold text-red-500"
            >
              Maximum limit of 400 characters reached.
            </p>

            <p
                class="text-gray-500"
            >
              {{ form.createProfileDto.bio ? form.createProfileDto.bio.length : 0 }} / 400 characters
            </p>
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
