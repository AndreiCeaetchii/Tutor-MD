import { createRouter, createWebHistory } from "vue-router";
import LoginPage from "../pages/LoginPage.vue";
import SignupPage from "../pages/SignupPage.vue";
import LandingPage from "../pages/LandingPage.vue";
import TutorDashboard from "../pages/TutorDashboard.vue";
import StudentDashboard from "../pages/StudentDashboard.vue";
import { userStore } from "../store/userStore";
import TutorProfile from "../components/tutor/TutorProfile.vue";
import TutorReview from "../components/tutor/TutorReview.vue";


const routes = [
  {
    path: '/login',
    component: LoginPage,
    meta: { requiresGuest: true },
  },
  {
    path: '/signup',
    component: SignupPage,
    meta: { requiresGuest: true },
  },
  {
    path: '/landing',
    component: LandingPage,
  },
  {
    path: '/',
    component: LandingPage,
  },
  {
    path: '/tutor-dashboard',
    component: TutorDashboard,
    meta: {
      requiresAuth: true,
      role: 'tutor',
    },
  },
  {
    path: '/tutor-dashboard-reviews',
    component: TutorReview,
    meta: {
      requiresAuth: true,
      role: 'tutor',
    },
  },
  {
    path: "/tutor-dashboard-profile",
    component: TutorProfile,
    meta: {
      requiresAuth: true,
      role: "tutor",
    },
  },
  {
    path: "/student-dashboard",

    component: StudentDashboard,
    meta: {
      requiresAuth: true,
      role: 'student',
    },
  },
  // {
  //   path: "/admin-dashboard",
  //   component: AdminDashboard,
  //   meta: {
  //     requiresAuth: true,
  //     role: 'admin'
  //   }
  // }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, _, next) => {
  const store = userStore();

  store.initializeFromStorage();

  const isAuthenticated = store.isAuthenticated;
  const userRole = store.userRole;

  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/login');
    return;
  }

  if (to.meta.role && userRole !== to.meta.role) {
    if (isAuthenticated) {
      if (userRole === 'tutor') next('/tutor-dashboard');
      else if (userRole === 'student') next('/student-dashboard');
      else if (userRole === 'admin') next('/admin-dashboard');
      return;
    }
  }

  if (to.meta.requiresGuest && isAuthenticated) {
    if (userRole === 'tutor') next('/tutor-dashboard');
    else if (userRole === 'student') next('/student-dashboard');
    else if (userRole === 'admin') next('/admin-dashboard');
    return;
  }

  next();
});

export default router;
