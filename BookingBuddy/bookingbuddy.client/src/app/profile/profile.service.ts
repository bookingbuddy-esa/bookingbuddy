import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable, catchError, map, of } from 'rxjs';
import { ProfileInfo } from '../models/profile';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private http: HttpClient) {
  }

  public getProfile(userId: string): Observable<ProfileInfo>{
    return this.http.get<ProfileInfo>(`${environment.apiUrl}/api/profile/${userId}`);
  }

  public saveProfile() {
    return this.http.put(`${environment.apiUrl}/api/profile/saveProfile`, {}, 
    {
      withCredentials: true,
      observe: 'response',
      //responseType: 'text'
    }).pipe(map((res: any) => { 
      return res.body; 
    }));
  }
}
