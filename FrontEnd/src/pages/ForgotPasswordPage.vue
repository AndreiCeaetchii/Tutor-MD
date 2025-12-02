<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useRouter } from 'vue-router';
  import { requestPasswordReset } from '../services/userService';

  const router = useRouter();

  const loading = ref(false);
  const emailSent = ref(false);
  const email = ref('');
  const errorMessage = ref('');
  const touched = ref(false);

  const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  const emailError = computed(() => {
    if (!touched.value || emailRegex.test(email.value)) return '';
    return 'Please enter a valid email address';
  });

  async function handleSubmit() {
    touched.value = true;
    errorMessage.value = '';

    if (emailError.value || !email.value) {
      return;
    }

    loading.value = true;

    try {
      await requestPasswordReset(email.value);
      emailSent.value = true;
    } catch (error: any) {
      console.error('Failed to send password reset email:', error);
      errorMessage.value = error.message || 'Failed to send reset email. Please try again.';
    } finally {
      loading.value = false;
    }
  }

  function navigateToLogin() {
    router.push('/login');
  }
</script>

<template>
  <div
    class="fixed inset-0 z-50 flex items-center justify-center p-3 overflow-y-auto sm:p-4 bg-gray-50"
  >
    <div
      class="w-full max-w-3xl mx-auto overflow-hidden bg-white shadow-xl sm:max-w-4xl rounded-2xl"
    >
      <div v-if="loading" class="flex items-center justify-center p-6 sm:p-8">
        <div
          class="w-10 h-10 border-4 border-t-transparent rounded-full border-[#5f22d9] animate-spin"
        ></div>
      </div>

      <div v-else-if="emailSent" class="p-4 mx-auto sm:p-6 md:max-w-2xl">
        <div class="flex flex-col items-center max-w-lg mx-auto">
          <div class="flex items-center justify-center mb-4 bg-green-100 rounded-full w-14 h-14">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-8 h-8 text-green-500"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
              />
            </svg>
          </div>

          <h2 class="mb-2 text-xl font-bold text-center text-gray-900">Check Your Email!</h2>

          <p class="mb-4 text-base text-center text-gray-600">
            We've sent a password reset link to <strong>{{ email }}</strong
            >. Please check your inbox and follow the instructions to reset your password.
          </p>

          <div class="w-full p-4 mb-4 border-l-4 border-blue-500 rounded-r-lg bg-blue-50">
            <div class="flex items-start">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5 text-blue-500 mr-2 flex-shrink-0 mt-0.5"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              <div class="text-sm text-blue-700">
                <p class="mb-1 font-semibold">Didn't receive the email?</p>
                <ul class="space-y-1 list-disc list-inside">
                  <li>Check your spam or junk folder</li>
                  <li>Make sure you entered the correct email address</li>
                  <li>Wait a few minutes and try again</li>
                </ul>
              </div>
            </div>
          </div>

          <p class="text-sm text-center text-gray-500">
            The reset link will expire in 1 hour for security reasons.
          </p>
        </div>

        <div class="px-4 mt-6 sm:px-6">
          <button
            @click="navigateToLogin"
            class="w-full px-4 py-2.5 text-base font-medium text-white transition-colors bg-[#5f22d9] rounded-lg hover:bg-purple-700"
          >
            Back to Login
          </button>
        </div>
      </div>

      <div v-else class="flex flex-col w-full md:flex-row">
        <div class="hidden md:flex md:w-1/2 bg-[#5f22d9] items-center justify-center p-6 md:p-8">
          <div class="max-w-md text-center text-white">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-20 h-20 mx-auto mb-5 md:w-24 md:h-24 md:mb-6 opacity-90"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
              />
            </svg>
            <h2 class="mb-3 text-xl font-bold md:mb-4 md:text-2xl">Forgot Your Password?</h2>
            <p class="mb-2 text-sm opacity-90">No worries! It happens to the best of us.</p>
            <p class="text-sm opacity-90">
              Enter your email address and we'll send you a link to reset your password.
            </p>
          </div>
        </div>

        <div class="w-full p-4 sm:p-6 md:w-1/2 md:p-8">
          <div class="flex items-center justify-center mb-4 sm:mb-5 text-[#5f22d9] md:hidden">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-16 h-16"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
              />
            </svg>
          </div>

          <h2 class="mb-3 text-xl font-bold text-center text-gray-900 sm:text-2xl">
            Reset Your Password
          </h2>

          <p class="mb-5 text-sm text-center text-gray-600">
            Enter the email address associated with your account and we'll send you a password reset
            link.
          </p>

          <form @submit.prevent="handleSubmit">
            <div class="mb-5">
              <label for="email" class="block mb-2 text-sm font-medium text-gray-700"
                >Email Address</label
              >
              <input
                v-model="email"
                id="email"
                type="email"
                placeholder="Enter your email"
                class="w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-[#5f22d9] focus:border-transparent transition-all"
                :class="[emailError ? 'border-red-500 focus:ring-red-200' : 'border-gray-300']"
                @blur="touched = true"
                @keyup.enter="handleSubmit"
              />
              <p v-if="emailError" class="mt-2 text-sm text-red-500">
                {{ emailError }}
              </p>
              <p v-if="errorMessage" class="mt-2 text-sm text-red-500">
                {{ errorMessage }}
              </p>
            </div>

            <button
              type="submit"
              class="w-full px-4 py-3 text-white font-medium transition-colors bg-[#5f22d9] rounded-lg hover:bg-purple-700 disabled:bg-purple-300 disabled:cursor-not-allowed"
              :disabled="!email || loading || !!emailError"
            >
              Send Reset Link
            </button>
          </form>

          <div class="mt-6 text-center">
            <p class="text-sm text-gray-600">
              Remember your password?
              <button
                @click="navigateToLogin"
                class="text-[#5f22d9] font-medium hover:underline ml-1"
              >
                Back to Login
              </button>
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
