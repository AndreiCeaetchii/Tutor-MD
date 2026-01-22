<template>
  <div class="bg-white">
    <div class="py-16 bg-gradient-to-br from-indigo-50 to-cyan-50 lg:py-24">
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
                d="M10 20l4-16m4 4l4 4-4 4M6 16l-4-4 4-4"
              />
            </svg>
          </div>
        </div>
        <h1 class="text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl">
          Master <span class="text-[#5f22d9]">Programming</span> with Industry Experts
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-gray-600">
          Learn to code from scratch or advance your skills with expert tutors who work in the tech
          industry.
        </p>
        <div class="mt-8">
          <router-link
            to="/student-dashboard/find-tutors"
            class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-[#5f22d9] border border-transparent rounded-full shadow hover:bg-purple-700"
          >
            Find a Programming Tutor
          </router-link>
          <router-link
            to="/become-a-tutor"
            class="ml-4 inline-flex items-center px-8 py-3 text-base font-medium text-gray-700 transition-colors bg-white border border-gray-300 rounded-full shadow hover:bg-gray-50"
          >
            Become a Programming Tutor
          </router-link>
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="mb-16 text-center">
          <h2 class="text-base font-semibold tracking-widest text-orange-500 uppercase">
            TECHNOLOGIES WE TEACH
          </h2>
          <p class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
            Comprehensive Programming Courses
          </p>
        </div>

        <div class="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Web Development</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• HTML, CSS, JavaScript</li>
              <li>• React, Vue, Angular</li>
              <li>• Node.js & Express</li>
              <li>• Full-Stack Development</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Python Programming</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Python Basics</li>
              <li>• Data Science & Analysis</li>
              <li>• Machine Learning</li>
              <li>• Django & Flask</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Java Development</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Core Java</li>
              <li>• Spring Framework</li>
              <li>• Android Development</li>
              <li>• Object-Oriented Programming</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Mobile Development</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• iOS (Swift, SwiftUI)</li>
              <li>• Android (Kotlin)</li>
              <li>• React Native</li>
              <li>• Flutter</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Database & Backend</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• SQL & NoSQL</li>
              <li>• Database Design</li>
              <li>• API Development</li>
              <li>• Cloud Services (AWS, Azure)</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Other Languages</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• C/C++</li>
              <li>• C# & .NET</li>
              <li>• Ruby on Rails</li>
              <li>• PHP & WordPress</li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 bg-gray-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <h2 class="text-3xl font-extrabold tracking-tight text-center text-gray-900 sm:text-4xl">
          Why Learn Programming with Us?
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
                  d="M21 13.255A23.931 23.931 0 0112 15c-3.183 0-6.22-.62-9-1.745M16 6V4a2 2 0 00-2-2h-4a2 2 0 00-2 2v2m4 6h.01M5 20h14a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Industry Professionals</h3>
            <p class="mt-2 text-gray-600">
              Learn from developers actively working in the tech industry.
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
                  d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Project-Based Learning</h3>
            <p class="mt-2 text-gray-600">
              Build real-world projects and create a portfolio that showcases your skills.
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
                  d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Career Support</h3>
            <p class="mt-2 text-gray-600">
              Get guidance on interview preparation and career advancement in tech.
            </p>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Ready to Start Your Coding Journey?
        </h2>
        <p class="mt-4 text-xl text-gray-600 max-w-3xl mx-auto">
          Whether you're a complete beginner or looking to advance your skills, our expert tutors
          are here to help you succeed.
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
