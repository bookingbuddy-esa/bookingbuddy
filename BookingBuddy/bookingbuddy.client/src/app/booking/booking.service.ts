import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  constructor(private http: HttpClient) {}

  public getBookings() {
    return this.http.get(`${environment.apiUrl}/api/bookings/`, { withCredentials: true });
  }

  public getBookingMessages(bookingId: string){
    return this.http.get(`${environment.apiUrl}/api/bookings/${bookingId}/messages`, { withCredentials: true });
  }

  public sendBookingMessage(bookingId: string, message: string){
    return this.http.post(`${environment.apiUrl}/api/bookings/${bookingId}/messages`, { message }, { withCredentials: true });
  }
}
