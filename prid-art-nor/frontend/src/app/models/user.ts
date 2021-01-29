import { Board } from "./board";
import { Card } from "./card";


export enum Role {
  Member = 0,
  Manager = 1,
  Admin = 2
}

export class User {
  id: number;
  pseudo: string;
  password: string;
  email: string;
  lastName: string;
  firstName: string;
  birthDate: string;
  role: Role;
  token: string;
  boards: (string | Board)[];
  cards: (string | Card)[];


  constructor(data: any) {
    if (data) {
      this.id = data.id;
      this.pseudo = data.pseudo;
      this.password = data.password;
      this.email = data.email;
      this.lastName = data.lastName;
      this.firstName = data.firstName;
      this.birthDate = data.birthDate &&
        data.birthDate.length > 10 ? data.birthDate.substring(0, 10) : data.birthDate;
      this.role = data.role || Role.Member;
      this.token = data.token;
      this.boards = data.boards;
      this.cards = data.cards;
    }
  }

  public get roleAsString(): string {
    return Role[this.role];
  }

}

export interface IFriend {
  id: number;
  pseudo: string;
  firstName: string;
  lastName: string;
}