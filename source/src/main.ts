import App from './App.vue';
import PrimeVue from 'primevue/config';
import Router from './routers';
import { createApp } from 'vue';
import { store, key } from './stores';
import InitPrototype from './assets/scripts/prototype/InitPrototype';

const app = createApp(App);
app.use(PrimeVue);
app.use(Router);
app.use(store, key);
new InitPrototype(app);

app.mount('#app');