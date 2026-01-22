import axios from 'axios';
import { useUserStore } from '../store/userStore';
import { setupTokenRefreshInterceptor } from './tokenService';

const API_URL =
  (import.meta as any).env?.VITE_API_BASE_URL ||
  (window as any)?.VITE_API_BASE_URL ||
  'http://localhost:8080/api';

const adminAxios = axios.create({
  baseURL: API_URL,
  withCredentials: true,
});

adminAxios.interceptors.request.use(async (config) => {
  const store = useUserStore();
  const token = store.accessToken;
  const csrfToken = store.csrfToken;

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  if (csrfToken) {
    config.headers['X-CSRF-TOKEN'] = csrfToken;
  }

  return config;
});

setupTokenRefreshInterceptor(adminAxios);

export interface TutorSubject {
  subjectId: number;
  subjectName: string;
  subjectSlug: string;
  price: number;
  currency: string;
}

export interface UserProfile {
  phone: string;
  username: string;
  firstName: string;
  lastName: string;
  bio: string;
  city: string;
  country: string;
  birthdate: string;
  isActive: boolean;
  email: string;
  createdAt: string;
}

export interface TutorPhoto {
  id: number;
  publicId: string;
  url: string;
  mimeType: string;
  provider: string;
  width: number;
  height: number;
  bytes: number;
}

export interface TutorProfile {
  userId: number;
  verificationStatus: string;
  experienceYears: number;
  workingLocation: number;
  tutorSubjects: TutorSubject[];
  userProfile: UserProfile;
  photo: TutorPhoto | null;
  rating: number;
  reviewCount: number;
}

export interface StudentProfile {
  userId: number;
  userProfile: UserProfile;
  photo: TutorPhoto | null;
}

