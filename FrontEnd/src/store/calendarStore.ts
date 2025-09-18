import { defineStore } from 'pinia';

interface TimeSlot {
  id: string;
  startTime: string;
  endTime: string;
  status: 'available' | 'booked';
  studentName?: string;
}

interface MonthData {
  daysWithSlots: number[];
  slotData: Record<number, TimeSlot[]>;
}

export const useCalendarStore = defineStore('calendar', {
  state: () => ({
    // Schimbăm structura de date pentru a organiza sloturile după lună/an
    slotsByMonth: {} as Record<string, MonthData>,
    selectedDate: new Date()
  }),
  
  getters: {
    currentDay(): number {
      const date = this.selectedDate instanceof Date ? 
        this.selectedDate : new Date(this.selectedDate);
      return date.getDate();
    },
    
    currentMonth(): number {
      const date = this.selectedDate instanceof Date ? 
        this.selectedDate : new Date(this.selectedDate);
      return date.getMonth();
    },
    
    currentYear(): number {
      const date = this.selectedDate instanceof Date ? 
        this.selectedDate : new Date(this.selectedDate);
      return date.getFullYear();
    },
    
    currentMonthKey(): string {
      return `${this.currentYear}-${this.currentMonth}`;
    },
    
    formattedDate(): string {
      const date = this.selectedDate instanceof Date ? 
        this.selectedDate : new Date(this.selectedDate);
      
      const month = date.getMonth() + 1;
      const day = date.getDate();
      const year = date.getFullYear();
      return `${day.toString().padStart(2, '0')}.${month.toString().padStart(2, '0')}.${year}`;
    },
    
    currentMonthData(): MonthData {
      return this.slotsByMonth[this.currentMonthKey] || { daysWithSlots: [], slotData: {} };
    },
    
    daysWithSlots(): number[] {
      return this.currentMonthData.daysWithSlots || [];
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
    }
  },
  
  actions: {
    // Utility function to convert time string to minutes for comparison
    timeToMinutes(time: string): number {
      const [hours, minutes] = time.split(':').map(Number);
      return hours * 60 + minutes;
    },
    
    // Inițializează datele pentru luna curentă dacă nu există
    initializeMonthData() {
      if (!this.slotsByMonth[this.currentMonthKey]) {
        this.slotsByMonth[this.currentMonthKey] = {
          daysWithSlots: [],
          slotData: {}
        };
      }
      return this.slotsByMonth[this.currentMonthKey];
    },
    
    // Check if a time slot overlaps with existing slots
    checkOverlap(newSlot: { startTime: string, endTime: string }, excludeId?: string): boolean {
      const day = this.currentDay;
      const monthData = this.currentMonthData;
      
      if (!monthData.slotData[day] || monthData.slotData[day].length === 0) {
        return false; // No existing slots, so no overlap
      }
      
      const newStart = this.timeToMinutes(newSlot.startTime);
      const newEnd = this.timeToMinutes(newSlot.endTime);
      
      return monthData.slotData[day].some(existingSlot => {
        // If we're editing a slot, exclude it from the check
        if (excludeId && existingSlot.id === excludeId) {
          return false;
        }
        
        const existingStart = this.timeToMinutes(existingSlot.startTime);
        const existingEnd = this.timeToMinutes(existingSlot.endTime);
        
        // Check for overlap: new start is before existing end AND new end is after existing start
        return (newStart < existingEnd && newEnd > existingStart);
      });
    },
    
    setSelectedDate(date: Date | string) {
      this.selectedDate = typeof date === 'string' ? new Date(date) : date;
    },
    
    // Modified addSlot to check for overlaps
    addSlot(newSlotData: { startTime: string, endTime: string }) {
      // Check for overlaps first
      if (this.checkOverlap(newSlotData)) {
        throw new Error('This time slot overlaps with an existing slot');
      }
      
      const day = this.currentDay;
      
      // Asigură-te că datele pentru luna curentă sunt inițializate
      const monthData = this.initializeMonthData();
      
      const newSlot: TimeSlot = {
        id: `${Date.now()}`,
        startTime: newSlotData.startTime,
        endTime: newSlotData.endTime,
        status: 'available'
      };
      
      if (!monthData.daysWithSlots.includes(day)) {
        monthData.daysWithSlots.push(day);
      }
      
      if (!monthData.slotData[day]) {
        monthData.slotData[day] = [];
      }
      
      monthData.slotData[day].push(newSlot);
    },
    
    // Modified editSlot to check for overlaps
    editSlot(id: string, updatedData: { startTime?: string, endTime?: string }) {
      const day = this.currentDay;
      const monthData = this.currentMonthData;
      
      if (!monthData || !monthData.slotData[day]) return;
      
      const slot = monthData.slotData[day].find(slot => slot.id === id);
      
      if (slot) {
        // Create temp slot with the updated data for overlap checking
        const tempSlot = {
          startTime: updatedData.startTime || slot.startTime,
          endTime: updatedData.endTime || slot.endTime
        };
        
        // Check for overlaps, excluding the current slot
        if (this.checkOverlap(tempSlot, id)) {
          throw new Error('This time slot would overlap with another existing slot');
        }
        
        // If no overlaps, update the slot
        if (updatedData.startTime) slot.startTime = updatedData.startTime;
        if (updatedData.endTime) slot.endTime = updatedData.endTime;
      }
    },
    
    deleteSlot(id: string) {
      const day = this.currentDay;
      const monthData = this.currentMonthData;
      
      if (!monthData || !monthData.slotData[day]) return;
      
      const slotIndex = monthData.slotData[day].findIndex(slot => slot.id === id);
      
      if (slotIndex !== -1) {
        monthData.slotData[day].splice(slotIndex, 1);
        
        if (monthData.slotData[day].length === 0) {
          const dayIndex = monthData.daysWithSlots.indexOf(day);
          if (dayIndex !== -1) {
            monthData.daysWithSlots.splice(dayIndex, 1);
          }
          delete monthData.slotData[day];
        }
      }
    },
    
    // Metodă nouă pentru a obține statistici pentru luna curentă
    getCurrentMonthStatistics() {
      const monthData = this.currentMonthData;
      
      if (!monthData || Object.keys(monthData.slotData).length === 0) {
        return {
          availableSlots: 0,
          bookedSlots: 0,
          activeDays: 0,
        };
      }
      
      let availableSlots = 0;
      let bookedSlots = 0;
      
      Object.entries(monthData.slotData).forEach(([_, daySlots]) => {
        daySlots.forEach((slot) => {
          if (slot.status === 'available') {
            availableSlots++;
          } else if (slot.status === 'booked') {
            bookedSlots++;
          }
        });
      });
      
      return {
        availableSlots,
        bookedSlots,
        activeDays: monthData.daysWithSlots.length,
      };
    }
  },
  
  persist: {
    serializer: {
      deserialize(serializedState) {
        const state = JSON.parse(serializedState);
        // Convertim înapoi la Date când încărcăm din localStorage
        if (state.selectedDate) {
          state.selectedDate = new Date(state.selectedDate);
        }
        return state;
      },
      serialize(state) {
        // Creăm o copie pentru a nu modifica starea originală
        const stateCopy = JSON.parse(JSON.stringify(state));
        return JSON.stringify(stateCopy);
      }
    }
  }
});
