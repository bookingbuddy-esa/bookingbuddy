import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';
import { UserInfo } from './authorize.dto';
import {environment} from "../../environments/environment";


@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {
  constructor(private http: HttpClient) { }

  private _authStateChanged: Subject<boolean> = new BehaviorSubject<boolean>(false);

  public onStateChanged() {
    return this._authStateChanged.asObservable();
  }

  // cookie-based login
  public signIn(email: string, password: string) {
    return this.http.post(`${environment.apiUrl}/login?useCookies=true`, {
      email: email,
      password: password
    }, {
      observe: 'response',
      responseType: 'text'
    })
      .pipe<boolean>(map((res: HttpResponse<string>) => {
        this._authStateChanged.next(res.ok);
        return res.ok;
      }));
  }

  // register new user
  public register(name: string, email: string, password: string) {
    return this.http.post(`${environment.apiUrl}/api/register`, {
      name: name,
      email: email,
      password: password
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  public confirmEmail(uid: string, token: string) {
    return this.http.post(`${environment.apiUrl}/api/confirmEmail`, {
      uid: uid,
      token: token
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  public resendConfirmationEmail(email: string) {
    return this.http.post(`${environment.apiUrl}/api/resendConfirmation`, {
      email: email
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  public checkConfirmationToken(uid: string, token: string) {
    return this.http.post(`${environment.apiUrl}/api/checkConfirmation`, {
      uid: uid,
      token: token
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  // recover user password by sending an email to the specified email
  public recoverPassword(email: string) {
    return this.http.post(`${environment.apiUrl}/api/forgotPassword`, {
      email: email
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  // reset user password
  public resetPassword(uid: string, token: string, newPassword: string) {
    return this.http.post( `${environment.apiUrl}/api/resetPassword`, {
      uid: uid,
      token: token,
      newPassword: newPassword
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  public checkResetToken(uid: string, token: string) {
    return this.http.post(`${environment.apiUrl}/api/checkResetPassword`, {
      uid: uid,
      token: token,
      newPassword: ''
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  // logout
  public signOut() {
    return this.http.post(`${environment.apiUrl}/api/logout`, {}, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      if (res.ok) {
        this._authStateChanged.next(false);
      }
      return res.ok;
    }));
  }

  // check if the user is authenticated. the endpoint is protected so 401 if not.
  public user() {
    return this.http.get<UserInfo>(`${environment.apiUrl}/api/manage/info`, {
      withCredentials: true
    }).pipe(catchError((_: HttpErrorResponse, __: Observable<UserInfo>) => {
      return of({} as UserInfo);
    }));
  }

  public manageUser(newName: string, newUserName: string, newEmail: string, newPassword: string, oldPassword: string) {
    return this.http.post<UserInfo>(`${environment.apiUrl}/api/manage/info`, {
      newName: newName,
      newUserName: newUserName,
      newEmail: newEmail,
      newPassword: newPassword,
      oldPassword: oldPassword
    }, {
      withCredentials: true,
    }).pipe(catchError((_: HttpErrorResponse, __: Observable<UserInfo>) => {
      return of({} as UserInfo);
    }));
  }

  public changePassword(newPassword: string, oldPassword: string) {
    return this.http.post(`${environment.apiUrl}/api/manage/changePassword`, {
      newPassword: newPassword,
      oldPassword: oldPassword
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  // is signed in when the call completes without error and the user has an email
  public isSignedIn(): Observable<boolean> {
    return this.user().pipe(
      map((userInfo) => {
        const valid = !!(userInfo && userInfo.email && userInfo.email.length > 0);
        return valid;
      }),
      catchError((_) => {
        return of(false);
      }));
  }
}
