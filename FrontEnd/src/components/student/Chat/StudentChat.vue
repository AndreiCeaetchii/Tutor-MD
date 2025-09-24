<script setup lang="ts">
import { ref, watch, defineProps, defineEmits } from 'vue';

interface Message {
  id: string;
  sender: string;
  text: string;
  timestamp: string;
  isCurrentUser: boolean;
  isFile?: boolean;
}

interface Conversation {
  id: string;
  name: string;
  avatar?: string;
  lastMessage: string;
  timestamp: string;
  unread: boolean;
  status: 'online' | 'offline';
  messages: Message[];
}

const props = defineProps<{ studentName?: string | null }>();
const emit = defineEmits(['close']);

watch(
  () => props.studentName,
  (newName) => {
    if (newName) {
      const found = conversations.value.find(c => c.name === newName);
      if (found) {
        activeConversation.value = found;
        showConversationList.value = false;
      }
    }
  },
  { immediate: true }
);

const conversations = ref<Conversation[]>([
  {
    id: '1',
    name: 'Alex Chen',
    lastMessage: 'Thank you for the practice...',
    timestamp: '2 min ago',
    unread: true,
    status: 'online',
    messages: [
      {
        id: '101',
        sender: 'Alex Chen',
        text: 'Hi Sarah, I had some trouble with problem 10 from the homework.',
        timestamp: '10:30 AM',
        isCurrentUser: false
      },
      {
        id: '102',
        sender: 'Me',
        text: "Hi Alex! I'd be happy to help. Can you tell me which part is confusing you?",
        timestamp: '10:32 AM',
        isCurrentUser: true
      },
      {
        id: '103',
        sender: 'Alex Chen',
        text: "I'm not sure how to approach the integration by parts.",
        timestamp: '10:35 AM',
        isCurrentUser: false
      },
      {
        id: '104',
        sender: 'Me',
        text: 'Let me share a step-by-step guide for integration by parts.',
        timestamp: '10:37 AM',
        isCurrentUser: true
      },
      {
        id: '105',
        sender: 'Me',
        text: 'integration-by-parts-guide.pdf',
        timestamp: '10:38 AM',
        isCurrentUser: true,
        isFile: true
      }
    ]
  },
  {
    id: '2',
    name: 'Maria Rodriguez',
    lastMessage: 'I completed all the statisti...',
    timestamp: '1 hour ago',
    unread: false,
    status: 'offline',
    messages: [
      {
        id: '201',
        sender: 'Maria Rodriguez',
        text: 'Hello, I have questions about the upcoming exam.',
        timestamp: '9:15 AM',
        isCurrentUser: false
      },
      {
        id: '202',
        sender: 'Me',
        text: 'Hi Maria, what would you like to know?',
        timestamp: '9:20 AM',
        isCurrentUser: true
      }
    ]
  },
  {
    id: '3',
    name: 'John Smith',
    lastMessage: 'Hello, Yes',
    timestamp: 'Just now',
    unread: true,
    status: 'online',
    messages: [
      {
        id: '301',
        sender: 'John Smith',
        text: 'Hello, Yes',
        timestamp: '8:05 AM',
        isCurrentUser: false
      }
    ]
  }
]);

const activeConversation = ref<Conversation | null>(conversations.value[0]);
const newMessage = ref('');
const showConversationList = ref(true);

const selectConversation = (conversation: Conversation) => {
  if (conversation.unread) {
    conversation.unread = false;
  }
  activeConversation.value = conversation;
  showConversationList.value = false;
};

const backToConversations = () => {
  showConversationList.value = true;
};

const sendMessage = () => {
  if (!newMessage.value.trim() || !activeConversation.value) return;
  
  const message: Message = {
    id: Date.now().toString(),
    sender: 'Me',
    text: newMessage.value,
    timestamp: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
    isCurrentUser: true
  };
  
  activeConversation.value.messages.push(message);
  activeConversation.value.lastMessage = newMessage.value;
  activeConversation.value.timestamp = 'Just now';
  
  newMessage.value = '';
};
</script>

