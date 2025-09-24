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

function decodeJwt(token: string) {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      window
        .atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join(''),
    );
    return JSON.parse(jsonPayload);
  } catch {
    return null;
  }
}

export function useAuth() {
  const router = useRouter();
  const accessToken = ref<string | null>(null);
  const currentUser = ref<any>(null);
  const errorMessage = ref<string | null>(null);

  const store = useUserStore();

  // --- Centralized API base ---
  const API_URL =
    (import.meta as any).env?.VITE_API_BASE_URL ||
    (window as any)?.VITE_API_BASE_URL ||
    'http://localhost:8080/api';

  const SIGNUP_URL = `${API_URL}/users/register`;
  const LOGIN_URL = `${API_URL}/users/login`;
  const GOOGLE_LOGIN_URL = `${API_URL}/users/login-auth`;
  const GOOGLE_REGISTER_URL = `${API_URL}/users/register-auth`;

  const handleAuthError = (
    err: any,
    context: 'signup' | 'login' | 'google',
    isSignup?: boolean,
  ): string => {
    if (err.response) {
      const { status, data } = err.response;
      if (status === 401) return 'Invalid email or password.';
      if (status === 409) return 'Email already in use.';
      if (typeof data === 'string') return data;
      if (typeof data?.message === 'string') return data.message;
    }

    if (context === 'signup') return 'Failed to create account. Please try again.';
    if (context === 'login') return 'This account does not exist. Please sign up first.';
    if (context === 'google') {
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
          RoleId: formData.role === 'tutor' ? 2 : 3,
        },
        { withCredentials: true, headers: { 'Content-Type': 'application/json' } },
      );
      const data = response.data;
      store.setUser(data.token, data.id, formData.role || 'student', formData.email);
      return true;
    } catch (err: any) {
      errorMessage.value = handleAuthError(err, 'signup');
      return false;
    }
  };

  const login = async (formData: {
    email: string;
    password: string;
  }): Promise<{ success: boolean; role?: string }> => {
    errorMessage.value = null;
    try {
      const response = await axios.post(
        LOGIN_URL,
        { Email: formData.email, Password: formData.password },
        { withCredentials: true, headers: { 'Content-Type': 'application/json' } },
      );
      const data = response.data;
      const decoded = decodeJwt(data.token);
      const userRole = decoded?.role?.toLowerCase() || 'student';
      store.setUser(data.token, data.id, userRole, formData.email);
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

  const loginWithGoogle = async (
    isSignup: boolean,
    role?: string,
  ): Promise<{ success: boolean; role?: string }> => {
    const getRoleId = (role: string): number => {
      switch (role?.toLowerCase()) {
        case 'admin':
          return 1;
        case 'tutor':
          return 2;
        default:
          return 3;
      }
    };

    return new Promise((resolve) => {
      const tokenClient = google.accounts.oauth2.initTokenClient({
        client_id: '425538151525-bhujljp8s9kn9vffkd0rf1cad6gd1epb.apps.googleusercontent.com',
        scope: 'openid email profile',
        callback: async (response: any) => {
          if (!response.access_token) {
            errorMessage.value = 'Google authentication failed: no access token.';
            return resolve({ success: false });
          }

          try {
            const googleUserRes = await axios.get('https://www.googleapis.com/oauth2/v3/userinfo', {
              headers: { Authorization: `Bearer ${response.access_token}` },
            });
            const email = googleUserRes.data.email;

            const endpoint = isSignup ? GOOGLE_REGISTER_URL : GOOGLE_LOGIN_URL;
            const requestData: any = {
              email,
              accessToken: response.access_token,
              provider: 'google',
            };
            if (isSignup && role) requestData.roleId = getRoleId(role);

            const res = await axios.post(endpoint, requestData, { withCredentials: true });
            const data = res.data;

            const decoded = decodeJwt(data.token);
            const userRole = decoded?.role?.toLowerCase() || role?.toLowerCase() || 'student';
            store.setUser(data.token, data.id, userRole, email);

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

  return { accessToken, currentUser, errorMessage, signup, logout, login, loginWithGoogle };
}
