import {Injectable, Component} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {BehaviorSubject, Observable, Subject, catchError, map, of} from 'rxjs';
import {environment} from "../../environments/environment";
import {Property, PropertyCreate} from "../models/property";


@Injectable({
  providedIn: 'root'
})
export class PropertyAdService {

  constructor(private http: HttpClient) {
  }

  public getProperty(propertyId: string) {
    return this.http.get(`${environment.apiUrl}/api/properties/${propertyId}`);
  }

  public createPropertyAd(property: PropertyCreate) {
    return this.http.post(`${environment.apiUrl}/api/properties/create`, {
      name: property.name,
      location: property.location,
      pricePerNight: property.pricePerNight,
      description: property.description,
      imagesUrl: property.imagesUrl,
      amenities: property.amenities,
    }, {
      withCredentials: true,
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

  public uploadImages(images: File[]): Observable<string[]> {
    const formData = new FormData();
    images.forEach(image => {
      formData.append('image', image);
    });

    return this.http.post(`${environment.apiUrl}/api/upload`, formData, {withCredentials: true})
      .pipe<string[]>(map((res: any) => {
        return res;
      }));
  }

  addToFavorites(propertyId: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}/api/properties/favorites/add/${propertyId}`, {
      propertyId
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
      return res.ok;
    }));
  }

  removeFromFavorites(propertyId: string): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/api/properties/favorites/remove/${propertyId}`, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe(
      map((res: HttpResponse<string>) => res.ok)
    );
  }

  public getPropertyBlockedDates(propertyId: string): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/api/properties/blockedDates/${propertyId}`);
  }

  public getPropertyDiscounts(propertyId: string): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/api/properties/discounts/${propertyId}`);
  }

  public isPropertyInFavorites(propertyId: string): Observable<boolean> {
    return this.http.get<boolean>(`${environment.apiUrl}/api/properties/favorites/check/${propertyId}`, {
      withCredentials: true
    });
  }

}
