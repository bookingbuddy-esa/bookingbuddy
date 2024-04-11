import {Injectable, Component} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {BehaviorSubject, Observable, Subject, catchError, map, of} from 'rxjs';
import {environment} from "../../environments/environment";
import { Property, PropertyCreate} from "../models/property";
import {Block} from "@angular/compiler";


@Injectable({
  providedIn: 'root'
})
export class PropertyAdService {

  constructor(private http: HttpClient) {
  }

  public getProperty(propertyId: string) {
    return this.http.get(`${environment.apiUrl}/api/properties/${propertyId}`);
  }

  public getProperties(itemsPerPage: number, startIndex: number) {
    return this.http.get(`${environment.apiUrl}/api/properties?itemsPerPage=${itemsPerPage}&startIndex=${startIndex}`);
  }

  public getPropertiesSearch(search: string,itemsPerPage: number, startIndex: number) {
    return this.http.get<Property[]>(`${environment.apiUrl}/api/properties/search/?search=${search}&itemsPerPage=${itemsPerPage}&startIndex=${startIndex}`);
  }

  public getPropertiesCount(){
    return this.http.get(`${environment.apiUrl}/api/properties/count`);
  }

  public createPropertyAd(property: PropertyCreate) {
    return this.http.post(`${environment.apiUrl}/api/properties/create`, {
      name: property.name,
      location: property.location,
      pricePerNight: property.pricePerNight,
      maxGuestsNumber: property.maxGuestNumber,
      roomsNumber:property.roomsNumber,
      description: property.description,
      imagesUrl: property.imagesUrl,
      amenities: property.amenities,
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'text'
    }).pipe<boolean>(map((res: HttpResponse<string>) => {
        return res.ok;
      }));
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


  public getPropertyBlockedDates(propertyId: string){
    return this.http.get<ReturnedBlockedDates[]>(`${environment.apiUrl}/api/properties/blockedDates/${propertyId}`);
  }

  public getPropertyDiscounts(propertyId: string){
    return this.http.get<ReturnedDiscount[]>(`${environment.apiUrl}/api/properties/discounts/${propertyId}`);
  }

  public isPropertyInFavorites(propertyId: string): Observable<boolean> {
    return this.http.get<boolean>(`${environment.apiUrl}/api/properties/favorites/check/${propertyId}`, {
      withCredentials: true
    });
  }
}

export interface ReturnedDiscount {
  discountId: string,
  propertyId: string,
  discountAmount: number,
  startDate: Date,
  endDate: Date,
}

export interface ReturnedBlockedDates {
  id: string,
  propertyId: string
  start: string,
  end: string,
}
