import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Character } from '../types/Character';
import type { FightCharacter } from '../types/FightCharacter';
import type { FightState } from '../types/FightState';
import type { Visit } from '../types/Visit';
import moment from 'moment';
import type { Page } from '../types/Page';
import type { Result } from '../types/Result';

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
		return { character, characters, visibleCharacter, fightCharacters, fightStates, isAdmin, visits, page, isRefreshed, results };
	}, actions: {
		async getCharacter() {
			// return true;
			const result = await fetch('/api/user');
			if (result.status === 401) return false;
			this.character = await result.json() as Character;
			this.character.avatar = `images/${this.character.avatar}`
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
			const result = await fetch('/api/users');
			this.characters = await result.json() as Character[];
			this.characters.forEach(x => x.avatar = `images/${x.avatar}`);
		},
		async getFight() {
			// return false;
			const result = await fetch('/api/fight/users');
			if (result.status === 401) return false;
			this.fightCharacters = await result.json() as FightCharacter[];
			this.fightCharacters.forEach(x => {
				if (x.character) x.character.avatar = `images/${x.character.avatar}`;
			});
			await this.getFightState();
			await this.getResults();
			return this.fightCharacters.length > 0;
		},
		async getFightState() {
			const result = await fetch('/api/fight/state');
			if (result.status === 401) return false;
			this.fightStates = await result.json() as FightState[];
			this.fightStates.forEach(x => {
				if (x.character?.character) x.character.character.avatar = `images/${x.character.character.avatar}`;
			});
			return true;
		},
		async getVisits() {
			const result = await fetch('/api/admin/visits');
			if (result.status === 401) return false;
			const visits = await result.json() as Record<string, (Omit<Visit, 'date'> & { date: string }) []>;
			this.visits = Object.keys(visits).reduce((list, y) =>
			{
				const date = moment(y, 'YYYY-MM-DD').format("YYYY-MM-DD");
				list[date] = visits[y].map(x => {
					x.character.avatar = `images/${x.character.avatar}`;
					return {
						date: moment(x.date),
						id: x.id,
						character: x.character,
						wasHere: x.wasHere,
						canSkip: x.canSkip
					};
				});
				return list;
			}, {} as Record<string, Visit[]>)
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
