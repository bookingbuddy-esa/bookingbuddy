import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PropertyAdService {

  constructor(private http: HttpClient) { }

  public getProperty(propertyId: string) {
    return this.http.get('/api/properties/' + propertyId);
  }

  public getProperties() {
    return this.http.get('/api/properties/');
  }
}
