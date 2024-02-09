import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HostingService {
  constructor(private http: HttpClient) { }

  public getDateRanges(): Observable<any[]> {
    return this.http.get<any[]>('/api/blockedDates');
  }

  public blockDates(startDate: string, endDate: string){
    return this.http.post('/api/blockedDates', {
      startDate: startDate,
      endDate: endDate
    }, {
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }
}

