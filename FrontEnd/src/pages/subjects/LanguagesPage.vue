<template>
  <div class="bg-white">
    <div class="py-16 bg-gradient-to-br from-pink-50 to-orange-50 lg:py-24">
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
                d="M3 5h12M9 3v2m1.048 9.5A18.022 18.022 0 016.412 9m6.088 9h7M11 21l5-10 5 10M12.751 5C11.783 10.77 8.07 15.61 3 18.129"
              />
            </svg>
          </div>
        </div>
        <h1 class="text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl">
          Learn <span class="text-[#5f22d9]">Languages</span> with Native Speakers
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-gray-600">
          Master a new language with personalized lessons from native speakers and experienced
          language instructors.
        </p>
        <div class="mt-8">
          <router-link
            to="/student-dashboard/find-tutors"
            class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-[#5f22d9] border border-transparent rounded-full shadow hover:bg-purple-700"
          >
            Find a Language Tutor
          </router-link>
          <router-link
            to="/become-a-tutor"
            class="ml-4 inline-flex items-center px-8 py-3 text-base font-medium text-gray-700 transition-colors bg-white border border-gray-300 rounded-full shadow hover:bg-gray-50"
          >
            Become a Language Tutor
          </router-link>
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="mb-16 text-center">
          <h2 class="text-base font-semibold tracking-widest text-orange-500 uppercase">
            LANGUAGES WE TEACH
          </h2>
          <p class="mt-2 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
            Popular Languages & Dialects
          </p>
        </div>

        <div class="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">English</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Conversational English</li>
              <li>• Business English</li>
              <li>• IELTS/TOEFL Preparation</li>
              <li>• Academic Writing</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Spanish</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Latin American Spanish</li>
              <li>• European Spanish</li>
              <li>• DELE Exam Prep</li>
              <li>• Travel Spanish</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">French</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Standard French</li>
              <li>• DELF/DALF Preparation</li>
              <li>• Business French</li>
              <li>• French Culture</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">German</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Standard German</li>
              <li>• Goethe Certificate Prep</li>
              <li>• Business German</li>
              <li>• German Grammar</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Mandarin Chinese</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Simplified Chinese</li>
              <li>• HSK Exam Preparation</li>
              <li>• Pinyin & Pronunciation</li>
              <li>• Chinese Characters</li>
            </ul>
          </div>

          <div
            class="p-6 transition-shadow bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md"
          >
            <h3 class="text-xl font-semibold text-gray-900">Other Languages</h3>
            <ul class="mt-4 space-y-2 text-gray-600">
              <li>• Italian</li>
              <li>• Japanese</li>
              <li>• Portuguese</li>
              <li>• Russian</li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 bg-gray-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <h2 class="text-3xl font-extrabold tracking-tight text-center text-gray-900 sm:text-4xl">
          Our Language Learning Approach
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
                  d="M17 8h2a2 2 0 012 2v6a2 2 0 01-2 2h-2v4l-4-4H9a1.994 1.994 0 01-1.414-.586m0 0L11 14h4a2 2 0 002-2V6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2v4l.586-.586z"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Immersive Conversation</h3>
            <p class="mt-2 text-gray-600">
              Practice speaking from day one with native speakers in real-life scenarios.
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
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Cultural Context</h3>
            <p class="mt-2 text-gray-600">
              Learn the language alongside its culture, customs, and traditions.
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
                  d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z"
                />
              </svg>
            </div>
            <h3 class="mt-4 text-xl font-semibold text-gray-900">Flexible Scheduling</h3>
            <p class="mt-2 text-gray-600">
              Learn at your own pace with schedules that fit your lifestyle.
            </p>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Ready to Start Your Language Journey?
        </h2>
        <p class="mt-4 text-xl text-gray-600 max-w-3xl mx-auto">
          Connect with native speakers and experienced language tutors to achieve fluency faster.
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
