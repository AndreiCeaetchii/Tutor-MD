<template>
  <div class="p-6 lg:p-8 bg-gray-50 rounded-2xl">
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <div class="lg:col-span-2">
        <h2 class="text-2xl font-bold text-gray-800 mb-2">About Me</h2>
        <p class="text-sm text-gray-600 leading-relaxed">{{ bio }}</p>

        <h2 class="text-2xl font-bold text-gray-800 mt-6 mb-4">Subjects & Pricing</h2>
        <div class="space-y-4">
          <PricingCard
            v-for="(subject, index) in subjects"
            :key="index"
            :subject="subject.name"
            :price="subject.price"
            :availability="subject.availability"
          />
        </div>
      </div>

      <div class="lg:col-span-1 space-y-6">
        <div class="bg-white p-4 rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Contact Information</h3>
          <div class="text-sm text-gray-600 mt-2 space-y-3">
            <div class="flex items-center space-x-2">
              <font-awesome-icon :icon="['fas', 'phone']" class="text-gray-400" />
              <div>
                <p>Phone</p>
                <p class="font-medium text-gray-800">{{ phone }}</p>
              </div>
            </div>
            <div class="flex items-center space-x-2">
              <font-awesome-icon :icon="['fas', 'envelope']" class="text-gray-400" />
              <div>
                <p>Email</p>
                <p class="font-medium text-gray-800">{{ email }}</p>
              </div>
            </div>
          </div>
        </div>

        <div class="bg-white p-4 rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Experience</h3>
          <p class="text-sm text-gray-600 mt-2">{{ experience }} ani de experiență</p>
        </div>

        <div class="bg-white p-4 rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Languages</h3>
          <div class="flex flex-wrap gap-2 mt-2 text-sm">
            <span
              v-for="(lang, index) in languages"
              :key="index"
              class="px-3 py-1 bg-purple-100 text-[#5f22d9] rounded-full"
            >
              {{ lang }}
            </span>
          </div>
        </div>

        <button
          class="w-full bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] text-white font-semibold py-3 px-6 rounded-full shadow hover:opacity-90 transition-opacity"
        >
          Contact Tutor
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import PricingCard from './PricingCard.vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { faPhone, faEnvelope } from '@fortawesome/free-solid-svg-icons';
  import { defineProps } from 'vue';

  library.add(faPhone, faEnvelope);

  const props = defineProps({
    bio: {
      type: String,
      default:
        'Sunt un tutor pasionat de matematică și fizică cu peste 8 ani de experiență în predare. Îmi place să ajut studenții să înțeleagă conceptele complexe prin explicații clare și exemple practice.',
    },
    phone: {
      type: String,
      default: '+40 123 456 789',
    },
    email: {
      type: String,
      default: 'maria.popescu@email.com',
    },
    experience: {
      type: Number,
      default: 8,
    },
    languages: {
      type: Array,
      default: () => ['Română', 'Engleză', 'Franceză'],
    },
    subjects: {
      type: Array,
      default: () => [
        { name: 'Matematică', price: '50 RON', availability: 'Materie disponibilă' },
        { name: 'Fizică', price: '45 RON', availability: 'Materie disponibilă' },
        { name: 'Informatică', price: '60 RON', availability: 'Materie disponibilă' },
      ],
    },
  });
</script>
