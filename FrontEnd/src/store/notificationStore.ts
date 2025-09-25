import { defineStore } from 'pinia';
import { getUserNotifications, type Notification } from '../services/notificationService';

interface NotificationState {
  notifications: Notification[];
  locallyReadIds: number[];
  loading: boolean;
  error: string | null;
  lastFetchTime: number;
}

interface ApiNotification extends Notification {
  CreatedAt?: string;
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
        
        this.notifications = response.notifications.map((notification: Notification) => {
          const dateValue = notification.createdAt || (notification as ApiNotification).CreatedAt;
          
          if (dateValue) {
            return {
              ...notification,
              createdAt: new Date(dateValue).toISOString()
            };
          } else {
            console.warn(`Notification ${notification.id} missing creation date, using fallback`);
            const MS_PER_DAY = 86400000;
            const MS_PER_MINUTE = 60000;

            const baseTime = new Date().getTime() - MS_PER_DAY;
            const offset = notification.id * MS_PER_MINUTE;
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
