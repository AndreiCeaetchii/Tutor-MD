<template>
  <div class="bg-white">
    <div class="py-16 bg-[#5f22d9] lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <svg class="w-16 h-16 mx-auto text-orange-300" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.047 12.004 12.004 0 00-1.72 6.556c0 5.42 3.321 10.395 8.16 12.333a11.99 11.99 0 002.396 0c4.839-1.938 8.16-6.913 8.16-12.333a12.004 12.004 0 00-1.72-6.556z" /></svg>
        <h1 class="mt-4 text-4xl font-extrabold tracking-tight text-white sm:text-5xl lg:text-6xl">
          Your <span class="text-orange-300">Safety</span>, Our Priority
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-indigo-200">
          Tutor is committed to creating a secure and trustworthy learning environment for students, parents, and tutors alike.
        </p>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="grid grid-cols-1 gap-10 md:grid-cols-3">
          <div class="p-6 bg-gray-50 rounded-xl shadow-lg border-t-4 border-orange-500">
            <h3 class="text-2xl font-bold text-gray-900">Student & Parent Safety</h3>
            <ul class="mt-4 space-y-3 text-gray-600">
              <li>Verified tutor profiles with background checks where applicable.</li>
              <li>Transparent tutor reviews and ratings.</li>
              <li>Secure online classroom with monitored communication channels.</li>
            </ul>
          </div>

          <div class="p-6 bg-gray-50 rounded-xl shadow-lg border-t-4 border-[#5f22d9]">
            <h3 class="text-2xl font-bold text-gray-900">Tutor Safety</h3>
            <ul class="mt-4 space-y-3 text-gray-600">
              <li>Clear cancellation and refund policies to prevent unfair loss of time.</li>
              <li>Secure payment system with guaranteed payouts.</li>
              <li>Reporting system to flag inappropriate student behavior.</li>
            </ul>
          </div>

          <div class="p-6 bg-gray-50 rounded-xl shadow-lg border-t-4 border-orange-500">
            <h3 class="text-2xl font-bold text-gray-900">Data & Platform Security</h3>
            <ul class="mt-4 space-y-3 text-gray-600">
              <li>End-to-end encrypted communication.</li>
              <li>Protected payment processing via trusted providers.</li>
              <li>Strict compliance with privacy regulations (GDPR-style).</li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 bg-[#5f22d9]/10">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Community Responsibility
        </h2>
        <p class="mt-4 text-xl text-gray-600 max-w-4xl mx-auto">
          Every user agrees to follow Community Guidelines. We take immediate action on reports of misconduct and continuously monitor and update security measures.
        </p>
        <div class="mt-8">
          <router-link to="/community-guidelines" class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-orange-500 border border-transparent rounded-full shadow hover:bg-orange-600">
            Read Community Guidelines
          </router-link>
        </div>
      </div>
    </div>

    <div class="py-8 text-center">
      <p class="text-lg text-gray-700">Safety is a shared responsibility. Together, we ensure a positive, reliable, and secure environment for learning.</p>
    </div>
  </div>
</template>