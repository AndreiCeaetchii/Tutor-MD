<script setup lang="ts">
import BaseAuthForm from '../auth/AuthForm.vue';

interface LoginFormData {
    email: string;
    password: string;
}

const handleSubmit = (formData: LoginFormData) => {
    console.log('Login form submitted:', formData);

};

const handleSocialLogin = ({ provider }: { provider: string }) => {
  console.log(`${provider} login clicked`);

};
</script>

<template>
  <BaseAuthForm
    title="Tutor"
    subtitle="Please sign up to continue"
    :showRoleSelector="true"
    :isSignupForm="true"
    submitButtonText="Sign up as Tutor"
    googleButtonText="Sign up with Google"
    footerText="Already have an account?"
    footerLinkText="Sign in"
    footerLinkPath="/login"
    @submit="handleSubmit"
    @socialLogin="handleSocialLogin"
  />
</template>
