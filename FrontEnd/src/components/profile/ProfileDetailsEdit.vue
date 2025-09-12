<template>
  <div class="p-6 lg:p-8 bg-gray-50 rounded-2xl">
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <div class="lg:col-span-2">
        <h2 class="text-2xl font-bold text-gray-800 mb-2">About Me</h2>
        <textarea
          v-model="editedProfile.bio"
          class="resize-none w-full h-32 p-4 text-sm text-gray-800 bg-gray-100 rounded-lg border border-gray-300 focus:outline-none focus:ring-1 focus:ring-[#5f22d9]"
        ></textarea>

        <h2 class="text-2xl font-bold text-gray-800 mt-6 mb-4">Subjects & Pricing</h2>
        <div class="space-y-4">
          <EditableSubjectCard
            v-for="(subject, index) in editedProfile.subjects"
            :key="index"
            :subject="subject"
            :index="index"
            @remove="removeSubject(index)"
            @update:currency="subject.currency = $event"
            @update:price="subject.price = $event"
          />
          <DropdownSelect
            :options="availableSubjects"
            placeholder="Add Subject"
            @update:modelValue="addSubject"
          />
        </div>
      </div>

      <div class="lg:col-span-1 space-y-6">
        <div class="bg-white p-4 rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Contact Information</h3>
          <div class="text-sm text-gray-600 mt-2 space-y-3">
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
          </div>
        </div>

        <div class="bg-white p-4 rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Languages</h3>
          <div class="flex flex-wrap gap-2 mt-2 text-sm">
            <div
              v-for="(lang, index) in editedProfile.languages"
              :key="index"
              class="relative flex items-center bg-purple-100 text-[#5f22d9] rounded-full pr-8 pl-3 py-1"
            >
              <input
                v-model="editedProfile.languages[index]"
                type="text"
                class="bg-transparent text-sm font-medium w-24 focus:outline-none"
              />
              <button
                @click.prevent="removeLanguage(index)"
                class="absolute right-2 text-xs text-red-500 hover:text-red-700"
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

<script setup lang="ts">
  import { defineProps, ref, computed } from 'vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { faPhone, faEnvelope, faTrash, faTimes } from '@fortawesome/free-solid-svg-icons';
  import DropdownSelect from './DropdownSelect.vue';
  import EditableSubjectCard from './EditableSubjectCard.vue';

  library.add(faPhone, faEnvelope, faTrash, faTimes);

  const props = defineProps({
    editedProfile: {
      type: Object,
      required: true,
    },
  });

  const selectedLanguage = ref('');
  const selectedSubject = ref('');

  const availableLanguages = ref([
    'Română',
    'Engleză',
    'Franceză',
    'Germană',
    'Spaniolă',
    'Italiană',
    'Rusă',
    'Chineză',
    'Japoneză',
  ]);

  const availableSubjects = ref([
    'Matematică',
    'Fizică',
    'Chimie',
    'Biologie',
    'Istorie',
    'Geografie',
    'Limba și Literatura Română',
    'Limba Engleză',
    'Informatică',
  ]);

  const addSubject = (subjectName: string) => {
    if (subjectName && !props.editedProfile.subjects.some((s) => s.name === subjectName)) {
      props.editedProfile.subjects.push({
        name: subjectName,
        price: 0,
        currency: 'MDL',
      });
    }
  };

  const removeSubject = (index: number) => {
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