<template>
  <div class="flex md:flex-row flex-col rounded-2xl shadow-lg overflow-hidden bg-white md:h-[600px]">
    <div 
      class="w-full border-r border-gray-200 md:w-1/3 md:block"
      :class="{'hidden': !showConversationList}"
    >
      <div class="p-4 border-b border-gray-200">
        <h2 class="text-xl font-semibold">Conversations</h2>
        <div class="relative mt-2">
          <span class="absolute inset-y-0 left-0 flex items-center pl-3 text-gray-500 pointer-events-none">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
            </svg>
          </span>
          <input 
            type="text" 
            placeholder="Search conversations..." 
            class="w-full py-2 pl-10 pr-3 text-sm border border-gray-200 rounded-full bg-gray-50 focus:outline-none focus:ring-1 focus:ring-purple-400 focus:border-purple-400"
          />
        </div>
      </div>
      <div class="md:h-[calc(100%-64px)] overflow-y-auto">
        <div 
          v-for="conversation in conversations" 
          :key="conversation.id"
          class="p-4 border-b border-gray-100 cursor-pointer hover:bg-gray-50"
          :class="{ 'bg-gray-100': activeConversation?.id === conversation.id }"
          @click="selectConversation(conversation)"
        >
          <div class="flex items-center">
            <div class="relative">
              <div class="flex items-center justify-center w-10 h-10 text-gray-600 bg-gray-200 rounded-full">
                {{ conversation.name.charAt(0) }}
              </div>
              
              <div 
                class="absolute -bottom-0.5 -right-0.5 w-3 h-3 border-2 border-white rounded-full"
                :class="conversation.status === 'online' ? 'bg-green-500' : 'bg-gray-400'"
              ></div>
            </div>
            
            <div class="flex-1 ml-3">
              <div class="flex items-center justify-between">
                <h3 class="font-medium">{{ conversation.name }}</h3>
                <span class="text-xs text-gray-500">{{ conversation.timestamp }}</span>
              </div>
              
              <div class="flex items-center justify-between mt-1">
                <p class="text-sm text-gray-600 truncate max-w-[180px]">
                  {{ conversation.lastMessage }}
                </p>
                <div v-if="conversation.unread" class="w-2 h-2 bg-purple-600 rounded-full"></div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  
    <div 
      class="flex flex-col h-full md:flex-1"
      :class="{'hidden md:flex': showConversationList, 'w-full': !showConversationList}"
    >
      <div v-if="activeConversation" class="flex flex-col h-full">
        <div class="flex items-center p-4 border-b border-gray-200">
          <button 
            @click="backToConversations" 
            class="mr-2 text-gray-600 md:hidden hover:text-purple-600"
          >
            <span class="material-icons">arrow_back</span>
          </button>
          
          <div class="relative">
            <div class="flex items-center justify-center w-10 h-10 text-gray-600 bg-gray-200 rounded-full">
              {{ activeConversation.name.charAt(0) }}
            </div>
            
            <div 
              class="absolute -bottom-0.5 -right-0.5 w-2.5 h-2.5 rounded-full border-2 border-white"
              :class="activeConversation.status === 'online' ? 'bg-green-500' : 'bg-gray-400'"
            ></div>
          </div>
          
          <div class="ml-3">
            <h3 class="font-medium">{{ activeConversation.name }}</h3>
            <p class="text-xs text-gray-500">{{ activeConversation.status === 'online' ? 'Online' : 'Offline' }}</p>
          </div>
        </div>
        
        <div class="flex-1 p-4 overflow-y-auto bg-white" ref="messagesContainer">
          <div class="flex flex-col gap-3">
            <div 
              v-for="message in activeConversation.messages" 
              :key="message.id"
              class="max-w-[75%] p-3 rounded-2xl text-left"
              :class="message.isCurrentUser 
                ? 'bg-purple-100 rounded-br-none self-end' 
                : 'bg-gray-100 rounded-bl-none self-start'"
            >
              <div v-if="message.isFile" class="flex items-center text-purple-700">
                <span class="mr-2 material-icons" style="font-size: 18px;">attachment</span>
                <span>{{ message.text }}</span>
                <span class="ml-2 material-icons" style="font-size: 18px;">download</span>
              </div>
              <p v-else class="text-sm">{{ message.text }}</p>
              <p class="mt-1 text-xs text-gray-500">{{ message.timestamp }}</p>
            </div>
          </div>
        </div>
        
        <div class="sticky bottom-0 z-10 px-2 py-2 bg-white border-t border-gray-200 sm:px-4 sm:py-3">
          <div class="flex items-center w-full gap-1 sm:gap-2">
            <button class="flex items-center justify-center flex-shrink-0 p-1 text-gray-500 sm:p-2 hover:text-purple-600">
              <span class="material-icons" style="font-size: 20px;">attachment</span>
            </button>
            <input 
              v-model="newMessage"
              type="text" 
              placeholder="Type your message..." 
              class="flex-grow px-3 py-2 text-sm border border-gray-300 rounded-full focus:outline-none focus:border-purple-400"
              @keyup.enter="sendMessage"
            />
            <button 
              class="flex items-center justify-center flex-shrink-0 text-white bg-purple-600 rounded-full w-9 h-9 sm:w-10 sm:h-10 hover:bg-purple-700"
              @click="sendMessage"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <line x1="22" y1="2" x2="11" y2="13"></line>
                <polygon points="22 2 15 22 11 13 2 9 22 2"></polygon>
              </svg>
            </button>
          </div>
        </div>
      </div>
      
      <div v-else class="flex items-center justify-center flex-1">
        <div class="p-6 text-center">
          <div class="text-6xl text-gray-300 material-icons">chat</div>
          <p class="mt-4 text-gray-500">Select a conversation to start messaging</p>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.material-icons {
  font-size: 24px;
}

::-webkit-scrollbar {
  width: 4px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
}

::-webkit-scrollbar-thumb {
  background: #ddd;
  border-radius: 3px;
}

::-webkit-scrollbar-thumb:hover {
  background: #ccc;
}
</style>