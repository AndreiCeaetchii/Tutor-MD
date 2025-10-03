<template>
  <div
    class="bg-gradient-to-r from-blue-50 to-purple-50 border border-blue-100 rounded-lg shadow-md flex items-center justify-between p-4 cursor-pointer transition-transform duration-300 hover:scale-[1.02]"
  >
    <div class="flex items-center space-x-3">
      <div
        class="w-10 h-10 flex items-center justify-center bg-gradient-to-r from-[#5f22d9] to-[#3a22d9] rounded-lg text-white"
      >
        <font-awesome-icon :icon="['fas', iconName]" />
      </div>
      <div>
        <div class="text-gray-800 font-medium">{{ subject }}</div>
        <div class="text-xs text-gray-500">{{ availability }}</div>
      </div>
    </div>
    <div class="text-lg font-bold text-[#5f22d9] text-right">
      {{ price }}<br />
      <span class="text-sm font-normal text-gray-500">/hour</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { defineProps, computed } from 'vue'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { library } from '@fortawesome/fontawesome-svg-core'
import {
  faCalculator,
  faBook,
  faAtom,
  faFlask,
  faLeaf,
  faGlobe,
  faLandmark,
  faLanguage,
  faLaptopCode,
  faCoins,
  faBrain,
  faPersonRunning,
  faHeartPulse,
  faPaintBrush,
  faMusic,
  faStar,
  faPenFancy,
  faChartLine,
  faSchool,
  faBookOpenReader
} from '@fortawesome/free-solid-svg-icons'

library.add(
  faCalculator,
  faBook,
  faAtom,
  faFlask,
  faLeaf,
  faGlobe,
  faLandmark,
  faLanguage,
  faLaptopCode,
  faCoins,
  faBrain,
  faPersonRunning,
  faHeartPulse,
  faPaintBrush,
  faMusic,
  faStar,
  faPenFancy,
  faChartLine,
  faSchool,
  faBookOpenReader
)

const props = defineProps({
  subject: String,
  price: String,
  availability: String
})

const iconName = computed(() => {
  const subject = props.subject?.toLowerCase() || ''

  switch (subject) {
    case 'Mathematics':
    case 'statistics':
      return 'calculator'
    case 'english':
    case 'literature':
    case 'creative writing':
      return 'book-open-reader'
    case 'science':
      return 'atom'
    case 'physics':
      return 'atom'
    case 'chemistry':
      return 'flask'
    case 'biology':
      return 'leaf'
    case 'geography':
      return 'globe'
    case 'history':
      return 'landmark'
    case 'foreign languages':
    case 'german':
    case 'french':
    case 'russian':
    case 'spanish':
    case 'italian':
      return 'language'
    case 'computer science':
      return 'laptop-code'
    case 'economics':
      return 'coins'
    case 'philosophy':
    case 'psychology':
    case 'sociology':
      return 'brain'
    case 'physical education':
      return 'person-running'
    case 'health education':
      return 'heart-pulse'
    case 'drawing':
      return 'paint-brush'
    case 'music':
      return 'music'
    case 'astronomy':
      return 'star'
    default:
      return 'book'
  }
})
</script>
