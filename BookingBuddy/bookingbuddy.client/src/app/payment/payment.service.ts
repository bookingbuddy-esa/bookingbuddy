import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private http: HttpClient) { }

  public createOrder(propertyId: string, startDate: Date, endDate: Date, paymentMethod: string, orderType: string, phoneNumber?: string){ // orderType: promote, book
    return this.http.post(`${environment.apiUrl}/api/orders/${orderType}`, {
      propertyId: propertyId,
      startDate: startDate,
      endDate: endDate,
      paymentMethod: paymentMethod,
      phoneNumber: phoneNumber
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'json'
    }).pipe<boolean>(map((res: HttpResponse<any>) => {
      return res.body;
    }));
  }

  public confirmOrder(orderId: string, paymentId: string){
    return this.http.get(`${environment.apiUrl}/api/payments/webhook?key=Ygh58zuWkpLL69E&orderId=${orderId}&amount=35.0&requestId=${paymentId}&payment_datetime=19-02-2024 12:36:00`);
  }
}
