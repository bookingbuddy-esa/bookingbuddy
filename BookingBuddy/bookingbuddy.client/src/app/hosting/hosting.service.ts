import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HostingService {
  constructor(private http: HttpClient) { }

  public getPropertyBlockedDates(propertyId: string): Observable<any[]> {
    return this.http.get<any[]>(`/api/blockedDates/property/${propertyId}`);
  }

  public blockDates(startDate: string, endDate: string, propertyId: string){
    return this.http.post('/api/blockedDates', {
      startDate: startDate,
      endDate: endDate,
      propertyId: propertyId
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  public getPropertiesByUserId(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`/api/properties/user/${userId}`);
  }

  public unblockDates(id: number): Observable<boolean> {
    return this.http.delete(`/api/blockedDates/unblock/${id}`, {
      observe: 'response',
      responseType: 'text'
    }).pipe(
      map((res: HttpResponse<string>) => res.ok)
    );
  }
}

