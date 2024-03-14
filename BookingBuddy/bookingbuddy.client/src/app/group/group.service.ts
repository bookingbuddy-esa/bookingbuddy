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

  public getProperty(groupId: string) {
    return this.http.get(`${environment.apiUrl}/api/group/${groupId}`);
  }

  public createGroup(group: GroupCreate) {
    return this.http.post(`${environment.apiUrl}/api/group/create`, {
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

  public getGroups() {
    return this.http.get(`${environment.apiUrl}/api/group/`);
  }
}
