<script setup lang="ts">
  import CardCarousel from "./CardCarousel.vue";
</script>

<template>
  <div class="py-16 bg-gray-50 lg:py-24">
    <div class="relative px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
      <div class="flex items-center justify-between mb-12">
        <h2 class="text-3xl font-bold leading-tight sm:text-4xl lg:text-5xl">
          See how our students<br>
          created their <span class="text-blue-500">#Success Stories</span>
        </h2>
      </div>

      <CardCarousel>
        <div class="flex-shrink-0 w-full p-4 md:w-1/2 lg:w-1/3">
          <div class="relative h-full p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img class="object-cover w-12 h-12 rounded-full" src="../../assets/people/boy5.jpg" alt="Andrei Ionescu">
              <div>
                <h4 class="font-semibold text-gray-900">Andrei Ionescu</h4>
                <p class="text-sm text-gray-500">10th Grade, Cluj-Napoca</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              My math grades improved from C to A in just two months!
            </p>
            <p class="text-sm text-gray-600">
              "Before using TutorMd, I struggled with advanced calculus. My tutor made complex concepts easy to understand with practical examples from Romanian engineering achievements. Now I'm confident in my abilities and planning to study engineering."
            </p>
          </div>
        </div>

        <div class="flex-shrink-0 w-full p-4 md:w-1/2 lg:w-1/3">
          <div class="relative h-full p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img class="object-cover w-12 h-12 rounded-full" src="../../assets/people/girl2.jpg" alt="Elena Dumitrescu">
              <div>
                <h4 class="font-semibold text-gray-900">Elena Dumitrescu</h4>
                <p class="text-sm text-gray-500">8th Grade, Timisoara</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              The flexible online tutoring saved my academic year
            </p>
            <p class="text-sm text-gray-600">
              "When I moved from Bucharest to Timisoara mid-school year, I was falling behind in English literature. My TutorMd tutor helped me catch up with the new curriculum while accommodating my schedule. The personalized lessons were exactly what I needed."
            </p>
          </div>
        </div>

        <div class="flex-shrink-0 w-full p-4 md:w-1/2 lg:w-1/3">
          <div class="relative h-full p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img class="object-cover w-12 h-12 rounded-full" src="../../assets/people/girl3.jpg" alt="Daniela Popa">
              <div>
                <h4 class="font-semibold text-gray-900">Daniela Popa</h4>
                <p class="text-sm text-gray-500">University Student, Iasi</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              Passed my computer science entrance exam with flying colors
            </p>
            <p class="text-sm text-gray-600">
              "The specialized tutoring for university entrance exams was exactly what I needed. My tutor was a graduate from my target university and shared insider tips on the admission process. Now I'm studying computer science at my dream university!"
            </p>
          </div>
        </div>

        <div class="flex-shrink-0 w-full p-4 md:w-1/2 lg:w-1/3">
          <div class="relative h-full p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img class="object-cover w-12 h-12 rounded-full" src="../../assets/people/girl4.jpg" alt="Ana Rusu">
              <div>
                <h4 class="font-semibold text-gray-900">Ana Rusu</h4>
                <p class="text-sm text-gray-500">11th Grade, Chisinau, Moldova</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              Improved my English language skills dramatically
            </p>
            <p class="text-sm text-gray-600">
              "As a student from Moldova preparing for international studies, I needed to perfect my English. My tutor customized lessons around my interests in technology and helped me prepare for the TOEFL exam. I scored 112 out of 120 points!"
            </p>
          </div>
        </div>

        <div class="flex-shrink-0 w-full p-4 md:w-1/2 lg:w-1/3">
          <div class="relative h-full p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img class="object-cover w-12 h-12 rounded-full" src="../../assets/people/boy6.jpg" alt="Vasile Munteanu">
              <div>
                <h4 class="font-semibold text-gray-900">Vasile Munteanu</h4>
                <p class="text-sm text-gray-500">Parent, Brasov</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-gray-300 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              The perfect solution for my daughter's chemistry struggles
            </p>
            <p class="text-sm text-gray-600">
              "My daughter was struggling with chemistry and losing confidence. Her TutorMd tutor not only helped her understand the subject but also rekindled her interest in science. The progress reports kept me informed of her improvement throughout the semester."
            </p>
          </div>
        </div>

        <div class="flex-shrink-0 w-full p-4 md:w-1/2 lg:w-1/3">
          <div class="relative h-full p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img class="object-cover w-12 h-12 rounded-full" src="../../assets/people/boy7.jpg" alt="Ion Cebanu">
              <div>
                <h4 class="font-semibold text-gray-900">Ion Cebanu</h4>
                <p class="text-sm text-gray-500">High School Teacher, Balti, Moldova</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                  <svg class="w-4 h-4 text-yellow-400 fill-current" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              I recommend TutorMd to all my students who need extra help
            </p>
            <p class="text-sm text-gray-600">
              "As a high school teacher, I've seen remarkable progress in students who use TutorMd for supplementary education. The platform complements classroom learning perfectly, and I appreciate how they incorporate our regional curriculum standards."
            </p>
          </div>
        </div>
      </CardCarousel>

    </div>
  </div>
</template>
