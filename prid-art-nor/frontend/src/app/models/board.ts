import { User } from "./user";
import {List} from "./list";

export class Board {
    id: number;
    name: string;
    owner: User;
    ownerId: number;
    collaborations?: number[];
    lists: (List)[];


constructor(data: any) {
    if (data) {
      this.id = data.id;
      this.name = data.name;
      this.owner = data.owner;
      this.ownerId = data.ownerId;
      this.collaborations = data.collaborations;
      this.lists = data.lists;
    }
}
}