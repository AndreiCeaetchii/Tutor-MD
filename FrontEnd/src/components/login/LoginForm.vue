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
  />
</template>

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