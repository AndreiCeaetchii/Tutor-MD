<script setup lang="ts">
import { onMounted } from "vue";
import { useRouter } from "vue-router";
import NavigationBar from "../components/navigation/NavigationBar.vue";
import { userStore } from "../store/userStore";

const store = userStore();
const router = useRouter();

onMounted(() => {
  if (!store.isAuthenticated) {
    router.push("/login");
    return;
  }

  if (store.userRole !== "tutor") {
    if (store.userRole === "student") {
      router.push("/student-dashboard");
    } else if (store.userRole === "admin") {
      router.push("/admin-dashboard");
    }
  }
});
</script>

<template>
  <div>
    <div class="p-4">
      <h1 class="text-2xl font-bold mb-4">Tutor Dashboard</h1>
      <p>
        Welcome to your dashboard! Here you can manage your tutoring sessions,
        view student progress, and update your profile.
      </p>

      <div class="mt-6">
        <h2 class="text-xl font-semibold mb-3">Your Upcoming Sessions</h2>
      </div>

      <div class="mt-6">
        <h2 class="text-xl font-semibold mb-3">Your Students</h2>
      </div>
    </div>

    <NavigationBar />
  </div>
</template>
