import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
import type { Character } from '../types/Character';
import type { FightCharacter } from '../types/FightCharacter';
import type { FightState } from '../types/FightState';
import type { Visit } from '../types/Visit';
import moment from 'moment';
import type { Page } from '../types/Page';
import type { Result } from '../types/Result';
import { mapAvatar } from './map-avatar.ts';
import type { Region } from '../types/Region';

export const useStore = defineStore('app', {
	state: () => {
		const character = ref<Character>({
			id: '0', name: 'test', story: 'test', age: 0, avatar: 'img/test.png', gender: 'male', agility: 0, power: 0, vitality: 0, wisdom: 0
		});
		const isAdmin = ref(false);
		const visibleCharacter = ref<Character>();
		const characters = ref<Character[]>([{
			id: '0', name: 'test', story: 'test', age: 0, avatar: 'img/test.png', gender: 'male', agility: 5, power: 3, vitality: 2, wisdom: 1
		}, {
			id: '0', name: 'test_2', story: 'test', age: 0, avatar: 'img/test.png', gender: 'male', agility: 0, power: 0, vitality: 0, wisdom: 0
		}]);
		const fightCharacters = ref<FightCharacter[]>([]);
		const fightStates = ref<FightState[]>([]);
		const visits = ref<Record<string, Visit[]>>({});
		const page = ref<Page>();
		const isRefreshed = ref<boolean>();
		const results = ref<Result[]>([]);
		const regions = ref<Region[]>([]);
		const region = computed({
			get: () => {
				const value = localStorage.getItem('region');
				if (value) return JSON.parse(value) as Region;
				return null;
			},
			set: (value) => {
				if (!value) localStorage.removeItem('region');
				else localStorage.setItem('region', JSON.stringify(value));
			}
		})
		return { character, regions, characters, visibleCharacter, fightCharacters, fightStates, isAdmin, visits, page, isRefreshed, results, region };
	}, actions: {
		async getRegions() {
			const result = await fetch('/api/regions');
			if (result.status === 401) return false;
			this.regions = await result.json() as Region[];
			return true;
		},
		async getCharacter() {
			// return true;
			const result = await fetch('/api/user');
			if (result.status === 401) return false;
			this.character = await result.json() as Character;
			mapAvatar(this.character);
			await this.getCharacters();
			await this.getIsAdmin();
			return true;
		},
		async getIsAdmin() {
			const result = await fetch('/api/admin/role');
			if (result.status === 401) return false;
			this.isAdmin = await result.json() as boolean;
			return true;
		},
		async getCharacters() {
			const result = await fetch(`/api/users?region=${this.region?.id ?? 'NOVOSIBIRSK'}`);
			this.characters = await result.json() as Character[];
			this.characters.forEach(mapAvatar);
		},
		async getFight() {
			// return false;
			const result = await fetch('/api/fight/users');
			if (result.status === 401) return false;
			this.fightCharacters = await result.json() as FightCharacter[];
			this.fightCharacters.map(x => x.character).forEach(mapAvatar);
			await this.getFightState();
			await this.getResults();
			return this.fightCharacters.length > 0;
		},
		async getFightState() {
			const result = await fetch('/api/fight/state');
			if (result.status === 401) return false;
			this.fightStates = await result.json() as FightState[];
			this.fightStates.map(x => x.character.character).forEach(mapAvatar);
			return true;
		},
		async getVisits() {
			const result = await fetch(`/api/admin/visits?region=${this.region?.id ?? 'NOVOSIBIRSK'}`);
			if (result.status === 401) return false;
			const visits = await result.json() as Record<string, (Omit<Visit, 'date'> & { date: string }) []>;
			this.visits = Object.keys(visits).reduce((list, y) =>
			{
				const date = moment(y, 'YYYY-MM-DD').format("YYYY-MM-DD");
				list[date] = visits[y].map(x => ({
					date: moment(x.date),
					id: x.id,
					character: mapAvatar(x.character),
					wasHere: x.wasHere,
					canSkip: x.canSkip
				}));
				return list;
			}, {} as Record<string, Visit[]>);
			return true;
		},
		async removePage(page: Page) {
			const headers = new Headers();
			headers.append('content-type', 'application/json');
			const result = await fetch('/api/pages/delete', { method: 'POST', headers, body: JSON.stringify({ id: page.id }) });
			if (result.status === 401) return false;
			return true;
		},
		async getResults() {
			const result = await fetch('/api/fight/results');
			if (result.status === 401) return false;
			this.results = await result.json() as Result[];
			return true;
		}
	},
});
