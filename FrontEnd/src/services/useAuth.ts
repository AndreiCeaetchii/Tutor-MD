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

  // --- Sign up ---
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
      if (axios.isAxiosError(err) && err.response) {
        errorMessage.value = err.response.data?.error || 'Signup failed';
      } else {
        errorMessage.value = err.message || 'Network or server error';
      }
      console.error('Signup error:', err);
      return false;
    }
  };

  // --- Log in ---
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
      if (axios.isAxiosError(err) && err.response) {
        errorMessage.value = err.response.data?.error || 'Login failed';
      } else {
        errorMessage.value = err.message || 'Network or server error';
      }
      console.error('Login error:', err);
      return false;
    }
  };

  // --- Logout ---
  const logout = () => {
    store.accessToken = null;
    router.push('/login');
  };

  // --- Login sau Signup cu Google ---
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
            if (role === 'tutor') router.push('/tutor-dashboard');
            else if (role === 'student') router.push('/student-dashboard');
            else if (role === 'admin') router.push('/admin-dashboard');

            resolve(true);
          } catch (err: any) {
            if (axios.isAxiosError(err) && err.response) {
              errorMessage.value = err.response.data?.error || 'Google auth failed';
            } else {
              errorMessage.value = err.message || 'Network or server error';
            }
            console.error('Google auth error:', err);
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
