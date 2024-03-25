import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  constructor(private http: HttpClient) {}

  public getBookings() {
    return this.http.get(`${environment.apiUrl}/api/bookings/`, { withCredentials: true });
  }

  public getBookingMessages(bookingId: string) {
    console.log("BookingId: " + bookingId);
    return this.http.get(`${environment.apiUrl}/api/bookings/${bookingId}/messages`, { withCredentials: true });
  }

  public sendBookingMessage(bookingId: string, message: string) {
    console.log("BookingId: " + bookingId + "Mensagem: " + message);
    return this.http.post(`${environment.apiUrl}/api/bookings/${bookingId}/messages`, {
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
