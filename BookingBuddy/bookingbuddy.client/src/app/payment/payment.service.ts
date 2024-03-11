import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private http: HttpClient) { }

  public createOrder(propertyId: string, startDate: Date, endDate: Date, paymentMethod: string, orderType: string, numberOfGuests?: number, phoneNumber?: string){ // orderType: promote, book
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

  // TODO: remover -> apenas de teste para simular a confirmação do pagamento
  public confirmOrder(orderId: string, paymentId: string){
    var amount = 1.0
    var paymentDatetime = new Date().toLocaleString('pt-PT').replace(/(\d+)\/(\d+)\/(\d+), (\d+):(\d+):(\d+)/, '$1-$2-$3 $4:$5:$6')

    return this.http.get(`${environment.apiUrl}/api/payments/webhook?key=Ygh58zuWkpLL69E&orderId=${orderId}&amount=${amount}&requestId=${paymentId}&payment_datetime=${paymentDatetime}`);
  }
}
