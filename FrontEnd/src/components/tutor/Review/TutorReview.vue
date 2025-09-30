<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import ReviewStats from '../Review/ReviewStats.vue';
import ReviewCard from '../Review/ReviewCard.vue';
import WriteReview from './WriteReview.vue';
import { useRoute } from 'vue-router';
import { useUserStore } from '../../../store/userStore.ts';
import type {
  TutorReview,
  TutorReviewsResponse,
} from '../../../services/reviewService.ts';
import { getTutorReviews } from '../../../services/reviewService.ts';

const props = defineProps<{
  bookingIdForReview: number;
  haveToWriteReview: boolean;
  tutorName: string | null;
  subjectName: string | null;
  tutorPhoto: string | null;
  lessonDate: string | null;
}>();

const ratingsOpen = ref(false);
const selectedRating = ref('All Ratings');
const ratingsOptions = ['All Ratings', '5 Stars', '4 Stars', '3 Stars', '2 Stars', '1 Star'];

const sortOpen = ref(false);
const selectedSort = ref('Most Recent');
const sortOptions = ['Most Recent', 'Highest Rated', 'Lowest Rated'];

const reviews = ref<TutorReview[]>([]);
const averageRating = ref<number | null>(null);
const isLoading = ref(true);
const error = ref<string | null>(null);

const store = useUserStore();
const route = useRoute();
const idFromUrl = route.params.id as string | undefined;
const tutorId = computed(() => (route.params.id as string) || store.userId?.toString() || null);

const fetchReviews = async () => {
  if (!tutorId.value) {
    error.value = 'Tutor ID is not available.';
    isLoading.value = false;
    return;
  }

  isLoading.value = true;
  error.value = null;

  try {
    const tutorIdNumber = parseInt(tutorId.value, 10);
    if (isNaN(tutorIdNumber)) {
      throw new Error('Invalid tutor ID.');
    }

    const response: TutorReviewsResponse = await getTutorReviews(tutorIdNumber);
    reviews.value = response.reviews;
    averageRating.value = response.averageRating;
  } catch (err: any) {
    console.error('Failed to fetch reviews:', err);
    error.value = err.message || 'Failed to load reviews.';
  } finally {
    isLoading.value = false;
  }
};

onMounted(() => {
  fetchReviews();
});

function toggleRatingsDropdown() {
  ratingsOpen.value = !ratingsOpen.value;
  sortOpen.value = false;
}

function selectRating(rating: string) {
  selectedRating.value = rating;
  ratingsOpen.value = false;
}

function toggleSortDropdown() {
  sortOpen.value = !sortOpen.value;
  ratingsOpen.value = false;
}

function selectSort(sort: string) {
  selectedSort.value = sort;
  sortOpen.value = false;
}

const reviewsStatsData = computed(() => ({
  reviews: reviews.value,
  averageRating: averageRating.value,
}));

const { bookingIdForReview, tutorName, subjectName, tutorPhoto, lessonDate, haveToWriteReview } = props;
</script>

<template>
  <div class="content-container">
    <WriteReview v-if="idFromUrl && haveToWriteReview"
                 :bookingIdForReview="bookingIdForReview"
                 :tutorName="tutorName"
                 :subjectName="subjectName"
                 :tutorPhoto="tutorPhoto"
                 :lessonDate="lessonDate"
    />

    <ReviewStats
      v-if="!idFromUrl && !isLoading && reviews.length > 0"
      :reviews-data="reviewsStatsData"
    />
    <div class="my-6">
      <div class="p-6 bg-white shadow-lg rounded-2xl md:p-8">
        <div
          class="flex flex-col justify-between pb-4 mb-6 border-b border-gray-200 sm:flex-row sm:items-center"
        >
          <h2 class="mb-4 text-xl font-semibold text-gray-900 sm:mb-0">Student Reviews</h2>
          <div class="flex space-x-3">
            <div class="relative inline-block text-left">
              <button
                @click="toggleRatingsDropdown"
                class="flex items-center px-4 py-2 text-gray-600 transition-colors duration-200 bg-white border border-gray-300 rounded-full shadow-sm cursor-pointer hover:bg-purple-600 hover:text-white hover:border-purple-600 focus:outline-none focus:ring-2 focus:ring-purple-600 focus:border-transparent"
              >
                <span>{{ selectedRating }}</span>
                <svg
                  :class="{ 'transform rotate-180': ratingsOpen }"
                  class="w-5 h-5 ml-2 -mr-1 transition-transform duration-200"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                  aria-hidden="true"
                >
                  <path
                    fill-rule="evenodd"
                    d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                    clip-rule="evenodd"
                  />
                </svg>
              </button>
              <div
                v-if="ratingsOpen"
                class="absolute right-0 z-10 w-56 mt-2 origin-top-right bg-white border border-gray-300 shadow-lg rounded-xl focus:outline-none"
              >
                <div class="p-1" role="none">
                  <a
                    v-for="rating in ratingsOptions"
                    :key="rating"
                    href="#"
                    @click.prevent="selectRating(rating)"
                    class="block px-4 py-2 text-sm text-gray-700 transition-colors duration-200 rounded-lg hover:bg-purple-50 hover:text-purple-600"
                  >{{ rating }}</a
                  >
                </div>
              </div>
            </div>

            <div class="relative inline-block text-left">
              <button
                @click="toggleSortDropdown"
                class="flex items-center px-4 py-2 text-gray-600 transition-colors duration-200 bg-white border border-gray-300 rounded-full shadow-sm cursor-pointer hover:bg-purple-600 hover:text-white hover:border-purple-600 focus:outline-none focus:ring-2 focus:ring-purple-600 focus:border-transparent"
              >
                <span>{{ selectedSort }}</span>
                <svg
                  :class="{ 'transform rotate-180': sortOpen }"
                  class="w-5 h-5 ml-2 -mr-1 transition-transform duration-200"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                  aria-hidden="true"
                >
                  <path
                    fill-rule="evenodd"
                    d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                    clip-rule="evenodd"
                  />
                </svg>
              </button>
              <div
                v-if="sortOpen"
                class="absolute right-0 z-10 w-56 mt-2 origin-top-right bg-white border border-gray-300 shadow-lg rounded-xl focus:outline-none"
              >
                <div class="p-1" role="none">
                  <a
                    v-for="sort in sortOptions"
                    :key="sort"
                    href="#"
                    @click.prevent="selectSort(sort)"
                    class="block px-4 py-2 text-sm text-gray-700 transition-colors duration-200 rounded-lg hover:bg-purple-50 hover:text-purple-600"
                  >{{ sort }}</a
                  >
                </div>
              </div>
            </div>
          </div>
        </div>
        <div v-if="isLoading" class="text-center text-gray-500">Se încarcă recenziile...</div>
        <div v-else-if="error" class="text-center text-red-500">
          {{ error }}
        </div>
        <div v-else-if="reviews.length > 0" class="space-y-6">
          <ReviewCard v-for="review in reviews" :key="review.id" :review="review" />
        </div>
        <div v-else class="text-center text-gray-500">There are no reviews for this tutor.</div>
      </div>
    </div>
  </div>
</template>