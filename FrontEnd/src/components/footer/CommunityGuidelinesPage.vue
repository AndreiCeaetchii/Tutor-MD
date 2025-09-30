<template>
  <div class="bg-white">
    <div class="py-16 bg-gray-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl">
          Building a <span class="text-[#5f22d9]">Respectful</span> Learning Community
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-gray-600">
          Trust and respect are the foundation of effective learning. These guidelines ensure a safe and positive experience for every user.
        </p>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="space-y-12">
          <div class="p-8 bg-white shadow-xl rounded-xl border-l-4 border-orange-500">
            <h2 class="text-2xl font-bold text-gray-900 flex items-center">
              <svg class="w-6 h-6 mr-3 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.5l6 6m0 0l-6 6m6-6H4" /></svg>
              Respect & Professionalism
            </h2>
            <ul class="mt-4 space-y-2 text-gray-600 list-disc list-inside">
              <li>Communicate politely and respectfully at all times.</li>
              <li>Be punctual and prepared for scheduled lessons.</li>
              <li>Avoid offensive, discriminatory, or inappropriate language and behavior.</li>
            </ul>
          </div>

          <div class="p-8 bg-white shadow-xl rounded-xl border-l-4 border-[#5f22d9]">
            <h2 class="text-2xl font-bold text-gray-900 flex items-center">
              <svg class="w-6 h-6 mr-3 text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5 5.754 5 4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18s-3.332.477-4.5 1.253" /></svg>
              Academic Integrity
            </h2>
            <ul class="mt-4 space-y-2 text-gray-600 list-disc list-inside">
              <li>Students must not request tutors to complete assignments, exams, or other forms of academic dishonesty.</li>
              <li>Tutors must encourage independent learning and skill development.</li>
            </ul>
          </div>

          <div class="p-8 bg-white shadow-xl rounded-xl border-l-4 border-orange-500">
            <h2 class="text-2xl font-bold text-gray-900 flex items-center">
              <svg class="w-6 h-6 mr-3 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2z" /></svg>
              Safety & Privacy
            </h2>
            <ul class="mt-4 space-y-2 text-gray-600 list-disc list-inside">
              <li>Share only necessary information for the learning process.</li>
              <li>Never exchange personal contact details outside the platform.</li>
              <li>Use the platform’s secure chat and video tools for all communication.</li>
            </ul>
          </div>

          <div class="p-8 bg-white shadow-xl rounded-xl border-l-4 border-[#5f22d9]">
            <h2 class="text-2xl font-bold text-gray-900 flex items-center">
              <svg class="w-6 h-6 mr-3 text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v-1a3 3 0 013-3h10a3 3 0 013 3v1m-6 4v-4m0 0v-4m0 4h.01M19 12a7 7 0 11-14 0 7 7 0 0114 0z" /></svg>
              Transparency & Trust
            </h2>
            <ul class="mt-4 space-y-2 text-gray-600 list-disc list-inside">
              <li>Tutors must keep their profiles accurate and updated.</li>
              <li>Students and parents should provide honest reviews.</li>
              <li>Report any suspicious activity immediately to the support team.</li>
            </ul>
          </div>

          <div class="p-8 bg-red-50 rounded-xl border border-red-200">
            <h2 class="text-2xl font-bold text-red-700 flex items-center">
              <svg class="w-6 h-6 mr-3 text-red-700" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
              Consequences of Violations
            </h2>
            <ul class="mt-4 space-y-2 text-red-600 list-disc list-inside">
              <li>Warnings for minor violations.</li>
              <li>Temporary suspension for repeated misconduct.</li>
              <li>Permanent ban for serious breaches of safety or trust.</li>
            </ul>
          </div>
        </div>

        <div class="mt-16 pt-8 border-t border-gray-200 text-center">
          <p class="text-lg text-gray-700">
            By joining Tutor, you agree to follow these guidelines. Together, we create a respectful, safe, and productive environment where learning thrives.
          </p>
          <div class="mt-8">
            <router-link to="/contact-us" class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-[#5f22d9] border border-transparent rounded-full shadow hover:bg-purple-700">
              Report a Violation
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>