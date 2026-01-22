<template>
  <div class="bg-white">
    <div class="py-16 bg-gradient-to-br from-blue-50 to-purple-50 lg:py-24">
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
                d="M9 7h6m0 10v-3m-3 3h.01M9 17h.01M9 14h.01M12 14h.01M15 11h.01M12 11h.01M9 11h.01M7 21h10a2 2 0 002-2V5a2 2 0 00-2-2H7a2 2 0 00-2 2v14a2 2 0 002 2z"
              />
            </svg>
          </div>
        </div>
        <h1 class="text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl">
          Master <span class="text-[#5f22d9]">Mathematics</span> with Expert Tutors
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-gray-600">
          From basic arithmetic to advanced calculus, our certified math tutors help you build
          confidence and achieve excellence in mathematics.
        </p>
        <div class="mt-8">
          <router-link
            to="/student-dashboard/find-tutors"
            class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-[#5f22d9] border border-transparent rounded-full shadow hover:bg-purple-700"
          >
            Find a Math Tutor
          </router-link>
          <router-link
            to="/become-a-tutor"
            class="ml-4 inline-flex items-center px-8 py-3 text-base font-medium text-gray-700 transition-colors bg-white border border-gray-300 rounded-full shadow hover:bg-gray-50"
          >
            Become a Math Tutor
          </router-link>
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="mb-16 text-center">
          <h2 class="text-base font-semibold tracking-widest text-orange-500 uppercase">
            TOPICS WE COVER
          </h2>
          <p class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
            Comprehensive Math Curriculum
          </p>
        </div>

        <div class="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Elementary Math</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Basic Arithmetic</li>
              <li>• Fractions & Decimals</li>
              <li>• Number Theory</li>
              <li>• Geometry Basics</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Middle School Math</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Pre-Algebra</li>
              <li>• Ratios & Proportions</li>
              <li>• Basic Statistics</li>
              <li>• Introduction to Algebra</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Algebra</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Linear Equations</li>
              <li>• Polynomials</li>
              <li>• Quadratic Functions</li>
              <li>• Systems of Equations</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Geometry</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Triangles & Circles</li>
              <li>• Area & Volume</li>
              <li>• Trigonometry</li>
              <li>• Proofs & Theorems</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Calculus</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Limits & Continuity</li>
              <li>• Derivatives</li>
              <li>• Integrals</li>
              <li>• Differential Equations</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Advanced Topics</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Linear Algebra</li>
              <li>• Statistics & Probability</li>
              <li>• Discrete Mathematics</li>
              <li>• Real Analysis</li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 bg-gray-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <h2 class="text-3xl font-extrabold tracking-tight text-center text-gray-900 sm:text-4xl">
          Why Choose Our Math Tutors?
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
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Certified Experts</h3>
            <p class="mt-2 text-gray-600">
              All tutors are verified professionals with proven track records in mathematics
              education.
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
                  d="M12 6V4m0 2a2 2 0 100 4m0-4a2 2 0 110 4m-6 8a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4m6 6v10m6-2a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Personalized Learning</h3>
            <p class="mt-2 text-gray-600">
              Tailored lesson plans that adapt to your learning style and pace.
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
                  d="M13 10V3L4 14h7v7l9-11h-7z"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Proven Results</h3>
            <p class="mt-2 text-gray-600">
              Our students consistently improve their grades and test scores.
            </p>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Ready to Excel in Mathematics?
        </h2>
        <p class="mt-4 text-xl text-gray-600 max-w-3xl mx-auto">
          Connect with expert math tutors today and transform your understanding of mathematics.
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
