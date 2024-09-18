import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Character } from '../types/Character';
import type { FightCharacter } from '../types/FightCharacter';
import type { FightState } from '../types/FightState';
import type { Visit } from '../types/Visit';
import moment from 'moment';

export const useStore = defineStore('app', {
	state: () => {
		const character = ref<Character>({
			name: 'test', story: 'test', age: 0, avatar: 'img/test.png', gender: 'male', agility: 0, power: 0, vitality: 0, wisdom: 0
		});
		const isAdmin = ref(false);
		const visibleCharacter = ref<Character>();
		const characters = ref<Character[]>([{
			name: 'test', story: 'test', age: 0, avatar: 'img/test.png', gender: 'male', agility: 5, power: 3, vitality: 2, wisdom: 1
		}, {
			name: 'test_2', story: 'test', age: 0, avatar: 'img/test.png', gender: 'male', agility: 0, power: 0, vitality: 0, wisdom: 0
		}]);
		const fightCharacters = ref<FightCharacter[]>([]);
		const fightStates = ref<FightState[]>([]);
		const visits = ref<Record<string, Visit[]>>({});
		return { character, characters, visibleCharacter, fightCharacters, fightStates, isAdmin, visits };
	}, actions: {
		async getCharacter() {
			// return true;
			const result = await fetch('/api/user');
			if (result.status === 401) return false;
			this.character = await result.json() as Character;
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
		},
		async getFight() {
			// return false;
			const result = await fetch('/api/fight/users');
			if (result.status === 401) return false;
			this.fightCharacters = await result.json() as FightCharacter[];
			await this.getFightState();
			return this.fightCharacters.length > 0;
		},
		async getFightState() {
			const result = await fetch('/api/fight/state');
			if (result.status === 401) return false;
			this.fightStates = await result.json() as FightState[];
			return true;
		},
		async getVisits() {
			const result = await fetch('/api/admin/visits');
			if (result.status === 401) return false;
			const visits = await result.json() as Record<string, (Omit<Visit, 'date'> & { date: string }) []>;
			this.visits = Object.keys(visits).reduce((list, y) =>
			{
				const date = moment(y, 'YYYY-MM-DD').format("YYYY-MM-DD");
				list[date] = visits[y].map(x => ({
					date: moment(x.date),
					id: x.id,
					character: x.character,
					wasHere: x.wasHere,
					canSkip: x.canSkip
				}));
				return list;
			}, {} as Record<string, Visit[]>)
			return true;
		}
	},
});
