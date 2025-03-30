import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse, Role } from '../models/auth-models';
import { BehaviorSubject, Observable, tap } from 'rxjs';

const endpoint = environment.apiUrl + 'api/auth';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) {}

  private authStatus = new BehaviorSubject<boolean>(this.isAuthenticated());

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${endpoint}/login`, request).pipe(
      tap((response: LoginResponse) => {
        localStorage.setItem("token", response.token);
        this.authStatus.next(true);
      }),
    );
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${endpoint}/register`, request);
  }

  logout() {
    localStorage.removeItem('token');
    this.authStatus.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getAuthStatus() {
    return this.authStatus.asObservable();
  }

  getUserRoles() {
    try {
      const token = localStorage.getItem('token');
      if (token) {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const userRoles: string[] = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        return userRoles;
      }
    }
    catch {
      return [];
    }
    return [];
  }

  isInRole(role: Role){
    const userRoles = this.getUserRoles();
    if (userRoles?.includes(Role[role])) {
      return true
    }
    return false;
  }

}
