import { createRouter, createWebHistory } from 'vue-router';
import LoginPage from '../pages/LoginPage.vue';
import SignupPage from '../pages/SignupPage.vue';
import LandingPage from '../pages/LandingPage.vue';
import TutorDashboard from '../pages/TutorDashboard.vue';
import StudentDashboard from '../pages/StudentDashboard.vue';
import { useUserStore } from '../store/userStore';
import TutorProfile from '../components/tutor/TutorProfile.vue';
import TutorReview from '../components/tutor/TutorReview.vue';

const routes = [
  { path: '/login', component: LoginPage, meta: { requiresGuest: true } },
  { path: '/signup', component: SignupPage, meta: { requiresGuest: true } },
  { path: '/landing', component: LandingPage },
  { path: '/', component: LandingPage },
  {
    path: '/tutor-dashboard',
    component: TutorDashboard,
    meta: { requiresAuth: true, role: 'tutor' },
  },
  {
    path: '/tutor-dashboard-reviews',
    component: TutorReview,
    meta: { requiresAuth: true, role: 'tutor' },
  },
  {
    path: '/tutor-dashboard-profile',
    component: TutorProfile,
    meta: { requiresAuth: true, role: 'tutor' },
  },
  {
    path: '/student-dashboard',
    component: StudentDashboard,
    meta: { requiresAuth: true, role: 'student' },
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, _, next) => {
  const store = useUserStore();

  const hasToken = store.isAuthenticated; // există token
  const userRole = store.role;
  console.log(userRole);

  // Rute care necesită autentificare
  if (to.meta.requiresAuth) {
    if (!hasToken) {
      next('/login'); // dacă nu e token → login
      return;
    }

    if (to.meta.role && to.meta.role !== userRole) {
      // rol greșit redirecționare către dashboard-ul corect
      if (userRole === 'tutor') next('/tutor-dashboard');
      else if (userRole === 'student') next('/student-dashboard');
      else next('/landing'); // fallback
      return;
    }
    // totul ok
    next();
    return;
  }

  // Rute pentru guest (login/signup)
  if (to.meta.requiresGuest && hasToken) {
    // deja logat, redirect către dashboard-ul corect
    if (userRole === 'tutor') next('/tutor-dashboard');
    else if (userRole === 'student') next('/student-dashboard');
    else next('/landing'); // fallback
    return;
  }

  next();
});

export default router;
