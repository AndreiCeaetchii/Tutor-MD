import { createRouter, createWebHistory } from "vue-router";
import LoginPage from "../pages/LoginPage.vue";
import SignupPage from "../pages/SignupPage.vue";
import LandingPage from "../pages/LandingPage.vue";
import TutorDashboard from "../pages/TutorDashboard.vue";
import StudentDashboard from "../pages/StudentDashboard.vue";

const routes = [
    {
    path: "/login",
    component: LoginPage,
  },
  {
    path: "/signup",
    component: SignupPage,
  },
  {
    path: "/landing",
    component: LandingPage,
  },
  {
    path: "/",
    component: LandingPage,
  },
  {
    path: "/tutor-dashboard",
    component: TutorDashboard,
  },
  {
    path: "/student-dashboard",
    component: StudentDashboard,
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
