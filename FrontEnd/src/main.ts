import { createApp } from 'vue';
import { createPinia } from 'pinia';
import './index.css';
import App from './App.vue';
import router from './router';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import vue3GoogleLogin from 'vue3-google-login';
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate';

const app = createApp(App);

app.component('font-awesome-icon', FontAwesomeIcon);
app.use(router);
app.use(vue3GoogleLogin, {
  clientId: '425538151525-bhujljp8s9kn9vffkd0rf1cad6gd1epb.apps.googleusercontent.com',
});
const pinia = createPinia();
pinia.use(piniaPluginPersistedstate);

app.use(pinia);
app.mount('#app');
