import { defineStore } from 'pinia';
import type { BookingRequest } from '../services/studentBookings';
import { 
  getStudentBookings as getStudentBookingsAPI, 
  createBooking as createBookingAPI, 
  cancelStudentBooking 
} from '../services/studentBookings';

export interface Booking {
  id: number;
  studentName: string;
  subject: string;
  status: string;
  date: string;
  startTime?: string;
  endTime?: string;
  time?: string;
  duration?: string;
  message: string;
  studentImage?: string | null;
  tutorName?: string;
  tutorId?: number;
  studentId?: number;
  description?: string;
  tutorImage?: string | null;
}

export interface TimeSlot {
  id: number;
  startTime: string;
  endTime: string;
  isAvailable: boolean;
}

// Helper function to convert numeric status to string
const mapApiStatusToString = (statusCode: number): string => {
  switch (statusCode) {
    case 0: return 'Pending';
    case 1: return 'Accepted';
    case 2: return 'Completed';
    case 3: return 'Cancelled';
    default: return 'Unknown';
  }
};

const calculateDuration = (startTime: string, endTime: string): string => {
  if (!startTime || !endTime) return '';
  
  const [startHour, startMinute] = startTime.split(':').map(Number);
  const [endHour, endMinute] = endTime.split(':').map(Number);
  
  let durationMinutes = (endHour * 60 + endMinute) - (startHour * 60 + startMinute);
  if (durationMinutes < 0) durationMinutes += 24 * 60;
  
  if (durationMinutes % 60 === 0) {
    return `${durationMinutes / 60} hours`;
  } else {
    return `${Math.floor(durationMinutes / 60)} hours ${durationMinutes % 60} minutes`;
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
    
    async fetchStudentBookings(_studentId?: number) {
      this.loading = true;
      this.error = null;
      
      try {
        const apiBookings = await getStudentBookingsAPI();
        
        // Transform API bookings to match our Booking interface
        const transformedBookings: Booking[] = apiBookings.map(booking => {
          return {
            id: booking.id,
            studentName: "Current Student",
            tutorName: booking.tutorName,
            subject: booking.subjectName,
            status: mapApiStatusToString(booking.status),
            date: booking.date,
            startTime: booking.startTime.substring(0, 5),
            endTime: booking.endTime.substring(0, 5),
            time: booking.startTime.substring(0, 5), // Pentru compatibilitate
            duration: calculateDuration(booking.startTime.substring(0, 5), booking.endTime.substring(0, 5)),
            message: booking.description,
            tutorImage: booking.tutorPhoto,
            tutorId: booking.tutorUserId,
          };
        });
        
        this.studentBookings = transformedBookings;
        console.log('Student bookings loaded:', this.studentBookings.length);
        
      } catch (error) {
        this.error = error instanceof Error ? error.message : 'Failed to fetch bookings';
        console.error('Error fetching student bookings:', error);
      } finally {
        this.loading = false;
      }
    },
    
    getAvailableTimeSlots(tutorId: number, date: string) {
      // Keep mock implementation for now, or replace with API call if available
      this.availableTimeSlots = [
        { id: 1, startTime: "09:00", endTime: "10:00", isAvailable: true },
        { id: 2, startTime: "10:00", endTime: "11:00", isAvailable: true },
        { id: 3, startTime: "11:00", endTime: "12:00", isAvailable: false },
        { id: 4, startTime: "13:00", endTime: "14:00", isAvailable: true },
        { id: 5, startTime: "14:00", endTime: "15:00", isAvailable: true },
        { id: 6, startTime: "15:00", endTime: "16:00", isAvailable: false },
        { id: 7, startTime: "16:00", endTime: "17:00", isAvailable: true }
      ].filter(slot => slot.isAvailable);

      console.log(`Getting available slots for tutor ${tutorId} on ${date}`);
      
      return this.availableTimeSlots;
    },
    
    async createBooking(bookingData: any) {
      this.loading = true;
      this.error = null;
      
      try {
        // Create the API booking request
        const bookingRequest: BookingRequest = {
          tutorUserId: bookingData.tutorId,
          subjectId: bookingData.subjectId, 
          availabilityRuleId: bookingData.timeSlotId || 1,
          description: bookingData.message || bookingData.description || '',
          status: 'Pending'
        };
        
        // Call the API to create the booking
        const response = await createBookingAPI(bookingRequest);
        
        // Create a booking object for the UI
        const newBooking: Booking = {
          id: response.id || Date.now(),
          studentName: "Current Student",
          tutorName: bookingData.tutorName || "Selected Tutor",
          subject: bookingData.subject || "",
          status: "Pending",
          date: bookingData.date || "",
          startTime: bookingData.startTime || "",
          endTime: bookingData.endTime || "",
          time: bookingData.startTime || "", // Pentru compatibilitate
          duration: calculateDuration(bookingData.startTime || "", bookingData.endTime || ""),
          message: bookingData.message || bookingData.description || "",
          tutorId: bookingData.tutorId,
          studentId: bookingData.studentId
        };
        
        // Add to local store for immediate UI update
        this.studentBookings.unshift(newBooking);
        
        return newBooking;
      } catch (error) {
        this.error = error instanceof Error ? error.message : 'Failed to create booking';
        console.error('Error creating booking:', error);
        throw error;
      } finally {
        this.loading = false;
      }
    },
    
    async cancelBooking(bookingId: number) {
      this.loading = true;
      this.error = null;
      
      try {
        // Call the API to cancel the booking
        await cancelStudentBooking(bookingId);
        
        // Update the local state
        const index = this.studentBookings.findIndex(b => b.id === bookingId);
        if (index !== -1) {
          this.studentBookings[index].status = 'Cancelled';
        }
      } catch (error) {
        this.error = error instanceof Error ? error.message : 'Failed to cancel booking';
        console.error('Error canceling booking:', error);
        throw error;
      } finally {
        this.loading = false;
      }
    }
  },
});