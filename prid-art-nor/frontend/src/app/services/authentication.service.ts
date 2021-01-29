import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, flatMap } from 'rxjs/operators';
import { User } from '../models/user';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

  // l'utilisateur couramment connecté (undefined sinon)
  public currentUser: User;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    // au départ on récupère un éventuel utilisateur stocké dans le sessionStorage
    const data = JSON.parse(sessionStorage.getItem('currentUser'));
    this.currentUser = data ? new User(data) : null;
  }

  login(pseudo: string, password: string) {
    return this.http.post<User>(`${this.baseUrl}api/users/authenticate`, { pseudo, password })
      .pipe(map(u => {
        u = new User(u);
        // login successful if there's a jwt token in the response
        if (u && u.token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          sessionStorage.setItem('currentUser', JSON.stringify(u));
          this.currentUser = u;
        }

        return u;
      }));
  }

  logout() {
    // remove user from local storage to log user out
    sessionStorage.removeItem('currentUser');
    this.currentUser = null;
  }

  public isPseudoAvailable(pseudo: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}api/users/available/${pseudo}`);
}

public signup(pseudo: string, password: string, firstName: string, lastName: string, email: string, birthDate: Date): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}api/users/signup`, { pseudo: pseudo, password: password, firstName: firstName, lastName: lastName, email: email, birthDate: birthDate }).pipe(
        flatMap(res => this.login(pseudo, password)),
    );
}
}