<script setup lang="ts">
  import { ref, onMounted, computed } from 'vue';
  import { fetchTutorSubjects } from '../../services/studentBookings';

  const props = defineProps<{
    isOpen: boolean;
    slot: {
      id: string;
      apiId?: number;
      startTime: string;
      endTime: string;
      date?: string;
      tutorSubjects?: { name: string; subjectId: number }[];
    };
    tutorId: number;
    availableLocations?: string[];
  }>();

  const emit = defineEmits<{
    (e: 'close'): void;
    (
      e: 'book',
      data: {
        subject: string;
        location: string;
        description: string;
      },
    ): void;
  }>();

  const formattedDate = computed(() => {
    if (!props.slot.date) return '';

    const date = new Date(props.slot.date);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${day}.${month}.${year}`;
  });

  const selectedSubject = ref('');
  const selectedLocation = ref('online');
  const description = ref('');
  const subjects = ref<{ name: string; subjectId: number }[]>([]);
  const isLoading = ref(false);

  const allLocationOptions = [
    { value: 'online', label: 'Online' },
    { value: 'student_home', label: "Student's Home" },
    { value: 'teacher_home', label: "Teacher's Home" },
  ];

  // Filter location options based on what the tutor offers
  const locationOptions = computed(() => {
    if (!props.availableLocations || props.availableLocations.length === 0) {
      // If no locations specified, show all options (backward compatibility)
      return allLocationOptions;
    }

    return allLocationOptions.filter((option) => props.availableLocations!.includes(option.value));
  });

  const subjectDropdownOpen = ref(false);
  const locationDropdownOpen = ref(false);

  onMounted(async () => {
    if (props.tutorId) {
      isLoading.value = true;
      try {
        if (props.slot.tutorSubjects && props.slot.tutorSubjects.length > 0) {
          subjects.value = props.slot.tutorSubjects;
        } else {
          subjects.value = await fetchTutorSubjects(props.tutorId);
        }

        if (subjects.value.length > 0) {
          selectedSubject.value = subjects.value[0].name;
        }

        // Set default location to first available option
        if (locationOptions.value.length > 0) {
          selectedLocation.value = locationOptions.value[0].value;
        }
      } catch (error) {
        console.error('Error loading tutor subjects:', error);
      } finally {
        isLoading.value = false;
      }
    }

    document.addEventListener('click', closeDropdowns);
  });

  const handleSubjectSelect = (subject: string) => {
    selectedSubject.value = subject;
    subjectDropdownOpen.value = false;
  };

  const handleLocationSelect = (location: string) => {
    selectedLocation.value = location;
    locationDropdownOpen.value = false;
  };

  const handleSubmit = () => {
    if (!selectedSubject.value) {
      return;
    }

    emit('book', {
      subject: selectedSubject.value,
      location: selectedLocation.value,
      description: description.value,
    });
  };

  const closeDropdowns = (e: Event) => {
    const target = e.target as HTMLElement;
    if (!target.closest('.subject-dropdown')) {
      subjectDropdownOpen.value = false;
    }
    if (!target.closest('.location-dropdown')) {
      locationDropdownOpen.value = false;
    }
  };
</script>

<template>
  <div
    v-if="isOpen"
    class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50"
  >
    <div class="relative w-full max-w-md p-6 bg-white rounded-lg shadow-xl">
      <div class="mb-4 text-center">
        <h3 class="text-xl font-semibold text-gray-800">Book Lesson</h3>
        <p class="text-gray-600">
          {{ slot.startTime }} - {{ slot.endTime }} on {{ formattedDate }}
        </p>
      </div>

      <div v-if="isLoading" class="flex justify-center py-8">
        <div
          class="w-8 h-8 border-4 border-purple-500 rounded-full border-t-transparent animate-spin"
        ></div>
      </div>

      <form v-else @submit.prevent="handleSubmit" class="space-y-4">
        <div class="space-y-1 subject-dropdown">
          <label class="text-sm font-medium text-gray-700">Subject</label>
          <div class="relative">
            <div
              @click="subjectDropdownOpen = !subjectDropdownOpen"
              class="flex items-center justify-between w-full px-3 py-2 bg-white border border-gray-300 rounded-md cursor-pointer focus:outline-none focus:ring-2 focus:ring-purple-500"
            >
              <span>{{ selectedSubject || 'Select a subject' }}</span>
              <svg
                class="w-5 h-5 ml-2 text-gray-400"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M19 9l-7 7-7-7"
                ></path>
              </svg>
            </div>

            <div
              v-if="subjectDropdownOpen"
              class="absolute z-10 w-full mt-1 bg-white border border-gray-300 rounded-md shadow-lg"
            >
              <div
                v-for="subject in subjects"
                :key="subject.subjectId"
                @click="handleSubjectSelect(subject.name)"
                class="flex items-center px-3 py-2 cursor-pointer hover:bg-gray-100"
              >
                <svg
                  v-if="selectedSubject === subject.name"
                  class="w-5 h-5 mr-2 text-blue-500"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M5 13l4 4L19 7"
                  ></path>
                </svg>
                <span v-else class="w-5 h-5 mr-2"></span>
                {{ subject.name }}
              </div>
            </div>
          </div>
        </div>

        <div class="space-y-1 location-dropdown">
          <label class="text-sm font-medium text-gray-700">Location</label>
          <div class="relative">
            <div
              @click="locationDropdownOpen = !locationDropdownOpen"
              class="flex items-center justify-between w-full px-3 py-2 bg-white border border-gray-300 rounded-md cursor-pointer focus:outline-none focus:ring-2 focus:ring-purple-500"
            >
              <span>{{
                locationOptions.find((o) => o.value === selectedLocation)?.label ||
                'Select location'
              }}</span>
              <svg
                class="w-5 h-5 ml-2 text-gray-400"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M19 9l-7 7-7-7"
                ></path>
              </svg>
            </div>

            <div
              v-if="locationDropdownOpen"
              class="absolute z-10 w-full mt-1 bg-white border border-gray-300 rounded-md shadow-lg"
            >
              <div
                v-for="option in locationOptions"
                :key="option.value"
                @click="handleLocationSelect(option.value)"
                class="flex items-center px-3 py-2 cursor-pointer hover:bg-gray-100"
              >
                <svg
                  v-if="selectedLocation === option.value"
                  class="w-5 h-5 mr-2 text-blue-500"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M5 13l4 4L19 7"
                  ></path>
                </svg>
                <span v-else class="w-5 h-5 mr-2"></span>
                {{ option.label }}
              </div>
            </div>
          </div>
        </div>

        <div class="space-y-1">
          <label class="text-sm font-medium text-gray-700">Description (optional)</label>
          <textarea
            v-model="description"
            rows="3"
            placeholder="Additional details about your lesson..."
            class="w-full px-3 py-2 bg-white border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-purple-500"
          ></textarea>
        </div>

        <div class="flex justify-end pt-2 space-x-3">
          <button
            type="button"
            @click="emit('close')"
            class="px-4 py-2 text-sm font-medium text-gray-700 bg-gray-100 rounded-md hover:bg-gray-200"
          >
            Cancel
          </button>
          <button
            type="submit"
            class="px-4 py-2 text-sm font-medium text-white bg-purple-600 rounded-md hover:bg-purple-700"
          >
            Book Lesson
          </button>
        </div>
      </form>

      <button
        @click="emit('close')"
        class="absolute text-gray-400 top-2 right-2 hover:text-gray-600"
        aria-label="Close"
      >
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M6 18L18 6M6 6l12 12"
          ></path>
        </svg>
      </button>
    </div>
  </div>
</template>

<style scoped>
  .subject-dropdown,
  .location-dropdown {
    position: relative;
  }
</style>
