<script setup lang="ts">
  import { computed, ref, onMounted } from 'vue';
  import VueApexCharts from 'vue3-apexcharts';
  import {
    getSubjectDistribution,
    getRatingDistribution,
    getVerificationStats,
    getStudentActivityStats,
    type SubjectStatistic,
    type RatingDistribution,
    type VerificationStatusStats,
    type StudentActivityStats,
  } from '../../services/adminService';

  const loading = ref(false);
  const subjectData = ref<SubjectStatistic[]>([]);
  const ratingData = ref<RatingDistribution[]>([]);
  const verificationData = ref<VerificationStatusStats>({
    verified: 0,
    pending: 0,
    declined: 0,
  });
  const studentActivityData = ref<StudentActivityStats[]>([]);

  const ratingChartOptions = computed(() => ({
    chart: {
      type: 'donut' as const,
      height: 300,
      animations: {
        enabled: true,
        easing: 'easeinout',
        speed: 800,
      },
    },
    labels: ratingData.value.map((d) => d.range),
    colors: ['#f97316', '#fb923c', '#fdba74', '#fed7aa', '#ffedd5', '#6b7280'],
    dataLabels: {
      enabled: true,
      formatter: (val: number) => `${Math.round(val)}%`,
      style: {
        fontSize: '12px',
        fontWeight: 600,
      },
    },
    legend: {
      position: 'bottom' as const,
      fontSize: '12px',
      fontWeight: 500,
      markers: {
        size: 6,
        offsetX: -3,
      },
      itemMargin: {
        horizontal: 8,
        vertical: 5,
      },
    },
    plotOptions: {
      pie: {
        donut: {
          size: '65%',
          labels: {
            show: true,
            name: {
              show: true,
              fontSize: '14px',
              fontWeight: 600,
            },
            value: {
              show: true,
              fontSize: '22px',
              fontWeight: 700,
              formatter: (val: string) => val,
            },
            total: {
              show: true,
              label: 'Total Tutors',
              fontSize: '13px',
              fontWeight: 600,
              formatter: () => {
                const total = ratingData.value.reduce((sum, d) => sum + d.count, 0);
                return total.toString();
              },
            },
          },
        },
      },
    },
    tooltip: {
      theme: 'light',
      y: {
        formatter: (val: number) => `${val} tutor${val !== 1 ? 's' : ''}`,
      },
    },
  }));

  const ratingChartSeries = computed(() => ratingData.value.map((d) => d.count));

  const verificationChartOptions = computed(() => ({
    chart: {
      type: 'donut' as const,
      height: 300,
      animations: {
        enabled: true,
        easing: 'easeinout',
        speed: 800,
      },
    },
    labels: ['Verified', 'Pending', 'Declined'],
    colors: ['#10b981', '#f59e0b', '#ef4444'],
    dataLabels: {
      enabled: true,
      formatter: (val: number) => `${Math.round(val)}%`,
      style: {
        fontSize: '14px',
        fontWeight: 600,
      },
    },
    legend: {
      position: 'bottom' as const,
      fontSize: '14px',
      fontWeight: 500,
      markers: {
        size: 6,
        offsetX: -3,
      },
      itemMargin: {
        horizontal: 10,
        vertical: 5,
      },
    },
    plotOptions: {
      pie: {
        donut: {
          size: '70%',
          labels: {
            show: true,
            name: {
              show: true,
              fontSize: '16px',
              fontWeight: 600,
            },
            value: {
              show: true,
              fontSize: '24px',
              fontWeight: 700,
              formatter: (val: string) => val,
            },
            total: {
              show: true,
              label: 'Total Tutors',
              fontSize: '14px',
              fontWeight: 600,
              formatter: () => {
                const total =
                  verificationData.value.verified +
                  verificationData.value.pending +
                  verificationData.value.declined;
                return total.toString();
              },
            },
          },
        },
      },
    },
    tooltip: {
      theme: 'light',
      y: {
        formatter: (val: number) => `${val} tutor${val !== 1 ? 's' : ''}`,
      },
    },
  }));

  const verificationChartSeries = computed(() => [
    verificationData.value.verified,
    verificationData.value.pending,
    verificationData.value.declined,
  ]);

  const subjectChartOptions = computed(() => ({
    chart: {
      type: 'donut' as const,
      height: 300,
      animations: {
        enabled: true,
        easing: 'easeinout',
        speed: 800,
      },
    },
    labels: subjectData.value.map((d) => d.subject),
    colors: [
      '#a55eea',
      '#3498db',
      '#e74c3c',
      '#f1c40f',
      '#9b59b6',
      '#1abc9c',
      '#e67e22',
      '#34495e',
      '#16a085',
      '#c0392b',
    ],
    dataLabels: {
      enabled: true,
      formatter: (val: number) => `${Math.round(val)}%`,
      style: {
        fontSize: '12px',
        fontWeight: 600,
      },
      dropShadow: {
        enabled: false,
      },
    },
    legend: {
      position: 'right' as const,
      fontSize: '14px',
      fontWeight: 500,
      markers: {
        size: 6,
        offsetX: -3,
      },
      itemMargin: {
        horizontal: 10,
        vertical: 5,
      },
      formatter: (seriesName: string, opts: any) => {
        const index = opts.seriesIndex;
        const count = subjectData.value[index]?.count || 0;
        return `${seriesName} (${count})`;
      },
    },
    plotOptions: {
      pie: {
        donut: {
          size: '65%',
          labels: {
            show: true,
            name: {
              show: true,
              fontSize: '18px',
              fontWeight: 600,
            },
            value: {
              show: true,
              fontSize: '28px',
              fontWeight: 700,
              formatter: (val: string) => val,
            },
            total: {
              show: true,
              label: 'Total Subjects',
              fontSize: '16px',
              fontWeight: 600,
              formatter: () => {
                const total = subjectData.value.reduce((sum, d) => sum + d.count, 0);
                return total.toString();
              },
            },
          },
        },
      },
    },
    tooltip: {
      theme: 'light',
      y: {
        formatter: (val: number) => `${val} tutor${val !== 1 ? 's' : ''}`,
      },
    },
  }));

  const subjectChartSeries = computed(() => subjectData.value.map((d) => d.count));

  const studentChartOptions = computed(() => ({
    chart: {
      type: 'donut' as const,
      height: 300,
      animations: {
        enabled: true,
        easing: 'easeinout',
        speed: 800,
      },
    },
    labels: studentActivityData.value.map((d) => d.range),
    colors: ['#3b82f6', '#8b5cf6', '#10b981', '#f59e0b', '#ef4444', '#ec4899'],
    dataLabels: {
      enabled: true,
      formatter: (val: number) => `${Math.round(val)}%`,
      style: {
        fontSize: '12px',
        fontWeight: 600,
      },
    },
    legend: {
      position: 'bottom' as const,
      fontSize: '12px',
      fontWeight: 500,
      markers: {
        size: 6,
        offsetX: -3,
      },
      itemMargin: {
        horizontal: 8,
        vertical: 5,
      },
    },
    plotOptions: {
      pie: {
        donut: {
          size: '65%',
          labels: {
            show: true,
            name: {
              show: true,
              fontSize: '14px',
              fontWeight: 600,
            },
            value: {
              show: true,
              fontSize: '22px',
              fontWeight: 700,
              formatter: (val: string) => val,
            },
            total: {
              show: true,
              label: 'Total Students',
              fontSize: '13px',
              fontWeight: 600,
              formatter: () => {
                const total = studentActivityData.value.reduce((sum, d) => sum + d.count, 0);
                return total.toString();
              },
            },
          },
        },
      },
    },
    tooltip: {
      theme: 'light',
      y: {
        formatter: (val: number) => `${val} student${val !== 1 ? 's' : ''}`,
      },
    },
  }));

  const studentChartSeries = computed(() => studentActivityData.value.map((d) => d.count));

  onMounted(async () => {
    loading.value = true;
    try {
      const [subjects, ratings, verification, studentActivity] = await Promise.all([
        getSubjectDistribution(),
        getRatingDistribution(),
        getVerificationStats(),
        getStudentActivityStats(),
      ]);

      subjectData.value = subjects;
      ratingData.value = ratings;
      verificationData.value = verification;
      studentActivityData.value = studentActivity;
    } catch (error) {
      console.error('Error loading analytics data:', error);
    } finally {
      loading.value = false;
    }
  });