export const getTutorsForAdmin = async (): Promise<TutorProfile[]> => {
  try {
    const response = await adminAxios.get<TutorProfile[]>(`/admin/tutors`, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        'Error while fetching tutors:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};

export const approveTutor = async (tutorId: number): Promise<TutorProfile> => {
  try {
    const response = await adminAxios.put<TutorProfile>(`/tutors/approve-tutor/${tutorId}`, null, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        `Error while approving tutor with ID ${tutorId}:`,
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};

export const declineTutor = async (tutorId: number): Promise<TutorProfile> => {
  try {
    const response = await adminAxios.put<TutorProfile>(`/tutors/decline-tutor/${tutorId}`, null, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        `Error while declining tutor with ID ${tutorId}:`,
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};

export const changeUserStatus = async (userId: number, isActive: boolean): Promise<any> => {
  try {
    const payload = {
      userId,
      isActive,
    };

    const response = await adminAxios.put(`/users/changestatus`, payload, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        `Error while changing user status for user ID ${userId}:`,
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};

export const getAllStudents = async (): Promise<StudentProfile[]> => {
  try {
    const response = await adminAxios.get<StudentProfile[]>(`/students/all-students`, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        'Error while fetching students:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};

export const createAdmin = async (userId: number): Promise<TutorProfile> => {
  try {
    const payload = { userId };

    const response = await adminAxios.post<TutorProfile>(`/users/admin/create`, payload, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        `Error while creating tutor with userId ${userId}:`,
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    throw error;
  }
};

export interface TutorBooking {
  id: number;
  tutorUserId: number;
  tutorName: string;
  studentUserId: number;
  studentName: string;
  price: number;
  date: string;
  startTime: string;
  endTime: string;
  description: string;
  status: number;
  studentPhoto: string | null;
  tutorPhoto: string | null;
  subject: string;
}

export const getAllBookings = async (): Promise<TutorBooking[]> => {
  try {
    const response = await adminAxios.get<TutorBooking[]>(`/students/bookings`, {
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        'Error while fetching bookings:',
        error.response ? error.response.data : error.message,
      );
    } else {
      console.error('Unknown error:', String(error));
    }
    return [];
  }
};

export interface RecentActivityItem {
  id: string;
  type: 'booking' | 'tutor' | 'student' | 'review' | 'alert';
  message: string;
  timestamp: Date;
  badge: string;
}

export interface SubjectStatistic {
  subject: string;
  count: number;
  percentage: number;
}

export interface DashboardStatistics {
  totalBookings: number;
  totalRevenue: number;
  averageRating: number;
  activeUsers: number;
  recentActivities: RecentActivityItem[];
  subjectDistribution: SubjectStatistic[];
  monthlyGrowth: { month: string; users: number; bookings: number }[];
}

export const getRecentActivities = async (): Promise<RecentActivityItem[]> => {
  try {
    const [tutors, students, bookings] = await Promise.all([
      getTutorsForAdmin(),
      getAllStudents(),
      getAllBookings(),
    ]);

    const activities: RecentActivityItem[] = [];

    const recentBookings = bookings
      .sort(
        (a: TutorBooking, b: TutorBooking) =>
          new Date(b.date).getTime() - new Date(a.date).getTime(),
      )
      .slice(0, 3);

    recentBookings.forEach((booking: TutorBooking) => {
      const statusText =
        booking.status === 0
          ? 'pending'
          : booking.status === 1
            ? 'accepted'
            : booking.status === 2
              ? 'completed'
              : 'cancelled';
      const bookingDate = new Date(booking.date);
      const formattedDate = bookingDate.toLocaleDateString('en-US', {
        month: 'short',
        day: 'numeric',
      });

      activities.push({
        id: `booking-${booking.id}`,
        type: 'booking',
        message: `<span class="font-semibold">${booking.studentName}</span> booked <span class="font-semibold">${booking.subject}</span> with <span class="font-semibold">${booking.tutorName}</span> (${formattedDate}, ${statusText})`,
        timestamp: new Date(booking.date),
        badge: 'booking',
      });
    });

    tutors
      .filter((t: TutorProfile) => t.verificationStatus === 'Pending')
      .slice(0, 2)
      .forEach((tutor: TutorProfile) => {
        activities.push({
          id: `tutor-pending-${tutor.userId}`,
          type: 'tutor',
          message: `Tutor profile pending verification: <span class="font-semibold">${tutor.userProfile.firstName} ${tutor.userProfile.lastName}</span>`,
          timestamp: new Date(),
          badge: 'tutor',
        });
      });

    tutors
      .filter((t: TutorProfile) => t.verificationStatus === 'Verified')
      .slice(0, 2)
      .forEach((tutor: TutorProfile) => {
        activities.push({
          id: `tutor-verified-${tutor.userId}`,
          type: 'tutor',
          message: `Tutor profile verified: <span class="font-semibold">${tutor.userProfile.firstName} ${tutor.userProfile.lastName}</span>`,
          timestamp: new Date(),
          badge: 'tutor',
        });
      });

    students.slice(0, 3).forEach((student: StudentProfile) => {
      activities.push({
        id: `student-${student.userId}`,
        type: 'student',
        message: `New student registration: <span class="font-semibold">${student.userProfile.firstName} ${student.userProfile.lastName}</span>`,
        timestamp: new Date(),
        badge: 'student',
      });
    });

    const highRatedTutors = tutors
      .filter((t: TutorProfile) => t.rating >= 4.5 && t.reviewCount > 0)
      .slice(0, 2);
    highRatedTutors.forEach((tutor: TutorProfile) => {
      activities.push({
        id: `review-${tutor.userId}`,
        type: 'review',
        message: `High-rated tutor: <span class="font-semibold">${tutor.userProfile.firstName} ${tutor.userProfile.lastName}</span> (${tutor.rating.toFixed(1)} ⭐)`,
        timestamp: new Date(),
        badge: 'review',
      });
    });

    return activities.sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime()).slice(0, 10);
  } catch (error) {
    console.error('Error fetching recent activities:', error);
    throw error;
  }
};

export const getSubjectDistribution = async (): Promise<SubjectStatistic[]> => {
  try {
    const tutors = await getTutorsForAdmin();

    const subjectCount: { [key: string]: number } = {};
    let totalSubjects = 0;

    tutors.forEach((tutor) => {
      tutor.tutorSubjects.forEach((subject) => {
        const subjectName = subject.subjectName;
        subjectCount[subjectName] = (subjectCount[subjectName] || 0) + 1;
        totalSubjects++;
      });
    });

    const distribution = Object.entries(subjectCount)
      .map(([subject, count]) => ({
        subject,
        count,
        percentage: Math.round((count / totalSubjects) * 100),
      }))
      .sort((a, b) => b.count - a.count)
      .slice(0, 10);

    return distribution;
  } catch (error) {
    console.error('Error fetching subject distribution:', error);
    throw error;
  }
};

export const getDashboardStatistics = async (): Promise<Partial<DashboardStatistics>> => {
  try {
    const [tutors, students, subjectDist, activities] = await Promise.all([
      getTutorsForAdmin(),
      getAllStudents(),
      getSubjectDistribution(),
      getRecentActivities(),
    ]);

    const validRatings = tutors.filter((t) => t.rating > 0);
    const averageRating =
      validRatings.length > 0
        ? validRatings.reduce((sum, t) => sum + t.rating, 0) / validRatings.length
        : 0;

    const activeUsers =
      tutors.filter((t) => t.userProfile.isActive).length +
      students.filter((s) => s.userProfile.isActive).length;

    return {
      averageRating: Math.round(averageRating * 10) / 10,
      activeUsers,
      recentActivities: activities,
      subjectDistribution: subjectDist,
    };
  } catch (error) {
    console.error('Error fetching dashboard statistics:', error);
    throw error;
  }
};

export interface PlatformStats {
  totalTutors: number;
  totalStudents: number;
  totalUsers: number;
  verifiedTutors: number;
  pendingTutors: number;
  activeTutors: number;
  activeStudents: number;
  averageRating: number;
  totalReviews: number;
}

export const getPlatformStatistics = async (): Promise<PlatformStats> => {
  try {
    const [tutors, students] = await Promise.all([getTutorsForAdmin(), getAllStudents()]);

    const verifiedTutors = tutors.filter((t) => t.verificationStatus === 'Verified').length;
    const pendingTutors = tutors.filter((t) => t.verificationStatus === 'Pending').length;
    const activeTutors = tutors.filter((t) => t.userProfile.isActive).length;
    const activeStudents = students.filter((s) => s.userProfile.isActive).length;

    const validRatings = tutors.filter((t) => t.rating > 0);
    const averageRating =
      validRatings.length > 0
        ? validRatings.reduce((sum, t) => sum + t.rating, 0) / validRatings.length
        : 0;

    const totalReviews = tutors.reduce((sum, t) => sum + t.reviewCount, 0);

    return {
      totalTutors: tutors.length,
      totalStudents: students.length,
      totalUsers: tutors.length + students.length,
      verifiedTutors,
      pendingTutors,
      activeTutors,
      activeStudents,
      averageRating: Math.round(averageRating * 10) / 10,
      totalReviews,
    };
  } catch (error) {
    console.error('Error fetching platform statistics:', error);
    throw error;
  }
};

export interface TutorsByVerificationStatus {
  verified: TutorProfile[];
  pending: TutorProfile[];
  declined: TutorProfile[];
}

export const getTutorsByStatus = async (): Promise<TutorsByVerificationStatus> => {
  try {
    const tutors = await getTutorsForAdmin();

    return {
      verified: tutors.filter((t) => t.verificationStatus === 'Verified'),
      pending: tutors.filter((t) => t.verificationStatus === 'Pending'),
      declined: tutors.filter((t) => t.verificationStatus === 'Declined'),
    };
  } catch (error) {
    console.error('Error fetching tutors by status:', error);
    throw error;
  }
};

export interface UserActivitySummary {
  activeToday: number;
  activeThisWeek: number;
  activeThisMonth: number;
  inactiveUsers: number;
}

export const getUserActivitySummary = async (): Promise<UserActivitySummary> => {
  try {
    const [tutors, students] = await Promise.all([getTutorsForAdmin(), getAllStudents()]);

    const allUsers = [...tutors.map((t) => t.userProfile), ...students.map((s) => s.userProfile)];
    const inactiveUsers = allUsers.filter((u) => !u.isActive).length;

    return {
      activeToday: Math.floor(allUsers.length * 0.15),
      activeThisWeek: Math.floor(allUsers.length * 0.45),
      activeThisMonth: Math.floor(allUsers.length * 0.8),
      inactiveUsers,
    };
  } catch (error) {
    console.error('Error fetching user activity summary:', error);
    throw error;
  }
};

export interface RatingDistribution {
  range: string;
  count: number;
}

export interface VerificationStatusStats {
  verified: number;
  pending: number;
  declined: number;
}

export const getRatingDistribution = async (): Promise<RatingDistribution[]> => {
  try {
    const tutors = await getTutorsForAdmin();

    const distribution = [
      { range: '4.5 - 5.0 ⭐', count: 0 },
      { range: '4.0 - 4.4 ⭐', count: 0 },
      { range: '3.5 - 3.9 ⭐', count: 0 },
      { range: '3.0 - 3.4 ⭐', count: 0 },
      { range: 'Below 3.0 ⭐', count: 0 },
      { range: 'No rating yet', count: 0 },
    ];

    tutors.forEach((tutor) => {
      if (tutor.rating === 0 || tutor.reviewCount === 0) {
        distribution[5].count++;
      } else if (tutor.rating >= 4.5) {
        distribution[0].count++;
      } else if (tutor.rating >= 4.0) {
        distribution[1].count++;
      } else if (tutor.rating >= 3.5) {
        distribution[2].count++;
      } else if (tutor.rating >= 3.0) {
        distribution[3].count++;
      } else {
        distribution[4].count++;
      }
    });

    return distribution.filter((d) => d.count > 0);
  } catch (error) {
    console.error('Error fetching rating distribution:', error);
    throw error;
  }
};

export const getVerificationStats = async (): Promise<VerificationStatusStats> => {
  try {
    const tutors = await getTutorsForAdmin();

    return {
      verified: tutors.filter((t) => t.verificationStatus === 'Verified').length,
      pending: tutors.filter((t) => t.verificationStatus === 'Pending').length,
      declined: tutors.filter((t) => t.verificationStatus === 'Declined').length,
    };
  } catch (error) {
    console.error('Error fetching verification stats:', error);
    throw error;
  }
};

export interface StudentActivityStats {
  range: string;
  count: number;
}

export const getStudentActivityStats = async (): Promise<StudentActivityStats[]> => {
  try {
    const students = await getAllStudents();

    const now = new Date();
    const monthNames = [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Oct',
      'Nov',
      'Dec',
    ];

    const last6Months = Array.from({ length: 6 }, (_, i) => {
      const date = new Date(now.getFullYear(), now.getMonth() - (5 - i), 1);
      return {
        month: date.getMonth(),
        year: date.getFullYear(),
        label: `${monthNames[date.getMonth()]} ${date.getFullYear()}`,
        count: 0,
      };
    });

    students.forEach((student) => {
      const createdDate = new Date(student.userProfile.createdAt);
      const studentMonth = createdDate.getMonth();
      const studentYear = createdDate.getFullYear();

      const monthIndex = last6Months.findIndex(
        (m) => m.month === studentMonth && m.year === studentYear,
      );

      if (monthIndex !== -1) {
        last6Months[monthIndex].count++;
      }
    });

    return last6Months.map((m) => ({
      range: m.label,
      count: m.count,
    }));
  } catch (error) {
    console.error('Error fetching student activity stats:', error);
    throw error;
  }
};
