<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useRouter } from 'vue-router';
  import BaseAuthForm from '../auth/AuthForm.vue';
  import { useAuth } from '../../services/useAuth.ts';
  import Notification from '../ui/NotificationMessage.vue';

  interface SignupFormData {
    email: string;
    password: string;
    role: string;
  }

  const { signup, loginWithGoogle, errorMessage } = useAuth();
  const router = useRouter();

  const formData = ref<SignupFormData>({
    email: '',
    password: '',
    role: '',
  });

  const touchedFields = ref<Set<string>>(new Set());

  const emailError = computed(() => {
    if (!touchedFields.value.has('email') || !formData.value.email) return '';
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    return emailRegex.test(formData.value.email) ? '' : 'Please enter a valid email address';
  });

  const roleError = computed(() => {
    if (!touchedFields.value.has('role')) return '';
    return formData.value.role ? '' : 'Please select a role';
  });

  const passwordRules = {
    minLength: (password: string) => password.length >= 8,
    hasUppercase: (password: string) => /[A-Z]/.test(password),
    hasLowercase: (password: string) => /[a-z]/.test(password),
  };

  const errors = computed(() => {
    const result = [];
    if (!formData.value.password) return [];
    if (!passwordRules.minLength(formData.value.password)) result.push('at least 8 characters');
    if (!passwordRules.hasUppercase(formData.value.password)) result.push('at least one uppercase letter');
    if (!passwordRules.hasLowercase(formData.value.password)) result.push('at least one lowercase letter');
    return result;
  });

  const passwordError = computed(() => {
    if (!touchedFields.value.has('password') || !errors.value.length) return '';
    return errors.value.length ? `Password must contain: ${errors.value.join(', ')}` : '';
  });

  const fieldErrors = computed(() => {
    return {
      email: emailError.value,
      password: passwordError.value,
      role: roleError.value,
    };
  });

  const handleFieldBlur = (field: string) => {
    touchedFields.value.add(field);
  };

  const handleFieldInput = (field: string, value: string) => {
    if (field === 'email') formData.value.email = value;
    else if (field === 'password') formData.value.password = value;
    else if (field === 'role') formData.value.role = value;
  };

  const showSuccessNotification = ref(false);
  const successMessage = ref('Account created successfully!');

  const showSuccess = () => {
    showSuccessNotification.value = true;
    setTimeout(() => {
      showSuccessNotification.value = false;
    }, 3000);
  };

  const handleSubmit = async (data: SignupFormData) => {
    touchedFields.value.add('email');
    touchedFields.value.add('password');
    touchedFields.value.add('role');
    
    formData.value = data;
    
    if (emailError.value || passwordError.value || roleError.value) {
      return;
    }

    const result = await signup(data);
    if (!result) return;

    showSuccess();

    if (data.role === 'tutor') router.push('/create-profile');
    else if (data.role === 'student') router.push('/student-dashboard');
    else if (data.role === 'admin') router.push('/admin-dashboard');
  };

  const handleSocialLogin = async ({ provider, role }: { provider: string; role?: string }) => {
    if (provider !== 'google') return;
    
    if (!role) {
      touchedFields.value.add('role');
      return;
    }
    
    touchedFields.value.add('role');
    formData.value.role = role;

    const result = await loginWithGoogle(true, role);
    if (!result.success) return;

    showSuccess();

    if (role === 'tutor') router.push('/create-profile');
    else if (role === 'student') router.push('/student-dashboard');
    else if (role === 'admin') router.push('/admin-dashboard');
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
    subtitle="Please sign up to continue"
    :showRoleSelector="true"
    submitButtonText="Sign up"
    googleButtonText="Sign up with Google"
    footerText="Already have an account?"
    footerLinkText="Log in"
    footerLinkPath="/login"
    :isSignupForm="true"
    :isLogin="false"
    :showPhoneNumber="false"
    @submit="handleSubmit"
    @socialLogin="handleSocialLogin"
    @fieldBlur="handleFieldBlur"
    @fieldInput="handleFieldInput"
    :fieldErrors="fieldErrors"
    :errorMessage="errorMessage ?? undefined"
  />
</template>
