<script setup lang="ts">
  import { useRouter } from 'vue-router';
  import BaseAuthForm from '../auth/AuthForm.vue';
  import { useAuth } from '../../services/useAuth.ts';

  interface SignupFormData {
    email: string;
    password: string;
    role: string;
    phoneNumber?: string;
  }

  const { signup, loginWithGoogle, errorMessage } = useAuth();
  const router = useRouter();

  const handleSubmit = async (formData: SignupFormData) => {
    console.log('Signup form submitted:', formData);

    const success = await signup(formData);
    if (!success) return; // pe viitor o sa returneze id

    console.log('debug');
    if (formData.role === 'tutor') {
      router.push('/tutor-dashboard');
    } else if (formData.role === 'student') {
      router.push('/student-dashboard');
    } else if (formData.role === 'admin') {
      router.push('/admin-dashboard');
    }
  };

  const handleSocialLogin = async ({ provider, role }: { provider: string; role: string }) => {
    if (provider !== 'google') return;
    //trimitem la loginWithGoogle
    await loginWithGoogle(
      role,
      true, // true = signup, false = login
    );
  };
</script>

<template>
  <BaseAuthForm
    title="Create an account"
    subtitle="Please sign up to continue"
    :showRoleSelector="true"
    :isSignupForm="true"
    :isLogin="false"
    submitButtonText="Sign up as Tutor"
    googleButtonText="Sign up with Google"
    footerText="Already have an account?"
    footerLinkText="Sign in"
    footerLinkPath="/login"
    @submit="handleSubmit"
    @socialLogin="handleSocialLogin"
    :errorMessage="errorMessage"
  />
</template>
