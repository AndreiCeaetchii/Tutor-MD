<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
  import { library } from '@fortawesome/fontawesome-svg-core';
  import { useUserStore } from '../../store/userStore';
  import {
    faEye,
    faEyeSlash,
    faChalkboardTeacher,
    faUserGraduate,
  } from '@fortawesome/free-solid-svg-icons';
  import logoImage from '../../assets/tutor2.png';
  import type { UserRole } from '../../store/userStore';

  library.add(faEye, faEyeSlash, faChalkboardTeacher, faUserGraduate);

  interface Role {
    value: string;
    label: string;
    icon: [string, string];
  }

  interface AuthFormProps {
    title?: string;
    subtitle?: string;
    logoSrc?: string;
    showRoleSelector?: boolean;
    roles?: Role[];
    submitButtonText?: string;
    googleButtonText?: string;
    footerText?: string;
    footerLinkText?: string;
    footerLinkPath?: string;
    isSignupForm?: boolean;
    isLogin: boolean;
    errorMessage?: string;
    fieldErrors?: Record<string, string>;
  }
  
  interface FormData {
    email: string;
    password: string;
    role: string;
    phoneNumber?: string;
  }

  interface SocialLoginPayload {
    provider: string;
    role: UserRole;
  }

  const props = withDefaults(defineProps<AuthFormProps>(), {
    title: 'Tutor',
    isSignupForm: false,
    showRoleSelector: false,
    fieldErrors: () => ({})
  });

  const defaultRoles: Role[] = [
    { value: 'tutor', label: 'Tutor', icon: ['fas', 'chalkboard-teacher'] },
    { value: 'student', label: 'Student', icon: ['fas', 'user-graduate'] },
  ];

  const roles = props.roles ?? defaultRoles;

  const emit = defineEmits<{
    (e: 'submit', formData: FormData): void;
    (e: 'socialLogin', payload: SocialLoginPayload): void;
    (e: 'fieldBlur', field: string): void;
    (e: 'fieldInput', field: string, value: string): void;
  }>();

  const logoSrc = props.logoSrc || logoImage;
  const email = ref('');
  const phoneNumber = ref('');
  const password = ref('');

  const showPassword = ref(false);
  const store = useUserStore();

  const selectRole = (role: UserRole) => {
    selectedRole.value = role;
    emit('fieldInput', 'role', role);
  };

  // Fix - Initialize as empty string instead of null
  const selectedRole = ref<UserRole | string>(store.userRole || '');

  const handleSubmit = () => {
    const formData: FormData = {
      email: email.value,
      password: password.value,
      role: selectedRole.value || '', // Ensure it's always a string
    };

    if (props.isSignupForm) {
      formData.phoneNumber = phoneNumber.value ? `+373${phoneNumber.value}` : '';
    }

    emit('submit', formData);
  };

  const handleSocialLogin = (provider: string): void => {
    if (!selectedRole.value) {
      emit('fieldBlur', 'role'); // Trigger validation for role
      return;
    }

    emit('socialLogin', {
      provider,
      role: selectedRole.value as UserRole, // Type assertion since we know it's not empty
    });
  };

  const actionText = computed(() => {
    if (!props.submitButtonText) return 'Submit';
    return props.submitButtonText.includes('Sign in') ? 'Sign in as' : 'Sign up as';
  });

  const dynamicSubmitButtonText = computed(() => {
    const currentRole = roles.find((r) => r.value === selectedRole.value);
    return `${actionText.value} ${currentRole?.label || 'Tutor or Student'}`;
  });
  
  // Fix - Type-safe event handlers
  const handleEmailInput = (event: Event) => {
    const target = event.target as HTMLInputElement;
    email.value = target.value;
    emit('fieldInput', 'email', target.value);
  };
  
  const handlePasswordInput = (event: Event) => {
    const target = event.target as HTMLInputElement;
    password.value = target.value;
    emit('fieldInput', 'password', target.value);
  };
  
  const handlePhoneNumberInput = (event: Event) => {
    const target = event.target as HTMLInputElement;
    phoneNumber.value = target.value;
    emit('fieldInput', 'phoneNumber', target.value);
  };
