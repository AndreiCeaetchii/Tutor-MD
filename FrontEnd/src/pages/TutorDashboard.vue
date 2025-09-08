<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import NavigationBar from "../components/navigation/NavigationBar.vue";
import TutorProfile from "../components/tutor/TutorProfile.vue";
import TutorBookings from "../components/tutor/TutorBookings.vue"; // Import the TutorBookings component
import TutorReview from "../components/tutor/TutorReview.vue";
import TutorCalendar from "../components/tutor/TutorCalendar.vue";
import TutorSlots from "../components/tutor/TutorSlots.vue";
import TutorSlotsCards from "../components/tutor/TutorSlotCards.vue";
import { userStore } from "../store/userStore";
import Footer from "../components/Footer.vue";

const store = userStore();
const router = useRouter();
const activeTab = ref('Availability');
const selectedDate = ref(new Date());

// Example slot data structure - in a real app, this would come from an API or store
const slotData = ref({
  8: [
    {
      id: '1',
      startTime: '10:00',
      endTime: '11:00',
      status: 'booked',
      studentName: 'Alex Chen'
    },
    {
      id: '2',
      startTime: '14:00',
      endTime: '15:00',
      status: 'available'
    }
  ],
  15: [
    {
      id: '3',
      startTime: '09:00',
      endTime: '10:00',
      status: 'available'
    },
    {
      id: '4',
      startTime: '15:00',
      endTime: '16:30',
      status: 'booked',
      studentName: 'Maria Rodriguez'
    }
  ],
  16: [
    {
      id: '5',
      startTime: '13:00',
      endTime: '14:00',
      status: 'available'
    }
  ],
  17: [
    {
      id: '6',
      startTime: '13:00',
      endTime: '14:00',
      status: 'available'
    }
  ]
});

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

