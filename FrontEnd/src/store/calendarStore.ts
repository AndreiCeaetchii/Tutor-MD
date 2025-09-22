import { defineStore } from 'pinia';
import {
  createAvailability,
  getTutorAvailability,
  updateAvailability,
  deleteAvailability,
  type AvailabilitySlot,
} from '../services/tutorAvailability';
import { useUserStore } from './userStore';

export interface TimeSlot {
  id: string;
  apiId?: number;
  startTime: string;
  endTime: string;
  date?: string;
  status: 'available' | 'booked';
  studentName?: string;
  isEditedPastSlot?: boolean;
  activeStatus?: boolean;
}

interface MonthData {
  daysWithSlots: number[];
  daysWithEditedPastSlots?: number[];
  slotData: Record<number, TimeSlot[]>;
}

export const useCalendarStore = defineStore('calendar', {
  state: () => ({
    slotsByMonth: {} as Record<string, MonthData>,
    selectedDate: new Date(),
    loading: false,
    error: null as string | null,
    tutorsWithNoAvailability: [] as number[],
  }),

  getters: {
    currentDay(): number {
      const date =
        this.selectedDate instanceof Date ? this.selectedDate : new Date(this.selectedDate);
      return date.getDate();
    },

    currentMonth(): number {
      const date =
        this.selectedDate instanceof Date ? this.selectedDate : new Date(this.selectedDate);
      return date.getMonth();
    },

    currentYear(): number {
      const date =
        this.selectedDate instanceof Date ? this.selectedDate : new Date(this.selectedDate);
      return date.getFullYear();
    },

    currentMonthKey(): string {
      return `${this.currentYear}-${this.currentMonth}`;
    },

    formattedDate(): string {
      const date =
        this.selectedDate instanceof Date ? this.selectedDate : new Date(this.selectedDate);

      const month = date.getMonth() + 1;
      const day = date.getDate();
      const year = date.getFullYear();
      return `${day.toString().padStart(2, '0')}.${month.toString().padStart(2, '0')}.${year}`;
    },

    currentMonthData(): MonthData {
      return (
        this.slotsByMonth[this.currentMonthKey] || {
          daysWithSlots: [],
          daysWithEditedPastSlots: [],
          slotData: {},
        }
      );
    },

    daysWithSlots(): number[] {
      return this.currentMonthData.daysWithSlots || [];
    },

    daysWithEditedSlots(): number[] {
      return this.currentMonthData.daysWithEditedPastSlots || [];
    },

    slotData(): Record<number, TimeSlot[]> {
      return this.currentMonthData.slotData || {};
    },

    hasSlots(): boolean {
      return this.daysWithSlots.includes(this.currentDay);
    },

    slots(): TimeSlot[] {
      if (!this.hasSlots || !this.slotData[this.currentDay]) {
        return [];
      }
      return this.slotData[this.currentDay];
    },
  },

  actions: {
    hasTutorWithNoAvailability(tutorId: number): boolean {
      return this.tutorsWithNoAvailability.includes(tutorId);
    },

    addTutorWithNoAvailability(tutorId: number): void {
      if (!this.tutorsWithNoAvailability.includes(tutorId)) {
        this.tutorsWithNoAvailability.push(tutorId);
      }
    },

    removeTutorWithNoAvailability(tutorId: number): void {
      const index = this.tutorsWithNoAvailability.indexOf(tutorId);
      if (index !== -1) {
        this.tutorsWithNoAvailability.splice(index, 1);
      }
    },

    getCurrentMonthStatistics() {
      const monthData = this.currentMonthData;

      let availableSlots = 0;
      let bookedSlots = 0;
      let activeDays = 0;

      if (monthData && monthData.daysWithSlots) {
        activeDays = monthData.daysWithSlots.length;

        Object.values(monthData.slotData).forEach((daySlots) => {
          daySlots.forEach((slot) => {
            if (slot.status === 'available') {
              availableSlots++;
            } else if (slot.status === 'booked') {
              bookedSlots++;
            }
          });
        });
      }

      return {
        availableSlots,
        bookedSlots,
        activeDays,
      };
    },

    timeToMinutes(time: string): number {
      const [hours, minutes] = time.split(':').map(Number);
      return hours * 60 + minutes;
    },

    initializeMonthData() {
      if (!this.slotsByMonth[this.currentMonthKey]) {
        this.slotsByMonth[this.currentMonthKey] = {
          daysWithSlots: [],
          daysWithEditedPastSlots: [],
          slotData: {},
        };
      }
      return this.slotsByMonth[this.currentMonthKey];
    },

    isPastDate(): boolean {
      const today = new Date();
      today.setHours(0, 0, 0, 0);

      const selectedDate =
        this.selectedDate instanceof Date ? this.selectedDate : new Date(this.selectedDate);
      selectedDate.setHours(0, 0, 0, 0);

      return selectedDate < today;
    },

    checkOverlap(newSlot: { startTime: string; endTime: string }, excludeId?: string): boolean {
      const day = this.currentDay;
      const monthData = this.currentMonthData;

      if (!monthData.slotData[day] || monthData.slotData[day].length === 0) {
        return false;
      }

      const newStart = this.timeToMinutes(newSlot.startTime);
      const newEnd = this.timeToMinutes(newSlot.endTime);

      return monthData.slotData[day].some((existingSlot) => {
        if (excludeId && existingSlot.id === excludeId) {
          return false;
        }

        const existingStart = this.timeToMinutes(existingSlot.startTime);
        const existingEnd = this.timeToMinutes(existingSlot.endTime);

        return newStart < existingEnd && newEnd > existingStart;
      });
    },

    setSelectedDate(date: Date | string) {
      this.selectedDate = typeof date === 'string' ? new Date(date) : date;
    },

    formatDateForAPI(date: Date): string {
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const day = String(date.getDate()).padStart(2, '0');
      return `${year}-${month}-${day}`;
    },

    formatTimeForAPI(time: string): string {
      return `${time}:00`;
    },

    async fetchAvailability() {
      try {
        this.loading = true;
        this.error = null;
        const userStore = useUserStore();
        const userId = Number(userStore.userId);

        if (!userId) {
          throw new Error('User not authenticated');
        }

        if (this.hasTutorWithNoAvailability(userId)) {
          console.log(`Tutor ${userId} is known to have no availability yet, skipping API call`);
          this.loading = false;
          return;
        }

        try {
          const availabilityData = await getTutorAvailability(userId);

          this.slotsByMonth = {};

          if (availabilityData && Array.isArray(availabilityData) && availabilityData.length > 0) {
            availabilityData.forEach((slot: any) => {
              const date = new Date(slot.date);
              const day = date.getDate();
              const month = date.getMonth();
              const year = date.getFullYear();
              const monthKey = `${year}-${month}`;

              if (!this.slotsByMonth[monthKey]) {
                this.slotsByMonth[monthKey] = {
                  daysWithSlots: [],
                  daysWithEditedPastSlots: [],
                  slotData: {},
                };
              }

              if (!this.slotsByMonth[monthKey].daysWithSlots.includes(day)) {
                this.slotsByMonth[monthKey].daysWithSlots.push(day);
              }

              if (!this.slotsByMonth[monthKey].slotData[day]) {
                this.slotsByMonth[monthKey].slotData[day] = [];
              }

              const startTime = slot.startTime.substring(0, 5);
              const endTime = slot.endTime.substring(0, 5);

              this.slotsByMonth[monthKey].slotData[day].push({
                id: `api-${slot.id}`,
                apiId: slot.id,
                startTime,
                endTime,
                date: slot.date,
                status: 'available',
                activeStatus: slot.activeStatus,
              });
            });
          }
        } catch (error: any) {
          if (error.response && error.response.status === 400 && error.response.data) {
            const errorData = error.response.data;
            const errorMessage = Array.isArray(errorData) ? errorData[0] : String(errorData);

            if (errorMessage.includes('does not have any availability')) {
              this.addTutorWithNoAvailability(userId);
              console.log(`Tutor ${userId} has no availability yet, marking for future reference`);
            } else {
              throw error;
            }
          } else {
            throw error;
          }
        }

        this.loading = false;
      } catch (error) {
        this.loading = false;
        this.error = error instanceof Error ? error.message : 'Failed to fetch availability';
        console.error('Error fetching availability:', error);
      }
    },

    async addSlot(newSlotData: { startTime: string; endTime: string }) {
      if (this.checkOverlap(newSlotData)) {
        throw new Error('This time slot overlaps with an existing slot');
      }

      const day = this.currentDay;
      const date =
        this.selectedDate instanceof Date ? this.selectedDate : new Date(this.selectedDate);

      try {
        this.loading = true;
        this.error = null;

        const userStore = useUserStore();
        const userId = Number(userStore.userId);

        const apiData: AvailabilitySlot = {
          date: this.formatDateForAPI(date),
          startTime: this.formatTimeForAPI(newSlotData.startTime),
          endTime: this.formatTimeForAPI(newSlotData.endTime),
          activeStatus: true,
        };

        const response = await createAvailability(apiData);

        const monthData = this.initializeMonthData();

        const newSlot: TimeSlot = {
          id: `api-${response.id}`,
          apiId: response.id,
          startTime: newSlotData.startTime,
          endTime: newSlotData.endTime,
          date: apiData.date,
          status: 'available',
          activeStatus: true,
        };

        if (!monthData.daysWithSlots.includes(day)) {
          monthData.daysWithSlots.push(day);
        }

        if (!monthData.slotData[day]) {
          monthData.slotData[day] = [];
        }

        monthData.slotData[day].push(newSlot);

        if (this.hasTutorWithNoAvailability(userId)) {
          this.removeTutorWithNoAvailability(userId);
        }

        this.loading = false;
      } catch (error) {
        this.loading = false;
        this.error = error instanceof Error ? error.message : 'Error adding availability';
        console.error('Error adding slot:', error);
        throw error;
      }
    },

    async editSlot(id: string, updatedData: { startTime?: string; endTime?: string }) {
      const day = this.currentDay;
      const monthData = this.currentMonthData;

      if (!monthData || !monthData.slotData[day]) return;

      const slot = monthData.slotData[day].find((slot) => slot.id === id);

      if (slot) {
        const tempSlot = {
          startTime: updatedData.startTime || slot.startTime,
          endTime: updatedData.endTime || slot.endTime,
        };

        if (this.checkOverlap(tempSlot, id)) {
          throw new Error('This time slot would overlap with another existing slot');
        }

        try {
          this.loading = true;
          this.error = null;

          if (slot.apiId) {
            const apiData: AvailabilitySlot = {
              id: slot.apiId,
              date: slot.date || this.formatDateForAPI(this.selectedDate as Date),
              startTime: this.formatTimeForAPI(updatedData.startTime || slot.startTime),
              endTime: this.formatTimeForAPI(updatedData.endTime || slot.endTime),
              activeStatus: slot.activeStatus !== undefined ? slot.activeStatus : true,
            };

            await updateAvailability(apiData);
          }

          if (updatedData.startTime) slot.startTime = updatedData.startTime;
          if (updatedData.endTime) slot.endTime = updatedData.endTime;

          if (this.isPastDate()) {
            slot.isEditedPastSlot = true;

            if (!monthData.daysWithEditedPastSlots) {
              monthData.daysWithEditedPastSlots = [];
            }
            if (!monthData.daysWithEditedPastSlots.includes(day)) {
              monthData.daysWithEditedPastSlots.push(day);
            }
          }

          this.loading = false;
        } catch (error) {
          this.loading = false;
          this.error = error instanceof Error ? error.message : 'Error updating availability';
          console.error('Error updating slot:', error);
          throw error;
        }
      }
    },

    async deleteSlot(id: string) {
      const day = this.currentDay;
      const monthData = this.currentMonthData;

      if (!monthData || !monthData.slotData[day]) return;

      const slotIndex = monthData.slotData[day].findIndex((slot) => slot.id === id);

      if (slotIndex !== -1) {
        const slot = monthData.slotData[day][slotIndex];

        try {
          this.loading = true;
          this.error = null;

          if (slot.apiId) {
            await deleteAvailability(slot.apiId);
          }

          monthData.slotData[day].splice(slotIndex, 1);

          if (monthData.slotData[day].length === 0) {
            const dayIndex = monthData.daysWithSlots.indexOf(day);
            if (dayIndex !== -1) {
              monthData.daysWithSlots.splice(dayIndex, 1);
            }

            if (monthData.daysWithEditedPastSlots) {
              const editedDayIndex = monthData.daysWithEditedPastSlots.indexOf(day);
              if (editedDayIndex !== -1) {
                monthData.daysWithEditedPastSlots.splice(editedDayIndex, 1);
              }
            }

            delete monthData.slotData[day];
          }

          this.loading = false;
        } catch (error) {
          this.loading = false;
          this.error = error instanceof Error ? error.message : 'Error deleting availability';
          console.error('Error deleting slot:', error);
          throw error;
        }
      }
    },
  },

  persist: {
    serializer: {
      deserialize(serializedState) {
        const state = JSON.parse(serializedState);
        if (state.selectedDate) {
          state.selectedDate = new Date(state.selectedDate);
        }

        if (!Array.isArray(state.tutorsWithNoAvailability)) {
          state.tutorsWithNoAvailability = [];
        }

        return state;
      },
      serialize(state) {
        return JSON.stringify(state);
      },
    },
  },
});
