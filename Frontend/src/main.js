import { createApp } from 'vue'
import './style.css'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import store from "./store/index.js"
import App from './App.vue'
import router from './router/router.js'
import axios from 'axios';
axios.defaults.withCredentials = true;

const app = createApp(App);

app.use(store)
app.use(ElementPlus)
app.use(router);

app.mount('#app');
    