import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Character } from '../types/Character';

export const useStore = defineStore('app', {
	state: () => {
		const character = ref<Character>({
			name: 'test', story: 'test', age: 0, avatar: 'img/test.png', gender: 'male', agility: 0, power: 0, vitality: 0, wisdom: 0
		});
		const visibleCharacter = ref<Character>();
		const characters = ref<Character[]>([{
			name: 'test', story: 'test', age: 0, avatar: 'img/test.png', gender: 'male', agility: 5, power: 3, vitality: 2, wisdom: 1
		}, {
			name: 'test_2', story: 'test', age: 0, avatar: 'img/test.png', gender: 'male', agility: 0, power: 0, vitality: 0, wisdom: 0
		}]);
		return { character, characters, visibleCharacter };
	}, actions: {
		async getCharacter() {
			// return true;
			const result = await fetch('/api/user');
			if (result.status === 401) return false;
			this.character = await result.json() as Character;
			await this.getCharacters();
			return true;
		},
		async getCharacters() {
			const result = await fetch('/api/users');
			this.characters = await result.json() as Character[];
		}
	},
});
