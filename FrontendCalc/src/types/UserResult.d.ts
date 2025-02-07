import type { Spell } from './Spell';

export type UserResult = {
	id: string;
	health: number;
	scoreHealth: number;
	damage: number;
	spells: Spell[];
}
