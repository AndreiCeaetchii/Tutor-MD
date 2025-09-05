import { createRouter, createWebHistory } from "vue-router";
import LoginPage from "../pages/LoginPage.vue";

import SignupPage from "../pages/SignupPage.vue";
import LandingPage from "../pages/LandingPage.vue";



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
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
