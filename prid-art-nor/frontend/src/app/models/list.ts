import { User } from "./user";
import {Card} from "./card";

export class List {
    id?: number;
    name?: string;
    cards?: (Card)[];
    boardId?: number;
    pos?: number;

    constructor(data: any) {
        if (data) {
          this.id = data.id;
          this.name = data.name;
          this.cards = data.cards;
          this.boardId = data.boardId;
          this.pos = data.pos;
        }
    }
}