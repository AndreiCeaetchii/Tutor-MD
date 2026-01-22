<template>
  <div class="bg-white">
    <div class="py-16 bg-gradient-to-br from-green-50 to-blue-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <div class="flex justify-center mb-6">
          <div class="p-4 bg-white rounded-full shadow-lg">
            <svg
              class="w-16 h-16 text-[#5f22d9]"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z"
              />
            </svg>
          </div>
        </div>
        <h1 class="text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl">
          Explore <span class="text-[#5f22d9]">Science</span> with Passionate Tutors
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-gray-600">
          Discover the wonders of biology, chemistry, physics, and more with expert tutors who make
          science engaging and understandable.
        </p>
        <div class="mt-8">
          <router-link
            to="/student-dashboard/find-tutors"
            class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-[#5f22d9] border border-transparent rounded-full shadow hover:bg-purple-700"
          >
            Find a Science Tutor
          </router-link>
          <router-link
            to="/become-a-tutor"
            class="ml-4 inline-flex items-center px-8 py-3 text-base font-medium text-gray-700 transition-colors bg-white border border-gray-300 rounded-full shadow hover:bg-gray-50"
          >
            Become a Science Tutor
          </router-link>
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="mb-16 text-center">
          <h2 class="text-base font-semibold tracking-widest text-orange-500 uppercase">
            SUBJECTS WE OFFER
          </h2>
          <p class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
            Complete Science Education
          </p>
        </div>

        <div class="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Biology</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Cell Biology</li>
              <li>• Genetics & Evolution</li>
              <li>• Ecology & Environment</li>
              <li>• Human Anatomy</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Chemistry</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Organic Chemistry</li>
              <li>• Inorganic Chemistry</li>
              <li>• Physical Chemistry</li>
              <li>• Biochemistry</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Physics</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Mechanics</li>
              <li>• Electricity & Magnetism</li>
              <li>• Thermodynamics</li>
              <li>• Quantum Physics</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Earth Science</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Geology</li>
              <li>• Meteorology</li>
              <li>• Oceanography</li>
              <li>• Environmental Science</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Astronomy</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Solar System</li>
              <li>• Stars & Galaxies</li>
              <li>• Cosmology</li>
              <li>• Astrophysics</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Test Preparation</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• AP Biology/Chemistry/Physics</li>
              <li>• SAT Subject Tests</li>
              <li>• GCSE Science</li>
              <li>• IB Science</li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 bg-gray-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <h2 class="text-3xl font-extrabold tracking-tight text-center text-gray-900 sm:text-4xl">
          Why Learn Science with Us?
        </h2>
        <div class="grid gap-8 mt-12 md:grid-cols-3">
          <div class="text-center">
            <div class="flex justify-center">
              <svg
                class="w-12 h-12 text-[#5f22d9]"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M9.663 17h4.673M12 3v1m6.364 1.636l-.707.707M21 12h-1M4 12H3m3.343-5.657l-.707-.707m2.828 9.9a5 5 0 117.072 0l-.548.547A3.374 3.374 0 0014 18.469V19a2 2 0 11-4 0v-.531c0-.895-.356-1.754-.988-2.386l-.548-.547z"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Hands-On Learning</h3>
            <p class="mt-2 text-gray-600">
              Interactive experiments and demonstrations that bring science concepts to life.
            </p>
          </div>

          <div class="text-center">
            <div class="flex justify-center">
              <svg
                class="w-12 h-12 text-[#5f22d9]"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Real-World Applications</h3>
            <p class="mt-2 text-gray-600">
              Connect scientific principles to everyday phenomena and practical uses.
            </p>
          </div>

          <div class="text-center">
            <div class="flex justify-center">
              <svg
                class="w-12 h-12 text-[#5f22d9]"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Expert Tutors</h3>
            <p class="mt-2 text-gray-600">
              Learn from qualified scientists and educators passionate about teaching.
            </p>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Ready to Discover the World of Science?
        </h2>
        <p class="mt-4 text-xl text-gray-600 max-w-3xl mx-auto">
          Start your scientific journey with our expert tutors and unlock your potential in STEM
          fields.
        </p>
        <div class="mt-8">
          <router-link
            to="/student-dashboard/find-tutors"
            class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-[#5f22d9] border border-transparent rounded-full shadow hover:bg-purple-700"
          >
            Get Started Now
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts"></script>
