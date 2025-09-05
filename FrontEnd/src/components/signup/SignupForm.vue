<script setup lang="ts">
import { useRouter } from "vue-router";
import BaseAuthForm from "../auth/AuthForm.vue";
import { userStore } from "../../store/userStore";

interface SignupFormData {
  email: string;
  password: string;
  role: string;
  phoneNumber?: string;
}

const router = useRouter();
const store = userStore();

const handleSubmit = (formData: SignupFormData) => {
  console.log("Signup form submitted:", formData);

  // Simulare înregistrare reușită (să fie înlocuit cu apelul real API)
  store.currentUser = {
    id: "123",
    email: formData.email,
    role: formData.role as any,
    token: "sample-token",
  };
  store.isAuthenticated = true;

  // Redirecționare în funcție de rol
  if (formData.role === "tutor") {
    router.push("/tutor-dashboard");
  } else if (formData.role === "student") {
    router.push("/student-dashboard");
  } else if (formData.role === "admin") {
    router.push("/admin-dashboard");
  }
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
    :isLogin="false"
    submitButtonText="Sign up as Tutor"
    googleButtonText="Sign up with Google"
    footerText="Already have an account?"
    footerLinkText="Sign in"
    footerLinkPath="/login"
    @submit="handleSubmit"
    @socialLogin="handleSocialLogin"
  />
</template>
