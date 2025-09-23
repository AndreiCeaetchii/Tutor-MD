<template>
  <div class="min-h-screen bg-gradient-to-br from-indigo-50 via-purple-50 to-blue-50 p-6 lg:p-12">
    <div class="max-w-4xl mx-auto">
      <form @submit.prevent="handleSubmit" class="space-y-8">
        <div
          class="bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] p-8 rounded-2xl shadow-2xl text-center"
        >
          <h1 class="text-4xl font-bold text-white mb-2">Complete Your Student Profile</h1>
          <p class="text-purple-100">Let's create your academic profile together</p>
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
                v-model="form.createProfileDto.firstName"
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
                v-model="form.createProfileDto.lastName"
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
                v-model="form.createProfileDto.username"
                type="text"
                required
                placeholder="e.g., john_student"
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="phone" class="text-sm font-semibold text-gray-700 mb-2"
                >Phone <span class="text-red-500">*</span></label
              >
              <input
                id="phone"
                v-model="form.createProfileDto.phone"
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
                v-model="form.createProfileDto.birthdate"
                type="date"
                required
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
          </div>
        </section>

        <section
          class="bg-white/80 backdrop-blur-sm border border-white/50 p-8 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300"
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
              Academic Details
            </h2>
          </div>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div class="flex flex-col">
              <label for="grade" class="text-sm font-semibold text-gray-700 mb-2"
                >Current Grade</label
              >
              <input
                id="grade"
                v-model.number="form.grade"
                type="number"
                min="1"
                max="12"
                class="px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md"
              />
            </div>
            <div class="flex flex-col">
              <label for="class" class="text-sm font-semibold text-gray-700 mb-2"
                >Current Class</label
              >
              <input
                id="class"
                v-model.number="form.class"
                type="number"
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
              class="w-8 h-8 bg-gradient-to-r from-yellow-500 to-orange-500 rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-white font-bold text-sm">3</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-yellow-500 to-orange-500 bg-clip-text text-transparent"
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
                @update:modelValue="form.createProfileDto.country = $event"
              />
              <div v-if="form.createProfileDto.country" class="mt-4">
                <span
                  class="px-5 py-2 bg-gradient-to-r from-emerald-100 to-teal-100 border border-emerald-200 text-[#5f22d9] rounded-full flex items-center gap-3 shadow-sm font-medium transition-all duration-300"
                >
                  {{ form.createProfileDto.country }}
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
                @update:modelValue="form.createProfileDto.city = $event"
              />
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
          class="bg-white/80 backdrop-blur-sm border border-white/50 p-8 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300"
        >
          <div class="flex items-center mb-6">
            <div
              class="w-8 h-8 bg-gradient-to-r from-pink-500 to-rose-500 rounded-full flex items-center justify-center mr-3"
            >
              <span class="text-white font-bold text-sm">4</span>
            </div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-pink-500 to-rose-500 bg-clip-text text-transparent"
            >
              A brief introduction
            </h2>
          </div>
          <textarea
            v-model="form.createProfileDto.bio"
            rows="6"
            class="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:border-[#5f22d9] outline-none bg-white shadow-sm hover:shadow-md resize-none"
            placeholder="Tell us about your academic interests, goals, and what you're looking for in a tutor..."
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
  import DropdownSelect from '../../profile/DropdownSelect.vue';
  import { useRouter } from 'vue-router';
  import { useStudentProfileStore } from '../../../store/studentProfileStore.ts';
  import { createStudentProfile } from '../../../services/studentService.ts';

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

  const form = ref<StudentProfileData>({
    grade: 8,
    class: 11,
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

  const cities = computed(() => {
    if (form.value.createProfileDto.country === 'Romania') {
      return ['Bucharest', 'Cluj-Napoca', 'Iași'];
    } else if (form.value.createProfileDto.country === 'Moldova') {
      return ['Chișinău', 'Bălți', 'Tiraspol'];
    }
    return [];
  });

  watch(
    () => form.value.createProfileDto.country,
    () => {
      form.value.createProfileDto.city = '';
    },
  );

  const handleSubmit = async () => {
    const birthdateFormatted = form.value.createProfileDto.birthdate
      ? `${form.value.createProfileDto.birthdate}T00:00:00`
      : '';

    const payload: StudentProfileData = {
      grade: form.value.grade,
      class: form.value.class,
      createProfileDto: {
        ...form.value.createProfileDto,
        birthdate: birthdateFormatted,
      },
    };

    try {
      const response = await createStudentProfile(payload);
      console.log('Profil de student creat cu succes:', response);

      studentProfileStore.updateGradeAndClass(form.value.grade, form.value.class);
      studentProfileStore.updateUserProfile(payload.createProfileDto);

      router.push('/student-dashboard');
    } catch (error) {
      console.error('Eroare la crearea profilului de student:', error);
    }
  };
</script>
