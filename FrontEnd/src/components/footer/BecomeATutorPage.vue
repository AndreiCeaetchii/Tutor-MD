<template>
  <div class="bg-white">
    <div class="py-16 bg-[#5f22d9] lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl font-extrabold tracking-tight text-white sm:text-5xl lg:text-6xl">
          Share Your Knowledge. <span class="text-orange-300">Inspire Students</span>.
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-indigo-200">
          Join our platform as a tutor and connect with learners who need your expertise. Earn on your own terms.
        </p>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <h2 class="text-base font-semibold tracking-widest text-orange-500 uppercase text-center">WHY CHOOSE US</h2>
        <p class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl text-center">
          The Tutor Advantage for Educators
        </p>
        <div class="mt-12 grid grid-cols-1 gap-8 md:grid-cols-3">
          <div class="p-6 text-center bg-gray-50 rounded-xl shadow-md">
            <svg class="w-10 h-10 mx-auto text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h.01M16 12h.01M21 21H3a2 2 0 01-2-2V9a2 2 0 012-2h18a2 2 0 012 2v10a2 2 0 01-2 2z" /></svg>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Flexible Schedule</h3>
            <p class="mt-2 text-gray-600">Teach when it suits you. Complete control over your availability.</p>
          </div>
          <div class="p-6 text-center bg-gray-50 rounded-xl shadow-md">
            <svg class="w-10 h-10 mx-auto text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-2.485 0-4 1.515-4 4s1.515 4 4 4 4-1.515 4-4-1.515-4-4-4zM21 12c0-4.418-4.03-8-9-8s-9 3.582-9 8 4.03 8 9 8 9-3.582 9-8z" /></svg>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Set Your Rates</h3>
            <p class="mt-2 text-gray-600">You set your own competitive hourly rates based on your expertise.</p>
          </div>
          <div class="p-6 text-center bg-gray-50 rounded-xl shadow-md">
            <svg class="w-10 h-10 mx-auto text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5h12M9 3v2m0 16v-2m0-12V7M7 21h10M12 4h.01" /></svg>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Global Reach</h3>
            <p class="mt-2 text-gray-600">Access to a global pool of students seeking your specific skills.</p>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 bg-[#5f22d9]/10 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <h2 class="text-base font-semibold tracking-widest text-orange-500 uppercase text-center">TUTOR JOURNEY</h2>
        <p class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl text-center">
          How to Start Teaching
        </p>

        <div class="mt-12 relative">
          <div class="hidden lg:block absolute inset-y-0 left-1/2 w-0.5 bg-gray-300 transform -translate-x-1/2"></div>

          <div class="grid grid-cols-1 gap-10 lg:grid-cols-3">
            <div class="relative flex flex-col items-center text-center">
              <div class="flex items-center justify-center w-12 h-12 text-white bg-[#5f22d9] rounded-full text-2xl font-bold shadow-lg ring-4 ring-white">1</div>
              <h3 class="mt-5 text-xl font-medium text-gray-900">Sign Up</h3>
              <p class="mt-2 text-gray-600">Create your detailed tutor profile with education, experience, and subjects taught.</p>
            </div>

            <div class="relative flex flex-col items-center text-center">
              <div class="flex items-center justify-center w-12 h-12 text-white bg-[#5f22d9] rounded-full text-2xl font-bold shadow-lg ring-4 ring-white">2</div>
              <h3 class="mt-5 text-xl font-medium text-gray-900">Get Verified</h3>
              <p class="mt-2 text-gray-600">Our team reviews your profile and qualifications to ensure quality and trust for our students.</p>
            </div>

            <div class="relative flex flex-col items-center text-center">
              <div class="flex items-center justify-center w-12 h-12 text-white bg-[#5f22d9] rounded-full text-2xl font-bold shadow-lg ring-4 ring-white">3</div>
              <h3 class="mt-5 text-xl font-medium text-gray-900">Start Teaching</h3>
              <p class="mt-2 text-gray-600">Receive booking requests, conduct lessons online, and build your reputation with positive reviews.</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16">
      <div class="px-4 mx-auto text-center max-w-7xl sm:px-6 lg:px-8">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Ready to make an impact?
        </h2>
        <div class="mt-8">
          <router-link to="/signup" class="inline-flex items-center px-8 py-3 text-lg font-medium text-white transition-colors bg-orange-500 border border-transparent rounded-full shadow-lg hover:bg-orange-600">
            Apply Now
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>