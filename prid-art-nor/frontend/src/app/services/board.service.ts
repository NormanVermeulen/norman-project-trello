import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { map, flatMap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { Board } from '../models/board';
import { List } from '../models/list';
import { Card } from '../models/card';
import { User } from '../models/user';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({ providedIn: 'root' })
export class BoardService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }
 
    getBoards() {
        return this.http.get<Board[]>(`${this.baseUrl}api/boards`).pipe(
            map(res => res.map(u => new Board(u))));
    }

    getBoardById(id : number){
        return this.http.get<Board>(`${this.baseUrl}api/boards/${id}`).pipe(
            map(b => !b ? null : new Board(b)),
            catchError(err => of(null))
        );
        
    }

    getBoardsByOwner(userId: number){
        return this.http.get<Board[]>(`${this.baseUrl}api/boards/Owner/${userId}`).pipe(
            map(res => res.map(u => new Board(u))));
    }

    getBoardsByCollabs(userId: number){
        return this.http.get<Board[]>(`${this.baseUrl}api/boards/Collabs/${userId}`).pipe(
            map(res => res.map(u => new Board(u))));
    }


    getListById(id : number){
        return this.http.get<List>(`${this.baseUrl}api/boards/${id}`).pipe(
            map(l => !l ? null : new List(l)),
            catchError(err => of(null))
        );
        
    }

    getCardByName(boardId: number, cardName: string){
        return this.http.get<Card>(`${this.baseUrl}api/boards/${boardId}/${cardName}`).pipe(
            map(c => !c ? null : new Card(c)),
            catchError(err => of(null))
        );
    }

    getCollaboraters(boardId : number){
        return this.http.get<User[]>(`${this.baseUrl}api/boards/collab/${boardId}`).pipe(
            map(res => res.map(u => new User(u)))
        );
    }

    getParticipaters(cardId : number){
        return this.http.get<User[]>(`${this.baseUrl}api/boards/part/${cardId}`).pipe(
            map(res => res.map(u => new User(u)))
        );
    }

    public add(b: Board): Observable<boolean> {
        return this.http.post<Board>(`${this.baseUrl}api/boards`, b).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public delete(b: Board): Observable<boolean> {
        return this.http.delete<boolean>(`${this.baseUrl}api/boards/${b.id}`).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public update(b: Board): Observable<boolean> {
        return this.http.put<Board>(`${this.baseUrl}api/boards/${b.id}`, b).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public addList(l: List, id : number): Observable<boolean> {
        return this.http.post<Board>(`${this.baseUrl}api/boards/${id}`, l).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
        
    }

    public updateList(l: List): Observable<boolean>{
        return this.http.put<List>(`${this.baseUrl}api/boards/updateList`, l).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            }) 
        );
    }

    public updatePosList(l: List, list1Id: number, list2Id: number): Observable<boolean> {
        return this.http.put<List>(`${this.baseUrl}api/boards/updatePosList/${list1Id}/${list2Id}`, l).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public updateCardsInList(l : List, c : Card, ci : number): Observable<boolean> {
        return this.http.put<List>(`${this.baseUrl}api/boards/updateList/${c.id}/${ci}`, l).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public deleteList(l: List): Observable<boolean> {
        return this.http.delete<boolean>(`${this.baseUrl}api/boards/list/${l.id}`).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    

    public addCard(c: Card, boardId : number, listId : number): Observable<Card> {
        return this.http.post<Board>(`${this.baseUrl}api/boards/${boardId}/${listId}`, c).pipe(
           // map(res => true),
            catchError(err => {
                console.error(err);
                return of(null);
            })
        );
        
    }

    public updatePosCard(c: Card, cardId : number): Observable<boolean> {
        return this.http.put<Card>(`${this.baseUrl}api/boards/updatePosCard/${cardId}`, c).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public updateCard(c: Card): Observable<boolean> {
        return this.http.put<Card>(`${this.baseUrl}api/boards/updateCard`, c).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public deleteCard(c: Card): Observable<boolean> {
        return this.http.delete<boolean>(`${this.baseUrl}api/boards/card/${c.id}`).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

   

}