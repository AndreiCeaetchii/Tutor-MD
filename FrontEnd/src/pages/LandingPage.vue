<script setup lang="ts">
import { ref, onMounted } from "vue";
import Hero1 from "../components/Hero1.vue";
import Hero3 from "../components/Hero3.vue";
import Footer from "../components/Footer.vue";
import { DotLottieVue } from "@lottiefiles/dotlottie-vue";
import InstructorCard from "../components/InstructorCard.vue";
import CardCarousel from "../components/CardCarousel.vue";

// stare pentru loader
const loading = ref(true);

onMounted(() => {
  // Simulăm timpul de încărcare (ex: fetch API, assets, etc.)
  setTimeout(() => {
    loading.value = false;
  }, 1500); // 1.5 secunde
});
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
        <div class="py-16 bg-white sm:py-24">
          <div class="relative mx-auto max-w-7xl px-4 text-center sm:px-6 lg:px-8">
            <div class="mb-6 flex justify-center">
              <svg width="60" height="25" viewBox="0 0 80 30" class="text-orange-500 ">
                <path d="M0,15 C10,5 20,25 40,15 C60,5 70,25 80,15" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" />
              </svg>
            </div>

            <p class="font-bold uppercase text-orange-500 text-sm tracking-widest">
              Our featured instructors
            </p>
            <h2 class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
              Every instructor is <span class="text-[#5f22d9]">professional</span><br class="hidden lg:inline" /> and highly qualified
            </h2>

            <p class="mx-auto mt-4 max-w-2xl text-base text-gray-500">
              Accusamus et iusidio dignissimos ducimus blanditiis praesentium voluptatum deleniti atque corrupti quos dolores etimquasa molestias epturi sint occaecati cupiditate non providentep.
            </p>

            <div class="mt-12">
              <CardCarousel>
                <instructorCard />
                <instructorCard />
                <instructorCard />
                <instructorCard />
                <instructorCard />
              </CardCarousel>
            </div>

            <div class="mt-1">
              <button
                  class="inline-flex items-center rounded-full border border-transparent bg-[#5f22d9] px-6 py-3 font-semibold text-white shadow-sm transition-colors hover:bg-purple-700 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:ring-offset-2">
                Explore all instructors
                <svg
                    class="-mr-1 ml-2 h-5 w-5"
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
