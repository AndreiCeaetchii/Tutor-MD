<template>
  <div class="max-w-6xl mx-auto rounded-2xl bg-white shadow-lg p-6">
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

  // ✅ Import imagine default
  import defaultProfileImage from '../assets/DefaultImg.png';

  const router = useRouter();
  const profileStore = useProfileStore();
  const userStore = useUserStore();

  const editedProfile = ref<ProfileState>({
    userName: '',
    firstName: '',
    lastName: '',
    bio: '',
    phone: '',
    email: '',
    experience: 0,
    age: 0,
    country: '',
    city: '',
    location: '',
    profileImage: defaultProfileImage, // inițial default
    rating: 0,
    reviews: 0,
    students: 0,
    languages: [],
    subjects: [],
    isEditing: false,
  });

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
      const userId = userStore.userId ? Number(userStore.userId) : 0;
      const serverData = await getTutorProfile(userId);

      if (!serverData || !serverData.userProfile || !serverData.userProfile.username) {
        console.log('No profile found, redirecting to /create-profile.');
        await router.push('/create-profile');
        return;
      }

      const profileData = {
        userName: serverData.userProfile.username || '',
        firstName: serverData.userProfile.firstName || '',
        lastName: serverData.userProfile.lastName || '',
        bio: serverData.userProfile.bio || '',
        phone: serverData.userProfile.phone || '',
        email: userStore.email || '',
        experience: serverData.experienceYears || 0,
        age: calculateAge(serverData.userProfile.birthdate),
        country: serverData.userProfile.country || '',
        city: serverData.userProfile.city || '',
        location: `${serverData.userProfile.city || ''}, ${serverData.userProfile.country || ''}`,
        profileImage: serverData.photo || defaultProfileImage, // fallback imagine default
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
        editedProfile.value = JSON.parse(JSON.stringify(profileStore));
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
          await updateSubject({
            subjectId: subject.subjectId,
            subjectName: subject.name,
            subjectSlug: subject.name.toLowerCase().replace(/\s+/g, '-'),
            price: subject.price,
            currency: subject.currency.toLowerCase(),
          });
          subject.isModified = false;
        }
      }

      profileStore.setProfileDetails(editedProfile.value);
      profileStore.toggleEditing();
    } catch (error) {
      console.error('Eroare la salvarea modificărilor profilului:', error);
    }
  };
</script>
