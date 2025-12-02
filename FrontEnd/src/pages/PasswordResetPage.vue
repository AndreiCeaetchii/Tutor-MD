<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useRouter } from 'vue-router';
  import { changePassword } from '../services/userService';
  import { useUserStore } from '../store/userStore';

  const router = useRouter();
  const userStore = useUserStore();

  const userRole = computed(() => userStore.userRole);

  const loading = ref(false);
  const passwordChangeSuccess = ref(false);
  const currentPassword = ref('');
  const newPassword = ref('');
  const confirmPassword = ref('');
  const errorMessage = ref('');
  const showCurrentPassword = ref(false);
  const showPassword = ref(false);
  const showConfirmPassword = ref(false);

  const passwordRequirements = computed(() => ({
    minLength: newPassword.value.length >= 8,
    hasUpperCase: /[A-Z]/.test(newPassword.value),
    hasLowerCase: /[a-z]/.test(newPassword.value),
    hasNumbers: /\d/.test(newPassword.value),
    hasSpecialChar: /[!@#$%^&*(),.?":{}|<>]/.test(newPassword.value),
  }));

  function validatePassword(): boolean {
    if (!currentPassword.value) {
      errorMessage.value = 'Please enter your current password';
      return false;
    }

    if (newPassword.value.length < 8) {
      errorMessage.value = 'New password must be at least 8 characters long';
      return false;
    }

    if (newPassword.value !== confirmPassword.value) {
      errorMessage.value = 'Passwords do not match';
      return false;
    }

    if (currentPassword.value === newPassword.value) {
      errorMessage.value = 'New password must be different from current password';
      return false;
    }

    const hasUpperCase = /[A-Z]/.test(newPassword.value);
    const hasLowerCase = /[a-z]/.test(newPassword.value);
    const hasNumbers = /\d/.test(newPassword.value);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(newPassword.value);

    if (!hasUpperCase || !hasLowerCase || !hasNumbers || !hasSpecialChar) {
      errorMessage.value =
        'Password must contain uppercase, lowercase, number, and special character';
      return false;
    }

    return true;
  }

  async function handleChangePassword() {
    errorMessage.value = '';

    if (!validatePassword()) {
      return;
    }

    loading.value = true;

    try {
      await changePassword(currentPassword.value, newPassword.value);
      passwordChangeSuccess.value = true;

      // Navigate back after 2 seconds
      setTimeout(() => {
        navigateBasedOnRole();
      }, 2000);
    } catch (error: any) {
      console.error('Failed to change password:', error);
      errorMessage.value = error.message || 'Failed to change password. Please try again.';
    } finally {
      loading.value = false;
    }
  }

  function navigateBasedOnRole() {
    const role = userRole.value;
    if (role === 'tutor') {
      router.push('/tutor-dashboard/profile');
    } else if (role === 'student') {
      router.push('/student-dashboard/account');
    } else if (role === 'admin') {
      router.push('/admin-dashboard/profile');
    } else {
      router.push('/landing');
    }
  }

  function cancelReset() {
    navigateBasedOnRole();
  }
</script>

<template>
  <div
    class="fixed inset-0 z-50 flex items-center justify-center p-3 overflow-y-auto sm:p-4 backdrop-blur-sm"
  >
    <div
      class="w-full max-w-3xl mx-auto overflow-hidden shadow-xl sm:max-w-4xl bg-white/90 rounded-2xl"
    >
      <div v-if="loading" class="flex items-center justify-center p-6 sm:p-8">
        <div
          class="w-10 h-10 border-4 border-t-transparent rounded-full border-[#5f22d9] animate-spin"
        ></div>
      </div>

      <div v-else-if="passwordChangeSuccess" class="p-4 mx-auto sm:p-6 md:max-w-2xl">
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
                d="M5 13l4 4L19 7"
              />
            </svg>
          </div>

          <h2 class="mb-2 text-xl font-bold text-center text-gray-900">
            Password Changed Successfully!
          </h2>

          <p class="mb-4 text-base text-center text-gray-600">
            Your password has been successfully updated. You can continue using your account with
            the new password.
          </p>

          <p class="text-sm text-center text-gray-500">Redirecting back...</p>
        </div>

        <div class="px-4 mt-6 sm:px-6">
          <button
            @click="navigateBasedOnRole"
            class="w-full px-4 py-2.5 text-base font-medium text-white transition-colors bg-[#5f22d9] rounded-lg hover:bg-purple-700"
          >
            Return to Profile
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
                d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z"
              />
            </svg>
            <h2 class="mb-3 text-xl font-bold md:mb-4 md:text-2xl">Change Your Password</h2>
            <p class="mb-2 text-sm opacity-90">
              Keep your account secure by regularly updating your password.
            </p>
            <p class="text-sm opacity-90">Choose a strong password that you haven't used before.</p>
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
                d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z"
              />
            </svg>
          </div>

          <h2 class="mb-3 text-xl font-bold text-center text-gray-900 sm:text-2xl">
            Change Password
          </h2>

          <p class="mb-5 text-sm text-center text-gray-600">
            Enter your current password and choose a new one.
          </p>

          <div class="mb-4">
            <label for="currentPassword" class="block mb-2 text-sm font-medium text-gray-700"
              >Current Password</label
            >
            <div class="relative">
              <input
                v-model="currentPassword"
                id="currentPassword"
                :type="showCurrentPassword ? 'text' : 'password'"
                placeholder="Enter current password"
                class="w-full px-4 py-3 pr-10 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#5f22d9] focus:border-transparent"
              />
              <button
                type="button"
                @click="showCurrentPassword = !showCurrentPassword"
                class="absolute inset-y-0 right-0 flex items-center pr-3 text-gray-500 hover:text-gray-700"
              >
                <svg
                  v-if="!showCurrentPassword"
                  xmlns="http://www.w3.org/2000/svg"
                  class="w-5 h-5"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
                  />
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"
                  />
                </svg>
                <svg
                  v-else
                  xmlns="http://www.w3.org/2000/svg"
                  class="w-5 h-5"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21"
                  />
                </svg>
              </button>
            </div>
          </div>

          <div class="mb-4">
            <label for="newPassword" class="block mb-2 text-sm font-medium text-gray-700"
              >New Password</label
            >
            <div class="relative">
              <input
                v-model="newPassword"
                id="newPassword"
                :type="showPassword ? 'text' : 'password'"
                placeholder="Enter new password"
                class="w-full px-4 py-3 pr-10 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#5f22d9] focus:border-transparent"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute inset-y-0 right-0 flex items-center pr-3 text-gray-500 hover:text-gray-700"
              >
                <svg
                  v-if="!showPassword"
                  xmlns="http://www.w3.org/2000/svg"
                  class="w-5 h-5"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
                  />
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"
                  />
                </svg>
                <svg
                  v-else
                  xmlns="http://www.w3.org/2000/svg"
                  class="w-5 h-5"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21"
                  />
                </svg>
              </button>
            </div>
          </div>

          <div class="mb-5">
            <label for="confirmPassword" class="block mb-2 text-sm font-medium text-gray-700"
              >Confirm New Password</label
            >
            <div class="relative">
              <input
                v-model="confirmPassword"
                id="confirmPassword"
                :type="showConfirmPassword ? 'text' : 'password'"
                placeholder="Confirm new password"
                class="w-full px-4 py-3 pr-10 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#5f22d9] focus:border-transparent"
                @keyup.enter="handleChangePassword"
              />
              <button
                type="button"
                @click="showConfirmPassword = !showConfirmPassword"
                class="absolute inset-y-0 right-0 flex items-center pr-3 text-gray-500 hover:text-gray-700"
              >
                <svg
                  v-if="!showConfirmPassword"
                  xmlns="http://www.w3.org/2000/svg"
                  class="w-5 h-5"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
                  />
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"
                  />
                </svg>
                <svg
                  v-else
                  xmlns="http://www.w3.org/2000/svg"
                  class="w-5 h-5"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21"
                  />
                </svg>
              </button>
            </div>
            <div v-if="errorMessage" class="mt-2 text-sm text-red-500">
              {{ errorMessage }}
            </div>
          </div>

          <div class="p-4 mb-5 rounded-lg bg-gray-50">
            <h3 class="mb-2 text-sm font-semibold text-gray-800">Password Requirements:</h3>
            <ul class="space-y-1 text-sm text-gray-600">
              <li class="flex items-center">
                <span
                  class="mr-2"
                  :class="passwordRequirements.minLength ? 'text-green-500' : 'text-gray-400'"
                  >●</span
                >
                <span>At least 8 characters long</span>
              </li>
              <li class="flex items-center">
                <span
                  class="mr-2"
                  :class="passwordRequirements.hasUpperCase ? 'text-green-500' : 'text-gray-400'"
                  >●</span
                >
                <span>One uppercase letter</span>
              </li>
              <li class="flex items-center">
                <span
                  class="mr-2"
                  :class="passwordRequirements.hasLowerCase ? 'text-green-500' : 'text-gray-400'"
                  >●</span
                >
                <span>One lowercase letter</span>
              </li>
              <li class="flex items-center">
                <span
                  class="mr-2"
                  :class="passwordRequirements.hasNumbers ? 'text-green-500' : 'text-gray-400'"
                  >●</span
                >
                <span>One number</span>
              </li>
              <li class="flex items-center">
                <span
                  class="mr-2"
                  :class="passwordRequirements.hasSpecialChar ? 'text-green-500' : 'text-gray-400'"
                  >●</span
                >
                <span>One special character (!@#$%^&*...)</span>
              </li>
            </ul>
          </div>

          <button
            @click="handleChangePassword"
            class="w-full px-4 py-3 text-white font-medium transition-colors bg-[#5f22d9] rounded-lg hover:bg-purple-700 disabled:bg-purple-300 disabled:cursor-not-allowed"
            :disabled="!currentPassword || !newPassword || !confirmPassword || loading"
          >
            Change Password
          </button>

          <button
            @click="cancelReset"
            class="w-full py-2 mt-3 text-gray-600 transition-colors hover:text-gray-800"
          >
            Cancel
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
