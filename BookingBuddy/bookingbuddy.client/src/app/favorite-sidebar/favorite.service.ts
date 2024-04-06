import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, map, tap } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class FavoriteService {

  private _isSidebarOpen = new BehaviorSubject<boolean>(false);
  private _propertyAddedSubject = new Subject<void>();
  propertyAdded$ = this._propertyAddedSubject.asObservable();

  constructor(private http: HttpClient) { }
  get isSidebarOpen$() {
    return this._isSidebarOpen.asObservable();
  }

  toggleSidebar() {
    this._isSidebarOpen.next(!this._isSidebarOpen.value);
  }

  public getUserFavorites(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/api/properties/favorites/user/${userId}`);
  }

  addToFavorites(propertyId: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}/api/properties/favorites/add/${propertyId}`, {
      propertyId
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe(
      map((res: HttpResponse<string>) => res.ok),
      tap(() => {
        this._propertyAddedSubject.next(); 
      })
    );
  }

  removeFromFavorites(propertyId: string): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/api/properties/favorites/remove/${propertyId}`, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe(
      map((res: HttpResponse<string>) => res.ok),
      tap(() => {
        this._propertyAddedSubject.next();
      })
    );
  }
}
