<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useAuth } from '../../services/useAuth.ts';
  import { useRouter } from 'vue-router';
  import BaseAuthForm from '../auth/AuthForm.vue';
  import Notification from '../ui/NotificationMessage.vue';

  interface LoginFormData {
    email: string;
    password: string;
    role: string;
  }

  const { login, loginWithGoogle, errorMessage } = useAuth();
  const router = useRouter();

  const formData = ref<LoginFormData>({
    email: '',
    password: '',
    role: ''
  });
  
  const touchedFields = ref<Set<string>>(new Set());

  // Validation logic
  const emailError = computed(() => {
    if (!touchedFields.value.has('email') || !formData.value.email) return '';
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    return emailRegex.test(formData.value.email) ? '' : 'Please enter a valid email address';
  });

  const roleError = computed(() => {
    if (!touchedFields.value.has('role')) return '';
    return formData.value.role ? '' : 'Please select a role';
  });

  const passwordError = computed(() => {
    if (!touchedFields.value.has('password') || formData.value.password) return '';
    return 'Password is required';
  });

  // Compile all field errors
  const fieldErrors = computed(() => {
    return {
      email: emailError.value,
      role: roleError.value,
      password: passwordError.value
    };
  });

  // Handle field events
  const handleFieldBlur = (field: string) => {
    touchedFields.value.add(field);
  };

  const handleFieldInput = (field: string, value: string) => {
    if (field === 'email') formData.value.email = value;
    else if (field === 'password') formData.value.password = value;
    else if (field === 'role') formData.value.role = value;
  };

  // Success notification
  const showSuccessNotification = ref(false);
  // Using the successMessage in the template
  const successMessage = ref('Login successful!');

  const showSuccess = () => {
    successMessage.value = 'Login successful!'; // Setting the message explicitly
    showSuccessNotification.value = true;
    setTimeout(() => {
      showSuccessNotification.value = false;
    }, 3000);
  };

  const handleSubmit = async (data: LoginFormData) => {
    // Mark all fields as touched to trigger validation
    touchedFields.value.add('email');
    touchedFields.value.add('password');
    touchedFields.value.add('role');
    
    // Update our local data
    formData.value = data;
    
    // Check for validation errors
    if (emailError.value || roleError.value || passwordError.value) {
      return; // Don't proceed if there are errors
    }
    
    const success = await login(data);
    if (!success) return; 

    // Show success message
    showSuccess();

    // Navigate based on role
    if (data.role === 'tutor') router.push('/tutor-dashboard');
    else if (data.role === 'student') router.push('/student-dashboard');
    else if (data.role === 'admin') router.push('/admin-dashboard');
  };

  const handleSocialLogin = async ({ provider, role }: { provider: string; role: string }) => {
    touchedFields.value.add('role');
    formData.value.role = role;
    
    if (provider !== 'google') return;
    if (!role) {
      return; // Role validation will show error
    }
    
    const success = await loginWithGoogle(role, false);
    if (!success) return;

    showSuccess();
    
    if (role === 'tutor') router.push('/tutor-dashboard');
    else if (role === 'student') router.push('/student-dashboard');
    else if (role === 'admin') router.push('/admin-dashboard');
  };
</script>

<template>
  <!-- Success notification - now using the successMessage variable -->
  <Notification 
    :show="showSuccessNotification"
    :message="successMessage"
    type="success"
    @close="showSuccessNotification = false"
  />

  <BaseAuthForm
    title="Tutor.md"
    subtitle="Please log in to continue"
    :showRoleSelector="true"
    submitButtonText="Log in as User"
    googleButtonText="Log in with Google"
    footerText="Don't have an account?"
    footerLinkText="Sign up"
    footerLinkPath="/signup"
    :isLogin="true"
    @submit="handleSubmit"
    @socialLogin="handleSocialLogin"
    @fieldBlur="handleFieldBlur"
    @fieldInput="handleFieldInput"
    :fieldErrors="fieldErrors"
    :errorMessage="errorMessage ?? undefined"
  />
</template>
