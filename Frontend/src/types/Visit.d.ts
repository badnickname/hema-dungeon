import type { Character } from './Character';
import type { Moment } from 'moment';

export type Visit = {
	id: string;
	character: Character;
	date: Moment;
	wasHere: boolean;
	canSkip: boolean;
}
