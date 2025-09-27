<script setup lang="ts">
  import { ref } from 'vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { faPhone, faEnvelope, faTrash, faTimes } from '@fortawesome/free-solid-svg-icons';
  import DropdownSelect from './DropdownSelect.vue';
  import EditableSubjectCard from './EditableSubjectCard.vue';
  import { deleteSubject } from '../../../services/tutorService.ts';

  library.add(faPhone, faEnvelope, faTrash, faTimes);

  const props = defineProps({
    editedProfile: {
      type: Object,
      required: true,
    },
  });

  const selectedLanguage = ref('');

  const availableLanguages = ref([
    'Romanian',
    'English',
    'French',
    'German',
    'Spanish',
    'Italian',
    'Portuguese',
    'Russian',
    'Chinese',
    'Japanese',
  ]);

  const availableSubjects = ref([
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
    'Statistics'
  ]);

  const addSubject = (subjectName: string) => {
    if (
      subjectName &&
      !props.editedProfile.subjects.some((s: { name: string }) => s.name === subjectName)
    ) {
      props.editedProfile.subjects.push({
        name: subjectName,
        price: 0,
        currency: 'MDL',
        isNew: true,
        isModified: false,
      });
    }
  };

  const updateSubjectPrice = (index: number, newPrice: number) => {
    const subject = props.editedProfile.subjects[index];
    subject.price = newPrice;
    if (!subject.isNew) subject.isModified = true;
  };

  const updateSubjectCurrency = (index: number, newCurrency: string) => {
    const subject = props.editedProfile.subjects[index];
    subject.currency = newCurrency;
    if (!subject.isNew) subject.isModified = true;
  };

  const removeSubject = async (index: number) => {
    const subject = props.editedProfile.subjects[index];

    if (subject.subjectId) {
      try {
        await deleteSubject(subject.subjectId);
      } catch (err) {
        console.error('Error deleting subject:', err);
        return;
      }
    }

    props.editedProfile.subjects.splice(index, 1);
  };

  const addLanguage = (langName: string) => {
    if (langName && !props.editedProfile.languages.includes(langName)) {
      props.editedProfile.languages.push(langName);
    }
  };

  const removeLanguage = (index: number) => {
    props.editedProfile.languages.splice(index, 1);
  };
</script>

<template>
  <div class="p-6 lg:p-8 bg-gray-50 rounded-2xl">
    <div class="grid grid-cols-1 gap-6 lg:grid-cols-3">
      <div class="lg:col-span-2">
        <h2 class="mb-2 text-2xl font-bold text-gray-800">About Me</h2>
        <textarea
          v-model="editedProfile.bio"
          class="resize-none w-full h-32 p-4 text-sm text-gray-800 bg-gray-100 rounded-lg border border-gray-300 focus:outline-none focus:ring-1 focus:ring-[#5f22d9]"
        ></textarea>

        <h2 class="mt-6 mb-4 text-2xl font-bold text-gray-800">Subjects & Pricing</h2>
        <div class="space-y-4">
          <EditableSubjectCard
            v-for="(subject, index) in editedProfile.subjects"
            :key="index"
            :subject="subject"
            :index="index"
            @remove="removeSubject(index)"
            @update:price="updateSubjectPrice(index, $event)"
            @update:currency="updateSubjectCurrency(index, $event)"
          />
          <DropdownSelect
            :options="availableSubjects"
            placeholder="Add Subject"
            @update:modelValue="addSubject"
          />
        </div>
      </div>

      <div class="space-y-6 lg:col-span-1">
        <div class="p-4 bg-white rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Contact Information</h3>
          <div class="mt-2 space-y-3 text-sm text-gray-600">
            <div class="flex items-center space-x-2">
              <font-awesome-icon :icon="['fas', 'phone']" class="text-gray-400" />
              <div class="flex-1">
                <span class="text-xs text-gray-400">Phone</span>
                <input
                  v-model="editedProfile.phone"
                  type="text"
                  class="w-full font-medium text-gray-800 px-2 py-1 bg-gray-100 rounded-lg border border-gray-300 focus:outline-none focus:ring-1 focus:ring-[#5f22d9]"
                />
              </div>
            </div>

            <div class="flex items-center space-x-2">
              <font-awesome-icon :icon="['fas', 'envelope']" class="text-gray-400" />
              <div class="flex-1">
                <span class="text-xs text-gray-400">Username</span>
                <input
                  v-model="editedProfile.userName"
                  type="text"
                  class="w-full font-medium text-gray-800 px-2 py-1 bg-gray-100 rounded-lg border border-gray-300 focus:outline-none focus:ring-1 focus:ring-[#5f22d9]"
                />
              </div>
            </div>
          </div>
        </div>

        <div class="p-4 bg-white rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Languages</h3>
          <div class="flex flex-wrap gap-2 mt-2 text-sm">
            <div
              v-for="index in editedProfile.languages.length"
              :key="index"
              class="relative flex items-center bg-purple-100 text-[#5f22d9] rounded-full pr-8 pl-3 py-1"
            >
              <input
                v-model="editedProfile.languages[index]"
                type="text"
                class="w-24 text-sm font-medium bg-transparent focus:outline-none"
              />
              <button
                @click.prevent="removeLanguage(index)"
                class="absolute text-xs text-red-500 right-2 hover:text-red-700"
              >
                <font-awesome-icon :icon="['fas', 'times']" />
              </button>
            </div>
            <DropdownSelect
              :options="availableLanguages"
              placeholder="+ Add Language"
              v-model="selectedLanguage"
              @update:modelValue="addLanguage"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
