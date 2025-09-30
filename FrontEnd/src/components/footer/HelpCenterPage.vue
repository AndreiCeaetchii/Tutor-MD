<template>
  <div class="bg-white">
    <div class="py-16 bg-[#5f22d9] lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl font-extrabold tracking-tight text-white sm:text-5xl lg:text-6xl">
          Welcome to the <span class="text-orange-300">Help Center</span>
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-indigo-200">
          We’re here to guide you through every step of your learning journey.
        </p>
        <div class="mt-8 max-w-2xl mx-auto">
          <label for="search" class="sr-only">Search for help topics</label>
          <input type="search" name="search" id="search" placeholder="Search for help topics..."
                 class="block w-full px-5 py-3 text-base text-gray-900 placeholder-gray-500 border border-transparent rounded-full shadow-lg focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-[#5f22d9]">
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="grid grid-cols-1 gap-12 lg:grid-cols-3">
          <div class="p-6 bg-gray-50 rounded-xl shadow-lg hover:shadow-xl transition duration-300">
            <div class="flex items-center">
              <svg class="w-8 h-8 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 111.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.047 12.004 12.004 0 00-1.72 6.556c0 5.42 3.321 10.395 8.16 12.333a11.99 11.99 0 002.396 0c4.839-1.938 8.16-6.913 8.16-12.333a12.004 12.004 0 00-1.72-6.556z" /></svg>
              <h3 class="ml-4 text-xl font-semibold text-gray-900">Getting Started</h3>
            </div>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li><a href="#" class="hover:text-[#5f22d9]">How to create an account</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">How to find a tutor</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">How to become a tutor</a></li>
            </ul>
          </div>

          <div class="p-6 bg-gray-50 rounded-xl shadow-lg hover:shadow-xl transition duration-300">
            <div class="flex items-center">
              <svg class="w-8 h-8 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h.01M16 12h.01M21 21L3 21M5 7h14a2 2 0 012 2v10a2 2 0 01-2 2H5a2 2 0 01-2-2V9a2 2 0 012-2z" /></svg>
              <h3 class="ml-4 text-xl font-semibold text-gray-900">Lessons & Scheduling</h3>
            </div>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li><a href="#" class="hover:text-[#5f22d9]">Booking a lesson</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">Cancelling or rescheduling</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">Joining an online session</a></li>
            </ul>
          </div>

          <div class="p-6 bg-gray-50 rounded-xl shadow-lg hover:shadow-xl transition duration-300">
            <div class="flex items-center">
              <svg class="w-8 h-8 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-2.485 0-4 1.515-4 4s1.515 4 4 4 4-1.515 4-4-1.515-4-4-4zM21 12c0-4.418-4.03-8-9-8s-9 3.582-9 8 4.03 8 9 8 9-3.582 9-8z" /></svg>
              <h3 class="ml-4 text-xl font-semibold text-gray-900">Payments</h3>
            </div>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li><a href="#" class="hover:text-[#5f22d9]">Adding a payment method</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">Refunds and cancellations</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">Tutor payout process</a></li>
            </ul>
          </div>

          <div class="p-6 bg-gray-50 rounded-xl shadow-lg hover:shadow-xl transition duration-300">
            <div class="flex items-center">
              <svg class="w-8 h-8 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2z" /></svg>
              <h3 class="ml-4 text-xl font-semibold text-gray-900">Account & Security</h3>
            </div>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li><a href="#" class="hover:text-[#5f22d9]">Resetting your password</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">Enabling two-factor authentication</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">Managing personal data</a></li>
            </ul>
          </div>

          <div class="p-6 bg-gray-50 rounded-xl shadow-lg hover:shadow-xl transition duration-300">
            <div class="flex items-center">
              <svg class="w-8 h-8 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.75 17L9.25 10H14.75L14.25 17H9.75zM12 6a3 3 0 100 6 3 3 0 000-6z" /></svg>
              <h3 class="ml-4 text-xl font-semibold text-gray-900">Policies & Safety</h3>
            </div>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li><a href="/safety" class="hover:text-[#5f22d9]">Policies & Safety</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">Reporting an issue</a></li>
              <li><a href="#" class="hover:text-[#5f22d9]">Privacy policy overview</a></li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 bg-gray-50 border-t border-gray-200">
      <div class="px-4 mx-auto text-center max-w-7xl sm:px-6 lg:px-8">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Didn’t find what you were looking for?
        </h2>
        <p class="mt-4 text-xl text-gray-600">
          Our support team is here to help.
        </p>
        <div class="mt-8">
          <router-link to="/contact-us" class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-orange-500 border border-transparent rounded-full shadow hover:bg-orange-600">
            Contact Us
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>