<script setup lang="ts">
  import { useProfileStore } from '../../../store/profileStore.ts';
  import { useRouter } from 'vue-router';
  import PricingCard from './PricingCard.vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { faPhone, faEnvelope, faLock } from '@fortawesome/free-solid-svg-icons';

  library.add(faPhone, faEnvelope, faLock);

  const profileStore = useProfileStore();
  const router = useRouter();
</script>

<template>
  <div class="p-6 lg:p-8 bg-gray-50 rounded-2xl">
    <div class="grid grid-cols-1 gap-6 lg:grid-cols-3">
      <div class="lg:col-span-2">
        <h2 class="mb-2 text-2xl font-bold text-gray-800">About Me</h2>
        <p class="text-sm leading-relaxed text-gray-600">{{ profileStore.bio }}</p>

        <h2 class="mt-6 mb-4 text-2xl font-bold text-gray-800">Subjects & Pricing</h2>
        <div class="space-y-4">
          <PricingCard
            v-for="(subject, index) in profileStore.subjects"
            :key="index"
            :subject="subject.name"
            :price="`${subject.price} ${subject.currency}`"
          />
        </div>
      </div>

      <div class="space-y-6 lg:col-span-1">
        <div class="p-4 bg-white rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Contact Information</h3>
          <div class="mt-2 space-y-3 text-sm text-gray-600">
            <div class="flex items-center space-x-2">
              <font-awesome-icon :icon="['fas', 'phone']" class="text-gray-400" />
              <div>
                <p>Phone</p>
                <p class="font-medium text-gray-800">{{ profileStore.phone }}</p>
              </div>
            </div>
            <div class="flex items-center space-x-2">
              <font-awesome-icon :icon="['fas', 'envelope']" class="text-gray-400" />
              <div>
                <p>Email</p>
                <p class="font-medium text-gray-800">{{ profileStore.email }}</p>
              </div>
            </div>
          </div>
        </div>

        <div class="p-4 bg-white rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Experience</h3>
          <p class="mt-2 text-sm text-gray-600">{{ profileStore.experience }} ani de experiență</p>
        </div>

        <div class="p-4 bg-white rounded-lg shadow-md">
          <h3 class="font-bold text-gray-800">Languages</h3>
          <div class="flex flex-wrap gap-2 mt-2 text-sm">
            <span
              v-for="(lang, index) in profileStore.languages"
              :key="index"
              class="px-3 py-1 bg-purple-100 text-[#5f22d9] rounded-full"
            >
              {{ lang }}
            </span>
          </div>
        </div>

        <button
          @click="router.push('/mfa-setup')"
          class="flex items-center justify-center w-full gap-2 px-6 py-3 font-semibold text-white transition-opacity rounded-full shadow bg-gradient-to-r from-indigo-600 to-purple-600 hover:opacity-90"
        >
          <font-awesome-icon :icon="['fas', 'lock']" />
          Enable MFA
        </button>
      </div>
    </div>
  </div>
</template>
