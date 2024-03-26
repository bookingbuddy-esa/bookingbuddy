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

  public updateDescription(description: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/api/profile/updateDescription`, {description}, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe(map((res: any) => { 
      return res.body;
    }));
  }

  public updateProfilePicture(imageUrl: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/api/profile/updateProfilePicture`, {imageUrl}, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe(map((res: any) => {
      return res.body;
    }));
  }

  public uploadProfilePicture(images: File[]): Observable<string[]> {
    const formData = new FormData();
    images.forEach(image => {
      formData.append('image', image);
    });

    return this.http.post(`${environment.apiUrl}/api/upload`, formData, {withCredentials: true})
      .pipe<string[]>(map((res: any) => {
        return res;
      }));
  }
}
