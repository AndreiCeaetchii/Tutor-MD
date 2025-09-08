<template>
  <footer class="bg-gray-100 text-gray-700 py-12 lg:py-16">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="grid grid-cols-1 md:grid-cols-6 gap-8 lg:gap-12">
        <div class="col-span-1 md:col-span-2">
                    <div class="flex items-center space-x-2 mb-4">
                      <img class="h-8" src="../assets/tutor2.png" alt="TutorConnect Logo">
                    </div>
          <p class="text-sm leading-relaxed mb-4">
            Connecting students and tutors worldwide for personalized, effective learning experiences.
          </p>
          <div class="space-y-2 text-sm">
            <a href="mailto:support@tutorconnect.com" class="flex items-center space-x-2 hover:text-white transition-colors">
              <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20"><path d="M2.003 5.884L10 9.882l7.997-3.998A2 2 0 0016 4H4a2 2 0 00-1.997 1.884z"/><path d="M18 8.118l-8 4-8-4V14a2 2 0 002 2h12a2 2 0 002-2V8.118z"/></svg>
              <span>support@tutorconnect.com</span>
            </a>
            <a href="tel:1-800-TUTOR-99" class="flex items-center space-x-2 hover:text-white transition-colors">
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
          <h5 class="font-semibold mb-3">Platform</h5>
          <ul class="space-y-2 text-sm">
            <li><a href="#" class="hover:text-white transition-colors">How it Works</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Find Tutors</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Become a Tutor</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Pricing</a></li>
          </ul>
        </div>
        <div>
          <h5 class=" font-semibold mb-3">Popular Subjects</h5>
          <ul class="space-y-2 text-sm">
            <li><a href="#" class="hover:text-white transition-colors">Mathematics</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Science</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Languages</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Programming</a></li>
          </ul>
        </div>
        <div>
          <h5 class=" font-semibold mb-3">Support</h5>
          <ul class="space-y-2 text-sm">
            <li><a href="#" class="hover:text-white transition-colors">Help Center</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Contact Us</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Safety</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Community Guidelines</a></li>
          </ul>
        </div>
        <div>
          <h5 class=" font-semibold mb-3">Company</h5>
          <ul class="space-y-2 text-sm">
            <li><a href="#" class="hover:text-white transition-colors">About Us</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Careers</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Press</a></li>
            <li><a href="#" class="hover:text-white transition-colors">Blog</a></li>
          </ul>
        </div>
      </div>

      <div class="mt-12 pt-8 border-t border-gray-400 flex flex-col md:flex-row justify-between items-center text-sm text-gray-400 space-y-4 md:space-y-0">
        <div class="flex-1 text-center md:text-left">
          © 2025 Tutor. All rights reserved.
        </div>
        <div class="flex-1 flex justify-center md:justify-end space-x-4">
          <a href="#" class="hover:text-white transition-colors">Privacy Policy</a>
          <a href="#" class="hover:text-white transition-colors">Terms of Service</a>
          <a href="#" class="hover:text-white transition-colors">Cookie Policy</a>
        </div>
        <div class="hidden md:flex flex-1 justify-end space-x-4 text-white">
          <a href="#" class="hover:text-gray-300 transition-colors"><font-awesome-icon :icon="['fab', 'facebook-f']" /></a>
          <a href="#" class="hover:text-gray-300 transition-colors"><font-awesome-icon :icon="['fab', 'twitter']" /></a>
          <a href="#" class="hover:text-gray-300 transition-colors"><font-awesome-icon :icon="['fab', 'instagram']" /></a>
          <a href="#" class="hover:text-gray-300 transition-colors"><font-awesome-icon :icon="['fab', 'linkedin-in']" /></a>
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