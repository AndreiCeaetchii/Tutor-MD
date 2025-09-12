<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useRouter } from 'vue-router';
  import BaseAuthForm from '../auth/AuthForm.vue';
  import { useAuth } from '../../services/useAuth.ts';
  import Notification from '../ui/NotificationMessage.vue';

  interface SignupFormData {
    email: string;
    password: string;
    role: string;
    phoneNumber?: string;
  }

  const { signup, loginWithGoogle, errorMessage } = useAuth();
  const router = useRouter();

  // Form data
  const formData = ref<SignupFormData>({
    email: '',
    password: '',
    role: '',
    phoneNumber: ''
  });

  // Track which fields have been touched
  const touchedFields = ref<Set<string>>(new Set());

  // Validation rules
  const emailError = computed(() => {
    if (!touchedFields.value.has('email') || !formData.value.email) return '';
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    return emailRegex.test(formData.value.email) ? '' : 'Please enter a valid email address';
  });

  const roleError = computed(() => {
    if (!touchedFields.value.has('role')) return '';
    return formData.value.role ? '' : 'Please select a role';
  });

  // Password validation with multiple rules
  const passwordRules = {
    minLength: (password: string) => password.length >= 8,
    hasUppercase: (password: string) => /[A-Z]/.test(password),
    hasLowercase: (password: string) => /[a-z]/.test(password),
    hasNumber: (password: string) => /\d/.test(password),
    hasSpecialChar: (password: string) => /[!@#$%^&*(),.?":{}|<>]/.test(password)
  };

  const passwordError = computed(() => {
    if (!touchedFields.value.has('password') || !formData.value.password) return '';

    const password = formData.value.password;
    const errors = [];

    if (!passwordRules.minLength(password)) {
      errors.push('At least 8 characters');
    }
    if (!passwordRules.hasUppercase(password)) {
      errors.push('At least one uppercase letter');
    }
    if (!passwordRules.hasLowercase(password)) {
      errors.push('At least one lowercase letter');
    }
    if (!passwordRules.hasNumber(password)) {
      errors.push('At least one number');
    }
    if (!passwordRules.hasSpecialChar(password)) {
      errors.push('At least one special character (!@#$%^&*,.?)');
    }

    return errors.length ? `Password must contain: ${errors.join(', ')}` : '';
  });

  // Phone number validation
  const phoneNumberError = computed(() => {
    if (!touchedFields.value.has('phoneNumber') || !formData.value.phoneNumber) return '';
    
    // Validate the full number with country code
    // First add the country code if it's not already there
    const fullNumber = formData.value.phoneNumber.startsWith('+373') 
      ? formData.value.phoneNumber 
      : `+373${formData.value.phoneNumber}`;
      
    const phoneRegex = /^\+373[0-9]{8,9}$/;
    return phoneRegex.test(fullNumber) 
      ? '' 
      : 'Please enter a valid Moldova phone number (+373 followed by 8-9 digits)';
  });

  // Compile all field errors
  const fieldErrors = computed(() => {
    return {
      email: emailError.value,
      role: roleError.value,
      password: passwordError.value,
      phoneNumber: phoneNumberError.value
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
    else if (field === 'phoneNumber') formData.value.phoneNumber = value;
  };

  // Success notification
  const showSuccessNotification = ref(false);
  const successMessage = ref('Account created successfully!');

  const showSuccess = () => {
    showSuccessNotification.value = true;
    setTimeout(() => {
      showSuccessNotification.value = false;
    }, 3000);
  };

  const handleSubmit = async (data: SignupFormData) => {
    // Mark all fields as touched to trigger validation
    touchedFields.value.add('email');
    touchedFields.value.add('password');
    touchedFields.value.add('role');
    if (data.phoneNumber) touchedFields.value.add('phoneNumber');
    
    // Update our local data
    formData.value = data;
    
    // Check for validation errors
    if (emailError.value || roleError.value || passwordError.value || 
        (data.phoneNumber && phoneNumberError.value)) {
      return; // Don't proceed if there are errors
    }
    
    const success = await signup(data);
    if (!success) return;

    // Show success notification
    showSuccess();

    // Navigate based on role
    if (data.role === 'tutor') {
      router.push('/tutor-dashboard');
    } else if (data.role === 'student') {
      router.push('/student-dashboard');
    } else if (data.role === 'admin') {
      router.push('/admin-dashboard');
    }
  };

  const handleSocialLogin = async ({ provider, role }: { provider: string; role: string }) => {
    touchedFields.value.add('role');
    formData.value.role = role;
    
    if (provider !== 'google') return;
    if (!role) {
      return; // Role validation will show error
    }
    
    const success = await loginWithGoogle(role, true);
    if (!success) return;

    showSuccess();
    
    if (role === 'tutor') router.push('/tutor-dashboard');
    else if (role === 'student') router.push('/student-dashboard');
    else if (role === 'admin') router.push('/admin-dashboard');
  };
</script>

<template>
  <!-- Success notification -->
  <Notification 
    :show="showSuccessNotification"
    :message="successMessage"
    type="success"
    @close="showSuccessNotification = false"
  />

  <BaseAuthForm
    title="Create an account"
    subtitle="Please sign up to continue"
    :showRoleSelector="true"
    :isSignupForm="true"
    :isLogin="false"
    submitButtonText="Sign up as Tutor"
    googleButtonText="Sign up with Google"
    footerText="Already have an account?"
    footerLinkText="Sign in"
    footerLinkPath="/login"
    @submit="handleSubmit"
    @socialLogin="handleSocialLogin"
    @fieldBlur="handleFieldBlur"
    @fieldInput="handleFieldInput"
    :fieldErrors="fieldErrors"
    :errorMessage="errorMessage ?? undefined"
  />
</template>
