<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import DropdownSelect from './DropdownSelect.vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { createTutorProfile } from '../../../services/tutorService.ts';
import type {
  Subject,
  TutorProfileData,
  CreateProfileDto,
} from '../../../services/tutorService.ts';
import { useRouter } from 'vue-router';

library.add(faTimes);

const router = useRouter();

const form = ref({
  firstName: '',
  lastName: '',
  username: '',
  phone: '',
  country: '',
  city: '',
  experienceYears: null as number | null,
  subjects: [] as { subjectName: string; pricePerHour: number | null; currency: string }[],
  languages: [] as string[],
  teachingPreferences: {
    myHome: false,
    studentHome: false,
    online: false,
  },
  bio: '',
});

const birthDay = ref<number | null>(null);
const birthMonth = ref<number | null>(null);
const birthYear = ref<number | null>(null);

const currentYear = new Date().getFullYear();
const minYear = 1930;
const maxYear = currentYear - 18;

const allLanguages = ['English', 'French', 'Spanish', 'German', 'Italian', 'Romanian'];
const allSubjects = [
  'Mathematics',
  'English',
  'Science',
  'Physics',
  'Chemistry',
  'Biology',
  'Geography',
  'History',
  'Foreign languages',
  'German',
  'French',
  'Russian',
  'Spanish',
  'Italian',
  'Computer science',
  'Economics',
  'Philosophy',
  'Psychology',
  'Sociology',
  'Physical Education',
  'Health Education',
  'Drawing',
  'Music',
  'Astronomy',
  'Literature',
  'Creative Writing',
  'Statistics',
];

const attemptedSubmit = ref(false);

const enforceMax = (val: number | null, maxLimit: number, targetRef: any) => {
  if (val === null) return;
  if (val > maxLimit) {
    targetRef.value = maxLimit;
  }
};

watch(birthDay, (newVal) => enforceMax(newVal, 31, birthDay));
watch(birthMonth, (newVal) => enforceMax(newVal, 12, birthMonth));
watch(birthYear, (newVal) => enforceMax(newVal, maxYear, birthYear));

const MAX_EXPERIENCE_YEARS = 50;
watch(() => form.value.experienceYears, (newVal) => enforceMax(newVal, MAX_EXPERIENCE_YEARS, form.value.experienceYears));

watch(() => form.value.subjects, (newSubjects) => {
  newSubjects.forEach(subject => {
    if (subject.pricePerHour !== null && subject.pricePerHour < 0) {
      subject.pricePerHour = 0;
    }
  });
}, { deep: true });

const availableLanguages = computed(() => {
  return allLanguages.filter((lang) => !form.value.languages.includes(lang));
});

const availableSubjects = computed(() => {
  const selectedSubjects = form.value.subjects.map((s) => s.subjectName);
  return allSubjects.filter((subject) => !selectedSubjects.includes(subject));
});

const isCountryInvalid = computed(() => attemptedSubmit.value && !form.value.country);
const isCityInvalid = computed(() => attemptedSubmit.value && !form.value.city);
const isExperienceInvalid = computed(() => attemptedSubmit.value && (form.value.experienceYears === null || form.value.experienceYears < 0));
const isSubjectsEmpty = computed(() => attemptedSubmit.value && form.value.subjects.length === 0);
const areSubjectsPricesInvalid = computed(() => attemptedSubmit.value && form.value.subjects.some(s => s.pricePerHour === null || s.pricePerHour <= 0));
const isLanguageInvalid = computed(() => attemptedSubmit.value && form.value.languages.length === 0);
const isTeachingLocationInvalid = computed(() => attemptedSubmit.value && workingLocationId.value === 0);

const addLanguage = (language: string) => {
  if (language && !form.value.languages.includes(language)) {
    form.value.languages.push(language);
  }
};

const removeLanguage = (language: string) => {
  form.value.languages = form.value.languages.filter((lang) => lang !== language);
};

