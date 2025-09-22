<template>
  <div class="max-w-6xl mx-auto rounded-2xl bg-white shadow-lg p-6">
    <template v-if="profileStore.isEditing">
      <ProfileHeaderEdit :editedProfile="editedProfile" @save-profile="saveChanges" />
      <ProfileDetailsEdit :editedProfile="editedProfile" />
    </template>
    <template v-else>
      <ProfileHeader :hasIdFromUrl="hasIdFromUrl" />
      <ProfileDetails />
    </template>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, watch } from 'vue';
  import { useProfileStore } from '../store/profileStore.ts';
  import type { ProfileState } from '../store/profileStore.ts';
  import ProfileHeader from '../components/profile/ProfileHeader.vue';
  import ProfileDetails from '../components/profile/ProfileDetails.vue';
  import ProfileHeaderEdit from '../components/profile/ProfileHeaderEdit.vue';
  import ProfileDetailsEdit from '../components/profile/ProfileDetailsEdit.vue';
  import {
    addSubject,
    updateSubject,
    getTutorProfile,
    editTutorProfile,
  } from '../services/tutorService.ts';
  import { useUserStore } from '../store/userStore.ts';
  import { useRouter } from 'vue-router';
  import defaultProfileImage from '../assets/DefaultImg.png';
  import { useRoute } from 'vue-router';
  const route = useRoute();

  const router = useRouter();
  const profileStore = useProfileStore();
  const userStore = useUserStore();

  const editedProfile = ref<ProfileState>({ ...profileStore.$state });

  const hasIdFromUrl = !!route.params.id;

  const calculateAge = (birthdate: string | null) => {
    if (!birthdate) return 0;
    const birth = new Date(birthdate);
    const today = new Date();
    let age = today.getFullYear() - birth.getFullYear();
    const monthDiff = today.getMonth() - birth.getMonth();
    const dayDiff = today.getDate() - birth.getDate();
    if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
      age--;
    }
    return age;
  };

  const fetchAndSetProfile = async () => {
    try {
      const userIdFromUrl = route.params.id ? Number(route.params.id) : Number(userStore.userId);
      const serverData = await getTutorProfile(userIdFromUrl);
      if (!serverData || !serverData.userProfile || !serverData.userProfile.username) {
        await router.push('/create-profile');
        return;
      }
      const profileData = {
        userName: serverData.userProfile.username || '',
        firstName: serverData.userProfile.firstName || '',
        lastName: serverData.userProfile.lastName || '',
        bio: serverData.userProfile.bio || '',
        phone: serverData.userProfile.phone || '',
        email: serverData.email || '',
        experience: serverData.experienceYears || 0,
        age: calculateAge(serverData.userProfile.birthdate),
        country: serverData.userProfile.country || '',
        city: serverData.userProfile.city || '',
        location: `${serverData.userProfile.city || ''}, ${serverData.userProfile.country || ''}`,
        profileImage: serverData.photo?.url || profileStore.profileImage || defaultProfileImage,
        rating: 0,
        reviews: 0,
        students: 0,
        languages: [],
        subjects: serverData.tutorSubjects.map((s: any) => ({
          name: s.subjectName || '',
          price: s.price || 0,
          currency: s.currency || 'mdl',
          subjectId: s.subjectId,
          isNew: false,
          isModified: false,
        })),
        birthdate: serverData.userProfile.birthdate,
      };
      profileStore.setProfileDetails(profileData);
    } catch (error) {
      console.error('Eroare la preluarea profilului:', error);
    }
  };

  onMounted(() => {
    fetchAndSetProfile();
  });

  watch(
    () => profileStore.isEditing,
    (newValue) => {
      if (newValue) {
        editedProfile.value = { ...profileStore.$state };
      }
    },
  );

  const saveChanges = async () => {
    try {
      const profileDataToUpdate = {
        userProfile: {
          username: editedProfile.value.userName,
          firstName: editedProfile.value.firstName,
          lastName: editedProfile.value.lastName,
          bio: editedProfile.value.bio,
          phone: editedProfile.value.phone,
          birthdate: profileStore.birthdate,
          country: editedProfile.value.country,
          city: editedProfile.value.city,
          experienceYears: editedProfile.value.experience,
          profileImage: editedProfile.value.profileImage,
        },
        tutorSubjects: editedProfile.value.subjects.map((s: any) => ({
          subjectId: s.subjectId || 0,
          subjectName: s.name,
          subjectSlug: s.name.toLowerCase().replace(/\s+/g, '-'),
          price: s.price,
          currency: s.currency.toLowerCase(),
        })),
      };
      await editTutorProfile(profileDataToUpdate.userProfile);
      for (const subject of editedProfile.value.subjects) {
        if (subject.isNew) {
          await addSubject({
            subjectName: subject.name,
            subjectSlug: subject.name.toLowerCase().replace(/\s+/g, '-'),
            pricePerHour: subject.price,
            currency: subject.currency.toLowerCase(),
          });
          subject.isNew = false;
        } else if (subject.isModified) {
          console.log(subject);
          await updateSubject({
            subjectId: subject.subjectId ?? 0,
            subjectName: subject.name,
            subjectSlug: subject.name.toLowerCase().replace(/\s+/g, '-'),
            price: subject.price,
            currency: subject.currency.toLowerCase(),
          });
          subject.isModified = false;
        }
      }
      const { profileImage, ...rest } = editedProfile.value;
      profileStore.setProfileDetails({
        ...rest,
        profileImage: profileStore.profileImage,
      });
      profileStore.toggleEditing();
    } catch (error) {
      console.error('Eroare la salvarea modificărilor profilului:', error);
    }
  };
</script>
