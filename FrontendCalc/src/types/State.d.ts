export type State = {
	firstUser: User;
	secondUser: User;
}

type User = {
	id: string;
	health?: number;
	score?: number;
	damage?: number;
}