const cities = computed(() => {
  if (form.value.country === 'Romania') {
    return ['Bucharest', 'Cluj-Napoca', 'Iași', 'Timișoara', 'Constanța', 'Craiova', 'Brașov'];
  } else if (form.value.country === 'Moldova') {
    return [
      'Anenii Noi',
      'Basarabeasca',
      'Briceni',
      'Cahul',
      'Cantemir',
      'Călărași',
      'Căușeni',
      'Chișinău',
      'Cimișlia',
      'Criuleni',
      'Dondușeni',
      'Drochia',
      'Dubăsari',
      'Edineț',
      'Fălești',
      'Florești',
      'Glodeni',
      'Hîncești',
      'Ialoveni',
      'Leova',
      'Nisporeni',
      'Ocnița',
      'Orhei',
      'Rezina',
      'Rîșcani',
      'Sîngerei',
      'Soroca',
      'Strășeni',
      'Șoldănești',
      'Ștefan Vodă',
      'Taraclia',
      'Telenești',
      'Ungheni',
      'Bălți',
      'Comrat',
      'Tighina',
      'Tiraspol',
    ];
  } else if (form.value.country === 'Italy') {
    return ['Rome', 'Milan', 'Florence', 'Naples', 'Turin'];
  } else if (form.value.country === 'Germany') {
    return ['Berlin', 'Munich', 'Hamburg', 'Frankfurt', 'Cologne'];
  } else if (form.value.country === 'England') {
    return ['London', 'Manchester', 'Birmingham', 'Liverpool', 'Leeds'];
  }
  return [];
});

const addNewSubject = (subjectName: string) => {
  form.value.subjects.push({
    subjectName: subjectName,
    pricePerHour: null,
    currency: 'MDL',
  });
};

const removeSubject = (index: number) => {
  form.value.subjects.splice(index, 1);
};

watch(
  () => form.value.country,
  () => {
    form.value.city = '';
  },
);

const workingLocationId = computed(() => {
  const { myHome, studentHome, online } = form.value.teachingPreferences;

  if (myHome && online && studentHome) return 7;
  if (myHome && online) return 4;
  if (myHome && studentHome) return 5;
  if (online && studentHome) return 6;
  if (myHome) return 1;
  if (online) return 2;
  if (studentHome) return 3;

  return 0;
});

const handleSubmit = async () => {
  attemptedSubmit.value = true;

  const pad = (num: number | null) => (num !== null ? String(num).padStart(2, '0') : '');

  const yyyy = birthYear.value;
  const mm = pad(birthMonth.value);
  const dd = pad(birthDay.value);

  if (!yyyy || !mm || !dd) {
    return;
  }

  if (
    isCountryInvalid.value ||
    isCityInvalid.value ||
    isExperienceInvalid.value ||
    isSubjectsEmpty.value ||
    areSubjectsPricesInvalid.value ||
    isLanguageInvalid.value ||
    isTeachingLocationInvalid.value
  ) {
    return;
  }

  const finalBirthdate = `${yyyy}-${mm}-${dd}`;
  const birthdateFormatted = finalBirthdate ? `${finalBirthdate}T00:00:00` : '';

  const subjectsPayload: Subject[] = form.value.subjects.map((subject) => ({
    subjectName: subject.subjectName,
    subjectSlug: subject.subjectName.toLowerCase().replace(/\s/g, '-').replace(/[^\w-]+/g, ''),
    pricePerHour: parseFloat(subject.pricePerHour!.toString()),
    currency: 'MDL',
  }));

  const profileDto: CreateProfileDto = {
    phone: form.value.phone,
    username: form.value.username,
    firstName: form.value.firstName,
    lastName: form.value.lastName,
    bio: form.value.bio.substring(0, 400),
    birthdate: birthdateFormatted,
    country: form.value.country,
    city: form.value.city,
  };

  const payload: TutorProfileData = {
    verificationStatus: 'Pending',
    experienceYears: form.value.experienceYears!,
    subjects: subjectsPayload,
    createProfileDto: profileDto,
    workingLocation: workingLocationId.value,
    languages: form.value.languages,
  };

  try {
    await createTutorProfile(payload);
    router.push('/tutor-dashboard');
  } catch (error) {
  }
};
</script>

