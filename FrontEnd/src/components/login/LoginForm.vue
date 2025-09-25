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
  }

  const { login, loginWithGoogle, errorMessage } = useAuth();
  const router = useRouter();

  const formData = ref<LoginFormData>({
    email: '',
    password: '',
  });

  const touchedFields = ref<Set<string>>(new Set());

  const emailError = computed(() => {
    if (!touchedFields.value.has('email') || !formData.value.email) return '';
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    return emailRegex.test(formData.value.email) ? '' : 'Please enter a valid email address';
  });

  const passwordError = computed(() => {
    if (!touchedFields.value.has('password') || formData.value.password) return '';
    return 'Password is required';
  });

  const fieldErrors = computed(() => {
    return {
      email: emailError.value,
      password: passwordError.value,
    };
  });

  const handleFieldBlur = (field: string) => {
    touchedFields.value.add(field);
  };

  const handleFieldInput = (field: string, value: string) => {
    if (field === 'email') formData.value.email = value;
    else if (field === 'password') formData.value.password = value;
  };

  const showSuccessNotification = ref(false);
  const successMessage = ref('Login successful! ');

  const showSuccess = () => {
    successMessage.value = 'Login successful! ';
    showSuccessNotification.value = true;
    setTimeout(() => {
      showSuccessNotification.value = false;
    }, 3000);
  };

  const handleSubmit = async (data: LoginFormData) => {
    touchedFields.value.add('email');
    touchedFields.value.add('password');

    formData.value = data;

    if (emailError.value || passwordError.value) {
      return;
    }

    const result = await login(data);
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

  const handleSocialLogin = async ({ provider }: { provider: string; role?: string }) => {
    if (provider !== 'google') return;

    const result = await loginWithGoogle(false);
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
  />
</template>
