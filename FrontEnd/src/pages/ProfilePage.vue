<template>
  <div class="max-w-6xl mx-auto rounded-2xl bg-white shadow-lg">
    <template v-if="profileStore.isEditing">
      <ProfileHeaderEdit :editedProfile="editedProfile" @save-profile="saveChanges" />
      <ProfileDetailsEdit :editedProfile="editedProfile" />
    </template>

    <template v-else>
      <ProfileHeader />
      <ProfileDetails />
    </template>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue';
  import { useProfileStore } from '../store/profileStore.ts';
  import ProfileHeader from '../components/profile/ProfileHeader.vue';
  import ProfileDetails from '../components/profile/ProfileDetails.vue';
  import ProfileHeaderEdit from '../components/profile/ProfileHeaderEdit.vue';
  import ProfileDetailsEdit from '../components/profile/ProfileDetailsEdit.vue';

  const profileStore = useProfileStore();

  const editedProfile = ref({
    firstName: profileStore.firstName,
    lastName: profileStore.lastName,
    experience: profileStore.experience,
    location: profileStore.location,
    bio: profileStore.bio,
    phone: profileStore.phone,
    email: profileStore.email,
    subjects: JSON.parse(JSON.stringify(profileStore.subjects)),
    languages: [...profileStore.languages],
  });

  const saveChanges = () => {
    profileStore.setProfileDetails(editedProfile.value);
    profileStore.toggleEditing();
  };
</script>