</script>

<template>
  <div class="p-6 mx-auto max-w-7xl">
    <h1 class="mb-8 text-3xl font-bold text-gray-900">Analytics Dashboard</h1>

    <div v-if="loading" class="flex items-center justify-center py-20">
      <div class="w-12 h-12 border-b-2 border-purple-600 rounded-full animate-spin"></div>
    </div>

    <div v-else>
      <!-- Grid 2x2 Layout -->
      <div class="grid grid-cols-1 gap-6 lg:grid-cols-2">
        <!-- Rating Distribution -->
        <div
          class="p-6 transition-shadow duration-300 bg-white shadow-md rounded-xl hover:shadow-lg"
        >
          <h3 class="flex items-center mb-4 text-lg font-bold text-gray-800">
            <svg
              class="w-5 h-5 mr-2 text-purple-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z"
              />
            </svg>
            Tutors by Rating
          </h3>
          <VueApexCharts
            type="donut"
            height="300"
            :options="ratingChartOptions"
            :series="ratingChartSeries"
          />
        </div>

        <!-- Student Activity -->
        <div
          class="p-6 transition-shadow duration-300 bg-white shadow-md rounded-xl hover:shadow-lg"
        >
          <h3 class="flex items-center mb-4 text-lg font-bold text-gray-800">
            <svg
              class="w-5 h-5 mr-2 text-blue-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"
              />
            </svg>
            Student Registrations (Last 6 Months)
          </h3>
          <VueApexCharts
            type="donut"
            height="300"
            :options="studentChartOptions"
            :series="studentChartSeries"
          />
        </div>

        <!-- Verification Status -->
        <div
          class="p-6 transition-shadow duration-300 bg-white shadow-md rounded-xl hover:shadow-lg"
        >
          <h3 class="flex items-center mb-4 text-lg font-bold text-gray-800">
            <svg
              class="w-5 h-5 mr-2 text-green-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
              />
            </svg>
            Tutor Verification Status
          </h3>
          <VueApexCharts
            type="donut"
            height="300"
            :options="verificationChartOptions"
            :series="verificationChartSeries"
          />
        </div>

        <!-- Subject Distribution -->
        <div
          class="p-6 transition-shadow duration-300 bg-white shadow-md rounded-xl hover:shadow-lg"
        >
          <h3 class="flex items-center mb-4 text-lg font-bold text-gray-800">
            <svg
              class="w-5 h-5 mr-2 text-indigo-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"
              />
            </svg>
            Subject Distribution
          </h3>
          <VueApexCharts
            type="donut"
            height="300"
            :options="subjectChartOptions"
            :series="subjectChartSeries"
          />
        </div>
      </div>
    </div>
  </div>
</template>