<template>
  <div class="min-h-screen p-6 bg-gradient-to-br from-indigo-50 via-purple-50 to-blue-50 lg:p-12">
    <div class="max-w-4xl mx-auto">
      <form @submit.prevent="handleSubmit" class="space-y-8">
        <div
          class="bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] p-8 rounded-2xl shadow-2xl text-center"
        >
          <h1 class="mb-2 text-4xl font-bold text-white">Complete Your Profile</h1>
          <p class="text-purple-100">Let's create your teaching profile together</p>
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
              <label for="firstName" class="mb-2 text-sm font-semibold text-gray-700">First name</label>
              <input
                id="firstName"
                v-model="form.firstName"
                type="text"
                required
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="lastName" class="mb-2 text-sm font-semibold text-gray-700">Last name</label>
              <input
                id="lastName"
                v-model="form.lastName"
                type="text"
                required
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="username" class="mb-2 text-sm font-semibold text-gray-700">Username</label>
              <input
                id="username"
                v-model="form.username"
                type="text"
                required
                placeholder="e.g., anna_teacher"
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="phone" class="mb-2 text-sm font-semibold text-gray-700">Phone</label>
              <input
                id="phone"
                v-model="form.phone"
                type="tel"
                required
                placeholder="+37360000000"
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col md:col-span-2">
              <label class="mb-2 text-sm font-semibold text-gray-700">Birthdate</label>
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
              <p v-if="attemptedSubmit && (!birthDay || !birthMonth || !birthYear)" class="mt-1 text-xs text-red-500">Please provide a complete and valid birthdate.</p>
            </div>
          </div>
        </section>

        <section
          class="relative z-40 p-8 transition-all duration-300 border shadow-xl bg-white/80 backdrop-blur-sm border-white/50 rounded-2xl hover:shadow-2xl"
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
              Location Information
            </h2>
          </div>
          <div class="grid grid-cols-1 gap-6 md:grid-cols-2">
            <div class="flex flex-col">
              <label for="country" class="mb-2 text-sm font-semibold text-gray-700">Country</label>
              <DropdownSelect
                :options="['Romania', 'Moldova', 'Italy', 'Germany', 'England']"
                placeholder="Select country from list"
                @update:modelValue="form.country = $event"
                :class="{'border-red-500': isCountryInvalid}"
              />
              <p v-if="isCountryInvalid" class="mt-1 text-xs text-red-500">Please select a country.</p>
              <div v-if="form.country" class="mt-4">
                <span
                  class="px-5 py-2 bg-gradient-to-r from-purple-100 to-indigo-100 border border-purple-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm font-medium transition-all duration-300"
                >
                  {{ form.country }}
                </span>
              </div>
            </div>

            <div class="flex flex-col">
              <label for="city" class="mb-2 text-sm font-semibold text-gray-700">City</label>
              <DropdownSelect
                :options="cities"
                placeholder="Select city from list"
                @update:modelValue="form.city = $event"
                :class="{'border-red-500': isCityInvalid}"
              />
              <p v-if="isCityInvalid" class="mt-1 text-xs text-red-500">Please select a city.</p>
              <div v-if="form.city" class="mt-4">
                <span
                  class="px-5 py-2 bg-gradient-to-r from-purple-100 to-indigo-100 border border-purple-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm font-medium transition-all duration-300"
                >
                  {{ form.city }}
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
              class="flex items-center justify-center w-8 h-8 mr-3 rounded-full bg-gradient-to-r from-yellow-500 to-orange-500"
            >
              <span class="text-sm font-bold text-white">3</span>
            </div>
            <h2
              class="text-2xl font-bold text-transparent bg-gradient-to-r from-yellow-500 to-orange-500 bg-clip-text"
            >
              Teaching Experience
            </h2>
          </div>
          <div class="flex flex-col">
            <label for="experienceYears" class="mb-2 text-sm font-semibold text-gray-700">Years of Experience</label>
            <input
              id="experienceYears"
              v-model.number="form.experienceYears"
              type="number"
              min="0"
              :max="MAX_EXPERIENCE_YEARS"
              required
              class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
            />
            <p v-if="isExperienceInvalid" class="mt-1 text-xs text-red-500">Please provide a valid number of years of experience (Max {{ MAX_EXPERIENCE_YEARS }}).</p>
          </div>
        </section>

        <section
          class="relative z-40 p-8 transition-all duration-300 border shadow-xl bg-white/80 backdrop-blur-sm border-white/50 rounded-2xl hover:shadow-2xl"
        >
          <div class="flex items-center mb-6">
            <div
              class="flex items-center justify-center w-8 h-8 mr-3 rounded-full bg-gradient-to-r from-purple-500 to-indigo-500"
            >
              <span class="text-sm font-bold text-white">4</span>
            </div>
            <h2
              class="text-2xl font-bold text-transparent bg-gradient-to-r from-purple-500 to-indigo-500 bg-clip-text"
            >
              Subjects & Pricing
            </h2>
          </div>
          <div class="space-y-6">
            <div
              v-for="(subject, index) in form.subjects"
              :key="index"
              class="p-6 border border-purple-100 bg-gradient-to-r from-purple-50 to-indigo-50 rounded-xl"
            >
              <div class="grid items-end grid-cols-1 gap-4 md:grid-cols-3">
                <div class="flex flex-col">
                  <label class="mb-2 text-sm font-semibold text-gray-700">Subject</label>
                  <div
                    class="px-4 py-3 font-medium text-gray-800 bg-white border-2 border-gray-200 shadow-sm rounded-xl hover:shadow-md"
                  >
                    {{ subject.subjectName }}
                  </div>
                </div>
                <div class="flex flex-col">
                  <label :for="`price-${index}`" class="mb-2 text-sm font-semibold text-gray-700">Price per Hour (MDL)</label>
                  <input
                    :id="`price-${index}`"
                    v-model.number="subject.pricePerHour"
                    type="number"
                    step="0.01"
                    min="1"
                    required
                    placeholder="25.50"
                    class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
                    :class="{'border-red-500': attemptedSubmit && (subject.pricePerHour === null || subject.pricePerHour <= 0)}"
                  />
                </div>
                <div class="flex justify-end">
                  <button
                    type="button"
                    @click="removeSubject(index)"
                    class="px-4 py-2 text-white transition-colors duration-300 bg-red-500 rounded-xl hover:bg-red-600"
                  >
                    Remove
                  </button>
                </div>
              </div>
            </div>

            <p v-if="isSubjectsEmpty" class="mt-1 text-xs text-red-500 font-semibold">Please add at least one subject to create your profile.</p>
            <p v-else-if="areSubjectsPricesInvalid" class="mt-1 text-xs text-red-500">Please provide a price greater than 0 for all selected subjects.</p>

            <div v-if="availableSubjects.length > 0">
              <DropdownSelect
                :options="availableSubjects"
                placeholder="+ Add Another Subject"
                @update:modelValue="addNewSubject"
                :class="{'border-red-500': isSubjectsEmpty}"
              />
            </div>
          </div>
        </section>

        <section
          class="relative z-30 p-8 transition-all duration-300 border shadow-xl bg-white/80 backdrop-blur-sm border-white/50 rounded-2xl hover:shadow-2xl"
        >
          <div class="flex items-center mb-6">
            <div
              class="flex items-center justify-center w-8 h-8 mr-3 rounded-full bg-gradient-to-r from-orange-500 to-red-500"
            >
              <span class="text-sm font-bold text-white">5</span>
            </div>
            <h2
              class="text-2xl font-bold text-transparent bg-gradient-to-r from-orange-500 to-red-500 bg-clip-text"
            >
              Languages
            </h2>
          </div>
          <div class="flex flex-wrap items-center gap-3 mb-4">
            <span
              v-for="(lang, index) in form.languages"
              :key="index"
              class="px-5 py-2 bg-gradient-to-r from-purple-100 to-indigo-100 border border-purple-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm hover:shadow-md transition-all duration-300 font-medium"
            >
              {{ lang }}
              <font-awesome-icon
                :icon="['fas', 'times']"
                class="w-4 h-4 text-gray-400 transition-colors duration-200 cursor-pointer hover:text-red-500"
                @click="removeLanguage(lang)"
              />
            </span>
            <DropdownSelect
              :options="availableLanguages"
              placeholder="+ Add Language"
              @update:modelValue="addLanguage"
              :class="{'border-red-500': isLanguageInvalid}"
            />
          </div>
          <p v-if="isLanguageInvalid" class="mt-1 text-xs text-red-500">Please select at least one language.</p>
        </section>

        <section
          class="p-8 transition-all duration-300 border shadow-xl bg-white/80 backdrop-blur-sm border-white/50 rounded-2xl hover:shadow-2xl"
        >
          <div class="flex items-center mb-6">
            <div
              class="flex items-center justify-center w-8 h-8 mr-3 rounded-full bg-gradient-to-r from-blue-500 to-cyan-500"
            >
              <span class="text-sm font-bold text-white">6</span>
            </div>
            <h2
              class="text-2xl font-bold text-transparent bg-gradient-to-r from-blue-500 to-cyan-500 bg-clip-text"
            >
              I can teach on
            </h2>
          </div>
          <div class="flex flex-wrap gap-6">
            <div
              class="flex items-center px-4 py-3 space-x-3 transition-all duration-300 border border-blue-100 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-xl hover:shadow-md"
            >
              <input
                id="myHome"
                type="checkbox"
                v-model="form.teachingPreferences.myHome"
                class="rounded-lg text-[#5f22d9] focus:ring-[#5f22d9] w-4 h-4"
              />
              <label for="myHome" class="font-medium text-gray-700">My home</label>
            </div>
            <div
              class="flex items-center px-4 py-3 space-x-3 transition-all duration-300 border border-green-100 bg-gradient-to-r from-green-50 to-emerald-50 rounded-xl hover:shadow-md"
            >
              <input
                id="studentHome"
                type="checkbox"
                v-model="form.teachingPreferences.studentHome"
                class="rounded-lg text-[#5f22d9] focus:ring-[#5f22d9] w-4 h-4"
              />
              <label for="studentHome" class="font-medium text-gray-700">Student's home</label>
            </div>
            <div
              class="flex items-center px-4 py-3 space-x-3 transition-all duration-300 border border-purple-100 bg-gradient-to-r from-purple-50 to-pink-50 rounded-xl hover:shadow-md"
            >
              <input
                id="online"
                type="checkbox"
                v-model="form.teachingPreferences.online"
                class="rounded-lg text-[#5f22d9] focus:ring-[#5f22d9] w-4 h-4"
              />
              <label for="online" class="font-medium text-gray-700">Online</label>
            </div>
          </div>
          <p v-if="isTeachingLocationInvalid" class="mt-1 text-xs text-red-500">Please select at least one teaching location.</p>
        </section>

        <section
          class="p-8 transition-all duration-300 border shadow-xl bg-white/80 backdrop-blur-sm border-white/50 rounded-2xl hover:shadow-2xl"
        >
          <div class="flex items-center mb-6">
            <div
              class="flex items-center justify-center w-8 h-8 mr-3 rounded-full bg-gradient-to-r from-pink-500 to-rose-500"
            >
              <span class="text-sm font-bold text-white">7</span>
            </div>
            <h2
              class="text-2xl font-bold text-transparent bg-gradient-to-r from-pink-500 to-rose-500 bg-clip-text"
            >
              A brief introduction
            </h2>
          </div>
          <textarea
            v-model="form.bio"
            rows="6"
            maxlength="400"
            required
            class="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md resize-none"
            placeholder="Tell us about yourself, your teaching experience, and what makes you passionate about education..."
          ></textarea>
          <div class="mt-2 text-right text-sm">
            <p
              v-if="form.bio && form.bio.length >= 400"
              class="text-red-500 font-semibold"
            >
              Maximum limit of 400 characters reached.
            </p>
            <p class="text-gray-500">
              {{ form.bio ? form.bio.length : 0 }} / 400 characters
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