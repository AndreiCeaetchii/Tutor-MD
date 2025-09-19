import { defineStore } from 'pinia';

export interface Booking {
  id: number;
  studentName: string;
  subject: string;
  status: string;
  date: string;
  time: string;
  duration: string;
  message: string;
  studentImage?: string | null;
}

export const useBookingStore = defineStore('booking', {
  state: () => ({
    bookings: [] as Booking[],
  }),
  actions: {
    setBookings(bookings: Booking[]) {
      this.bookings = bookings;
    },
    updateBookingStatus(id: number, status: string) {
      const booking = this.bookings.find(b => b.id === id);
      if (booking) booking.status = status;
    },
  },
});