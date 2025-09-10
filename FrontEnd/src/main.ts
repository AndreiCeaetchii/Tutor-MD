import { createApp } from 'vue';
import { createPinia } from 'pinia';
import './index.css';
import App from './App.vue';
import router from './router';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import vue3GoogleLogin from 'vue3-google-login';

const app = createApp(App);
const pinia = createPinia();

app.component('font-awesome-icon', FontAwesomeIcon);

app.use(pinia);
app.use(router);
app.use(vue3GoogleLogin, {
  clientId: '425538151525-bhujljp8s9kn9vffkd0rf1cad6gd1epb.apps.googleusercontent.com', // pune aici Client ID din Google Cloud Console
});
app.mount('#app');
