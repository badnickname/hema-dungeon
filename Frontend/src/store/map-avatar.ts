import type { Character } from '../types/Character';

export function mapAvatar(character: Character) {
	if (character.avatar) character.avatar = `images/${character.avatar}`;
	return character;
}
