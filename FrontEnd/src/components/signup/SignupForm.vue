<script setup lang="ts">
import { ref } from 'vue';
import BaseAuthForm from '../auth/AuthForm.vue';
import { useAuth } from '../../services/useAuth.ts'

const { signup, errorMessage } = useAuth();

const handleSubmit = async (formData: any) => {
  try {
      await signup(formData);
  } catch (err) {
    console.error('Signup error:', err);
  }
};

const handleSocialLogin = ({ provider }: { provider: string }) => {
  console.log(`${provider} login clicked`);
};
</script>

<template>
  <BaseAuthForm
      title="Create an account"
      subtitle="Please sign up to continue"
      :showRoleSelector="true"
      :isSignupForm="true"
      submitButtonText="Sign up"
      googleButtonText="Sign up with Google"
      footerText="Already have an account?"
      footerLinkText="Sign in"
      footerLinkPath="/login"
      @submit="handleSubmit"
      @socialLogin="handleSocialLogin"
      :errorMessage="errorMessage"
  />
</template>
