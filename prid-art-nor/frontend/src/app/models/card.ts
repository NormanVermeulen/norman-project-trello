import { User } from "./user";

export class Card {
    id?: number;
    name?: string;
    author?: User;
    authorId? : number;
    participaters?: number[];
    listId? : number;
    pos? : number;

    constructor(data: any) {
        if (data) {
          this.id = data.id;
          this.name = data.name;
          this.author = data.author;
          this.authorId = data.authorId;
          this.participaters = data.participaters;
          this.listId = data.listId;
          this.pos = data.pos;
        }
    }
}