<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import NavigationBar from "../components/navigation/NavigationBar.vue";
import TutorProfile from "../components/tutor/TutorProfile.vue";
import { userStore } from "../store/userStore";
import Footer from "../components/Footer.vue";

const store = userStore();
const router = useRouter();
const activeTab = ref('Availability'); // Default to first tab

onMounted(() => {
  if (!store.isAuthenticated) {
    router.push("/login");
    return;
  }

  if (store.userRole !== "tutor") {
    if (store.userRole === "student") {
      router.push("/student-dashboard");
    } else if (store.userRole === "admin") {
      router.push("/admin-dashboard");
    }
  }
});

const handleTabChange = (tabName: string) => {
  activeTab.value = tabName;
};
</script>

<template>
  <div class="bg-gray-50 min-h-screen">
    <div class="p-4 md:p-8 max-w-7xl mx-auto">
      <div class="mb-6">
        <h1 class="text-2xl font-bold mb-2">Tutor Dashboard</h1>
        <p class="text-gray-600">
          Welcome to your dashboard! Here you can manage your tutoring sessions,
          view student progress, and update your profile.
        </p>
      </div>
      
      <NavigationBar @tabChange="handleTabChange" />
      
      <!-- Dynamic content based on active tab -->
      <div class="mt-8">
        <TutorProfile v-if="activeTab === 'Profile'" />
        
        <!-- Content for other tabs -->
        <div 
          v-else-if="activeTab === 'Availability'" 
          class="content-container"
        >
          <div class="bg-white rounded-2xl shadow-lg p-6 md:p-8">
            <h2 class="text-xl font-semibold mb-4">Manage Your Availability</h2>
            <p class="mb-6">Set your available hours for tutoring sessions.</p>
            
            <!-- Calendar component or availability settings would go here -->
            <div class="bg-gray-50 border border-gray-200 rounded-xl p-4 mb-6">
              <h3 class="text-lg font-medium mb-3">Weekly Schedule</h3>
              <div class="grid grid-cols-1 md:grid-cols-7 gap-2 text-center">
                <div v-for="day in ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']" 
                     :key="day" 
                     class="p-3 bg-white rounded-lg border border-gray-100">
                  <p class="font-medium mb-2">{{ day }}</p>
                  <div class="text-sm text-gray-600">
                    <p>9:00 AM - 5:00 PM</p>
                    <button class="mt-2 text-purple-600 text-xs hover:underline">Edit</button>
                  </div>
                </div>
              </div>
            </div>
            
            <div class="flex justify-end">
              <button class="bg-purple-600 text-white px-6 py-2 rounded-full hover:bg-purple-700 transition-colors">
                Save Schedule
              </button>
            </div>
          </div>
        </div>
        
        <div 
          v-else-if="activeTab === 'Bookings'" 
          class="content-container"
        >
          <div class="bg-white rounded-2xl shadow-lg p-6 md:p-8">
            <h2 class="text-xl font-semibold mb-4">Your Bookings</h2>
            <p class="mb-6">View and manage your upcoming and past tutoring sessions.</p>
            
            <!-- Booking list would go here -->
            <div class="space-y-4">
              <div class="border border-gray-200 rounded-xl p-4 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
                <div>
                  <h3 class="font-medium">Math Tutoring Session</h3>
                  <p class="text-sm text-gray-600">With Alex Johnson • Monday, Sept 9 • 3:00 PM - 4:00 PM</p>
                </div>
                <div class="flex gap-2">
                  <button class="px-4 py-1 bg-purple-600 text-white text-sm rounded-full">Details</button>
                  <button class="px-4 py-1 border border-gray-300 text-gray-700 text-sm rounded-full">Cancel</button>
                </div>
              </div>
              
              <div class="border border-gray-200 rounded-xl p-4 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
                <div>
                  <h3 class="font-medium">Calculus Review</h3>
                  <p class="text-sm text-gray-600">With Michael Smith • Wednesday, Sept 11 • 2:00 PM - 3:30 PM</p>
                </div>
                <div class="flex gap-2">
                  <button class="px-4 py-1 bg-purple-600 text-white text-sm rounded-full">Details</button>
                  <button class="px-4 py-1 border border-gray-300 text-gray-700 text-sm rounded-full">Cancel</button>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <div 
          v-else-if="activeTab === 'Reviews'" 
          class="content-container"
        >
          <div class="bg-white rounded-2xl shadow-lg p-6 md:p-8">
            <h2 class="text-xl font-semibold mb-4">Student Reviews</h2>
            <p class="mb-6">See what your students are saying about your tutoring.</p>
            
            <!-- Reviews list would go here -->
            <div class="space-y-6">
              <div class="border-b border-gray-200 pb-4">
                <div class="flex items-center gap-2 mb-2">
                  <div class="flex text-yellow-400">
                    <span class="material-icons">star</span>
                    <span class="material-icons">star</span>
                    <span class="material-icons">star</span>
                    <span class="material-icons">star</span>
                    <span class="material-icons">star</span>
                  </div>
                  <span class="font-medium">5.0</span>
                </div>
                <p class="text-gray-700 mb-2">"Sarah is an amazing math tutor! She explains complex concepts in a way that's easy to understand. I went from struggling with calculus to acing my exams."</p>
                <p class="text-sm text-gray-500">- Alex Johnson, 3 days ago</p>
              </div>
              
              <div class="border-b border-gray-200 pb-4">
                <div class="flex items-center gap-2 mb-2">
                  <div class="flex text-yellow-400">
                    <span class="material-icons">star</span>
                    <span class="material-icons">star</span>
                    <span class="material-icons">star</span>
                    <span class="material-icons">star</span>
                    <span class="material-icons">star_half</span>
                  </div>
                  <span class="font-medium">4.5</span>
                </div>
                <p class="text-gray-700 mb-2">"Very patient and knowledgeable. Would recommend to anyone struggling with statistics."</p>
                <p class="text-sm text-gray-500">- Michael Smith, 1 week ago</p>
              </div>
            </div>
          </div>
        </div>
        
        <div 
          v-else-if="activeTab === 'Messages'" 
          class="content-container"
        >
          <div class="bg-white rounded-2xl shadow-lg p-6 md:p-8">
            <h2 class="text-xl font-semibold mb-4">Messages</h2>
            <p class="mb-6">Communicate with your students.</p>
            
            <!-- Messages interface would go here -->
            <div class="flex flex-col h-80 border border-gray-200 rounded-xl overflow-hidden">
              <div class="bg-gray-50 p-3 border-b border-gray-200">
                <p class="font-medium">Alex Johnson</p>
              </div>
              
              <div class="flex-grow p-4 overflow-y-auto">
                <div class="flex flex-col gap-3">
                  <div class="max-w-[70%] bg-gray-100 p-3 rounded-xl rounded-bl-none self-start">
                    <p class="text-sm">Hi Sarah, I was wondering if we could schedule an extra session this week to review for my upcoming exam?</p>
                    <p class="text-xs text-gray-500 mt-1">10:30 AM</p>
                  </div>
                  
                  <div class="max-w-[70%] bg-purple-100 p-3 rounded-xl rounded-br-none self-end">
                    <p class="text-sm">Hi Alex! Yes, I have time available on Thursday at 4pm. Would that work for you?</p>
                    <p class="text-xs text-gray-500 mt-1">10:45 AM</p>
                  </div>
                </div>
              </div>
              
              <div class="p-3 border-t border-gray-200">
                <div class="flex gap-2">
                  <input type="text" placeholder="Type a message..." class="flex-grow p-2 border border-gray-300 rounded-full text-sm" />
                  <button class="bg-purple-600 text-white p-2 rounded-full">
                    <span class="material-icons text-sm">send</span>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <Footer/>
</template>

<style scoped>
.content-container {
  max-width: 1200px;
  width: 100%;
  margin: 0 auto;
}

@media (max-width: 640px) {
  .content-container {
    padding: 0 0.5rem;
  }
}
</style>