<script setup lang="ts">
  import { computed, ref } from 'vue';
  import { DoughnutChart, LineChart } from 'vue-chart-3';
  import { Chart, registerables } from 'chart.js';

  Chart.register(...registerables);

  const monthlyGrowthData = ref([
    { month: 'Jan', value: 3000 },
    { month: 'Feb', value: 3500 },
    { month: 'Mar', value: 4000 },
    { month: 'Apr', value: 4500 },
  ]);

  const subjectDistributionData = ref([
    { subject: 'Mathematics', value: 35, color: '#a55eea' },
    { subject: 'Physics', value: 22, color: '#3498db' },
    { subject: 'Chemistry', value: 18, color: '#e74c3c' },
    { subject: 'Biology', value: 15, color: '#f1c40f' },
    { subject: 'Other', value: 10, color: '#9b59b6' },
  ]);

  const monthlyChartData = computed(() => {
    const labels = monthlyGrowthData.value.map((d) => d.month);
    const data = monthlyGrowthData.value.map((d) => d.value);

    return {
      labels: labels,
      datasets: [
        {
          label: 'Monthly Growth',
          data: data,
          backgroundColor: 'rgba(52, 152, 219, 0.5)',
          borderColor: '#3498db',
          fill: true,
          tension: 0.4,
        },
      ],
    };
  });

  const subjectChartData = computed(() => {
    const labels = subjectDistributionData.value.map((d) => `${d.subject} (${d.value}%)`);
    const data = subjectDistributionData.value.map((d) => d.value);
    const backgroundColors = subjectDistributionData.value.map((d) => d.color);

    return {
      labels: labels,
      datasets: [
        {
          data: data,
          backgroundColor: backgroundColors,
        },
      ],
    };
  });

  const subjectChartOptions = ref({
    responsive: true,
    cutout: '60%',
    plugins: {
      legend: {
        position: 'bottom',
      },
    },
  });
</script>

<template>
  <div class="p-6">
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <div class="bg-white p-6 rounded-lg shadow-md">
        <h2 class="text-xl font-semibold mb-4 text-gray-800">Monthly Growth Trends</h2>
        <LineChart :chartData="monthlyChartData" class="w-full" />
      </div>

      <div class="bg-white p-6 rounded-lg shadow-md">
        <h2 class="text-xl font-semibold mb-4 text-gray-800">Subject Distribution</h2>
        <DoughnutChart
          :chartData="subjectChartData"
          :options="subjectChartOptions"
          class="w-full h-80"
        />
      </div>
    </div>
  </div>
</template>
