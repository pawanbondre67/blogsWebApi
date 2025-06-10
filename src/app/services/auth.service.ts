import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public tokenSubject: BehaviorSubject<string | null>;
  isAuthenticated$: Observable<boolean>;

  constructor(private http: HttpClient) {
    let token: string | null = null;
    if (typeof window !== 'undefined' && window.localStorage) {
      token = localStorage.getItem('token');
      console.log('AuthService initialized. Initial token:', localStorage.getItem('token'));
    }
    this.tokenSubject = new BehaviorSubject<string | null>(token);
    this.isAuthenticated$ = this.tokenSubject.asObservable().pipe(map(token => !!token));
  }

  login(email: string, password: string): Observable<boolean> {
    console.log('Attempting login with email:', email);
    return this.http.post<{ token: string }>(`${environment.apiUrl}/Auth/login`, { email, password })
      .pipe(
        tap(response => {
          if (typeof window !== 'undefined' && window.localStorage) {
            localStorage.setItem('token', response.token);
          }
          this.tokenSubject.next(response.token);
        }),
        map(() => {
          console.log('Login successful, returning true');
          return true;
        }),
        catchError(err => {
          return of(false);
        })
      );
  }

  register(email: string, password: string, fullName: string): Observable<boolean> {
    return this.http.post<{ token: string }>(`${environment.apiUrl}/Auth/register`, { email, password, fullName })
      .pipe(
        tap(response => {
          if (typeof window !== 'undefined' && window.localStorage) {
            localStorage.setItem('token', response.token);
          }
          this.tokenSubject.next(response.token);
        }),
        map(() => true),
        catchError(err => {
          return of(false);
        })
      );
  }

  logout(): void {
    if (typeof window !== 'undefined' && window.localStorage) {
      localStorage.removeItem('token');
    }
    this.tokenSubject.next(null);
  }

  getToken(): Observable<string | null> {
    return this.tokenSubject.asObservable();
  }

  getUserId(): Observable<string> {
    return this.getToken().pipe(
      map(token => {
        if (token) {
          try {
            const decoded: any = jwt_decode.jwtDecode(token);
            return decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || '';
          } catch (e) {
            return '';
          }
        }
        return '';
      })
    );
  }
}
