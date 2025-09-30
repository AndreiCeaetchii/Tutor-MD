<script setup lang="ts">
import { ref, onMounted } from "vue";
import Hero1 from "../components/landing/Hero1.vue";
import Hero3 from "../components/landing/Hero3.vue";
import Footer from "../components/Footer.vue";
import { DotLottieVue } from "@lottiefiles/dotlottie-vue";
import CardCarousel from "../components/landing/CardCarousel.vue";
import InstructorCard from "../components/landing/InstructorCard.vue";
import { useRouter } from "vue-router";

const router = useRouter();

const featuredTutors = ref([
  {
    id: 1,
    name: 'Dr. Evelyn Reed',
    location: 'Cambridge, MA',
    hourlyRate: '950',
    rating: 4.8,
    reviews: 1250,
    profileImage: 'https://public-cdn-s3-us-west-2.oss-us-east-1.aliyuncs.com/talkie-user-img/168959871254650/189887322947984.jpeg?x-oss-process=image/resize,w_1024/format,webp',
    imageUrl: 'https://www.business-academy.ro/cms/mestoZaUploadFajlove/cambridge-university.jpg',
  },
  {
    id: 2,
    name: 'Mihai Popescu',
    location: 'Cluj, RO',
    hourlyRate: '350',
    rating: 5.0,
    reviews: 200,
    profileImage: 'https://www.upet.ro/profile/improf/MII/Popescu-Stelea.Mihai.jpg',
    imageUrl: 'https://images.unsplash.com/photo-1517048676732-d65bc937f952?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1470&q=80',
  },
  {
    id: 3,
    name: 'Frederick Hicks',
    location: 'Fresno, OH',
    hourlyRate: '820',
    rating: 4.5,
    reviews: 4448,
    profileImage: 'https://images.unsplash.com/photo-1517841905240-472988babdf9?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=687&q=80',
    imageUrl: 'https://upload.wikimedia.org/wikipedia/commons/thumb/8/8e/Staples_High_School%2C_Westport%2C_CT.jpg/500px-Staples_High_School%2C_Westport%2C_CT.jpg',
  },
  {
    id: 4,
    name: 'Anna Schmidt',
    location: 'Berlin, DE',
    hourlyRate: '550',
    rating: 4.9,
    reviews: 980,
    profileImage: 'https://upload.wikimedia.org/wikipedia/commons/8/80/Anna_Schmidt2.jpg',
    imageUrl: 'https://api.mygermanuniversity.com/images/article_images/019d385eb67632a7e958e23f24bd07d7_vNS5iRAGHcSE1594021547.jpeg',
  },
]);

// stare pentru loader
// const loading = ref(true);

// onMounted(() => {
//   // Simulăm timpul de încărcare (ex: fetch API, assets, etc.)
//   setTimeout(() => {
//     loading.value = false;
//   }, 1500); // 1.5 secunde
// });
</script>

<template>

  <div>
    <!-- Loader pe tot ecranul -->
    <!--    <div v-if="loading" class="fixed inset-0 flex items-center justify-center bg-gray-50">-->
    <!--      <DotLottieVue autoplay loop class="w-72 h-72" src="https://lottie.host/72ca364b-846a-4290-aa54-2846d475d657/T8ZWCsBzkS.lottie"/>-->
    <!--    </div>-->

    <!-- Conținutul real -->

    <div>
      <Hero1 />
      <!-- Section 2-->
      <div class="py-16 bg-white sm:py-24">
        <div class="relative px-4 mx-auto text-center max-w-7xl sm:px-6 lg:px-8">
          <div class="flex justify-center mb-6">
            <svg width="60" height="25" viewBox="0 0 80 30" class="text-orange-500 ">
              <path d="M0,15 C10,5 20,25 40,15 C60,5 70,25 80,15" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" />
            </svg>
          </div>

          <p class="text-sm font-bold tracking-widest text-orange-500 uppercase">
            Our featured Tutors
          </p>
          <h2 class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
            Every tutor is <span class="text-[#5f22d9]">professional</span><br class="hidden lg:inline" /> and highly qualified
          </h2>

          <p class="max-w-2xl mx-auto mt-4 text-base text-gray-500">
            We connect tutors with students to make learning easier and more engaging. Our platform helps build knowledge with guidance, trust, and support.
          </p>

          <div class="mt-12">
            <CardCarousel>
              <!-- ITERAREA AICI -->
              <InstructorCard
                v-for="tutor in featuredTutors"
                :key="tutor.id"
                :name="tutor.name"
                :location="tutor.location"
                :hourlyRate="tutor.hourlyRate"
                :rating="tutor.rating"
                :reviews="tutor.reviews"
                :profileImage="tutor.profileImage"
                :imageUrl="tutor.imageUrl"
              />
            </CardCarousel>
          </div>

          <div class="mt-1">
            <button
              @click="router.push('/signup')"
              class="inline-flex items-center rounded-full border border-transparent bg-[#5f22d9] px-6 py-3 font-semibold text-white shadow-sm transition-colors hover:bg-purple-700 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:ring-offset-2">
              Explore all tutors
              <svg
                class="w-5 h-5 ml-2 -mr-1"
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 20 20"
                fill="currentColor"
                aria-hidden="true"
              >
                <path
                  fill-rule="evenodd"
                  d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"
                  clip-rule="evenodd"
                />
              </svg>
            </button>
          </div>
        </div>
      </div>
      <Hero3 />


      <Footer />
    </div>
  </div>
</template>
