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
    return this.http.get(`${environment.apiUrl}/api/groups/`, { withCredentials: true });
  }

  public getGroup(groupId: string) {
    return this.http.get(`${environment.apiUrl}/api/groups/${groupId}`, { withCredentials: true });
  }

  public getGroupsByUserId(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/api/groups/user/${userId}`, { withCredentials: true });
  }

  public createGroup(group: GroupCreate) {
    return this.http.post(`${environment.apiUrl}/api/groups/create`, {
      name: group.name,
      propertyId: group.propertyId,
      memberEmails: group.members,
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe(map((res: any) => {
      return res.body;
    }));
  }

  public addMemberToGroup(groupId: string) {
    return this.http.put(`${environment.apiUrl}/api/groups/addMember?groupId=${groupId}`, {}, 
    {
      withCredentials: true,
      observe: 'response',
      //responseType: 'text'
    }).pipe(map((res: any) => { 
      return res.body as Group; 
    }));
  }

  public deleteGroup(groupId: string): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/api/groups/delete/${groupId}`, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe(
      map((res: HttpResponse<string>) => res.ok)
    );
  }

  public getGroupMessages(groupId: string) {
    return this.http.get(`${environment.apiUrl}/api/groups/${groupId}/messages`, { withCredentials: true });
  }

  public sendGroupMessage(groupId: string, message: string) {
    console.log("Mensagem: " + message + " Grupo: " + groupId);
    return this.http.post(`${environment.apiUrl}/api/groups/${groupId}/messages`, {
      message
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

}
