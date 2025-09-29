<script setup lang="ts">
import { defineProps, computed } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faGraduationCap, faUserTie, faUsers } from '@fortawesome/free-solid-svg-icons';

library.add(faGraduationCap, faUserTie, faUsers);

const props = defineProps({
  title: {
    type: String,
    required: true,
  },
  value: {
    type: Number,
    required: true,
  },
  icon: {
    type: String,
    required: true,
  },
  color: {
    type: String,
    default: 'gray',
  },
  trend: {
    type: String,
    default: null,
  },
});

const cardClasses = computed(() => {
  const base = 'p-6 rounded-2xl shadow-lg text-white relative overflow-hidden';
  const colors: { [key: string]: string } = {
    purple: 'bg-gradient-to-br from-purple-500 to-indigo-600',
    green: 'bg-gradient-to-br from-teal-500 to-green-600',
    blue: 'bg-gradient-to-br from-blue-500 to-sky-600',
  };
  return `${base} ${colors[props.color] || colors.purple}`;
});

const trendClasses = computed(() => {
  const base = 'text-sm font-semibold mt-2';
  return props.trend ? `${base}` : 'hidden';
});
</script>

<template>
  <div :class="cardClasses">
    <div class="flex justify-between items-center mb-2">
      <div class="text-sm font-light uppercase opacity-80">{{ title }}</div>
      <font-awesome-icon :icon="['fas', icon]" class="text-4xl opacity-50 absolute top-4 right-4" />
    </div>
    <div class="text-5xl font-bold mb-1">{{ value.toLocaleString() }}</div>
    <div :class="trendClasses">
      <span v-if="trend">{{ trend }}</span>
    </div>
  </div>
</template>