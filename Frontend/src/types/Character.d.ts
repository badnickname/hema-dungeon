import type { Page } from './Page';

export type Character = {
	id: string;
	avatar: string;
	name: string;
	age: number;
	gender: string;
	story: string;
	power: number;
	agility: number;
	wisdom: number;
	vitality: number;
	pushUp?: number;
	pullUp?: number;
	abdominal?: number;
	runTwenty?: number;
	runFifteen?: number;
	rang?: number;
	score?: number;
	stamina?: number;
	rope?: number;
	author?: string;
	pages?: Page[];
	isDead?: boolean;
}
