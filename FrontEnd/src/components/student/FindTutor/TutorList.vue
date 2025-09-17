<script setup lang="ts">
import { ref, computed } from 'vue';
import TutorCard from './TutorCards.vue';
import TutorPagination from './TutorPagination.vue';

// Mock data pentru tutori, pe viitor va fi înlocuit cu date de la API
const tutors = ref([
  {
    id: 1,
    name: 'Mistie Monge',
    location: 'Phoenix, MN',
    hourlyRate: '712.93',
    rating: 5.0,
    reviews: 4448,
    profileImage: 'https://randomuser.me/api/portraits/women/44.jpg',
    description: "Hello there! My name is Mistie and I'm currently a pre-medical student. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
    services: ['My home', "Student's home", 'Online'],
    saved: true
  },
  {
    id: 2,
    name: 'Jamie Armstrong',
    location: 'New Orleans, OR',
    hourlyRate: '610.29',
    rating: 5.0,
    reviews: 4448,
    profileImage: 'https://randomuser.me/api/portraits/women/25.jpg',
    description: "Hello there! My name is Jamie and I'm currently a pre-medical student. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
    services: ['My home', "Student's home", 'Online'],
    saved: false
  },
  {
    id: 3,
    name: 'Nathaniel Fletcher',
    location: 'San Francisco, CA',
    hourlyRate: '481.80',
    rating: 4.5,
    reviews: 2356,
    profileImage: 'https://randomuser.me/api/portraits/men/11.jpg',
    description: "Hello there! My name is Nathaniel and I'm currently a computer science graduate. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
    services: ['My home', 'Online'],
    saved: false
  },
  {
    id: 4,
    name: 'Elizabeth Leonard',
    location: 'Tampa, LA',
    hourlyRate: '475.60',
    rating: 4.8,
    reviews: 3189,
    profileImage: 'https://randomuser.me/api/portraits/women/33.jpg',
    description: "Hello there! My name is Elizabeth and I'm currently an English literature major. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
    services: ['My home', "Student's home", 'Online'],
    saved: true
  },
  {
    id: 5,
    name: 'Bernard Ketler',
    location: 'Austin, AZ',
    hourlyRate: '505.80',
    rating: 4.2,
    reviews: 1246,
    profileImage: 'https://randomuser.me/api/portraits/men/45.jpg',
    description: "Hello there! My name is Bernard and I'm currently a mathematics professor. I have a deep passion for teaching and I am looking for students, mainly primary, to help them get ahead of school and ace their grades!",
    services: ["Student's home", 'Online'],
    saved: false
  }
]);

// Pagination logic
const currentPage = ref(1);
const tutorsPerPage = ref(3); // Număr de tutori pe pagină

const totalPages = computed(() => Math.ceil(tutors.value.length / tutorsPerPage.value));

const paginatedTutors = computed(() => {
  const start = (currentPage.value - 1) * tutorsPerPage.value;
  const end = start + tutorsPerPage.value;
  return tutors.value.slice(start, end);
});

const changePage = (page: number) => {
  currentPage.value = page;
  // Aici putem adăuga scrollToTop sau alte acțiuni la schimbarea paginii
};

// Pentru a gestiona tutorul care a fost salvat/desalvat
const handleTutorSaveChange = (tutorId: number, isSaved: boolean) => {
  const tutorIndex = tutors.value.findIndex(tutor => tutor.id === tutorId);
  if (tutorIndex !== -1) {
    tutors.value[tutorIndex].saved = isSaved;
  }
};
</script>

<template>
  <div>
    <div v-for="tutor in paginatedTutors" :key="tutor.id">
      <TutorCard 
        :name="tutor.name"
        :location="tutor.location"
        :hourlyRate="tutor.hourlyRate"
        :rating="tutor.rating"
        :reviews="tutor.reviews"
        :profileImage="tutor.profileImage"
        :description="tutor.description"
        :services="tutor.services"
        :saved="tutor.saved"
        @save-toggled="handleTutorSaveChange(tutor.id, $event)"
      />
    </div>
    
    <TutorPagination
      :currentPage="currentPage"
      :totalPages="totalPages"
      @page-changed="changePage"
    />
  </div>
</template>
