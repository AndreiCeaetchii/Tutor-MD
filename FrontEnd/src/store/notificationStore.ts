import { defineStore } from 'pinia';
import { getUserNotifications, type Notification } from '../services/notificationService';

interface NotificationState {
  notifications: Notification[];
  locallyReadIds: number[];
  loading: boolean;
  error: string | null;
  lastFetchTime: number;
}

export const useNotificationStore = defineStore('notification', {
  state: (): NotificationState => ({
    notifications: [],
    locallyReadIds: [],
    loading: false,
    error: null,
    lastFetchTime: 0
  }),

  getters: {
    unreadCount(): number {
      return this.notifications.filter((n: Notification) => 
        n.status === 0 && !this.locallyReadIds.includes(n.id)
      ).length;
    }
  },

  actions: {
    async fetchNotifications() {
      this.loading = true;
      this.error = null;
      
      try {
        const response = await getUserNotifications();
        console.log('Raw notification data:', response.notifications[0]);
        
        // Map notifications to ensure consistent date format, but preserve actual dates
        this.notifications = response.notifications.map((notification: Notification) => {
          // Check for both camelCase and PascalCase versions of the date field
          const dateValue = notification.createdAt || (notification as any).CreatedAt;
          
          if (dateValue) {
            // If we have a date, ensure it's properly formatted
            return {
              ...notification,
              createdAt: new Date(dateValue).toISOString()
            };
          } else {
            // Only if no date exists, use a fallback based on notification ID
            console.warn(`Notification ${notification.id} missing creation date, using fallback`);
            const baseTime = new Date().getTime() - 86400000; // yesterday
            const offset = notification.id * 60000;
            const timestamp = new Date(baseTime - offset).toISOString();
            
            return {
              ...notification,
              createdAt: timestamp
            };
          }
        });
        
        this.lastFetchTime = Date.now();
      } catch (error) {
        this.error = error instanceof Error ? error.message : 'Failed to fetch notifications';
        console.error('Error fetching notifications:', error);
      } finally {
        this.loading = false;
      }
    },
    
    markAsRead(notificationId: number) {
      if (!this.locallyReadIds.includes(notificationId)) {
        this.locallyReadIds.push(notificationId);
        this.saveReadStatusToLocalStorage();
      }
    },
    
    saveReadStatusToLocalStorage() {
      localStorage.setItem('notification-read-ids', JSON.stringify(this.locallyReadIds));
    },
    
    loadReadStatusFromLocalStorage() {
      const savedIds = localStorage.getItem('notification-read-ids');
      if (savedIds) {
        try {
          this.locallyReadIds = JSON.parse(savedIds);
        } catch (e) {
          console.error('Error parsing saved notification read status:', e);
        }
      }
    },
    
    initStore() {
      this.loadReadStatusFromLocalStorage();
    }
  }
});
