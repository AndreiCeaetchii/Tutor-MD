<script setup lang="ts">
  import { useAuth } from '../../services/useAuth.ts';
  import { useRouter } from 'vue-router';
  import BaseAuthForm from '../auth/AuthForm.vue';
  //import { userStore } from '../../store/userStore';

  interface LoginFormData {
    email: string;
    password: string;
    role: string;
  }

  const { login, loginWithGoogle, errorMessage } = useAuth();
  const router = useRouter();
  //const store = userStore();

  const handleSubmit = async (formData: LoginFormData) => {
    const success = await login(formData);
    if (!success) return; // pe viitor o sa returneze id

    if (formData.role === 'tutor') router.push('/tutor-dashboard');
    else if (formData.role === 'student') router.push('/student-dashboard');
    else if (formData.role === 'admin') router.push('/admin-dashboard');
  };

  const handleSocialLogin = async ({ provider, role }: { provider: string; role: string }) => {
    if (provider !== 'google') return;
    //trimitem la loginWithGoogle
    await loginWithGoogle(
      role,
      false, // true = signup, false = login
    );
  };
</script>

<template>
  <BaseAuthForm
    title="Tutor"
    subtitle="Please sign in to continue"
    :showRoleSelector="true"
    submitButtonText="Sign in as Tutor"
    googleButtonText="Log in with Google"
    footerText="Don't have an account?"
    footerLinkText="Sign up"
    footerLinkPath="/signup"
    :isLogin="true"
    @submit="handleSubmit"
    @socialLogin="handleSocialLogin"
    :errorMessage="errorMessage"
  />
</template>
