export type Result = {
	firstUser: User;
	secondUser: User;
}

type User = {
	id: string;
	health: number;
	hits: number;
}
