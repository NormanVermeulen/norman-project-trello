import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { map, flatMap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { User, IFriend } from '../models/user';

@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    getAll() {
        return this.http.get<User[]>(`${this.baseUrl}api/users`)
            .pipe(map(res => res.map(u => new User(u))));
    }

    getByPseudo(pseudo: string) {
        return this.http.get<User>(`${this.baseUrl}api/users/${pseudo}`).pipe(
            map(u => !u ? null : new User(u)),
            catchError(err => of(null))
        );
    }

    getById(id: number) {
        return this.http.get<User>(`${this.baseUrl}api/users/userId/${id}`).pipe(
            map(u => !u ? null : new User(u)),
            catchError(err => of(null))
        );
    }

    public update(u: User): Observable<boolean> {
        return this.http.put<User>(`${this.baseUrl}api/users/${u.pseudo}`, u).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public delete(u: User): Observable<boolean> {
        return this.http.delete<boolean>(`${this.baseUrl}api/users/${u.pseudo}`).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public add(u: User): Observable<boolean> {
        return this.http.post<User>(`${this.baseUrl}api/users`, u).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

}