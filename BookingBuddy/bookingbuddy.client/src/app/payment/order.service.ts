import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {map} from 'rxjs';
import {GroupBookingOrder} from "../models/order";

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient) {
  }

  public createOrder(propertyId: string, startDate: Date, endDate: Date, paymentMethod: string, orderType: string, numberOfGuests?: number, phoneNumber?: string) { // orderType: promote, book
    return this.http.post(`${environment.apiUrl}/api/orders/${orderType}`, {
      propertyId: propertyId,
      startDate: startDate,
      endDate: endDate,
      paymentMethod: paymentMethod,
      phoneNumber: phoneNumber,
      numberOfGuests: numberOfGuests
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'json'
    }).pipe<boolean>(map((res: HttpResponse<any>) => {
      return res.body;
    }));
  }

  public getOrder(orderId: string) {
    return this.http.get<GroupBookingOrder>(`${environment.apiUrl}/api/orders/${orderId}`, {
      withCredentials: true
    });
  }

  public createGroupBookingOrder(groupId: string, startDate: Date, endDate: Date) {
    return this.http.post(`${environment.apiUrl}/api/orders/group-booking`, {
      groupId: groupId,
      startDate: startDate,
      endDate: endDate
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'json'
    }).pipe(map((res: any) => {
      return res.body;
    }));
  }

  public payGroupBooking(groupBookingId: string, paymentMethod: string, phoneNumber?: string) {
    return this.http.post(`${environment.apiUrl}/api/orders/group-booking/pay`, {
      groupBookingId: groupBookingId,
      paymentMethod: paymentMethod,
      phoneNumber: phoneNumber
    }, {
      withCredentials: true,
      observe: 'response',
      responseType: 'json'
    }).pipe(map((res: any) => {
      return res.body;
    }));
  }

  // TODO: remover -> apenas de teste para simular a confirmação do pagamento
  public confirmOrder(orderId: string, paymentId: string) {
    var amount = 1.0
    var paymentDatetime = new Date().toLocaleString('pt-PT').replace(/(\d+)\/(\d+)\/(\d+), (\d+):(\d+):(\d+)/, '$1-$2-$3 $4:$5:$6')

    return this.http.get(`${environment.apiUrl}/api/payments/webhook?key=Ygh58zuWkpLL69E&orderId=${orderId}&amount=${amount}&requestId=${paymentId}&payment_datetime=${paymentDatetime}`);
  }
}
