import { defineStore } from 'pinia';

export interface Booking {
  id: number;
  studentName: string;
  subject: string;
  status: string;
  date: string;
  startTime: string;
  endTime: string;
  duration?: string;
  message: string;
  studentImage?: string | null;
  tutorName?: string;
  tutorId?: number;
  studentId?: number;
  description?: string;
}

export interface TimeSlot {
  id: number;
  startTime: string;
  endTime: string;
  isAvailable: boolean;
}

const calculateDuration = (startTime: string, endTime: string): string => {
  const [startHour, startMinute] = startTime.split(':').map(Number);
  const [endHour, endMinute] = endTime.split(':').map(Number);
  
  let durationMinutes = (endHour * 60 + endMinute) - (startHour * 60 + startMinute);
  if (durationMinutes < 0) durationMinutes += 24 * 60;
  
  if (durationMinutes % 60 === 0) {
    return `${durationMinutes / 60} ore`;
  } else {
    return `${Math.floor(durationMinutes / 60)} ore ${durationMinutes % 60} minute`;
  }
};

export const useBookingStore = defineStore('booking', {
  state: () => ({
    bookings: [] as Booking[],
    
    studentBookings: [] as Booking[],
    
    availableTimeSlots: [] as TimeSlot[],
    
    loading: false,
    error: null as string | null,
  }),
  
  actions: {
    setBookings(bookings: Booking[]) {
      this.bookings = bookings;
    },
    
    updateBookingStatus(id: number, status: string) {
      const booking = this.bookings.find(b => b.id === id);
      if (booking) booking.status = status;

      const studentBooking = this.studentBookings.find(b => b.id === id);
      if (studentBooking) studentBooking.status = status;
    },
    
    fetchStudentBookings(studentId: number) {
      this.loading = true;
      
      // Mock data for student bookings
      this.studentBookings = [
        {
          id: 1,
          studentName: "Current Student",
          tutorName: "Alexandru Munteanu",
          subject: "Matematică",
          status: "confirmed",
          date: "2025-09-25",
          startTime: "14:00",
          endTime: "15:00",
          duration: "1 oră",
          message: "Am nevoie de ajutor cu exercițiile de trigonometrie.",
          tutorId: 101
        },
        {
          id: 2,
          studentName: "Current Student",
          tutorName: "Maria Popescu",
          subject: "Fizică",
          status: "pending",
          date: "2025-09-26",
          startTime: "10:00",
          endTime: "11:30",
          duration: "1 oră 30 minute",
          message: "Aș dori să clarific conceptele de mecanică cuantică.",
          tutorId: 102
        },
        {
          id: 3,
          studentName: "Current Student",
          tutorName: "Ion Ionescu",
          subject: "Informatică",
          status: "cancelled",
          date: "2025-09-20",
          startTime: "16:00",
          endTime: "17:00",
          duration: "1 oră",
          message: "Am nevoie de ajutor cu algoritmii de sortare.",
          tutorId: 103
        },
        {
          id: 4,
          studentName: "Current Student",
          tutorName: "Elena Codreanu",
          subject: "Chimie",
          status: "completed",
          date: "2025-09-15",
          startTime: "11:30",
          endTime: "12:30",
          duration: "1 oră",
          message: "Am nevoie de ajutor cu reacțiile organice.",
          tutorId: 104
        }
      ];
      
      this.loading = false;
    },
    
    getAvailableTimeSlots(tutorId: number, date: string) {
      this.availableTimeSlots = [
        { id: 1, startTime: "09:00", endTime: "10:00", isAvailable: true },
        { id: 2, startTime: "10:00", endTime: "11:00", isAvailable: true },
        { id: 3, startTime: "11:00", endTime: "12:00", isAvailable: false },
        { id: 4, startTime: "13:00", endTime: "14:00", isAvailable: true },
        { id: 5, startTime: "14:00", endTime: "15:00", isAvailable: true },
        { id: 6, startTime: "15:00", endTime: "16:00", isAvailable: false },
        { id: 7, startTime: "16:00", endTime: "17:00", isAvailable: true }
      ].filter(slot => slot.isAvailable);
      
      return this.availableTimeSlots;
    },
    
    createBooking(bookingData: Partial<Booking> & { time?: string }) {
      let startTime = bookingData.startTime || "";
      let endTime = bookingData.endTime || "";
      
      if (bookingData.time && bookingData.time.includes(' - ')) {
        const [start, end] = bookingData.time.split(' - ');
        startTime = start;
        endTime = end;
      }
      
      const duration = startTime && endTime ? calculateDuration(startTime, endTime) : "";
      
      const newBooking: Booking = {
        id: Math.floor(Math.random() * 1000) + 10,
        studentName: "Current Student",
        tutorName: bookingData.tutorName || "Selected Tutor",
        subject: bookingData.subject || "",
        status: "pending",
        date: bookingData.date || "",
        startTime,
        endTime,
        duration,
        message: bookingData.message || bookingData.description || "",
        tutorId: bookingData.tutorId,
        studentId: bookingData.studentId
      };
      
      this.studentBookings.unshift(newBooking);
      return newBooking;
    },
    
    cancelBooking(bookingId: number) {
      const index = this.studentBookings.findIndex(b => b.id === bookingId);
      if (index !== -1) {
        this.studentBookings[index].status = 'cancelled';
      }
    }
  },
});
