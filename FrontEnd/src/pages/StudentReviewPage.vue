<template>
  <div class="bg-white">
    <div class="py-16 bg-gradient-to-br from-purple-50 to-blue-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <div class="flex justify-center mb-6">
          <svg width="60" height="25" viewBox="0 0 80 30" class="text-orange-500">
            <path
              d="M0,15 C10,5 20,25 40,15 C60,5 70,25 80,15"
              fill="none"
              stroke="currentColor"
              stroke-width="4"
              stroke-linecap="round"
            />
          </svg>
        </div>
        <p class="text-sm font-bold tracking-widest text-orange-500 uppercase">TESTIMONIALS</p>
        <h1
          class="mt-2 text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl"
        >
          See How Our Students Created Their<br />
          <span class="text-[#5f22d9]">#Success Stories</span>
        </h1>
        <p class="max-w-3xl mx-auto mt-4 text-xl text-gray-600">
          Real experiences from students, parents, and educators who have transformed their learning
          journey with TutorMd.
        </p>
      </div>
    </div>

    <!-- Stats Section -->
    <div class="py-12 bg-white">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="grid gap-8 md:grid-cols-4">
          <div class="text-center">
            <div class="text-4xl font-bold text-[#5f22d9]">50+</div>
            <div class="mt-2 text-sm text-gray-600">Subjects Available</div>
          </div>
          <div class="text-center">
            <div class="text-4xl font-bold text-[#5f22d9]">500+</div>
            <div class="mt-2 text-sm text-gray-600">Tutors by Subject</div>
          </div>
          <div class="text-center">
            <div class="text-4xl font-bold text-[#5f22d9]">25,000+</div>
            <div class="mt-2 text-sm text-gray-600">Sessions Completed</div>
          </div>
          <div class="text-center">
            <div class="text-4xl font-bold text-[#5f22d9]">2,500+</div>
            <div class="mt-2 text-sm text-gray-600">Years of Combined Experience</div>
          </div>
        </div>
      </div>
    </div>

    <!-- Reviews Grid -->
    <div class="py-16 bg-gray-50 lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8">
        <div class="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
          <!-- Review 1 -->
          <div class="relative p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img
                class="object-cover w-12 h-12 rounded-full"
                src="../assets/people/boy5.jpg"
                alt="Andrei Ionescu"
              />
              <div>
                <h4 class="font-semibold text-gray-900">Andrei Ionescu</h4>
                <p class="text-sm text-gray-500">10th Grade, Cluj-Napoca</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg
                    v-for="i in 5"
                    :key="i"
                    class="w-4 h-4 text-yellow-400 fill-current"
                    viewBox="0 0 20 20"
                  >
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              My math grades improved from C to A in just two months!
            </p>
            <p class="text-sm text-gray-600">
              Before using TutorMd, I struggled with advanced calculus. My tutor made complex
              concepts easy to understand with practical examples from Romanian engineering
              achievements. Now I'm confident in my abilities and planning to study engineering.
            </p>
          </div>

          <!-- Review 2 -->
          <div class="relative p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img
                class="object-cover w-12 h-12 rounded-full"
                src="../assets/people/girl2.jpg"
                alt="Elena Dumitrescu"
              />
              <div>
                <h4 class="font-semibold text-gray-900">Elena Dumitrescu</h4>
                <p class="text-sm text-gray-500">8th Grade, Timisoara</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg
                    v-for="i in 5"
                    :key="i"
                    class="w-4 h-4 text-yellow-400 fill-current"
                    viewBox="0 0 20 20"
                  >
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              The flexible online tutoring saved my academic year
            </p>
            <p class="text-sm text-gray-600">
              When I moved from Bucharest to Timisoara mid-school year, I was falling behind in
              English literature. My TutorMd tutor helped me catch up with the new curriculum while
              accommodating my schedule. The personalized lessons were exactly what I needed.
            </p>
          </div>

          <!-- Review 3 -->
          <div class="relative p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img
                class="object-cover w-12 h-12 rounded-full"
                src="../assets/people/girl3.jpg"
                alt="Daniela Popa"
              />
              <div>
                <h4 class="font-semibold text-gray-900">Daniela Popa</h4>
                <p class="text-sm text-gray-500">University Student, Iasi</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg
                    v-for="i in 5"
                    :key="i"
                    class="w-4 h-4 text-yellow-400 fill-current"
                    viewBox="0 0 20 20"
                  >
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              Passed my computer science entrance exam with flying colors
            </p>
            <p class="text-sm text-gray-600">
              The specialized tutoring for university entrance exams was exactly what I needed. My
              tutor was a graduate from my target university and shared insider tips on the
              admission process. Now I'm studying computer science at my dream university!
            </p>
          </div>

          <!-- Review 4 -->
          <div class="relative p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img
                class="object-cover w-12 h-12 rounded-full"
                src="../assets/people/girl4.jpg"
                alt="Ana Rusu"
              />
              <div>
                <h4 class="font-semibold text-gray-900">Ana Rusu</h4>
                <p class="text-sm text-gray-500">11th Grade, Chisinau, Moldova</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg
                    v-for="i in 5"
                    :key="i"
                    class="w-4 h-4 text-yellow-400 fill-current"
                    viewBox="0 0 20 20"
                  >
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              Improved my English language skills dramatically
            </p>
            <p class="text-sm text-gray-600">
              As a student from Moldova preparing for international studies, I needed to perfect my
              English. My tutor customized lessons around my interests in technology and helped me
              prepare for the TOEFL exam. I scored 112 out of 120 points!
            </p>
          </div>

          <!-- Review 5 - Parent -->
          <div class="relative p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img
                class="object-cover w-12 h-12 rounded-full"
                src="../assets/people/boy6.jpg"
                alt="Vasile Munteanu"
              />
              <div>
                <h4 class="font-semibold text-gray-900">Vasile Munteanu</h4>
                <p class="text-sm text-gray-500">Parent, Brasov</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg
                    v-for="i in 4"
                    :key="i"
                    class="w-4 h-4 text-yellow-400 fill-current"
                    viewBox="0 0 20 20"
                  >
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                  <svg class="w-4 h-4 text-gray-300 fill-current" viewBox="0 0 20 20">
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              The perfect solution for my daughter's chemistry struggles
            </p>
            <p class="text-sm text-gray-600">
              My daughter was struggling with chemistry and losing confidence. Her TutorMd tutor not
              only helped her understand the subject but also rekindled her interest in science. The
              progress reports kept me informed of her improvement throughout the semester.
            </p>
          </div>

          <!-- Review 6 - Teacher -->
          <div class="relative p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img
                class="object-cover w-12 h-12 rounded-full"
                src="../assets/people/boy7.jpg"
                alt="Ion Cebanu"
              />
              <div>
                <h4 class="font-semibold text-gray-900">Ion Cebanu</h4>
                <p class="text-sm text-gray-500">High School Teacher, Balti, Moldova</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg
                    v-for="i in 5"
                    :key="i"
                    class="w-4 h-4 text-yellow-400 fill-current"
                    viewBox="0 0 20 20"
                  >
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              I recommend TutorMd to all my students who need extra help
            </p>
            <p class="text-sm text-gray-600">
              As a high school teacher, I've seen remarkable progress in students who use TutorMd
              for supplementary education. The platform complements classroom learning perfectly,
              and I appreciate how they incorporate our regional curriculum standards.
            </p>
          </div>

          <!-- Review 7 -->
          <div class="relative p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img
                class="object-cover w-12 h-12 rounded-full"
                src="../assets/people/boy1.jpg"
                alt="Mihai Popescu"
              />
              <div>
                <h4 class="font-semibold text-gray-900">Mihai Popescu</h4>
                <p class="text-sm text-gray-500">12th Grade, Bucharest</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg
                    v-for="i in 5"
                    :key="i"
                    class="w-4 h-4 text-yellow-400 fill-current"
                    viewBox="0 0 20 20"
                  >
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              Finally understood physics after years of struggling
            </p>
            <p class="text-sm text-gray-600">
              Physics had always been my weakest subject until I found TutorMd. My tutor used
              real-world examples and interactive simulations that made everything click. I went
              from barely passing to getting the highest grade in my class on the final exam.
            </p>
          </div>

          <!-- Review 8 -->
          <div class="relative p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img
                class="object-cover w-12 h-12 rounded-full"
                src="../assets/people/girl1.jpg"
                alt="Maria Stan"
              />
              <div>
                <h4 class="font-semibold text-gray-900">Maria Stan</h4>
                <p class="text-sm text-gray-500">9th Grade, Constanta</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg
                    v-for="i in 5"
                    :key="i"
                    class="w-4 h-4 text-yellow-400 fill-current"
                    viewBox="0 0 20 20"
                  >
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">Best investment in my education</p>
            <p class="text-sm text-gray-600">
              The personalized attention I get from my TutorMd tutor is invaluable. Unlike crowded
              classrooms, my tutor focuses entirely on my learning pace and style. I've become more
              confident in asking questions and participating in class discussions.
            </p>
          </div>

          <!-- Review 9 -->
          <div class="relative p-8 bg-white border border-gray-200 shadow-lg rounded-2xl">
            <div class="absolute font-bold text-gray-200 top-4 right-4 text-9xl opacity-5">"</div>
            <div class="flex items-center mb-4 space-x-4">
              <img
                class="object-cover w-12 h-12 rounded-full"
                src="../assets/people/boy2.jpg"
                alt="Alexandru Stanescu"
              />
              <div>
                <h4 class="font-semibold text-gray-900">Alexandru Stanescu</h4>
                <p class="text-sm text-gray-500">University Student, Sibiu</p>
                <div class="flex items-center space-x-0.5 mt-1">
                  <svg
                    v-for="i in 5"
                    :key="i"
                    class="w-4 h-4 text-yellow-400 fill-current"
                    viewBox="0 0 20 20"
                  >
                    <path
                      d="M10 15l-5.878 3.09 1.123-6.545L.489 7.91l6.572-.955L10 1l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"
                    />
                  </svg>
                </div>
              </div>
            </div>
            <p class="mb-4 text-lg font-semibold text-gray-800">
              Perfect for programming and software development
            </p>
            <p class="text-sm text-gray-600">
              As a computer science student, I needed help with advanced algorithms. My TutorMd
              tutor is a professional developer who provided industry insights along with academic
              guidance. The practical coding sessions were incredibly valuable.
            </p>
          </div>
        </div>
      </div>
    </div>

    <!-- CTA Section -->
    <div class="py-16 bg-white lg:py-24">
      <div class="px-4 mx-auto max-w-7xl sm:px-6 lg:px-8 text-center">
        <h2 class="text-3xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
          Ready to Write Your Success Story?
        </h2>
        <p class="mt-4 text-xl text-gray-600 max-w-3xl mx-auto">
          Join thousands of students who have transformed their academic journey with personalized
          tutoring.
        </p>
        <div class="mt-8">
          <router-link
            to="/student-dashboard/find-tutors"
            class="inline-flex items-center px-8 py-3 text-base font-medium text-white transition-colors bg-[#5f22d9] border border-transparent rounded-full shadow hover:bg-purple-700"
          >
            Find Your Perfect Tutor
          </router-link>
          <router-link
            to="/signup"
            class="ml-4 inline-flex items-center px-8 py-3 text-base font-medium text-gray-700 transition-colors bg-white border border-gray-300 rounded-full shadow hover:bg-gray-50"
          >
            Sign Up Free
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts"></script>
