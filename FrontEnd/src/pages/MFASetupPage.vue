<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';
import { enableMfa, verifyMfaSetup, disableMfa } from '../services/userService';
import { useUserStore } from '../store/userStore';

const router = useRouter();
const userStore = useUserStore();

const userRole = computed(() => userStore.userRole);
const isMfaEnabled = computed(() => userStore.mfaEnabled);
const mfaStatus = ref('');

const loading = ref(false);
const verifying = ref(false);
const showQrCode = ref(false);
const setupComplete = ref(false);
const qrCodeImage = ref('');
const secretKey = ref('');
const recoveryCodes = ref<string[]>([]);
const verificationCode = ref('');
const verificationError = ref('');
const debugInfo = ref('');

async function startMfaSetup() {
  loading.value = true;
  try {
    const response = await enableMfa();
    qrCodeImage.value = response.qrCodeImage;
    secretKey.value = response.secretKey;
    recoveryCodes.value = response.recoveryCodes;
    showQrCode.value = true;
  } catch (error: any) {
    console.error('Failed to start MFA setup:', error);

  } finally {
    loading.value = false;
  }
}

async function verifyCode() {
  if (verificationCode.value.length !== 6) {
    verificationError.value = 'Please enter a 6-digit code';
    return;
  }
  
  verifying.value = true;
  try {
    await verifyMfaSetup(verificationCode.value);
    setupComplete.value = true;
    showQrCode.value = false;
    mfaStatus.value = 'Two-factor authentication has been successfully enabled for your account.';
    
    userStore.updateUserMfaStatus(true);
  } catch (error: any) {
    console.error('MFA verification error:', error);
    verificationError.value = error.message || 'Invalid verification code. Please try again.';
    if (import.meta.env.DEV) {
      debugInfo.value = JSON.stringify(error, null, 2);
    }
  } finally {
    verifying.value = false;
  }
}

async function disableMfaAuth() {
  loading.value = true;
  try {
    await disableMfa();
    userStore.updateUserMfaStatus(false);
    mfaStatus.value = 'Two-factor authentication has been successfully disabled for your account.';
    setupComplete.value = true;
    recoveryCodes.value = [];
  } catch (error: any) {
    console.error('Failed to disable MFA:', error);
  } finally {
    loading.value = false;
  }
}

function downloadRecoveryCodes() {
  const codesText = recoveryCodes.value.join('\n');
  const blob = new Blob([codesText], { type: 'text/plain' });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = 'tutor-md-recovery-codes.txt';
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
  URL.revokeObjectURL(url);
}

function navigateBasedOnRole() {
  const role = userRole.value;
  if (role === 'tutor') {
    router.push('/tutor-dashboard/profile');
  } else if (role === 'student') {
    router.push('/student-dashboard/account');
  } else if (role === 'admin') {
    router.push('/admin-dashboard/profile');
  } else {
    router.push('/landing');
  }
}

function cancelSetup() {
  navigateBasedOnRole();
}

function navigateToProfile() {
  navigateBasedOnRole();
}
</script>

