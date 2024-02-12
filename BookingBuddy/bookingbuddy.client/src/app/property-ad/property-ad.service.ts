import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class PropertyAdService {

  constructor(private http: HttpClient) { }

  public getProperty(propertyId: string) {
    return this.http.get(`${environment.apiUrl}/api/properties/${propertyId}`);
  }

  public createPropertyAd(name: string, location: string, pricePerNight: number, description: string,   imagesUrl: string[], amenityIds : string[] ) {
    return this.http.post(`${environment.apiUrl}/api/properties/create`, {
      name: name,
      location: location,
      pricePerNight: pricePerNight,
      description : description,
      imagesUrl: imagesUrl,
      amenityIds: amenityIds,
    }, {
      observe: 'response',
      responseType: 'text'
    })
      .pipe<boolean>(map((res: HttpResponse<string>) => {
        return res.ok;
      }));
  }

  public getProperties() {
    return this.http.get(`${environment.apiUrl}/api/properties/`);
  }
}
