import { createApp } from 'vue';
import './style.css';
import App from './App.vue';
import { createMemoryHistory, createRouter } from 'vue-router';
import Index from './components/Index.vue';
import Register from './components/Register.vue';
import Login from './components/Login.vue';
import { createPinia } from 'pinia';
import Dashboard from './components/Dashboard.vue';
import Character from './components/Character.vue';
import Edit from './components/Edit.vue';
import Fight from './components/Fight.vue';
import Password from './components/Password.vue';

const router = createRouter({
	history: createMemoryHistory(), routes: [{
		path: '/', component: Index,
	}, {
		path: '/register', component: Register
	}, {
		path: '/login', component: Login
	}, {
		path: '/dashboard', component: Dashboard
	}, {
		path: '/character', component: Character
	}, {
		path: '/edit', component: Edit
	}, {
		path: '/fight', component: Fight
	}, {
		path: '/password', component: Password
	}]
});

createApp(App).use(router).use(createPinia()).mount('#app');