<template>
  <div class="fixed inset-0 z-50 flex items-center justify-center p-3 overflow-y-auto sm:p-4 backdrop-blur-sm">
    <div class="w-full max-w-3xl mx-auto overflow-hidden shadow-xl sm:max-w-4xl bg-white/90 rounded-2xl">
      <div v-if="loading" class="flex items-center justify-center p-6 sm:p-8">
        <div class="w-10 h-10 border-4 border-t-transparent rounded-full border-[#5f22d9] animate-spin"></div>
      </div>
      
      <div v-else-if="setupComplete" class="p-4 mx-auto sm:p-6 md:max-w-2xl">
        <div class="flex flex-col items-center max-w-lg mx-auto">
          <div class="flex items-center justify-center mb-4 bg-green-100 rounded-full w-14 h-14">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-8 h-8 text-green-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
            </svg>
          </div>
          
          <h2 class="mb-2 text-xl font-bold text-center text-gray-900">
            Setup Complete!
          </h2>
          
          <p class="mb-4 text-base text-center text-gray-600">
            {{ mfaStatus || 'Two-factor authentication has been successfully enabled for your account.' }}
          </p>
        </div>
        
        <div v-if="recoveryCodes.length > 0" class="max-w-3xl p-4 mx-auto mb-5 bg-gray-50 rounded-xl sm:p-5 md:p-6 sm:mb-6">
          <h3 class="mb-2 text-lg font-semibold text-gray-800 sm:text-xl">Recovery Codes</h3>
          
          <p class="mb-4 text-sm text-gray-600">
            Store these recovery codes in a safe place. They can be used to regain access to your account if you lose your authentication device.
          </p>
          
          <div class="grid grid-cols-1 gap-2 mb-4 sm:grid-cols-2 sm:gap-3 sm:mb-5">
            <div v-for="(code, index) in recoveryCodes" :key="index" 
                 class="p-2 font-mono text-sm text-center bg-white border border-gray-200 rounded-lg sm:p-3">
              {{ code }}
            </div>
          </div>
          
          <button @click="downloadRecoveryCodes" 
                  class="w-full py-3 px-4 bg-[#5f22d9] hover:bg-purple-700 text-white text-base font-medium rounded-lg transition-colors duration-200 flex items-center justify-center">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
            </svg>
            Download Recovery Codes
          </button>
        </div>
        
        <div class="px-4 sm:px-6">
          <button @click="navigateToProfile" 
                  class="w-full px-4 py-2.5 text-base font-medium text-white transition-colors bg-gray-800 rounded-lg hover:bg-gray-900">
            Return to Profile
          </button>
        </div>
      </div>
      
      <div v-else-if="showQrCode" class="p-4 sm:p-6 md:p-8">
        <h2 class="mb-4 text-xl font-bold text-center text-gray-900 sm:text-2xl sm:mb-6">
          Set Up Two-Factor Authentication
        </h2>
        
        <div class="flex flex-col gap-4 sm:flex-row sm:gap-6">
          <div class="w-full sm:w-1/2">
            <div class="p-4 rounded-lg bg-gray-50">
              <h3 class="mb-2 text-lg font-semibold text-gray-800">1. Scan QR Code</h3>
              <p class="mb-3 text-sm text-gray-600">
                Use an authenticator app like Google Authenticator, Microsoft Authenticator, or Authy to scan this QR code.
              </p>
              <div class="flex justify-center my-4">
                <img :src="`data:image/png;base64,${qrCodeImage}`" alt="QR Code" 
                     class="object-contain w-auto h-auto max-h-40 sm:max-h-48" />
              </div>
            </div>
            
            <div class="p-4 mt-4 rounded-lg bg-gray-50">
              <h3 class="mb-2 text-lg font-semibold text-gray-800">2. Or Enter Secret Key</h3>
              <p class="mb-3 text-sm text-gray-600">
                If you can't scan the QR code, enter this secret key manually in your authenticator app:
              </p>
              <div class="p-3 font-mono text-sm text-center break-all bg-white border border-gray-200 rounded-lg">
                {{ secretKey }}
              </div>
            </div>
          </div>
          
          <div class="w-full sm:w-1/2">
            <div class="p-4 rounded-lg bg-gray-50">
              <h3 class="mb-2 text-lg font-semibold text-gray-800">3. Enter Verification Code</h3>
              <p class="mb-3 text-sm text-gray-600">
                Enter the 6-digit code from your authenticator app to verify setup:
              </p>
              <input 
                v-model="verificationCode"
                type="text"
                placeholder="Enter 6-digit code"
                maxlength="6"
                pattern="[0-9]*"
                inputmode="numeric"
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#5f22d9] focus:border-transparent"
              />
              <div v-if="verificationError" class="mt-2 text-sm text-red-500">
                {{ verificationError }}
              </div>
            </div>
            
            <div class="flex flex-col gap-3 mt-4 sm:mt-6">
              <button 
                @click="verifyCode" 
                class="w-full px-4 py-3 text-white transition-colors bg-[#5f22d9] rounded-lg hover:bg-purple-700 disabled:bg-purple-300 disabled:cursor-not-allowed"
                :disabled="verificationCode.length !== 6 || verifying"
              >
                <span v-if="verifying" class="flex items-center justify-center">
                  <svg class="w-4 h-4 mr-2 -ml-1 text-white animate-spin" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                    <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                    <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                  </svg>
                  Verifying...
                </span>
                <span v-else>Verify and Enable</span>
              </button>
              <button @click="cancelSetup" class="py-2 text-gray-600 transition-colors hover:text-gray-800">
                Cancel
              </button>
            </div>
          </div>
        </div>

        <div v-if="debugInfo" class="p-4 mt-6 bg-gray-100 rounded-lg">
          <h4 class="mb-2 text-sm font-bold">Debug Information:</h4>
          <pre class="overflow-auto text-xs">{{ debugInfo }}</pre>
        </div>
      </div>
      
      <div v-else class="flex flex-col w-full md:flex-row">
        <div class="hidden md:flex md:w-1/2 bg-[#5f22d9] items-center justify-center p-6 md:p-8">
          <div class="max-w-md text-center text-white">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-20 h-20 mx-auto mb-5 md:w-24 md:h-24 md:mb-6 opacity-90" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
            </svg>
            <h2 class="mb-3 text-xl font-bold md:mb-4 md:text-2xl">Protect Your Account</h2>
            <p class="mb-2 text-sm opacity-90">
              Two-factor authentication adds an essential second layer of security to your account.
            </p>
            <p class="text-sm opacity-90">
              This simple step helps ensure that only you can access your information, even if someone knows your password.
            </p>
          </div>
        </div>
        
        <div class="w-full p-4 sm:p-6 md:w-1/2 md:p-8">
          <div class="flex items-center justify-center mb-4 sm:mb-5 text-[#5f22d9] md:hidden">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-16 h-16" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
            </svg>
          </div>
          
          <div v-if="!isMfaEnabled">
            <h2 class="mb-3 text-xl font-bold text-center text-gray-900 sm:text-2xl">Enhance Your Account Security</h2>
            
            <p class="mb-5 text-sm text-center text-gray-600">
              Two-factor authentication adds an extra layer of security to your account. 
              After enabling, you'll need your phone in addition to your password to log in.
            </p>
            
            <div class="p-4 mb-5 rounded-lg bg-gray-50">
              <h3 class="mb-2 text-lg font-semibold text-gray-800">How it works:</h3>
              <ul class="space-y-2.5 text-sm text-gray-600">
                <li class="flex items-start">
                  <span class="inline-flex items-center justify-center min-w-[1.5rem] h-6 mr-2 text-[#5f22d9] bg-purple-100 rounded-full">1</span>
                  <span>Download an authenticator app like Google Authenticator or Authy</span>
                </li>
                <li class="flex items-start">
                  <span class="inline-flex items-center justify-center min-w-[1.5rem] h-6 mr-2 text-[#5f22d9] bg-purple-100 rounded-full">2</span>
                  <span>Scan the provided QR code with the app</span>
                </li>
                <li class="flex items-start">
                  <span class="inline-flex items-center justify-center min-w-[1.5rem] h-6 mr-2 text-[#5f22d9] bg-purple-100 rounded-full">3</span>
                  <span>Enter the verification code from the app</span>
                </li>
                <li class="flex items-start">
                  <span class="inline-flex items-center justify-center min-w-[1.5rem] h-6 mr-2 text-[#5f22d9] bg-purple-100 rounded-full">4</span>
                  <span>Use the app to generate codes each time you log in</span>
                </li>
              </ul>
            </div>
            
            <button 
              @click="startMfaSetup" 
              class="w-full px-4 py-3 text-white font-medium transition-colors bg-[#5f22d9] rounded-lg hover:bg-purple-700"
            >
              Set Up Two-Factor Authentication
            </button>
          </div>
          
          <div v-else>
            <h2 class="mb-3 text-xl font-bold text-center text-gray-900 sm:text-2xl">Manage Two-Factor Authentication</h2>
            
            <div class="flex items-center p-3 mb-4 rounded-lg bg-green-50">
              <svg class="w-6 h-6 mr-2 text-green-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
              </svg>
              <p class="text-green-700">Two-factor authentication is currently enabled</p>
            </div>
            
            <p class="mb-5 text-sm text-center text-gray-600">
              Your account is protected with an additional layer of security.
              You need to enter a code from your authenticator app when logging in.
            </p>
            
            <button 
              @click="disableMfaAuth" 
              class="w-full px-4 py-3 font-medium text-white transition-colors bg-red-500 rounded-lg hover:bg-red-600"
            >
              Disable Two-Factor Authentication
            </button>
          </div>
          
          <button 
            @click="cancelSetup" 
            class="w-full py-2 mt-3 text-gray-600 transition-colors hover:text-gray-800"
          >
            Cancel
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
