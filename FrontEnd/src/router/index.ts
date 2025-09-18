import { createRouter, createWebHistory } from 'vue-router';

import LoginPage from '../pages/LoginPage.vue';
import SignupPage from '../pages/SignupPage.vue';
import LandingPage from '../pages/LandingPage.vue';
import TutorDashboard from '../pages/TutorDashboard.vue';
import StudentDashboard from '../pages/StudentDashboard.vue';

//import TutorProfile from '../components/tutor/TutorProfile.vue';
import TutorReview from '../components/tutor/TutorReview.vue';
import TutorBookings from '../components/tutor/TutorBookings.vue';
import TutorChat from '../components/tutor/TutorChat.vue';
import TutorAvailability from '../components/tutor/TutorAvailability.vue';
import ProfilePage from '../pages/ProfilePage.vue';

import CreateProfile from '../components/profile/CreateProfile.vue';

const FindTutors = { template: '<div>Find Tutors (work in progress)</div>' };
const StudentBookings = { template: '<div>My Bookings (work in progress)</div>' };
const StudentReviews = { template: '<div>Reviews (work in progress)</div>' };
const StudentMessages = { template: '<div>Messages (work in progress)</div>' };
const StudentAccount = { template: '<div>My Account (work in progress)</div>' };

import { useUserStore } from '../store/userStore';
import { useProfileStore } from '../store/profileStore';

const routes = [
  // Guest routes
  { path: '/login', component: LoginPage, meta: { requiresGuest: true } },
  { path: '/signup', component: SignupPage, meta: { requiresGuest: true } },
  { path: '/landing', component: LandingPage },
  {
    path: '/create-profile',
    component: CreateProfile,
    meta: { requiresAuth: true, role: 'tutor' },
  },
  { path: '/', component: LandingPage, meta: { requiresGuest: true } },

  {
    path: '/student-dashboard',
    component: StudentDashboard,
    meta: { requiresAuth: true, role: 'student' },
    children: [
      { path: '', redirect: '/student-dashboard/find' }, // implicit → Find Tutors
      { path: 'find', component: FindTutors },
      { path: 'bookings', component: StudentBookings },
      { path: 'reviews', component: StudentReviews },
      { path: 'messages', component: StudentMessages },
      { path: 'account', component: StudentAccount },
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
