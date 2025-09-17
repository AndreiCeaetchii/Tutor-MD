import { ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import { useUserStore } from '../store/userStore';

declare const google: any;

interface AuthFormData {
  email: string;
  password: string;
  role?: string;
}

// Helper function to decode JWT tokens
function decodeJwt(token: string) {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    
    return JSON.parse(jsonPayload);
  } catch (error) {
    console.error('Error decoding JWT token:', error);
    return null;
  }
}

export function useAuth() {
  const router = useRouter();
  const accessToken = ref<string | null>(null);
  const currentUser = ref<any>(null);
  const errorMessage = ref<string | null>(null);

  const store = useUserStore();

  // --- API Endpoints ---
  const SIGNUP_URL = 'https://localhost:7123/api/users/register';
  const LOGIN_URL = 'https://localhost:7123/api/users/login';
  const GOOGLE_LOGIN_URL = 'https://localhost:7123/api/users/login-auth';
  const GOOGLE_REGISTER_URL = 'https://localhost:7123/api/users/register-auth';

  const handleAuthError = (
    err: any,
    context: 'signup' | 'login' | 'google',
    isSignup?: boolean,
  ): string => {
    console.error(`${context} error:`, err);
    
    if (err.response) {
      const status = err.response.status;
      const data = err.response.data;
      
      if (status === 401) {
        return 'Invalid email or password';
      } else if (status === 409) {
        return 'Email already in use';
      } else if (data && typeof data === 'string') {
        return data;
      } else if (data && typeof data.message === 'string') {
        return data.message;
      }
    }
    
    if (context === 'signup') {
      return 'Failed to create account. Please try again.';
    } else if (context === 'login') {
      return 'Failed to log in. Please check your credentials.';
    } else if (context === 'google') {
      return isSignup 
        ? 'Failed to sign up with Google. Please try again.' 
        : 'Failed to log in with Google. Please try again.';
    }
    
    return 'An unexpected error occurred. Please try again.';
  };

  const signup = async (formData: AuthFormData): Promise<boolean> => {
    errorMessage.value = null;
    
    try {
      const response = await axios.post(
        SIGNUP_URL,
        {
          Email: formData.email,
          Password: formData.password,
          RoleId: formData.role === 'tutor' ? 2 : 3, // 2 for tutor, 3 for student
        },
        {
          withCredentials: true,
          headers: {
            'Content-Type': 'application/json',
          },
        }
      );
      
      const data = response.data;
      // @ts-ignore
      store.setUser(data.token, data.id, formData.role || 'student');
      
      console.log('Signup successful!');
      return true;
    } catch (err: any) {
      errorMessage.value = handleAuthError(err, 'signup');
      return false;
    }
  };

  const login = async (formData: { email: string; password: string }): Promise<{ success: boolean, role?: string }> => {
    errorMessage.value = null;

    try {
      const response = await axios.post(
        LOGIN_URL,
        {
          Email: formData.email,
          Password: formData.password,
        },
        {
          withCredentials: true,
          headers: {
            'Content-Type': 'application/json',
          },
        },
      );

      const data = response.data;
      
      const decoded = decodeJwt(data.token);
      const userRole = decoded?.role?.toLowerCase() || 'student';
      
      console.log('Login response data:', data);
      console.log('JWT decoded:', decoded);
      console.log('User role from token:', userRole);

      // @ts-ignore
      store.setUser(data.token, data.id, userRole);
      
      console.log('Store user role after setting:', store.userRole);
      console.log('Login successful!');
      return { success: true, role: userRole };
    } catch (err: any) {
      errorMessage.value = handleAuthError(err, 'login');
      return { success: false };
    }
  };

  const logout = () => {
    store.clearUser();
    accessToken.value = null;
    currentUser.value = null;
    router.push('/login');
  };

  const loginWithGoogle = async (isSignup: boolean, role?: string): Promise<{ success: boolean, role?: string }> => {
    const store = useUserStore();
  
    const getRoleId = (role: string): number => {
      switch (role?.toLowerCase()) {
        case 'admin': return 1;
        case 'tutor': return 2;
        case 'student': return 3;
        default: return 3; // Default to student if unknown
      }
    };
  
    return new Promise((resolve) => {
      const tokenClient = google.accounts.oauth2.initTokenClient({
        client_id: '425538151525-bhujljp8s9kn9vffkd0rf1cad6gd1epb.apps.googleusercontent.com',
        scope: 'openid email profile',
        callback: async (response: any) => {
          if (!response.access_token) {
            errorMessage.value = 'Google authentication failed: no access token';
            return resolve({ success: false });
          }
  
          try {
            // Get user info from Google
            const googleUserRes = await axios.get('https://www.googleapis.com/oauth2/v3/userinfo', {
              headers: { Authorization: `Bearer ${response.access_token}` },
            });
            const googleUser = googleUserRes.data;
            const email = googleUser.email;
            const AccessToken = response.access_token;
  
            // Choose the endpoint (signup vs login)
            const endpoint = isSignup ? GOOGLE_REGISTER_URL : GOOGLE_LOGIN_URL;
  
            // Prepare request data
            const requestData: any = {
              email: email,
              accessToken: AccessToken,
              provider: 'google',
            };
            
            // Only include roleId if it's a signup request
            if (isSignup && role) {
              requestData.roleId = getRoleId(role);
            }
  
            const res = await axios.post(
              endpoint,
              requestData,
              { withCredentials: true },
            );
  
            const data = res.data;
            console.log('Google auth response data:', data);
            
            // Extract role from JWT token
            const decoded = decodeJwt(data.token);
            const userRole = decoded?.role?.toLowerCase() || (role?.toLowerCase() || 'student');
            
            console.log('JWT decoded:', decoded);
            console.log('User role from token:', userRole);
  
            // @ts-ignore
            store.setUser(data.token, data.id, userRole);
            
            console.log('Store user role after Google login:', store.userRole);
            console.log(`${isSignup ? 'Signup' : 'Login'} with Google successful!`);
            
            resolve({ success: true, role: userRole });
          } catch (err: any) {
            errorMessage.value = handleAuthError(err, 'google', isSignup);
            resolve({ success: false });
          }
        },
      });
  
      tokenClient.requestAccessToken();
    });
  };

  return {
    accessToken,
    currentUser,
    errorMessage,
    signup,
    logout,
    login,
    loginWithGoogle,
  };
}
