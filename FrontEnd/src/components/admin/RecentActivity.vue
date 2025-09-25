<script setup lang="ts">
</script>

<template>
  <div class="bg-white p-6 rounded-lg shadow-md">
    <h2 class="text-xl font-bold text-gray-800 mb-4">Recent Activity</h2>
    <ul class="space-y-4">
      <li class="flex items-start">
        <div class="w-2 h-2 rounded-full bg-blue-500 mt-2 mr-3"></div>
        <div class="flex-1">
          <p class="text-gray-800">
            New booking created by <span class="font-semibold">Sarah Johnson</span>
          </p>
          <p class="text-gray-500 text-sm">2 minutes ago</p>
        </div>
        <span class="ml-auto px-2 py-1 text-xs font-semibold rounded-full bg-blue-100 text-blue-800">booking</span>
      </li>
      <li class="flex items-start">
        <div class="w-2 h-2 rounded-full bg-green-500 mt-2 mr-3"></div>
        <div class="flex-1">
          <p class="text-gray-800">
            Tutor profile verified: <span class="font-semibold">Dr. Michael Chen</span>
          </p>
          <p class="text-gray-500 text-sm">15 minutes ago</p>
        </div>
        <span class="ml-auto px-2 py-1 text-xs font-semibold rounded-full bg-green-100 text-green-800">tutor</span>
      </li>
      <li class="flex items-start">
        <div class="w-2 h-2 rounded-full bg-blue-500 mt-2 mr-3"></div>
        <div class="flex-1">
          <p class="text-gray-800">
            New student registration: <span class="font-semibold">Emma Davis</span>
          </p>
          <p class="text-gray-500 text-sm">32 minutes ago</p>
        </div>
        <span class="ml-auto px-2 py-1 text-xs font-semibold rounded-full bg-gray-200 text-gray-800">user</span>
      </li>
      <li class="flex items-start">
        <div class="w-2 h-2 rounded-full bg-green-500 mt-2 mr-3"></div>
        <div class="flex-1">
          <p class="text-gray-800">
            New review submitted <span class="font-semibold">(5 stars)</span>
          </p>
          <p class="text-gray-500 text-sm">1 hour ago</p>
        </div>
        <span class="ml-auto px-2 py-1 text-xs font-semibold rounded-full bg-green-100 text-green-800">review</span>
      </li>
      <li class="flex items-start">
        <div class="w-2 h-2 rounded-full bg-yellow-500 mt-2 mr-3"></div>
        <div class="flex-1">
          <p class="text-gray-800">
            High system load detected
          </p>
          <p class="text-gray-500 text-sm">2 hours ago</p>
        </div>
        <span class="ml-auto px-2 py-1 text-xs font-semibold rounded-full bg-yellow-100 text-yellow-800">alert</span>
      </li>
    </ul>
  </div>
</template>

