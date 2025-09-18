<template>
  <div
    class="relative w-32 h-32 rounded-full overflow-hidden border-4 border-white shadow-lg cursor-pointer group"
    @click="openFilePicker"
  >
    <img :src="imageUrl" alt="Profile Image" class="w-full h-full object-cover" />

    <div
      class="absolute inset-0 flex items-center justify-center bg-black bg-opacity-50 opacity-0 group-hover:opacity-100 transition-opacity"
    >
      <span class="text-white text-sm text-center">Change Photo</span>
    </div>

    <input ref="fileInput" type="file" class="hidden" @change="handleFileChange" accept="image/*" />
  </div>
</template>

<script setup lang="ts">
  import { ref, watchEffect } from 'vue';

  const props = defineProps({
    modelValue: {
      type: [String, File],
      required: true,
    },
  });

  const emit = defineEmits(['update:modelValue']);

  const fileInput = ref<HTMLInputElement | null>(null);
  const imageUrl = ref(props.modelValue as string);

  // Deschide fereastra de selectare a fișierului
  const openFilePicker = () => {
    fileInput.value?.click();
  };

  // Gestionează fișierul selectat și previzualizează imaginea
  const handleFileChange = (event: Event) => {
    const target = event.target as HTMLInputElement;
    const file = target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        imageUrl.value = e.target?.result as string;
      };
      reader.readAsDataURL(file);
      emit('update:modelValue', file);
    }
  };

  // Setează imaginea inițială
  watchEffect(() => {
    if (typeof props.modelValue === 'string') {
      imageUrl.value = props.modelValue;
    }
  });
</script>
