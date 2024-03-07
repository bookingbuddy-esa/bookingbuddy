import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class FavoriteService {

  private _isSidebarOpen = new BehaviorSubject<boolean>(false);

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
}
