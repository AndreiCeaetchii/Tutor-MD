<script setup lang="ts">
import { useRouter } from "vue-router";
import BaseAuthForm from "../auth/AuthForm.vue";
import { useAuth } from "../../services/useAuth.ts";
import { userStore } from "../../store/userStore";

interface SignupFormData {
  email: string;
  password: string;
  role: string;
  phoneNumber?: string;
}

const { signup, errorMessage } = useAuth();
const router = useRouter();
const store = userStore();

const handleSubmit = async (formData: SignupFormData) => {
  console.log("Signup form submitted:", formData);

  try {
    // Apelăm API-ul real de signup
    const userData = await signup(formData);

    console.log(userData);

    // Actualizăm store-ul după signup
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
  } catch (err) {
    console.error("Signup error:", err);
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

