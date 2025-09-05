import { ref } from 'vue';
import { useRouter } from 'vue-router';

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

    // --- Sign up ---
    const signup = async (formData: AuthFormData) => {
        errorMessage.value = null;
        try {
            const res = await fetch('http://localhost:3000/api/auth/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    email: formData.email,
                    password: formData.password,
                    role: formData.role || 'student',
                }),
                credentials: 'include', // pentru refresh token cookie
            });

            const data = await res.json();

            if (!res.ok) {
                throw new Error(data.error || 'Registration failed');
            }

            accessToken.value = data.access_token;
            currentUser.value = data.user;

            // Mesaj în consolă pentru succes
            console.log('Signup successful!');

            await router.push('/');
        } catch (err: any) {
            console.error('Signup error:', err);
            errorMessage.value = err.message || 'Network or server error';
            throw err;
        }
    };

    // --- Log in ---
    const login = async (formData: AuthFormData) => {
        errorMessage.value = null;
        try {
            const res = await fetch('http://localhost:3000/api/auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    email: formData.email,
                    password: formData.password,
                }),
                credentials: 'include', // pentru refresh token cookie
            });

            const data = await res.json();

            if (!res.ok) {
                throw new Error(data.error || 'Login failed');
            }

            accessToken.value = data.access_token;
            currentUser.value = data.user;

            console.log('Login successful!');

            await router.push('/');
        } catch (err: any) {
            console.error('Login error:', err);
            errorMessage.value = err.message || 'Network or server error';
            throw err;
        }
    };

    // --- Logout ---
    const logout = async () => {
        try {
            await fetch('http://localhost:3000/api/auth/logout', {
                method: 'POST',
                credentials: 'include', // șterge refresh token cookie
            });
            accessToken.value = null;
            currentUser.value = null;
            router.push('/login');
        } catch (err) {
            console.error('Logout error:', err);
        }
    };

    return {
        accessToken,
        currentUser,
        errorMessage,
        signup,
        logout,
        login,
    };
}
