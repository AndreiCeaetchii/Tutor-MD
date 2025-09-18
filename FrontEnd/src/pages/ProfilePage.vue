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
  import { ref, onMounted } from 'vue';
  import { useProfileStore } from '../store/profileStore.ts';
  import ProfileHeader from '../components/profile/ProfileHeader.vue';
  import ProfileDetails from '../components/profile/ProfileDetails.vue';
  import ProfileHeaderEdit from '../components/profile/ProfileHeaderEdit.vue';
  import ProfileDetailsEdit from '../components/profile/ProfileDetailsEdit.vue';
  import { getTutorProfile } from '../services/tutorService.ts'; // presupunem că ai funcția asta
  import { useUserStore } from '../store/userStore.ts';

  const profileStore = useProfileStore();
  const userStore = useUserStore();
  const editedProfile = ref({
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
    profileImage: '',
    rating: 0,
    reviews: 0,
    students: 0,
    languages: [],
    subjects: [],
  });

  onMounted(async () => {
    if (!profileStore.firstName) {
      try {
        const userId = userStore.userId ? Number(userStore.userId) : 0;
        const serverData = await getTutorProfile(userId);
        if (!serverData.userProfile.username) {
          window.location.href = '/create-profile';
          return;
        }

        const calculateAge = (birthdate: string | null) => {
          if (!birthdate) return 0; // default dacă nu există
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
          profileImage: serverData.photo || '',
          rating: 0,
          reviews: 0,
          students: 0,
          languages: [], // pentru moment nu avem date despre limbi
          subjects: [
            ...serverData.tutorSubjects.map((s: any) => ({
              name: s.subjectName || '',
              price: s.price || 0,
              currency: s.currency || 'MDL',
            })),
          ],
        };

        // Setăm store-ul doar cu state-ul
        profileStore.setProfileDetails(profileData);

        // Setăm și editedProfile
        editedProfile.value = JSON.parse(JSON.stringify(profileData));
      } catch (error) {
        console.error('Eroare la preluarea profilului:', error);
      }
    } else {
      // Dacă exista deja in profileStore
      editedProfile.value = JSON.parse(JSON.stringify(profileStore));
    }
  });

  const saveChanges = () => {
    profileStore.setProfileDetails({
      firstName: editedProfile.value.firstName,
      lastName: editedProfile.value.lastName,
      bio: editedProfile.value.bio,
      phone: editedProfile.value.phone,
      email: editedProfile.value.email,
      experience: editedProfile.value.experience,
      age: editedProfile.value.age,
      country: editedProfile.value.country,
      city: editedProfile.value.city,
      location: editedProfile.value.location,
      profileImage: editedProfile.value.profileImage,
      rating: editedProfile.value.rating,
      reviews: editedProfile.value.reviews,
      students: editedProfile.value.students,
      languages: editedProfile.value.languages,
      subjects: editedProfile.value.subjects,
    });

    profileStore.toggleEditing();
  };
</script>
