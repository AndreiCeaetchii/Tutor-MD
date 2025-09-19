import { createRouter, createWebHistory } from 'vue-router';

import LoginPage from '../pages/LoginPage.vue';
import SignupPage from '../pages/SignupPage.vue';
import LandingPage from '../pages/LandingPage.vue';
import TutorDashboard from '../pages/TutorDashboard.vue';
import StudentDashboard from '../pages/StudentDashboard.vue';

import StudentProfile from '../components/student/profile/StudentProfile.vue';
import TutorReview from '../components/tutor/TutorReview.vue';
import TutorBookings from '../components/tutor/TutorBookings.vue';
import TutorChat from '../components/tutor/TutorChat.vue';
import TutorAvailability from '../components/tutor/TutorAvailability.vue';
import ProfilePage from '../pages/ProfilePage.vue';

import CreateProfile from '../components/profile/CreateProfile.vue';
import CreateStudentProfile from '../components/student/profile/CreateStudentProfile.vue';

import StudentSearchPage from '../pages/StudentSearchPage.vue';
const StudentBookings = { template: '<div>My Bookings (work in progress)</div>' };
const StudentReviews = { template: '<div>Reviews (work in progress)</div>' };
const StudentMessages = { template: '<div>Messages (work in progress)</div>' };

import { useUserStore } from '../store/userStore';
import { useProfileStore } from '../store/profileStore';

const routes = [
  { path: '/login', component: LoginPage, meta: { requiresGuest: true } },
  { path: '/signup', component: SignupPage, meta: { requiresGuest: true } },
  { path: '/landing', component: LandingPage },
  {
    path: '/create-profile',
    component: CreateProfile,
    meta: { requiresAuth: true, role: 'tutor' },
  },
  {
    path: '/create-student-profile',
    component: CreateStudentProfile,
    meta: { requiresAuth: true, role: 'student' },
  },
  { path: '/', component: LandingPage, meta: { requiresGuest: true } },

  {
    path: '/student-dashboard',
    component: StudentDashboard,
    meta: { requiresAuth: true, role: 'student' },
    children: [
      { path: '', redirect: '/student-dashboard/find' },
      { path: 'find', component: StudentSearchPage },
      { path: 'bookings', component: StudentBookings },
      { path: 'reviews', component: StudentReviews },
      { path: 'messages', component: StudentMessages },
      { path: 'account', component: StudentProfile },
    ],
  },

  {
    path: '/tutor-dashboard',
    component: TutorDashboard,
    meta: { requiresAuth: true, role: 'tutor' },
    children: [
      { path: '', redirect: '/tutor-dashboard/profile' },
      { path: 'profile', component: ProfilePage },
      { path: 'reviews', component: TutorReview },
      { path: 'availability', component: TutorAvailability },
      { path: 'bookings', component: TutorBookings },
      { path: 'messages', component: TutorChat },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, _, next) => {
  const store = useUserStore();
  const hasToken = store.isAuthenticated;
  const userRole = store.userRole;
  const profileStore = useProfileStore();

  if (to.path === '/create-profile') {
    if (profileStore.firstName && profileStore.lastName) {
      next('/tutor-dashboard');
      return;
    }
  }

  if (to.meta.requiresAuth) {
    if (!hasToken) {
      next('/login');
      return;
    }
    if (to.meta.role && to.meta.role !== userRole) {
      if (userRole === 'tutor') next('/tutor-dashboard');
      else if (userRole === 'student') next('/student-dashboard');
      else next('/landing');
      return;
    }
    next();
    return;
  }

  if (to.meta.requiresGuest && hasToken) {
    if (userRole === 'tutor') next('/tutor-dashboard');
    else if (userRole === 'student') next('/student-dashboard');
    else next('/landing');
    return;
  }

  next();
});

export default router;
