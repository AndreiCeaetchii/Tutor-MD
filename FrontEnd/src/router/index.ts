import { createRouter, createWebHistory } from 'vue-router';

import LoginPage from '../pages/LoginPage.vue';
import SignupPage from '../pages/SignupPage.vue';
import LandingPage from '../pages/LandingPage.vue';
import TutorDashboard from '../pages/TutorDashboard.vue';
import StudentDashboard from '../pages/StudentDashboard.vue';

import StudentProfile from '../components/student/profile/StudentProfile.vue';
import TutorReview from '../components/tutor/Review/TutorReview.vue';
import TutorBookings from '../components/tutor/Bookings/TutorBookings.vue';
import TutorChat from '../components/tutor/Chat/TutorChat.vue';
import TutorAvailability from '../components/tutor/Availability/TutorAvailability.vue';

import ProfilePage from '../pages/ProfilePage.vue';

import CreateProfile from '../components/tutor/Profile/CreateProfile.vue';
import CreateStudentProfile from '../components/student/profile/CreateStudentProfile.vue';

import { useUserStore } from '../store/userStore';
import { useProfileStore } from '../store/profileStore';
import { useStudentProfileStore } from '../store/studentProfileStore';
import FindTutor from '../components/student/FindTutor/FindTutor.vue';
import StudentBookings from '../components/student/Bookings/StudentBookings.vue';
import StudentChat from '../components/student/Chat/StudentChat.vue';
import FavouriteTutors from '../components/student/FavouriteTutor/FavouriteTutors.vue';

import GuestPage from '../pages/GuestPage.vue';

import AdminDashboard from '../pages/AdminDashboard.vue';
import AdminUsers from '../components/admin/AdminUsers.vue';
import AdminAnalytics from '../components/admin/AdminAnalytics.vue';
import AdminOverview from '../components/admin/AdminOverview.vue';
import AdminNotifications from '../components/admin/AdminNotifications.vue';
import MFASetupPage from '../pages/MFASetupPage.vue';

import HowItWorksPage from '../components/footer/HowItWorksPage.vue';
import HelpCenterPage from '../components/footer/HelpCenterPage.vue';
import AboutUsPage from '../components/footer/AboutUsPage.vue';
import BecomeATutorPage from '../components/footer/BecomeATutorPage.vue';
import PricingPage from '../components/footer/PricingPage.vue';
import SafetyPage from '../components/footer/SafetyPage.vue';
import CommunityGuidelinesPage from '../components/footer/CommunityGuidelinesPage.vue';
import ContactUsPage from '../components/footer/ContactUsPage.vue';
import CareersPage from '../components/footer/CareersPage.vue';
import PressPage from '../components/footer/PressPage.vue';
import BlogPage from '../components/footer/BlogPage.vue';

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
      { path: '', redirect: '/student-dashboard/find-tutors' },
      { path: 'find-tutors', component: FindTutor },
      { path: 'bookings', component: StudentBookings },
      { path: 'favourite-tutors', component: FavouriteTutors },
      { path: 'messages', component: StudentChat },
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

  {
    path: '/admin-dashboard',
    component: AdminDashboard,
    meta: { requiresAuth: true, role: 'admin' },
    children: [
      { path: '', redirect: '/admin-dashboard/overview' },
      { path: 'overview', component: AdminOverview },
      { path: 'users', component: AdminUsers },
      { path: 'notifications', component: AdminNotifications },
      { path: 'analytics', component: AdminAnalytics },
    ],
  },

  {
    path: '/tutor/:id',
    component: GuestPage,
    meta: { requiresAuth: true },
    children: [
      { path: '', redirect: 'profile' },
      { path: 'profile', component: ProfilePage },
      { path: 'availability', component: TutorAvailability },
      { path: 'reviews', component: TutorReview },
    ],
  },
  {
    path: '/student/:id',
    component: GuestPage,
    meta: { requiresAuth: true },
  },
  {
  path: '/mfa-setup',
  name: 'MFASetup',
  component: MFASetupPage,
  meta: {
    requiresAuth: true
  }
},

  { path: '/how-it-works', component: HowItWorksPage },
  { path: '/become-a-tutor', component: BecomeATutorPage },
  { path: '/pricing', component: PricingPage },

  { path: '/help-center', component: HelpCenterPage },
  { path: '/contact-us', component: ContactUsPage },
  { path: '/safety', component: SafetyPage },
  { path: '/community-guidelines', component: CommunityGuidelinesPage },

  { path: '/about-us', component: AboutUsPage },
  { path: '/careers', component: CareersPage },
  { path: '/press', component: PressPage },
  { path: '/blog', component: BlogPage },

  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: () => import('../pages/NotFoundPage.vue'),
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(_to, _from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    }
    return { top: 0 }
  },
});

router.beforeEach(async (to, _, next) => {
  const store = useUserStore();
  const hasToken = store.isAuthenticated;
  const userRole = store.userRole;
  const profileStore = useProfileStore();
  const studentProfileStore = useStudentProfileStore();

  if (to.meta.requiresAuth && !hasToken) {
    next('/login');
    return;
  }

  if (hasToken) {
    if (userRole === 'tutor' && !profileStore.firstName) {
      await profileStore.loadProfile();
    }
    else if (userRole === 'student' && !studentProfileStore.userProfile.firstName) {
      await studentProfileStore.loadProfile();
    }
  }
  const isTutorProfileCreated = profileStore.firstName && profileStore.lastName;
  const isStudentProfileCreated = studentProfileStore.userProfile.firstName && studentProfileStore.userProfile.lastName;

  if (hasToken && userRole) {
    const isTutorCreationRoute = to.path === '/create-profile';
    const isStudentCreationRoute = to.path === '/create-student-profile';

    if (userRole === 'tutor' && !isTutorProfileCreated && !isTutorCreationRoute) {
      next('/create-profile');
      return;
    }

    if (userRole === 'student' && !isStudentProfileCreated && !isStudentCreationRoute) {
      next('/create-student-profile');
      return;
    }

    if (isTutorCreationRoute && isTutorProfileCreated) {
      next('/tutor-dashboard');
      return;
    }
    if (isStudentCreationRoute && isStudentProfileCreated) {
      next('/student-dashboard');
      return;
    }
  }

  if (hasToken && to.meta.role && to.meta.role !== userRole) {
    if (userRole === 'tutor') next('/tutor-dashboard');
    else if (userRole === 'student') next('/student-dashboard');
    else if (userRole === 'admin') next('/admin-dashboard');
    else next('/landing');
    return;
  }

  if (to.meta.requiresGuest && hasToken) {
    if (userRole === 'tutor') next('/tutor-dashboard');
    else if (userRole === 'student') next('/student-dashboard');
    else if (userRole === 'admin') next('/admin-dashboard');
    else next('/landing');
    return;
  }

  next();
});

export default router;
