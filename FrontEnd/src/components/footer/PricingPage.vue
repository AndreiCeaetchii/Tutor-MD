<template>
  <div class="bg-white">
    <div class="py-16 bg-gray-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl">
          Simple, Transparent, and <span class="text-[#5f22d9]">Flexible Pricing</span>
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-gray-600">
          Choose a plan that fits your learning goals — no hidden fees.
        </p>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="grid grid-cols-1 gap-12 lg:grid-cols-2">
          <div class="p-8 bg-[#5f22d9] rounded-2xl shadow-xl">
            <h2 class="text-3xl font-extrabold text-white sm:text-4xl">For Students</h2>
            <p class="mt-4 text-lg text-indigo-200">
              Find the perfect balance of flexibility and savings for your learning plan.
            </p>
            <ul class="mt-8 space-y-5">
              <li class="flex items-start">
                <svg class="flex-shrink-0 w-6 h-6 text-orange-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <div class="ml-3 text-base text-white">
                  <strong class="font-semibold">Pay-per-lesson:</strong> Book individual lessons with tutors and pay only for the time you use.
                </div>
              </li>
              <li class="flex items-start">
                <svg class="flex-shrink-0 w-6 h-6 text-orange-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <div class="ml-3 text-base text-white">
                  <strong class="font-semibold">Lesson Packages:</strong> Get discounts when you purchase multiple sessions in advance.
                </div>
              </li>
              <li class="flex items-start">
                <svg class="flex-shrink-0 w-6 h-6 text-orange-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <div class="ml-3 text-base text-white">
                  <strong class="font-semibold">Subscription Plans (Optional):</strong> Unlock ongoing tutoring support with a monthly package.
                </div>
              </li>
            </ul>
          </div>

          <div class="p-8 bg-gray-50 rounded-2xl shadow-xl border border-gray-200">
            <h2 class="text-3xl font-extrabold text-gray-900 sm:text-4xl">For Tutors</h2>
            <p class="mt-4 text-lg text-gray-600">
              Maximize your earning potential with clear fees and flexible rates.
            </p>
            <ul class="mt-8 space-y-5">
              <li class="flex items-start">
                <svg class="flex-shrink-0 w-6 h-6 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <div class="ml-3 text-base text-gray-800">
                  <strong class="font-semibold">Free to Join:</strong> No upfront costs or hidden subscription fees to start teaching.
                </div>
              </li>
              <li class="flex items-start">
                <svg class="flex-shrink-0 w-6 h-6 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <div class="ml-3 text-base text-gray-800">
                  <strong class="font-semibold">Set Your Own Rates:</strong> Earn based on your expertise, experience, and subject demand.
                </div>
              </li>
              <li class="flex items-start">
                <svg class="flex-shrink-0 w-6 h-6 text-orange-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <div class="ml-3 text-base text-gray-800">
                  <strong class="font-semibold">Platform Fee:</strong> A small service fee is applied to each completed lesson to cover platform maintenance and security.
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="py-16 bg-[#5f22d9]/10">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Why Our Pricing Works for You
        </h2>
        <div class="mt-8 flex justify-center space-x-10">
          <div class="max-w-xs">
            <p class="text-xl font-medium text-[#5f22d9]">Flexibility</p>
            <p class="mt-2 text-gray-600">Great for occasional learners and those with long-term study plans.</p>
          </div>
          <div class="max-w-xs">
            <p class="text-xl font-medium text-[#5f22d9]">Affordability</p>
            <p class="mt-2 text-gray-600">Rates tailored to different budgets and academic levels.</p>
          </div>
          <div class="max-w-xs">
            <p class="text-xl font-medium text-[#5f22d9]">Clarity</p>
            <p class="mt-2 text-gray-600">Clear earnings for tutors and transparent costs for students.</p>
          </div>
        </div>
        <div class="mt-12">
          <router-link to="/find-tutors" class="inline-flex items-center px-8 py-3 text-lg font-medium text-white transition-colors bg-orange-500 border border-transparent rounded-full shadow-lg hover:bg-orange-600">
            Browse Tutors →
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>