<template>
  <div class="bg-gradient-to-br from-purple-50 to-white p-8 flex justify-center items-center h-full overflow-y-auto">
    <div class="w-full max-w-lg mx-auto"> <!-- Am adăugat mx-auto și mărit max-w-lg -->
      <!-- Wavy SVG -->
      <div class="mb-6">
        <svg width="60" height="25" viewBox="0 0 80 30" class="text-orange-500">
          <path d="M0,15 C10,5 20,25 40,15 C60,5 70,25 80,15" fill="none" stroke="currentColor" stroke-width="4" stroke-linecap="round" />
        </svg>
      </div>
      
      <!-- Subtitle -->
      <p class="text-base text-gray-700 font-medium mb-1">
        Why our working is so unique
      </p>
      
      <!-- Main heading -->
      <h1 class="text-4xl font-bold text-gray-800 mb-10">
        See how our working process easily adapt your need
      </h1>
      
      <!-- Feature 1 -->
      <div class="flex items-start mb-8">
        <div class="h-14 w-14 rounded-full bg-purple-100 flex items-center justify-center flex-shrink-0">
          <font-awesome-icon :icon="['fas', 'smile']" class="text-2xl text-purple-500" />
        </div>
        <div class="ml-4 flex-1"> <!-- Am adăugat flex-1 pentru distribuție mai bună a spațiului -->
          <h3 class="text-xl font-semibold text-gray-800 mb-2">User friendly hiring process</h3>
          <p class="text-gray-600">
            Accuscamus etmaes iusto odiomae dignissimos ducimus quistames blanditiis praesentium voluptatum loramkes anuten.
          </p>
        </div>
      </div>
      
      <!-- Feature 2 -->
      <div class="flex items-start mb-8">
        <div class="h-14 w-14 rounded-full bg-green-100 flex items-center justify-center flex-shrink-0">
          <font-awesome-icon :icon="['fas', 'umbrella']" class="text-2xl text-green-500" />
        </div>
        <div class="ml-4 flex-1"> <!-- Am adăugat flex-1 -->
          <h3 class="text-xl font-semibold text-gray-800 mb-2">Verified process with ease</h3>
          <p class="text-gray-600">
            Accuscamus etmaes iusto odiomae dignissimos ducimus quistames blanditiis praesentium voluptatum loramkes anuten.
          </p>
        </div>
      </div>
      
      <!-- Feature 3 -->
      <div class="flex items-start">
        <div class="h-14 w-14 rounded-full bg-red-100 flex items-center justify-center flex-shrink-0">
          <font-awesome-icon :icon="['fas', 'shield-alt']" class="text-2xl text-red-400" />
        </div>
        <div class="ml-4 flex-1"> <!-- Am adăugat flex-1 -->
          <h3 class="text-xl font-semibold text-gray-800 mb-2">Secure payment gateway integrated</h3>
          <p class="text-gray-600">
            Accuscamus etmaes iusto odiomae dignissimos ducimus quistames blanditiis praesentium voluptatum loramkes anuten.
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faArrowRight, faSmile, faUmbrella, faShieldAlt } from '@fortawesome/free-solid-svg-icons';

library.add(faArrowRight, faSmile, faUmbrella, faShieldAlt);
</script>