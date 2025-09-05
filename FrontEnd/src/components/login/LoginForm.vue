<script setup lang="ts">
import BaseAuthForm from '../auth/AuthForm.vue';
import { useAuth } from '../../services/useAuth.ts';

interface LoginFormData {
  email: string;
  password: string;
}

const { login, errorMessage } = useAuth();

const handleSubmit = async (formData: LoginFormData) => {
  console.log('Login form submitted:', formData);

  try {
    await login(formData); // apelăm funcția login din composable
  } catch (err) {
    console.error('Login failed');
  }
};

const handleSocialLogin = ({ provider }: { provider: string }) => {
  console.log(`${provider} login clicked`);
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
      @submit="handleSubmit"
      @socialLogin="handleSocialLogin"
      :errorMessage="errorMessage"
  />
</template>
