<script setup lang="ts">

import { useAuth } from '../../services/useAuth.ts';
import { useRouter } from "vue-router";
import BaseAuthForm from "../auth/AuthForm.vue";
import { userStore } from "../../store/userStore";

interface LoginFormData {
  email: string;
  password: string;
  role: string;
}

const { login, errorMessage } = useAuth();
const router = useRouter();
const store = userStore();

const handleSubmit = async (formData: LoginFormData) => {
  console.log("Login form submitted:", formData);

  try {
    // Apelăm funcția login din servicii
    const userData = await login(formData);

    console.log(userData);

    // Actualizăm store-ul dacă login-ul a reușit
    store.login({
      id: "123",
      email: formData.email,
      role: formData.role as any,
      token: "sample-token",
    });

    // Redirecționare pe baza rolului
    if (formData.role === "tutor") {
      router.push("/tutor-dashboard");
    } else if (formData.role === "student") {
      router.push("/student-dashboard");
    } else if (formData.role === "admin") {
      router.push("/admin-dashboard");
    }
  } catch (err) {
    console.error("Login failed", err);
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
    :isLogin="true"
    @submit="handleSubmit"
    @socialLogin="handleSocialLogin"
    :errorMessage="errorMessage"
  />
</template>

