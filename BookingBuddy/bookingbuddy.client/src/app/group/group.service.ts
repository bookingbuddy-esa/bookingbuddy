import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(private http: HttpClient) { }

  public getGroup(groupId: string) {
    return this.http.get(`${environment.apiUrl}/api/groups/${groupId}`);
  }

  public getGroupsByUserId(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/api/groups/user/${userId}`);
  }
}
