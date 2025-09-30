<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useAuth } from '../../services/useAuth.ts';
  import { useRouter } from 'vue-router';
  import BaseAuthForm from '../auth/AuthForm.vue';
  import Notification from '../ui/AlertMessage.vue';
  import { useUserStore } from '../../store/userStore.ts';

  interface LoginFormData {
    email: string;
    password: string;
    mfaCode?: string; // Adaugă codul MFA opțional
  }

  const { login, loginWithGoogle, errorMessage, requiresMfa } = useAuth();
  const router = useRouter();

  const formData = ref<LoginFormData>({
    email: '',
    password: '',
    mfaCode: '',
  });

  const touchedFields = ref<Set<string>>(new Set());

  // Definirea validărilor
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  
  const emailError = computed(() => {
    if (!touchedFields.value.has('email') || emailRegex.test(formData.value.email)) return '';
    return 'Please enter a valid email address';
  });

  const passwordError = computed(() => {
    if (!touchedFields.value.has('password') || formData.value.password) return '';
    return 'Password is required';
  });
  
  // Fixed error message translation to English
  const mfaCodeError = computed(() => {
    if (!requiresMfa.value || !touchedFields.value.has('mfaCode') || 
        (formData.value.mfaCode && formData.value.mfaCode.length === 6)) return '';
    return 'Please enter a valid 6-digit code';
  });

  const fieldErrors = computed(() => {
    return {
      email: emailError.value,
      password: passwordError.value,
      ...(requiresMfa.value && { mfaCode: mfaCodeError.value }),
    };
  });

  const handleFieldBlur = (field: string) => {
    touchedFields.value.add(field);
  };

  const handleFieldInput = (field: string, value: string) => {
    formData.value[field as keyof LoginFormData] = value;
  };

  const showSuccessNotification = ref(false);
  const successMessage = ref('Login successful! ');

  const showSuccess = () => {
    showSuccessNotification.value = true;
    setTimeout(() => {
      showSuccessNotification.value = false;
    }, 3000);
  };

  const handleSubmit = async (data: LoginFormData) => {
    touchedFields.value.add('email');
    touchedFields.value.add('password');

    formData.value.email = data.email;
    formData.value.password = data.password;

    if (emailError.value || passwordError.value) {
      return;
    }

    if (requiresMfa.value) {
      touchedFields.value.add('mfaCode');
      if (!formData.value.mfaCode || formData.value.mfaCode.length !== 6) {
        return;
      }
    }

    const loginData: LoginFormData = {
      email: formData.value.email,
      password: formData.value.password,
    };

    if (requiresMfa.value && formData.value.mfaCode) {
      loginData.mfaCode = formData.value.mfaCode;
    }

    const result = await login(loginData);
    
    if (result.requiresMfa) {
      requiresMfa.value = true;
      return;
    }
    
    if (!result.success) return;

    showSuccess();

    const userStore = useUserStore();
    const userRole = userStore.userRole;

    setTimeout(() => {
      navigateBasedOnRole(userRole ?? '');
    }, 1000);
  };

  const navigateBasedOnRole = (role: string) => {
    if (role === 'tutor') {
      router.push('/tutor-dashboard');
    } else if (role === 'student') {
      router.push('/student-dashboard');
    } else if (role === 'admin') {
      router.push('/admin-dashboard');
    } else {
      router.push('/landing');
    }
  };

  const handleSocialLogin = async ({ provider }: { provider: string; role?: string }) => {
    if (provider !== 'google') return;

    if (requiresMfa.value) {
      touchedFields.value.add('mfaCode');
      if (!formData.value.mfaCode || formData.value.mfaCode.length !== 6) {
        return;
      }
    }

    const result = await loginWithGoogle(
      false, 
      undefined, 
      requiresMfa.value ? formData.value.mfaCode : undefined
    );

    if (result.requiresMfa) {
      requiresMfa.value = true;
      return;
    }
    
    if (!result.success) return;

    showSuccess();

    const userStore = useUserStore();
    const userRole = userStore.userRole;

    setTimeout(() => {
      if (userRole === 'tutor') {
        router.push('/tutor-dashboard');
      } else if (userRole === 'student') {
        router.push('/student-dashboard');
      } else if (userRole === 'admin') {
        router.push('/admin-dashboard');
      } else {
        router.push('/landing');
      }
    }, 1000);
  };
</script>

<template>
  <Notification
    :show="showSuccessNotification"
    :message="successMessage"
    type="success"
    @close="showSuccessNotification = false"
  />

  <BaseAuthForm
    title="Tutor.md"
    subtitle="Please log in to continue"
    :showRoleSelector="false"
    submitButtonText="Log in"
    googleButtonText="Log in with Google"
    footerText="Don't have an account?"
    footerLinkText="Sign up"
    footerLinkPath="/signup"
    :isLogin="true"
    @submit="handleSubmit"
    @socialLogin="handleSocialLogin"
    @fieldBlur="handleFieldBlur"
    @fieldInput="handleFieldInput"
    :fieldErrors="fieldErrors"
    :errorMessage="errorMessage ?? undefined"
    :forgotPasswordPath="'/landing'"
  >
    <template #fields-after v-if="requiresMfa">
      <div class="mb-4 sm:mb-5">
        <label for="mfaCode" class="block mb-1 text-sm font-medium text-gray-700">
          Authentication Code
        </label>
        <div class="relative">
          <input
            type="text"
            id="mfaCode"
            v-model="formData.mfaCode"
            placeholder="Enter the 6-digit code"
            maxlength="6"
            inputmode="numeric"
            pattern="[0-9]*"
            class="w-full px-3 sm:px-4 py-2.5 sm:py-3 border rounded-lg focus:outline-none focus:ring-2 transition-all"
            :class="[
              fieldErrors?.mfaCode
                ? 'border-red-500 focus:ring-red-200 focus:border-red-500'
                : 'border-gray-300 focus:ring-purple-100 focus:border-[#5f22d9]'
            ]"
            @blur="handleFieldBlur('mfaCode')"
            @input="(e) => handleFieldInput('mfaCode', (e.target as HTMLInputElement).value)"
          />
          <div class="absolute text-sm text-gray-400 transform -translate-y-1/2 right-3 top-1/2">
            123456
          </div>
        </div>
        <p v-if="fieldErrors?.mfaCode" class="mt-1 text-sm text-red-500">
          {{ fieldErrors.mfaCode }}
        </p>
        <div class="flex items-start mt-2">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-500 mr-1 flex-shrink-0 mt-0.5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          <p class="text-sm text-gray-600">
            Open your authentication app and enter the code displayed for your account.
          </p>
        </div>
      </div>
    </template>
  </BaseAuthForm>
</template>
