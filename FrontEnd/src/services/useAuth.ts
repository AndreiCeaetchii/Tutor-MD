import { ref } from 'vue';
import { useRouter } from 'vue-router';

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

  // --- URL-urile  ---
  const SIGNUP_URL = 'http://localhost:7122/api/users/register';
  const LOGIN_URL = 'http://localhost:7122/api/users/login';
  const LOGOUT_URL = 'http://localhost:7122/api/users/logout';
  const GOOGLE_LOGIN_URL = 'http://localhost:7122/api/users/login-auth';
  const GOOGLE_REGISTER_URL = 'http://localhost:7122/api/users/register-auth';

  // --- Sign up ---
  const signup = async (formData: AuthFormData) => {
    errorMessage.value = null;
    try {
      const res = await fetch(SIGNUP_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          Email: formData.email,
          Password: formData.password,
          Role: formData.role || 'student',
        }),
        credentials: 'include',
      });

      const data = await res.json();

      if (!res.ok) {
        throw new Error(data.error || 'Registration failed');
      }

      accessToken.value = data.Token;
      currentUser.value = { id: data.Id, email: data.Email };

      console.log('Signup successful!');
    } catch (err: any) {
      console.error('Signup error:', err);
      errorMessage.value = err.message || 'Network or server error';
      throw err;
    }
  };

  // --- Log in ---
  const login = async (formData: AuthFormData): Promise<boolean> => {
    errorMessage.value = null;
    try {
      const res = await fetch(LOGIN_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          Email: formData.email,
          Password: formData.password,
        }),
        credentials: 'include',
      });

      if (!res.ok) {
        const data = await res.json();
        throw new Error(data.error || 'Login failed');
      }

      console.log('Login successful!');
      return true;
    } catch (err: any) {
      console.error('Login error:', err);
      errorMessage.value = err.message || 'Network or server error';
      return false;
    }
  };

  // --- Logout ---
  const logout = async () => {
    try {
      await fetch(LOGOUT_URL, {
        method: 'POST',
      });
      accessToken.value = null;
      currentUser.value = null;
      router.push('/login');
    } catch (err) {
      console.error('Logout error:', err);
    }
  };

  // --- Login sau Signup cu Google ---
  const loginWithGoogle = async (role: string, isSignup: boolean): Promise<boolean> => {
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
            const googleUserRes = await fetch('https://www.googleapis.com/oauth2/v3/userinfo', {
              headers: { Authorization: `Bearer ${response.access_token}` },
            });
            const googleUser = await googleUserRes.json();
            const email = googleUser.email;
            console.log('Google user info:', googleUser);

            //  Alegem endpoint-ul (signup vs login)
            const endpoint = isSignup ? GOOGLE_REGISTER_URL : GOOGLE_LOGIN_URL;

            const res = await fetch(endpoint, {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
              body: JSON.stringify({
                Email: email,
                AccessToken: response.access_token,
                Role: role || 'student',
              }),
              credentials: 'include',
            });

            const data = await res.json();

            if (!res.ok) {
              throw new Error(data.error || 'Google authentication failed');
            }

            // Salvăm user și token
            accessToken.value = data.Token;
            currentUser.value = {
              id: data.Id,
              email: data.Email,
              username: data.Username || null,
              role: role,
            };

            console.log(`${isSignup ? 'Signup' : 'Login'} cu Google reușit!`, data);

            // Redirect după rol
            if (role === 'tutor') router.push('/tutor-dashboard');
            else if (role === 'student') router.push('/student-dashboard');
            else if (role === 'admin') router.push('/admin-dashboard');

            resolve(true);
          } catch (err: any) {
            console.error('Google auth error:', err);
            errorMessage.value = err.message || 'Network or server error';
            resolve(false);
          }
        },
      });

      // 6. Lansăm popup-ul Google
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
