<template>
  <div
    class="relative w-32 h-32 rounded-full overflow-hidden border-4 border-white shadow-lg cursor-pointer group"
    @click="openFilePicker"
  >
    <img v-if="!loading" :src="imageUrl" alt="Profile Image" class="w-full h-full object-cover" />
    <div v-else class="w-full h-full flex items-center justify-center bg-gray-200">
      <span class="text-gray-500">Loading...</span>
    </div>

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
  import { deleteProfilePhoto, uploadProfilePhoto } from '../../../services/userService.ts'; // Ajustează calea
  import { useProfileStore } from '../../../store/profileStore.ts';

  const props = defineProps({
    modelValue: {
      type: [String, File],
      required: true,
    },
  });

  const profileStore = useProfileStore();

  const emit = defineEmits(['update:modelValue']);

  const fileInput = ref<HTMLInputElement | null>(null);
  const imageUrl = ref(props.modelValue as string);
  const loading = ref(false);

  const openFilePicker = () => {
    fileInput.value?.click();
  };

  const handleFileChange = async (event: Event) => {
    const target = event.target as HTMLInputElement;
    const file = target.files?.[0];

    if (!file) {
      return;
    }

    loading.value = true;

    try {
      const defaultImgPath = '/src/assets/DefaultImg.png';

      if (profileStore.profileImage && profileStore.profileImage !== defaultImgPath) {
        await deleteProfilePhoto();
      }

      const uploadResponse = await uploadProfilePhoto(file);

      profileStore.setProfileImage(uploadResponse.url);

      const reader = new FileReader();
      reader.onload = (e) => {
        imageUrl.value = e.target?.result as string;
      };
      reader.readAsDataURL(file);

      emit('update:modelValue', file);
    } catch (error) {
      console.error('Procesul de actualizare a fotografiei a eșuat:', error);
    } finally {
      loading.value = false;
    }
  };

  watchEffect(() => {
    if (typeof props.modelValue === 'string') {
      imageUrl.value = props.modelValue;
    }
  });
</script>
