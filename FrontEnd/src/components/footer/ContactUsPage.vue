<template>
  <div class="bg-white">
    <div class="py-16 bg-[#5f22d9] lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl font-extrabold tracking-tight text-white sm:text-5xl lg:text-6xl">
          Have a Question? <span class="text-orange-300">We're Here to Help!</span>
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-indigo-200">
          You can reach us via the contact form, email, or phone, depending on your urgency.
        </p>
      </div>
    </div>

    <div class="py-16 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 lg:grid lg:grid-cols-2 lg:gap-x-12">
        <div class="lg:pr-8">
          <h2 class="text-3xl font-extrabold text-gray-900 sm:text-4xl">Send Us a Quick Message</h2>
          <p class="mt-4 text-lg text-gray-500">
            For general inquiries or feedback, please fill out the form below. We will get back to you within 24 hours.
          </p>
          <form class="mt-9 space-y-6">
            <div>
              <label for="name" class="block text-sm font-medium text-gray-700">Your Name</label>
              <input type="text" id="name" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-[#5f22d9] focus:ring-[#5f22d9] p-3">
            </div>
            <div>
              <label for="email" class="block text-sm font-medium text-gray-700">Email Address</label>
              <input type="email" id="email" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-[#5f22d9] focus:ring-[#5f22d9] p-3">
            </div>
            <div>
              <label for="subject" class="block text-sm font-medium text-gray-700">Subject</label>
              <input type="text" id="subject" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-[#5f22d9] focus:ring-[#5f22d9] p-3">
            </div>
            <div>
              <label for="message" class="block text-sm font-medium text-gray-700">Message</label>
              <textarea id="message" rows="4" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-[#5f22d9] focus:ring-[#5f22d9] p-3"></textarea>
            </div>
            <button type="submit" class="inline-flex items-center justify-center px-8 py-3 text-base font-medium text-white transition-colors bg-orange-500 border border-transparent rounded-full shadow-lg hover:bg-orange-600">
              Send Message
            </button>
          </form>
        </div>

        <div class="mt-12 lg:mt-0 bg-gray-50 p-8 rounded-xl shadow-lg">
          <h2 class="text-3xl font-extrabold text-gray-900">Direct Contact</h2>
          <p class="mt-4 text-lg text-gray-500">
            Prefer to speak directly? Here's how you can reach us:
          </p>
          <div class="mt-6 space-y-4">
            <div class="flex items-center space-x-4">
              <svg class="w-6 h-6 text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" /></svg>
              <a href="mailto:support@tutorconnect.com" class="text-lg font-medium text-gray-700 hover:text-[#5f22d9]">support@tutorconnect.com</a>
            </div>
            <div class="flex items-center space-x-4">
              <svg class="w-6 h-6 text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" /></svg>
              <a href="tel:0780TUTOR99" class="text-lg font-medium text-gray-700 hover:text-[#5f22d9]">0-780-TUTOR-99</a>
            </div>
            <div class="flex items-center space-x-4">
              <svg class="w-6 h-6 text-[#5f22d9]" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
              <p class="text-lg font-medium text-gray-700">Mon-Fri, 9:00 AM - 6:00 PM (EEST)</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>