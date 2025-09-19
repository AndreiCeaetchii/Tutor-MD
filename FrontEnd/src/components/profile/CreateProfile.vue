<template>
  <div class="min-h-screen bg-gradient-to-br from-indigo-50 via-purple-50 to-blue-50 p-6 lg:p-12">
    <div class="max-w-4xl mx-auto">
      <form @submit.prevent="handleSubmit" class="space-y-8">
        <div
          class="bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] p-8 rounded-2xl shadow-2xl text-center"
        >
          <h1 class="text-4xl font-bold text-white mb-2">Complete Your Profile</h1>
          <p class="text-purple-100">Let's create your teaching profile together</p>
        </div>

        <section
          class="bg-white/80 backdrop-blur-sm border border-white/50 p-8 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300"
        >
          <div class="flex items-center mb-6">
            <div
              class="w-8 h-8 bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-white font-bold text-sm">1</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] bg-clip-text text-transparent"
            >
              Personal Details
            </h2>
          </div>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div class="flex flex-col">
              <label for="firstName" class="text-sm font-semibold text-gray-700 mb-2"
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
              <label for="lastName" class="text-sm font-semibold text-gray-700 mb-2"
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
              <label for="username" class="text-sm font-semibold text-gray-700 mb-2"
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
              <label for="phone" class="text-sm font-semibold text-gray-700 mb-2"
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
              <label for="birthdate" class="text-sm font-semibold text-gray-700 mb-2"
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
          class="bg-white/80 backdrop-blur-sm border border-white/50 p-8 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300 relative z-40"
        >
          <div class="flex items-center mb-6">
            <div
              class="w-8 h-8 bg-gradient-to-r from-emerald-500 to-teal-500 rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-white font-bold text-sm">2</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-emerald-500 to-teal-500 bg-clip-text text-transparent"
            >
              Location Information
            </h2>
          </div>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div class="flex flex-col">
              <label for="country" class="text-sm font-semibold text-gray-700 mb-2"
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
              <label for="city" class="text-sm font-semibold text-gray-700 mb-2"
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
          class="bg-white/80 backdrop-blur-sm border border-white/50 p-8 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300"
        >
          <div class="flex items-center mb-6">
            <div
              class="w-8 h-8 bg-gradient-to-r from-yellow-500 to-orange-500 rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-white font-bold text-sm">3</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-yellow-500 to-orange-500 bg-clip-text text-transparent"
            >
              Teaching Experience
            </h2>
          </div>
          <div class="flex flex-col">
            <label for="experienceYears" class="text-sm font-semibold text-gray-700 mb-2"
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
          class="bg-white/80 backdrop-blur-sm border border-white/50 p-8 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300 relative z-40"
        >
          <div class="flex items-center mb-6">
            <div
              class="w-8 h-8 bg-gradient-to-r from-purple-500 to-indigo-500 rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-white font-bold text-sm">4</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-purple-500 to-indigo-500 bg-clip-text text-transparent"
            >
              Subjects & Pricing
            </h2>
          </div>
          <div class="space-y-6">
            <div
              v-for="(subject, index) in form.subjects"
              :key="index"
              class="bg-gradient-to-r from-purple-50 to-indigo-50 p-6 rounded-xl border border-purple-100"
            >
              <div class="grid grid-cols-1 md:grid-cols-3 gap-4 items-end">
                <div class="flex flex-col">
                  <label class="text-sm font-semibold text-gray-700 mb-2">Subject</label>
                  <div
                    class="px-4 py-3 border-2 border-gray-200 rounded-xl bg-white shadow-sm hover:shadow-md font-medium text-gray-800"
                  >
                    {{ subject.subjectName }}
                  </div>
                </div>
                <div class="flex flex-col">
                  <label :for="`price-${index}`" class="text-sm font-semibold text-gray-700 mb-2"
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
                    class="px-4 py-2 bg-red-500 text-white rounded-xl hover:bg-red-600 transition-colors duration-300 disabled:bg-gray-300 disabled:cursor-not-allowed"
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
          class="bg-white/80 backdrop-blur-sm border border-white/50 p-8 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300 relative z-30"
        >
          <div class="flex items-center mb-6">
            <div
              class="w-8 h-8 bg-gradient-to-r from-orange-500 to-red-500 rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-white font-bold text-sm">5</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-orange-500 to-red-500 bg-clip-text text-transparent"
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
                class="cursor-pointer text-gray-400 hover:text-red-500 transition-colors duration-200 w-4 h-4"
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
          class="bg-white/80 backdrop-blur-sm border border-white/50 p-8 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300"
        >
          <div class="flex items-center mb-6">
            <div
              class="w-8 h-8 bg-gradient-to-r from-blue-500 to-cyan-500 rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-white font-bold text-sm">6</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-blue-500 to-cyan-500 bg-clip-text text-transparent"
            >
              I can teach on
            </h2>
          </div>
          <div class="flex flex-wrap gap-6">
            <div
              class="flex items-center space-x-3 bg-gradient-to-r from-blue-50 to-indigo-50 px-4 py-3 rounded-xl border border-blue-100 hover:shadow-md transition-all duration-300"
            >
              <input
                id="myHome"
                type="checkbox"
                v-model="form.teachingPreferences.myHome"
                class="rounded-lg text-[#5f22d9] focus:ring-[#5f22d9] w-4 h-4"
              />
              <label for="myHome" class="text-gray-700 font-medium">My home</label>
            </div>
            <div
              class="flex items-center space-x-3 bg-gradient-to-r from-green-50 to-emerald-50 px-4 py-3 rounded-xl border border-green-100 hover:shadow-md transition-all duration-300"
            >
              <input
                id="studentHome"
                type="checkbox"
                v-model="form.teachingPreferences.studentHome"
                class="rounded-lg text-[#5f22d9] focus:ring-[#5f22d9] w-4 h-4"
              />
              <label for="studentHome" class="text-gray-700 font-medium">Student's home</label>
            </div>
            <div
              class="flex items-center space-x-3 bg-gradient-to-r from-purple-50 to-pink-50 px-4 py-3 rounded-xl border border-purple-100 hover:shadow-md transition-all duration-300"
            >
              <input
                id="online"
                type="checkbox"
                v-model="form.teachingPreferences.online"
                class="rounded-lg text-[#5f22d9] focus:ring-[#5f22d9] w-4 h-4"
              />
              <label for="online" class="text-gray-700 font-medium">Online</label>
            </div>
          </div>
        </section>

        <section
          class="bg-white/80 backdrop-blur-sm border border-white/50 p-8 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300"
        >
          <div class="flex items-center mb-6">
            <div
              class="w-8 h-8 bg-gradient-to-r from-pink-500 to-rose-500 rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-white font-bold text-sm">7</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-pink-500 to-rose-500 bg-clip-text text-transparent"
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
              class="absolute inset-0 bg-gradient-to-r from-white/20 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300"
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
    languages: [],
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

    const payload: TutorProfileData = {
      verificationStatus: 'verified',
      experienceYears: form.value.experienceYears,
      subjects: subjectsPayload,
      createProfileDto: profileDto,
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
