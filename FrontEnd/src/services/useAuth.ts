import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useUserStore } from '../store/userStore';
import axios from 'axios';

declare const google: any;

interface AuthFormData {
  email: string;
  password: string;
  role?: string;
}

export function useAuth() {
  const router = useRouter();
  const accessToken = ref<string | null>(null);
  const currentUser = ref<any>(null);
  const errorMessage = ref<string | null>(null);

  const store = useUserStore();

  // --- URL-urile  ---
  const SIGNUP_URL = 'https://localhost:7123/api/users/register';
  const LOGIN_URL = 'https://localhost:7123/api/users/login';
  const GOOGLE_LOGIN_URL = 'https://localhost:7123/api/users/login-auth';
  const GOOGLE_REGISTER_URL = 'https://localhost:7123/api/users/register-auth';

  const handleAuthError = (
    err: any,
    context: 'signup' | 'login' | 'google',
    isSignup?: boolean,
  ): string => {
    if (axios.isAxiosError(err) && err.response) {
      const status = err.response.status;
      const errorData = err.response.data;

      console.error(`${context} error details:`, {
        status,
        data: errorData,
        message: Array.isArray(errorData)
          ? errorData[0]
          : errorData?.error || errorData?.message || 'Unknown error',
      });

      let errorMsg = '';
      if (Array.isArray(errorData)) {
        errorMsg = errorData[0] || '';
      } else if (typeof errorData === 'string') {
        errorMsg = errorData;
      } else {
        errorMsg = errorData?.error || errorData?.message || '';
      }

      if (
        errorMsg.toLowerCase().includes('already exists') ||
        errorMsg.toLowerCase().includes('already registered') ||
        errorMsg.toLowerCase().includes('email already') ||
        errorMsg.toLowerCase().includes('user already') ||
        errorMsg.toLowerCase().includes('oauth provider') ||
        errorMsg.toLowerCase().includes('unique constraint') ||
        errorMsg.toLowerCase().includes('uniqueconstraint')
      ) {
        if (context === 'google') {
          return isSignup
            ? 'An account with this Google email already exists. Please use login instead.'
            : 'Authentication failed with this Google account. Please check your credentials.';
        } else {
          return isSignup
            ? 'An account with this email already exists. Please use login instead.'
            : 'Authentication failed. Please check your email and password.';
        }
      }

      switch (status) {
        case 400:
          return errorMsg || `Invalid ${context} data. Please check your details.`;
        case 401:
          return context === 'login'
            ? 'Incorrect email or password. Please try again.'
            : 'Authentication failed. Please check your credentials.';
        case 404:
          return context === 'login'
            ? 'Account not found. Please check your email or sign up.'
            : `${context.charAt(0).toUpperCase() + context.slice(1)} failed: resource not found`;
        case 409:
          return context === 'google'
            ? 'Google authentication failed. Please try again.'
            : 'A conflict occurred. Please try again.';
        case 500:
          return 'Server error. Please try again later.';
        default:
          return (
            errorMsg || `${context.charAt(0).toUpperCase() + context.slice(1)} failed (${status})`
          );
      }
    } else {
      console.error(`${context} network error:`, err);
      return err.message || 'Temporary server issue. Please retry.';
    }
  };

  const signup = async (formData: AuthFormData): Promise<boolean> => {
    errorMessage.value = null;

    try {
      const response = await axios.post(
        SIGNUP_URL,
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

      // @ts-ignore
      store.setUser(data.token, data.id, formData.role);

      console.log('Signup successful!');
      return true;
    } catch (err: any) {
      errorMessage.value = handleAuthError(err, 'signup');
      return false;
    }
  };

  const login = async (formData: AuthFormData): Promise<boolean> => {
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

      // @ts-ignore
      store.setUser(data.token, data.id, formData.role);

      console.log('Login successful!');
      return true;
    } catch (err: any) {
      errorMessage.value = handleAuthError(err, 'login');
      return false;
    }
  };

  const logout = () => {
    store.accessToken = null;
    router.push('/login');
  };

  const loginWithGoogle = async (role: string, isSignup: boolean): Promise<boolean> => {
    const store = useUserStore();

    return new Promise((resolve) => {
      const tokenClient = google.accounts.oauth2.initTokenClient({
        client_id: '425538151525-bhujljp8s9kn9vffkd0rf1cad6gd1epb.apps.googleusercontent.com',
        scope: 'openid email profile',
        callback: async (response: any) => {
          if (!response.access_token) {
            errorMessage.value = 'Google authentication failed: no access token';
            return resolve(false);
          }

          try {
            // Obținem user info de la Google
            const googleUserRes = await axios.get('https://www.googleapis.com/oauth2/v3/userinfo', {
              headers: { Authorization: `Bearer ${response.access_token}` },
            });
            const googleUser = googleUserRes.data;
            const email = googleUser.email;
            const AccessToken = response.access_token;

            // Alegem endpoint-ul (signup vs login)
            const endpoint = isSignup ? GOOGLE_REGISTER_URL : GOOGLE_LOGIN_URL;

            const res = await axios.post(
              endpoint,
              {
                email: email,
                accessToken: AccessToken,
                provider: 'google',
              },
              { withCredentials: true },
            );

            const data = res.data;

            // Salvăm token, id și rol în store
            // @ts-ignore
            store.setUser(data.token, data.id, role);

            console.log(`${isSignup ? 'Signup' : 'Login'} cu Google reușit!`, data);

            // Redirect după rol
            if (role === 'tutor') router.push('/create-profile');
            else if (role === 'student') router.push('/student-dashboard');
            else if (role === 'admin') router.push('/admin-dashboard');

            resolve(true);
          } catch (err: any) {
            errorMessage.value = handleAuthError(err, 'google', isSignup);
            resolve(false);
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