const handleDateSelected = (date: Date) => {
  selectedDate.value = date;
};
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="p-4 mx-auto md:p-8 max-w-7xl">
      <div class="mb-6">
        <h1 class="mb-2 text-2xl font-bold">Tutor Dashboard</h1>
        <p class="text-gray-600">
          Welcome to your dashboard! Here you can manage your tutoring sessions,
          view student progress, and update your profile.
        </p>
      </div>
      
      <NavigationBar @tabChange="handleTabChange" />
      
      <!-- Dynamic content based on active tab -->
      <div class="mt-8">
        <!-- Profile Tab -->
        <TutorProfile v-if="activeTab === 'Profile'" />
        
        <!-- Reviews Tab -->
        <TutorReview v-else-if="activeTab === 'Reviews'" />
        
        <!-- Availability Tab -->
        <div 
          v-else-if="activeTab === 'Availability'" 
          class="content-container"
        >
          <div class="p-6 bg-white shadow-lg rounded-2xl md:p-8">
            <h2 class="mb-4 text-xl font-semibold">Manage Your Availability</h2>
            <p class="mb-6">Set your available hours for tutoring sessions.</p>
            
            <!-- Calendar component or availability settings would go here -->
            <div class="p-4 mb-6 border border-gray-200 bg-gray-50 rounded-xl">
              <h3 class="mb-3 text-lg font-medium">Weekly Schedule</h3>
              <div class="grid grid-cols-1 gap-2 text-center md:grid-cols-7">
                <div v-for="day in ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']" 
                     :key="day" 
                     class="p-3 bg-white border border-gray-100 rounded-lg">
                  <p class="mb-2 font-medium">{{ day }}</p>
                  <div class="text-sm text-gray-600">
                    <p>9:00 AM - 5:00 PM</p>
                    <button class="mt-2 text-xs text-purple-600 hover:underline">Edit</button>
                  </div>
                </div>
              </div>
            </div>
            
            <div class="flex justify-end">
              <button class="px-6 py-2 text-white transition-colors bg-purple-600 rounded-full hover:bg-purple-700">
                Save Schedule
              </button>
            </div>
          <div class="grid grid-cols-1 gap-6 md:grid-cols-2">
            <TutorCalendar @dateSelected="handleDateSelected" />
            <TutorSlots :date="selectedDate" />
          </div>
          
          <!-- New Statistics Cards -->
          <TutorSlotsCards :slot-data="slotData" />
        </div>
        
        <TutorBookings v-else-if="activeTab === 'Bookings'" />
        
        <div 
          v-else-if="activeTab === 'Reviews'" 
          class="content-container"
        >
          <div class="p-6 bg-white shadow-lg rounded-2xl md:p-8">
            <h2 class="mb-4 text-xl font-semibold">Student Reviews</h2>
            <p class="mb-6">See what your students are saying about your tutoring.</p>
            
            <!-- Reviews list would go here -->
            <div class="space-y-6">
              <div class="pb-4 border-b border-gray-200">
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
                <p class="mb-2 text-gray-700">"Sarah is an amazing math tutor! She explains complex concepts in a way that's easy to understand. I went from struggling with calculus to acing my exams."</p>
                <p class="text-sm text-gray-500">- Alex Johnson, 3 days ago</p>
              </div>
              
              <div class="pb-4 border-b border-gray-200">
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
                <p class="mb-2 text-gray-700">"Very patient and knowledgeable. Would recommend to anyone struggling with statistics."</p>
                <p class="text-sm text-gray-500">- Michael Smith, 1 week ago</p>
              </div>
            </div>
          </div>
        <!-- Bookings Tab -->
        <div 
          v-else-if="activeTab === 'Bookings'" 
          class="content-container"
        >
          <div class="p-6 bg-white shadow-lg rounded-2xl md:p-8">
            <h2 class="mb-4 text-xl font-semibold">Your Bookings</h2>
            <p class="mb-6">View and manage your upcoming and past tutoring sessions.</p>
            
            <!-- Booking list would go here -->
            <div class="space-y-4">
              <div class="flex flex-col items-start justify-between gap-4 p-4 border border-gray-200 rounded-xl md:flex-row md:items-center">
                <div>
                  <h3 class="font-medium">Math Tutoring Session</h3>
                  <p class="text-sm text-gray-600">With Alex Johnson • Monday, Sept 9 • 3:00 PM - 4:00 PM</p>
                </div>
                <div class="flex gap-2">
                  <button class="px-4 py-1 text-sm text-white bg-purple-600 rounded-full">Details</button>
                  <button class="px-4 py-1 text-sm text-gray-700 border border-gray-300 rounded-full">Cancel</button>
                </div>
              </div>
              
              <div class="flex flex-col items-start justify-between gap-4 p-4 border border-gray-200 rounded-xl md:flex-row md:items-center">
                <div>
                  <h3 class="font-medium">Calculus Review</h3>
                  <p class="text-sm text-gray-600">With Michael Smith • Wednesday, Sept 11 • 2:00 PM - 3:30 PM</p>
                </div>
                <div class="flex gap-2">
                  <button class="px-4 py-1 text-sm text-white bg-purple-600 rounded-full">Details</button>
                  <button class="px-4 py-1 text-sm text-gray-700 border border-gray-300 rounded-full">Cancel</button>
                </div>
              </div>
            </div>
          </div>
          
          <!-- Statistics Cards for Bookings Tab -->
          <TutorSlotsCards :slot-data="slotData" />
        </div>
        
        <!-- Messages Tab -->
        <div 
          v-else-if="activeTab === 'Messages'" 
          class="content-container"
        >
          <div class="p-6 bg-white shadow-lg rounded-2xl md:p-8">
            <h2 class="mb-4 text-xl font-semibold">Messages</h2>
            <p class="mb-6">Communicate with your students.</p>
            
            <!-- Messages interface would go here -->
            <div class="flex flex-col overflow-hidden border border-gray-200 h-80 rounded-xl">
              <div class="p-3 border-b border-gray-200 bg-gray-50">
                <p class="font-medium">Alex Johnson</p>
              </div>
              
              <div class="flex-grow p-4 overflow-y-auto">
                <div class="flex flex-col gap-3">
                  <div class="max-w-[70%] bg-gray-100 p-3 rounded-xl rounded-bl-none self-start">
                    <p class="text-sm">Hi Sarah, I was wondering if we could schedule an extra session this week to review for my upcoming exam?</p>
                    <p class="mt-1 text-xs text-gray-500">10:30 AM</p>
                  </div>
                  
                  <div class="max-w-[70%] bg-purple-100 p-3 rounded-xl rounded-br-none self-end">
                    <p class="text-sm">Hi Alex! Yes, I have time available on Thursday at 4pm. Would that work for you?</p>
                    <p class="mt-1 text-xs text-gray-500">10:45 AM</p>
                  </div>
                </div>
              </div>
              
              <div class="p-3 border-t border-gray-200">
                <div class="flex gap-2">
                  <input type="text" placeholder="Type a message..." class="flex-grow p-2 text-sm border border-gray-300 rounded-full" />
                  <button class="p-2 text-white bg-purple-600 rounded-full">
                    <span class="text-sm material-icons">send</span>
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
