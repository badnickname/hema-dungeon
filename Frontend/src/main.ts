import { createApp } from 'vue';
import './style.css';
import App from './App.vue';
import { createRouter, createWebHistory } from 'vue-router';
import Index from './components/Index.vue';
import Register from './components/Register.vue';
import Login from './components/Login.vue';
import { createPinia } from 'pinia';
import Dashboard from './components/Dashboard.vue';
import Character from './components/Character.vue';
import Edit from './components/Edit.vue';
import Fight from './components/Fight.vue';

const router = createRouter({
	history: createWebHistory(), routes: [{
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
	}]
});

createApp(App).use(router).use(createPinia()).mount('#app');
