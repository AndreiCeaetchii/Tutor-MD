<template>
  <footer class="py-12 text-gray-700 bg-gray-100 lg:py-16">
    <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
      <div class="grid grid-cols-1 gap-8 md:grid-cols-6 lg:gap-12">
        <div class="col-span-1 md:col-span-2">
                    <div class="flex items-center mb-4 space-x-2">
                      <img class="h-8" src="../assets/tutor2.png" alt="TutorConnect Logo">
                    </div>
          <p class="mb-4 text-sm leading-relaxed">
            Connecting students and tutors worldwide for personalized, effective learning experiences.
          </p>
          <div class="space-y-2 text-sm">
            <a href="mailto:support@tutorconnect.com" class="flex items-center space-x-2 hover:text-[#5f22d9] transition-colors">
              <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20"><path d="M2.003 5.884L10 9.882l7.997-3.998A2 2 0 0016 4H4a2 2 0 00-1.997 1.884z"/><path d="M18 8.118l-8 4-8-4V14a2 2 0 002 2h12a2 2 0 002-2V8.118z"/></svg>
              <span>support@tutorconnect.com</span>
            </a>
            <a href="tel:1-800-TUTOR-99" class="flex items-center space-x-2 transition-colors hover:text-[#5f22d9]">
              <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20"><path d="M2 3a1 1 0 011-1h2.153a1 1 0 01.986.836l.74 4.435a1 1 0 01-.54 1.06l-1.548.774a11.01 11.01 0 005.885 5.885l.774-1.548a1 1 0 011.06-.54l4.435.74a1 1 0 01.836.986V17a1 1 0 01-1 1h-2C7.82 18 2 12.18 2 5V3z"/></svg>
              <span>1-800-TUTOR-99</span>
            </a>
            <div class="flex items-center space-x-2">
              <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20"><path fill-rule="evenodd" d="M5.05 4.05a7 7 0 119.9 9.9L10 18.9l-4.95-4.95a7 7 0 010-9.9zM10 11a2 2 0 100-4 2 2 0 000 4z" clip-rule="evenodd"/></svg>
              <span>San Francisco, CA</span>
            </div>
          </div>
        </div>

        <div>
          <h5 class="mb-3 font-semibold">Platform</h5>
          <ul class="space-y-2 text-sm">
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">How it Works</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Find Tutors</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Become a Tutor</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Pricing</a></li>
          </ul>
        </div>
        <div>
          <h5 class="mb-3 font-semibold ">Popular Subjects</h5>
          <ul class="space-y-2 text-sm">
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Mathematics</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Science</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Languages</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Programming</a></li>
          </ul>
        </div>
        <div>
          <h5 class="mb-3 font-semibold ">Support</h5>
          <ul class="space-y-2 text-sm">
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Help Center</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Contact Us</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Safety</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Community Guidelines</a></li>
          </ul>
        </div>
        <div>
          <h5 class="mb-3 font-semibold ">Company</h5>
          <ul class="space-y-2 text-sm">
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">About Us</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Careers</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Press</a></li>
            <li><a href="#" class="transition-colors hover:text-[#5f22d9]">Blog</a></li>
          </ul>
        </div>
      </div>

      <div class="flex flex-col items-center justify-between pt-8 mt-12 space-y-4 text-sm text-gray-400 border-t border-gray-400 md:flex-row md:space-y-0">
        <div class="flex-1 text-center md:text-left">
          © 2025 Tutor. All rights reserved.
        </div>
        <div class="flex justify-center flex-1 space-x-4 md:justify-end">
          <a href="#" class="transition-colors hover:text-[#5f22d9]">Privacy Policy</a>
          <a href="#" class="transition-colors hover:text-[#5f22d9]">Terms of Service</a>
          <a href="#" class="transition-colors hover:text-[#5f22d9]">Cookie Policy</a>
        </div>
        <div class="justify-end flex-1 hidden space-x-4 text-[#5f22d9] md:flex">
          <a href="#" class="transition-colors hover:text-[#8f63e6]"><font-awesome-icon :icon="['fab', 'facebook-f']" /></a>
          <a href="#" class="transition-colors hover:text-[#8f63e6]"><font-awesome-icon :icon="['fab', 'twitter']" /></a>
          <a href="#" class="transition-colors hover:text-[#8f63e6]"><font-awesome-icon :icon="['fab', 'instagram']" /></a>
          <a href="#" class="transition-colors hover:text-[#8f63e6]"><font-awesome-icon :icon="['fab', 'linkedin-in']" /></a>
        </div>
      </div>
    </div>
  </footer>
</template>

<script setup lang="ts">
import { library } from '@fortawesome/fontawesome-svg-core';
import { faFacebookF, faTwitter, faInstagram, faLinkedinIn } from '@fortawesome/free-brands-svg-icons';
import {FontAwesomeIcon} from "@fortawesome/vue-fontawesome";

library.add(faFacebookF, faTwitter, faInstagram, faLinkedinIn);
</script>