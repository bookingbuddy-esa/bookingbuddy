import { Injectable, Component } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';
import { environment } from "../../environments/environment";
import { Group, GroupCreate } from "../models/group";

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(private http: HttpClient) {
  }

  public getGroups() {
    return this.http.get(`${environment.apiUrl}/api/groups/`);
  }

  public getGroup(groupId: string) {
    return this.http.get(`${environment.apiUrl}/api/groups/${groupId}`);
  }

  public getGroupByUserId(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/api/groups/user/${userId}`);
  }

  public createGroup(group: GroupCreate) {
    return this.http.post(`${environment.apiUrl}/api/groups/create`, {
      name: group.name,
      properties: group.properties,
      members: group.members,
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    })
      .pipe<boolean>(map((res: HttpResponse<string>) => {
        return res.ok;
      }));
  }
}
