import '@babel/polyfill';
import Vue from 'vue';
import './plugins/axios';
import './plugins/vuetify';
import './plugins/element-ui';
// import './plugins/pivottable';
import App from './App.vue';
import router from './router';
import store from '@/store/index';
import './registerServiceWorker';
import axios from 'axios';

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');

axios.interceptors.request.use((config) => {
  const user = store.getters['auth/getUser'];

  if (user) {
    const authToken = user.access_token;
    if (authToken) {
      config.headers.Authorization = `Bearer ${authToken}`;
    }
    //console.log('authToken: ' +authToken);
  }
  return config;
},
(err) => {
  console.log('Error en interceptors :'+ store.getters['auth/getUser'] );
});