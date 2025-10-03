<template>
  <div class="bg-white">
    <div class="py-16 bg-gray-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl">
          The Tutor <span class="text-[#5f22d9]">Blog</span>
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-gray-600">
          Tips, news, and resources to maximize your learning potential.
        </p>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="flex flex-wrap justify-center space-x-4 mb-12">
          <span class="inline-flex items-center rounded-full bg-orange-100 px-3 py-1 text-sm font-medium text-orange-800">Education</span>
          <span class="inline-flex items-center rounded-full bg-indigo-100 px-3 py-1 text-sm font-medium text-[#5f22d9]">Technology</span>
          <span class="inline-flex items-center rounded-full bg-gray-200 px-3 py-1 text-sm font-medium text-gray-700">Parenting Tips</span>
          <span class="inline-flex items-center rounded-full bg-gray-200 px-3 py-1 text-sm font-medium text-gray-700">Tutor Life</span>
        </div>

        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Recent Articles
        </h2>
        <div class="mt-10 grid grid-cols-1 gap-10 md:grid-cols-3">
          <div class="flex flex-col overflow-hidden rounded-lg shadow-lg">
            <div class="flex-shrink-0">
              <img class="h-48 w-full object-cover" src="https://picsum.photos/400/300?random=1" alt="Article Image">
            </div>
            <div class="flex flex-1 flex-col justify-between bg-white p-6">
              <div class="flex-1">
                <p class="text-sm font-medium text-orange-500">Education</p>
                <a href="#" class="mt-2 block">
                  <p class="text-xl font-semibold text-gray-900">5 Ways to Improve Focus While Studying</p>
                  <p class="mt-3 text-base text-gray-500">Proven techniques to maintain your concentration during long study sessions.</p>
                </a>
              </div>
              <div class="mt-6 flex items-center">
                <p class="text-sm font-medium text-[#5f22d9]">Read Article &rarr;</p>
              </div>
            </div>
          </div>
          <div class="flex flex-col overflow-hidden rounded-lg shadow-lg">
            <div class="flex-shrink-0">
              <img class="h-48 w-full object-cover" src="https://picsum.photos/400/300?random=2" alt="Article Image">
            </div>
            <div class="flex flex-1 flex-col justify-between bg-white p-6">
              <div class="flex-1">
                <p class="text-sm font-medium text-[#5f22d9]">Technology</p>
                <a href="#" class="mt-2 block">
                  <p class="text-xl font-semibold text-gray-900">The Future of Tutoring: How AI is Helping</p>
                  <p class="mt-3 text-base text-gray-500">Exploring the role of artificial intelligence in personalizing the learning experience.</p>
                </a>
              </div>
              <div class="mt-6 flex items-center">
                <p class="text-sm font-medium text-[#5f22d9]">Read Article &rarr;</p>
              </div>
            </div>
          </div>
          <div class="flex flex-col overflow-hidden rounded-lg shadow-lg">
            <div class="flex-shrink-0">
              <img class="h-48 w-full object-cover" src="https://picsum.photos/400/300?random=3" alt="Article Image">
            </div>
            <div class="flex flex-1 flex-col justify-between bg-white p-6">
              <div class="flex-1">
                <p class="text-sm font-medium text-orange-500">Parenting Tips</p>
                <a href="#" class="mt-2 block">
                  <p class="text-xl font-semibold text-gray-900">Parent's Guide: Choosing the Perfect Tutor</p>
                  <p class="mt-3 text-base text-gray-500">The essential checklist for making the best decision for your child's success.</p>
                </a>
              </div>
              <div class="mt-6 flex items-center">
                <p class="text-sm font-medium text-[#5f22d9]">Read Article &rarr;</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>