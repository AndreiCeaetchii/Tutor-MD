<template>
  <div class="bg-white">
    <div class="py-16 bg-gray-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl">
          Our <span class="text-[#5f22d9]">Mission</span>: Unlocking Potential Through Education
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-gray-600">
          Tutor connects students with dedicated educators to bridge knowledge gaps, build confidence, and achieve academic success.
        </p>
        <div class="mt-8">
          <router-link to="/find-tutors" class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-[#5f22d9] border border-transparent rounded-full shadow hover:bg-purple-700">
            Find Your Tutor
          </router-link>
          <router-link to="/become-a-tutor" class="ml-4 inline-flex items-center px-8 py-3 text-base font-medium text-gray-700 transition-colors bg-white border border-gray-300 rounded-full shadow hover:bg-gray-50">
            Join as a Tutor
          </router-link>
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="mb-16">
          <h2 class="text-base font-semibold tracking-widest text-orange-500 uppercase">WHO WE ARE</h2>
          <p class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
            A Platform Built for Learning
          </p>
          <p class="mt-4 text-lg text-gray-600 max-w-3xl">
            Tutor is an innovative platform designed to make **quality education accessible and personalized**. We bring together students, parents, and professional tutors in one safe, easy-to-use digital environment.
          </p>
        </div>

        <div>
          <h2 class="text-base font-semibold tracking-widest text-orange-500 uppercase">WHAT WE DO</h2>
          <div class="mt-6 space-y-10 md:space-y-0 md:grid md:grid-cols-3 md:gap-x-8 md:gap-y-10">
            <div class="flex">
              <svg class="flex-shrink-0 w-6 h-6 text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" /></svg>
              <p class="ml-3 text-lg font-medium text-gray-900">
                Help students find verified tutors in mathematics, sciences, languages, and programming.
              </p>
            </div>
            <div class="flex">
              <svg class="flex-shrink-0 w-6 h-6 text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z" /></svg>
              <p class="ml-3 text-lg font-medium text-gray-900">
                Empower tutors to reach more learners and manage their schedules efficiently.
              </p>
            </div>
            <div class="flex">
              <svg class="flex-shrink-0 w-6 h-6 text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 10l4.55 4.55L21 17M2 13.5l5-5L12 11l7-7 2 2-7 7-5-5-5 5z" /></svg>
              <p class="ml-3 text-lg font-medium text-gray-900">
                Provide parents with tools to monitor progress and ensure accountability.
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 bg-[#5f22d9]/5 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <h2 class="text-base font-semibold tracking-widest text-orange-500 uppercase text-center">OUR VALUES</h2>
        <p class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl text-center">
          The Principles That Guide Us
        </p>
        <dl class="mt-12 space-y-10 md:space-y-0 md:grid md:grid-cols-4 md:gap-x-8 md:gap-y-10">
          <div class="relative">
            <dt>
              <p class="text-xl font-medium leading-6 text-gray-900">Accessibility</p>
            </dt>
            <dd class="mt-2 text-base text-gray-600">Learning opportunities for every student, regardless of location or budget.</dd>
          </div>
          <div class="relative">
            <dt>
              <p class="text-xl font-medium leading-6 text-gray-900">Trust</p>
            </dt>
            <dd class="mt-2 text-base text-gray-600">Verified tutors and secure communication built on transparency.</dd>
          </div>
          <div class="relative">
            <dt>
              <p class="text-xl font-medium leading-6 text-gray-900">Quality</p>
            </dt>
            <dd class="mt-2 text-base text-gray-600">Focus on effective, result-driven tutoring that achieves academic success.</dd>
          </div>
          <div class="relative">
            <dt>
              <p class="text-xl font-medium leading-6 text-gray-900">Innovation</p>
            </dt>
            <dd class="mt-2 text-base text-gray-600">Modern tools and features that continuously enhance the online learning process.</dd>
          </div>
        </dl>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Why Choose Tutor?
        </h2>
        <p class="mt-4 text-xl text-gray-600 max-w-4xl mx-auto">
          Unlike traditional tutoring or unverified listings, Tutor ensures a structured, safe, and transparent learning experience, combining flexibility with accountability.
        </p>
        <p class="mt-12 text-2xl font-semibold text-gray-700">
          At Tutor, we believe education has the power to transform lives. By connecting learners and educators, we’re building a future where everyone has the opportunity to thrive.
        </p>
      </div>
    </div>
  </div>
</template>