</script>

<template>
  <div class="flex flex-col max-w-md p-5 mx-auto bg-white sm:p-8">
    <div class="mb-4 text-center sm:mb-5">
      <img :src="logoSrc" :alt="title" class="h-12 mx-auto mb-2 sm:h-14 sm:mb-3" />
      <p class="text-sm text-gray-500">{{ subtitle }}</p>
    </div>

    <!-- Role selector with error handling -->
    <div v-if="showRoleSelector" class="mb-4 sm:mb-5">
      <div class="flex flex-wrap justify-center gap-3">
        <button
          v-for="role in roles"
          :key="role.value"
          type="button"
          class="flex-1 min-w-[100px] px-4 sm:px-6 py-2.5 bg-transparent border font-semibold rounded-full transition-colors flex items-center justify-center gap-2"
          :class="[
            selectedRole === role.value
              ? 'border-[#5f22d9] bg-[#5f22d9]/5 text-[#5f22d9]'
              : 'border-gray-300 text-gray-500 hover:bg-gray-100',
            fieldErrors?.role && !selectedRole ? 'border-red-500' : ''
          ]"
          @click="selectRole(role.value as UserRole)"
        >
          <font-awesome-icon :icon="role.icon" />
          <span>{{ role.label }}</span>
        </button>
      </div>
      <p v-if="fieldErrors?.role" class="mt-1 text-sm text-red-500">
        {{ fieldErrors.role }}
      </p>
    </div>

    <!-- Global error message -->
    <div v-if="errorMessage" class="p-3 mb-4 text-red-700 bg-red-100 border-l-4 border-red-500 rounded">
      {{ errorMessage }}
    </div>

    <form @submit.prevent="handleSubmit">
      <slot name="fields-before"></slot>

      <!-- Email field with error handling - Fixed event handler -->
      <div class="mb-3 sm:mb-4">
        <label for="email" class="block mb-1 text-sm font-medium text-gray-700">Email</label>
        <input
          type="email"
          id="email"
          v-model="email"
          placeholder="Enter your email"
          class="w-full px-3 sm:px-4 py-2.5 sm:py-3 border rounded-lg focus:outline-none focus:ring-2"
          :class="[
            fieldErrors?.email
              ? 'border-red-500 focus:ring-red-200 focus:border-red-500'
              : 'border-gray-300 focus:ring-purple-100 focus:border-[#5f22d9]'
          ]"
          required
          @blur="emit('fieldBlur', 'email')"
          @input="handleEmailInput"
        />
        <p v-if="fieldErrors?.email" class="mt-1 text-sm text-red-500">
          {{ fieldErrors.email }}
        </p>
      </div>

      <!-- Phone number field with error handling - Fixed event handler -->
      <div v-if="isSignupForm" class="mb-4 sm:mb-5">
        <label for="phone" class="block mb-1 text-sm font-medium text-gray-700">Phone Number</label>
        <div class="flex">
          <div
            class="flex items-center px-3 font-medium text-gray-600 border border-gray-300 rounded-l-lg bg-gray-50"
            :class="{ 'border-red-500': fieldErrors?.phoneNumber }"
          >
            <img src="https://flagcdn.com/w20/md.png" alt="Moldova flag" class="mr-2 h-" />
            <span>+373</span>
          </div>
          <input
            type="tel"
            id="phone"
            v-model="phoneNumber"
            placeholder="Enter phone number"
            class="flex-1 px-3 sm:px-4 py-2.5 sm:py-3 border rounded-r-lg focus:outline-none focus:ring-2"
            :class="[
              fieldErrors?.phoneNumber
                ? 'border-red-500 focus:ring-red-200 focus:border-red-500'
                : 'border-gray-300 focus:ring-purple-100 focus:border-[#5f22d9]'
            ]"
            @blur="emit('fieldBlur', 'phoneNumber')"
            @input="handlePhoneNumberInput"
          />
        </div>
        <p v-if="fieldErrors?.phoneNumber" class="mt-1 text-sm text-red-500">
          {{ fieldErrors.phoneNumber }}
        </p>
      </div>

      <!-- Password field with error handling - Fixed event handler -->
      <div class="mb-4 sm:mb-5">
        <label for="password" class="block mb-1 text-sm font-medium text-gray-700">Password</label>
        <div class="relative">
          <input
            :type="showPassword ? 'text' : 'password'"
            id="password"
            v-model="password"
            placeholder="Enter your password"
            class="w-full px-3 sm:px-4 py-2.5 sm:py-3 border rounded-lg focus:outline-none focus:ring-2 pr-12"
            :class="[
              fieldErrors?.password
                ? 'border-red-500 focus:ring-red-200 focus:border-red-500'
                : 'border-gray-300 focus:ring-purple-100 focus:border-[#5f22d9]'
            ]"
            required
            @blur="emit('fieldBlur', 'password')"
            @input="handlePasswordInput"
          />
          <button
            type="button"
            class="absolute right-3 sm:right-4 top-1/2 -translate-y-1/2 text-gray-500 hover:text-[#5f22d9]"
            @click="showPassword = !showPassword"
          >
            <font-awesome-icon :icon="['fas', showPassword ? 'eye' : 'eye-slash']" />
          </button>
        </div>
        <p v-if="fieldErrors?.password" class="mt-1 text-sm text-red-500">
          {{ fieldErrors.password }}
        </p>
      </div>

      <slot name="fields-after"></slot>

      <button
        type="submit"
        class="w-full px-6 sm:px-8 py-2.5 sm:py-3 bg-gradient-to-r from-[#5f22d9] to-[#2c016d] text-white font-semibold rounded-full transition-all hover:shadow-lg"
      >
        {{ dynamicSubmitButtonText }}
      </button>
    </form>

    <div class="flex items-center my-4 sm:my-5">
      <div class="flex-grow h-px bg-gray-200"></div>
      <span class="px-3 text-sm text-gray-500">or continue</span>
      <div class="flex-grow h-px bg-gray-200"></div>
    </div>

    <button
      type="button"
      class="w-full px-6 sm:px-8 py-2.5 sm:py-3 bg-transparent border border-gray-300 text-gray-800 font-semibold rounded-full hover:bg-gray-100 transition-colors flex items-center justify-center gap-2 sm:gap-3"
      @click="handleSocialLogin('google')"
    >
      <svg class="w-[18px] h-[18px] sm:w-[20px] sm:h-[20px]" viewBox="0 0 48 48">
        <path
          fill="#EA4335"
          d="M24 9.5c3.54 0 6.71 1.22 9.21 3.6l6.85-6.85C35.9 2.38 30.47 0 24 0 14.62 0 6.51 5.38 2.56 13.22l7.98 6.19C12.43 13.72 17.74 9.5 24 9.5z"
        />
        <path
          fill="#4285F4"
          d="M46.98 24.55c0-1.57-.15-3.09-.38-4.55H24v9.02h12.94c-.58 2.96-2.26 5.48-4.78 7.18l7.73 6c4.51-4.18 7.09-10.36 7.09-17.65z"
        />
        <path
          fill="#FBBC05"
          d="M10.53 28.59c-.48-1.45-.76-2.99-.76-4.59s.27-3.14.76-4.59l-7.98-6.19C.92 16.46 0 20.12 0 24c0 3.88.92 7.54 2.56 10.78l7.97-6.19z"
        />
        <path
          fill="#34A853"
          d="M24 48c6.48 0 11.93-2.13 15.89-5.81l-7.73-6c-2.15 1.45-4.92 2.3-8.16 2.3-6.26 0-11.57-4.22-13.47-9.91l-7.98 6.19C6.51 42.62 14.62 48 24 48z"
        />
      </svg>
      <span>{{ googleButtonText }}</span>
    </button>

    <div class="pt-3 mt-4 text-center border-t border-gray-100 sm:mt-5 sm:pt-4">
      <slot name="footer">
        <p class="text-sm text-gray-500">
          {{ footerText }}
          <router-link
            :to="footerLinkPath || '/'"
            class="text-[#5f22d9] font-medium hover:underline"
            >{{ footerLinkText }}</router-link
          >
        </p>
      </slot>
    </div>
  </div>
</template>
