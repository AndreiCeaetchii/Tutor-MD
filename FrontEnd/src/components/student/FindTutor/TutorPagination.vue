<script setup>
import { computed, ref, onMounted } from 'vue';

const props = defineProps({
  currentPage: { type: Number, default: 1 },
  totalPages: { type: Number, default: 10 },
  siblingCount: { type: Number, default: 1 }
});

const emit = defineEmits(['page-changed']);
const windowWidth = ref(typeof window !== 'undefined' ? window.innerWidth : 1024);

onMounted(() => {
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', () => {
      windowWidth.value = window.innerWidth;
    });
  }
});

const changePage = (page) => {
  if (page >= 1 && page <= props.totalPages) {
    emit('page-changed', page);
  }
};

const pageNumbers = computed(() => {
  const pages = [];
  const { currentPage, totalPages, siblingCount } = props;
  const isSmallScreen = windowWidth.value < 640;
  const effectiveSiblingCount = isSmallScreen ? 0 : siblingCount;
  pages.push(1);
  
  const startPage = Math.max(2, currentPage - effectiveSiblingCount);
  const endPage = Math.min(totalPages - 1, currentPage + effectiveSiblingCount);
  
  if (startPage > 2) {
    pages.push('...');
  }
  

  for (let i = startPage; i <= endPage; i++) {
    pages.push(i);
  }
  
  if (endPage < totalPages - 1) {
    pages.push('...');
  }
  
  if (totalPages > 1) {
    pages.push(totalPages);
  }
  
  return pages;
});
</script>

<template>
  <div class="flex justify-center mt-6 mb-8">
    <div class="flex items-center gap-1 sm:gap-2">
      <button
        @click="changePage(currentPage - 1)"
        class="flex items-center justify-center w-8 h-8 text-gray-600 border rounded-md hover:bg-purple-100 disabled:opacity-50"
        :disabled="currentPage === 1"
      >
        <span>&lt;</span>
      </button>

      <template v-for="(page, index) in pageNumbers" :key="index">
        <div
          v-if="page === '...'"
          class="items-center justify-center hidden w-8 h-8 text-gray-600 sm:flex"
        >
          ...
        </div>
        <button
          v-else
          @click="changePage(page)"
          :class="[
            'flex items-center justify-center w-8 h-8 rounded-md',
            page === currentPage
              ? 'bg-purple-700 text-white'
              : 'text-gray-700 hover:bg-purple-100',
            typeof page === 'number' && (page === 1 || page === totalPages || Math.abs(page - currentPage) <= 1 || windowWidth >= 640)
              ? 'flex'
              : 'hidden sm:flex'
          ]"
        >
          {{ page }}
        </button>
      </template>

      <button
        @click="changePage(currentPage + 1)"
        class="flex items-center justify-center w-8 h-8 text-gray-600 border rounded-md hover:bg-purple-100 disabled:opacity-50"
        :disabled="currentPage === totalPages"
      >
        <span>&gt;</span>
      </button>
    </div>
  </div>
</template>
