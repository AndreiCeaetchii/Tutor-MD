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
              <label for="firstName" class="mb-2 text-sm font-semibold text-gray-700"
                >First name <span class="text-red-500">*</span></label
              >
              <input
                id="firstName"
                v-model="form.firstName"
                type="text"
                required
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="lastName" class="mb-2 text-sm font-semibold text-gray-700"
                >Last name <span class="text-red-500">*</span></label
              >
              <input
                id="lastName"
                v-model="form.lastName"
                type="text"
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
                v-model="form.username"
                type="text"
                required
                placeholder="e.g., anna_teacher"
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="phone" class="mb-2 text-sm font-semibold text-gray-700"
                >Phone <span class="text-red-500">*</span></label
              >
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
              <label for="birthdate" class="mb-2 text-sm font-semibold text-gray-700"
                >Birthdate <span class="text-red-500">*</span></label
              >
              <input
                id="birthdate"
                v-model="form.birthdate"
                type="date"
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
              <label for="country" class="mb-2 text-sm font-semibold text-gray-700"
                >Country <span class="text-red-500">*</span></label
              >
              <DropdownSelect
                :options="['Romania', 'Moldova']"
                placeholder="Select country from list"
                @update:modelValue="form.country = $event"
              />
              <div v-if="form.country" class="mt-4">
                <span
                  class="px-5 py-2 bg-gradient-to-r from-emerald-100 to-teal-100 border border-emerald-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm font-medium transition-all duration-300"
                >
                  {{ form.country }}
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
                @update:modelValue="form.city = $event"
              />
              <div v-if="form.city" class="mt-4">
                <span
                  class="px-5 py-2 bg-gradient-to-r from-emerald-100 to-teal-100 border border-emerald-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm font-medium transition-all duration-300"
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
            <label for="experienceYears" class="mb-2 text-sm font-semibold text-gray-700"
              >Years of Experience <span class="text-red-500">*</span></label
            >
            <input
              id="experienceYears"
              v-model.number="form.experienceYears"
              type="number"
              min="0"
              max="50"
              required
              class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
            />
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
                  <label :for="`price-${index}`" class="mb-2 text-sm font-semibold text-gray-700"
                    >Price per Hour (MDL) <span class="text-red-500">*</span></label
                  >
                  <input
                    :id="`price-${index}`"
                    v-model.number="subject.pricePerHour"
                    type="number"
                    step="0.01"
                    min="0"
                    required
                    placeholder="25.50"
                    class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
                  />
                </div>
                <div class="flex justify-end">
                  <button
                    type="button"
                    @click="removeSubject(index)"
                    :disabled="form.subjects.length === 1"
                    class="px-4 py-2 text-white transition-colors duration-300 bg-red-500 rounded-xl hover:bg-red-600 disabled:bg-gray-300 disabled:cursor-not-allowed"
                  >
                    Remove
                  </button>
                </div>
              </div>
            </div>

            <div v-if="availableSubjects.length > 0">
              <DropdownSelect
                :options="availableSubjects"
                placeholder="+ Add Another Subject"
                @update:modelValue="addNewSubject"
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
            />
          </div>
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
            class="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md resize-none"
            placeholder="Tell us about yourself, your teaching experience, and what makes you passionate about education..."
          ></textarea>
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

<script setup lang="ts">
  import { ref, watch, computed } from 'vue';
  import DropdownSelect from './DropdownSelect.vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { faTimes } from '@fortawesome/free-solid-svg-icons';
  import { createTutorProfile } from '../../services/tutorService.ts';
  import type { Subject, TutorProfileData, CreateProfileDto } from '../../services/tutorService.ts';
  import { useRouter } from 'vue-router';

  library.add(faTimes);

  const router = useRouter();

  const form = ref({
    firstName: '',
    lastName: '',
    username: '',
    phone: '',
    birthdate: '',
    country: '',
    city: '',
    experienceYears: 1,
    subjects: [
      {
        subjectName: 'Mathematics',
        pricePerHour: 0,
        currency: 'MDL',
      },
    ],
    languages: [] as string[],
    teachingPreferences: {
      myHome: false,
      studentHome: false,
      online: false,
    },
    bio: '',
  });

  const allLanguages = ['English', 'French', 'Spanish', 'German', 'Italian', 'Romanian'];
  const allSubjects = [
    'Mathematics',
    'Physics',
    'Computer Science',
    'Chemistry',
    'Biology',
    'English',
    'History',
    'Geography',
  ];

  const availableLanguages = computed(() => {
    return allLanguages.filter((lang) => !form.value.languages.includes(lang));
  });

  const availableSubjects = computed(() => {
    const selectedSubjects = form.value.subjects.map((s) => s.subjectName);
    return allSubjects.filter((subject) => !selectedSubjects.includes(subject));
  });

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
      return ['Bucharest', 'Cluj-Napoca', 'Iași'];
    } else if (form.value.country === 'Moldova') {
      return ['Chișinău', 'Bălți', 'Tiraspol'];
    }
    return [];
  });

  const addNewSubject = (subjectName: string) => {
    form.value.subjects.push({
      subjectName: subjectName,
      pricePerHour: 0,
      currency: 'MDL',
    });
  };

  const removeSubject = (index: number) => {
    if (form.value.subjects.length > 1) {
      form.value.subjects.splice(index, 1);
    }
  };

  watch(
    () => form.value.country,
    () => {
      form.value.city = '';
    },
  );

  const workingLocationId = computed(() => {
    const { myHome, studentHome, online } = form.value.teachingPreferences;

    if (myHome && online && studentHome) return 7; // Home + Online + Student Home
    if (myHome && online) return 4; // Home + Online
    if (myHome && studentHome) return 5; // Home + Student Home
    if (online && studentHome) return 6; // Online + Student Home
    if (myHome) return 1; // Home
    if (online) return 2; // Online
    if (studentHome) return 3; // Student Home

    return 0; // Nicio opțiune selectată
  });

  const handleSubmit = async () => {
    const birthdateFormatted = form.value.birthdate ? `${form.value.birthdate}T00:00:00` : '';

    const subjectsPayload: Subject[] = form.value.subjects.map((subject) => ({
      subjectName: subject.subjectName,
      subjectSlug: 'default-slug',
      pricePerHour: parseFloat(subject.pricePerHour.toString()),
      currency: 'MDL',
    }));

    const profileDto: CreateProfileDto = {
      phone: form.value.phone,
      username: form.value.username,
      firstName: form.value.firstName,
      lastName: form.value.lastName,
      bio: form.value.bio,
      birthdate: birthdateFormatted,
      country: form.value.country,
      city: form.value.city,
    };

    // Aici este corectat: am adaugat workingLocation si createProfileDto
    const payload: TutorProfileData = {
      verificationStatus: 'Pending',
      experienceYears: form.value.experienceYears,
      subjects: subjectsPayload,
      createProfileDto: profileDto,
      workingLocation: workingLocationId.value,
    };

    try {
      const response = await createTutorProfile(payload);
      console.log('Profil creat cu succes:', response);
      router.push('/tutor-dashboard');
    } catch (error) {
      console.error('Eroare la trimiterea datelor:', error);
    }
  };
</script>
