import { ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import { useUserStore } from '../store/userStore';
import { fetchCsrfToken } from './csrfService';

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

  const API_URL =
    (import.meta as any).env?.VITE_API_BASE_URL ||
    (window as any)?.VITE_API_BASE_URL ||
    'http://localhost:8080/api';

  const SIGNUP_URL = `${API_URL}/users/register`;
  const LOGIN_URL = `${API_URL}/users/login`;
  const GOOGLE_LOGIN_URL = `${API_URL}/users/login-auth`;
  const GOOGLE_REGISTER_URL = `${API_URL}/users/register-auth`;
  const LOGOUT_URL = `${API_URL}/users/logout`;

  const handleAuthError = (
    err: any,
    context: 'signup' | 'login' | 'google' | 'mfa',
    isSignup?: boolean,
  ): string => {
    if (err?.response) {
      const { status, data } = err.response;

      if (status === 401) return 'Invalid email or password.';
      if (status === 409) return 'Email already in use.';

      if (typeof data === 'string') return data;

      if (data?.errors && typeof data.errors === 'object') {
        try {
          const messages: string[] = [];
          for (const key of Object.keys(data.errors)) {
            const val = data.errors[key];
            if (Array.isArray(val)) messages.push(...val.filter(Boolean));
            else if (typeof val === 'string') messages.push(val);
          }
          if (messages.length) return messages.join(' ');
        } catch {}
      }

      if (data?.modelState && typeof data.modelState === 'object') {
        const messages: string[] = [];
        for (const key of Object.keys(data.modelState)) {
          const val = data.modelState[key];
          if (Array.isArray(val)) messages.push(...val.filter(Boolean));
          else if (typeof val === 'string') messages.push(val);
        }
        if (messages.length) return messages.join(' ');
      }

      if (typeof data?.message === 'string') return data.message;

      if (Array.isArray(data) && data.length) {
        const strings = data.filter((d: any) => typeof d === 'string');
        if (strings.length) return strings.join(' ');
      }
    }

    if (context === 'signup') return 'Failed to create account. Please try again.';
    if (context === 'login') return 'This account does not exist. Please sign up first.';
    if (context === 'mfa') return 'Invalid verification code. Please try again.';
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
      const userRole = data?.role?.toLowerCase() || formData.role?.toLowerCase() || 'student';
      store.setUser(
        data.token,
        data.id.toString(),
        userRole,
        formData.email,
        data.twoFactorEnabled,
      );

      await fetchCsrfToken();

      return true;
    } catch (err: any) {
      errorMessage.value = handleAuthError(err, 'signup');
      return false;
    }
  };

  const requiresMfa = ref(false);

  const login = async (formData: {
    email: string;
    password: string;
    mfaCode?: string;
  }): Promise<{ success: boolean; role?: string; requiresMfa?: boolean }> => {
    errorMessage.value = null;
    try {
      const requestData: any = {
        Email: formData.email,
        Password: formData.password,
      };

      if (formData.mfaCode) {
        const formattedMfaCode = formData.mfaCode
          .replace(/\s/g, '')
          .substring(0, 6)
          .padStart(6, '0');
        Object.assign(requestData, { MfaCode: formattedMfaCode });
      }

      const response = await axios.post(LOGIN_URL, requestData, {
        withCredentials: true,
        headers: { 'Content-Type': 'application/json' },
      });

      const data = response.data;

      if (
        data.requiresMfa === true ||
        data.message === 'MFA_REQUIRED' ||
        (Array.isArray(data) && data.includes('MFA_REQUIRED')) ||
        (typeof data === 'string' && data.includes('MFA_REQUIRED')) ||
        JSON.stringify(data).includes('MFA_REQUIRED')
      ) {
        requiresMfa.value = true;
        sessionStorage.setItem('mfa_email', formData.email);
        return { success: false, requiresMfa: true };
      }

      const decoded = decodeJwt(data.token);
      const userRole = decoded?.role?.toLowerCase() || 'student';
      store.setUser(data.token, data.id, userRole, formData.email, data.twoFactorEnabled);

      await fetchCsrfToken();

      requiresMfa.value = false;
      return { success: true, role: userRole };
    } catch (err: any) {
      if (
        err.response?.data?.requiresMfa === true ||
        err.response?.data?.message === 'MFA_REQUIRED' ||
        err.response?.data?.detail === 'MFA_REQUIRED' ||
        (Array.isArray(err.response?.data) && err.response.data.includes('MFA_REQUIRED')) ||
        (err.response?.status === 401 &&
          JSON.stringify(err.response?.data).includes('MFA_REQUIRED'))
      ) {
        requiresMfa.value = true;
        sessionStorage.setItem('mfa_email', formData.email);
        return { success: false, requiresMfa: true };
      }

      errorMessage.value = handleAuthError(err, 'login');
      return { success: false };
    }
  };

  const verifyMfaLogin = async (code: string): Promise<{ success: boolean; role?: string }> => {
    errorMessage.value = null;
    try {
      const email = sessionStorage.getItem('mfa_email');
      if (!email) {
        errorMessage.value = 'Session expired. Please login again.';
        return { success: false };
      }

      const formattedMfaCode = code.replace(/\s/g, '').substring(0, 6).padStart(6, '0');

      const response = await axios.post(
        LOGIN_URL,
        { Email: email, MfaCode: formattedMfaCode },
        { withCredentials: true, headers: { 'Content-Type': 'application/json' } },
      );

      const data = response.data;
      const decoded = decodeJwt(data.token);
      const userRole = decoded?.role?.toLowerCase() || 'student';
      store.setUser(data.token, data.id, userRole, email, data.twoFactorEnabled || true);

      await fetchCsrfToken();

      requiresMfa.value = false;
      sessionStorage.removeItem('mfa_email');
      return { success: true, role: userRole };
    } catch (err: any) {
      errorMessage.value = handleAuthError(err, 'mfa');
      return { success: false };
    }
  };

  const logout = async () => {
    try {
      await axios.post(
        LOGOUT_URL,
        {},
        {
          withCredentials: true,
          headers: {
            Authorization: `Bearer ${store.accessToken}`,
          },
        },
      );
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      store.clearUser();
      accessToken.value = null;
      currentUser.value = null;
      router.push('/login');
    }
  };

  const loginWithGoogle = async (
    isSignup: boolean,
    role?: string,
    mfaCode?: string,
  ): Promise<{ success: boolean; role?: string; requiresMfa?: boolean }> => {
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

    if (mfaCode) {
      const savedAccessToken = sessionStorage.getItem('mfa_google_token');
      const savedEmail = sessionStorage.getItem('mfa_email');

      if (!savedAccessToken || !savedEmail) {
        errorMessage.value = 'Session expired. Please login again.';
        return { success: false };
      }

      const endpoint = isSignup ? GOOGLE_REGISTER_URL : GOOGLE_LOGIN_URL;
      const requestData: any = {
        email: savedEmail,
        accessToken: savedAccessToken,
        provider: 'google',
        MfaCode: mfaCode,
      };

      if (isSignup && role) requestData.roleId = getRoleId(role);

      try {
        const res = await axios.post(endpoint, requestData, { withCredentials: true });
        const data = res.data;

        const decoded = decodeJwt(data.token);
        const userRole = decoded?.role?.toLowerCase() || role?.toLowerCase() || 'student';
        store.setUser(data.token, data.id, userRole, savedEmail, data.twoFactorEnabled);

        await fetchCsrfToken();

        requiresMfa.value = false;
        sessionStorage.removeItem('mfa_email');
        sessionStorage.removeItem('mfa_google_token');

        return { success: true, role: userRole };
      } catch (err: any) {
        errorMessage.value = handleAuthError(err, 'mfa');
        return { success: false };
      }
    }

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

            try {
              const res = await axios.post(endpoint, requestData, { withCredentials: true });
              const data = res.data;

              if (
                data.requiresMfa === true ||
                data.message === 'MFA_REQUIRED' ||
                (Array.isArray(data) && data.includes('MFA_REQUIRED')) ||
                (typeof data === 'string' && data.includes('MFA_REQUIRED')) ||
                JSON.stringify(data).includes('MFA_REQUIRED')
              ) {
                requiresMfa.value = true;
                sessionStorage.setItem('mfa_email', email);
                sessionStorage.setItem('mfa_google_token', response.access_token);
                return resolve({ success: false, requiresMfa: true });
              }

              const decoded = decodeJwt(data.token);
              const userRole = decoded?.role?.toLowerCase() || role?.toLowerCase() || 'student';
              store.setUser(data.token, data.id, userRole, email, data.twoFactorEnabled);

              await fetchCsrfToken();

              resolve({ success: true, role: userRole });
            } catch (err: any) {
              if (
                err.response?.data?.requiresMfa === true ||
                err.response?.data?.message === 'MFA_REQUIRED' ||
                err.response?.data?.detail === 'MFA_REQUIRED' ||
                (Array.isArray(err.response?.data) && err.response.data.includes('MFA_REQUIRED')) ||
                (err.response?.status === 401 &&
                  JSON.stringify(err.response?.data).includes('MFA_REQUIRED'))
              ) {
                requiresMfa.value = true;
                sessionStorage.setItem('mfa_email', email);
                sessionStorage.setItem('mfa_google_token', response.access_token);
                return resolve({ success: false, requiresMfa: true });
              }

              errorMessage.value = handleAuthError(err, 'google', isSignup);
              resolve({ success: false });
            }
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
    requiresMfa,
    signup,
    logout,
    login,
    loginWithGoogle,
    verifyMfaLogin,
  };
}
