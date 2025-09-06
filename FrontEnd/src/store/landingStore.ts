import { defineStore } from "pinia";
import { ref, computed } from "vue";

export type UserRole = "tutor" | "student" | null;

export const useUserStore = defineStore("user", () => {
    const selectedRole = ref<UserRole>(null);

    // Setează rolul selectat
    const setSelectedRole = (role: UserRole) => {
        selectedRole.value = role;
    };

    // Stiluri dinamice în funcție de rol
    const roleButtonClass = computed(() => {
        return (role: UserRole) => {
            if (selectedRole.value === role) {
                return "border-[#5f22d9] bg-[#5f22d9]/5 text-[#5f22d9]";
            } else {
                return "border-gray-300 text-gray-500 hover:bg-gray-100";
            }
        };
    });

    return {
        selectedRole,
        setSelectedRole,
        roleButtonClass,
    };
});
