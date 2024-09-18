import type { FightCharacter } from './FightCharacter';

export type FightState = {
	character: FightCharacter;
	scoreHealth: number;
	damage: number;
	isOpened?: boolean;
	name: string;
	description: string;
	calculated: boolean;
}
