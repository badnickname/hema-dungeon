import type { Spell } from './Spell';

export type State = {
	firstUser: User;
	secondUser: User;
}

type User = {
	id: string;
	health?: number;
	score?: number;
	damage?: number;
	spells: Spell[];
}
