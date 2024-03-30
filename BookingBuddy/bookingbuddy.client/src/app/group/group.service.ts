import {Injectable, Component} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {BehaviorSubject, Observable, Subject, catchError, map, of} from 'rxjs';
import {environment} from "../../environments/environment";
import {Group, GroupCreate} from "../models/group";
import {group} from '@angular/animations';

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(private http: HttpClient) {
  }

  public getGroups() {
    return this.http.get<Group[]>(`${environment.apiUrl}/api/groups/`, {
      withCredentials: true
    });
  }

  public getGroup(groupId: string) {
    return this.http.get<Group>(`${environment.apiUrl}/api/groups/${groupId}`, {withCredentials: true});
  }

  public getGroupsByUserId(userId: string) {
    return this.http.get<Group[]>(`${environment.apiUrl}/api/groups/user/${userId}`, {withCredentials: true});
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
    }).pipe<Group>((map((res: HttpResponse<any>) => JSON.parse(res.body))));
  }

  public addMemberToGroup(groupId: string) {
    return this.http.put(`${environment.apiUrl}/api/groups/addMember?groupId=${groupId}`, {},
      {
        withCredentials: true,
        observe: 'response',
      }).pipe<boolean>((map((res: HttpResponse<any>) => res.ok)));
  }

  public updateGroupAction(groupId: string, groupAction: string, socketId: string | undefined = undefined) {
    return this.http.put(`${environment.apiUrl}/api/groups/updateGroupAction`, {
        groupId: groupId,
        groupAction: groupAction,
        socketId: socketId
      },
      {
        withCredentials: true,
        observe: 'response',
        responseType: 'text'
      }).pipe<boolean>(map((res: HttpResponse<any>) => res.ok));
  }

  public updateChosenProperty(groupId: string, propertyId: string, socketId: string | undefined = undefined) {
    return this.http.put(`${environment.apiUrl}/api/groups/updateChosenProperty`, {
        groupId: groupId,
        propertyId: propertyId,
        socketId: socketId
      },
      {
        withCredentials: true,
        observe: 'response',
        responseType: 'text'
      }).pipe<boolean>(map((res: HttpResponse<any>) => res.ok));
  }

  public addPropertyToGroup(groupId: string, propertyId: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/api/groups/addProperty?groupId=${groupId}&propertyId=${propertyId}`, {},
      {
        withCredentials: true,
        observe: 'response',
      }).pipe(map((res: HttpResponse<any>) => {
      return res.body;
    }));
  }

  public removePropertyFromGroup(groupId: string, propertyId: string, socketId: string | undefined = undefined) {
    return this.http.put(`${environment.apiUrl}/api/groups/removeProperty`, {
        groupId: groupId,
        propertyId: propertyId,
        socketId: socketId
      },
      {
        withCredentials: true,
        responseType: 'text',
        observe: 'response',
      }).pipe<boolean>(map((res: HttpResponse<any>) => res.ok)
    );
  }

  public deleteGroup(groupId: string, socketId: string | undefined = undefined): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/api/groups/delete/${groupId}${socketId ? `?socketId=${socketId}` : ""}`, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe(map((res: HttpResponse<string>) => res.ok));
  }

  public addPropertyVote(groupId: string, propertyId: string, socketId: string | undefined = undefined) {
    return this.http.put(`${environment.apiUrl}/api/groups/addPropertyVote`, {
        groupId: groupId,
        propertyId: propertyId,
        socketId: socketId
      },
      {
        withCredentials: true,
        observe: 'response',
        responseType: 'text'
      }).pipe<boolean>(map((res: HttpResponse<any>) => res.ok));
  }

  public updatePropertyVote(groupId: string, propertyId: string, socketId: string | undefined = undefined) {
    return this.http.put(`${environment.apiUrl}/api/groups/updatePropertyVote`, {
        groupId: groupId,
        propertyId: propertyId,
        socketId: socketId
      },
      {
        withCredentials: true,
        observe: 'response',
        responseType: 'text'
      }).pipe<boolean>(map((res: HttpResponse<any>) => res.ok));
  }

  public removePropertyVote(groupId: string, propertyId: string, socketId: string | undefined = undefined) {
    return this.http.put(`${environment.apiUrl}/api/groups/removePropertyVote`, {
        groupId: groupId,
        propertyId: propertyId,
        socketId: socketId
      },
      {
        withCredentials: true,
        observe: 'response',
        responseType: 'text'
      }).pipe<boolean>(map((res: HttpResponse<any>) => res.ok));
  }
}
