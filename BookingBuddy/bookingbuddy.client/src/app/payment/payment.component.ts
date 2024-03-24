import {NgIf} from '@angular/common';
import {Component, Input} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {OrderService} from './order.service';
import {Payment} from "../models/payment";
import {environment} from "../../environments/environment";
import {WebsocketMessage} from "../models/websocket-message";

@Component({
  standalone: true,
  imports: [NgIf, FormsModule],
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})

export class PaymentComponent {
  @Input() data!: any;
  nif: string | undefined = undefined;
  paymentMethod: string = "multibanco";
  phoneNumber: string | undefined = undefined;
  currentPhase: number = 1;
  paymentResponse: any;
  payment: Payment | undefined = undefined;

  constructor(private orderService: OrderService) {
  }

  public addPaymentMethod(newPaymentMethod: string): void {
    this.paymentMethod = newPaymentMethod
  }

  public submitPaymentRequest(): void {
    this.data = {
      ...this.data,
      paymentMethod: this.paymentMethod,
      phoneNumber: this.phoneNumber || null,
      nif: this.nif || null
    };
    //console.log(JSON.stringify(this.data));

    if(this.data.orderType == 'group-booking'){
      this.orderService.payGroupBooking(this.data.groupBookingId, this.paymentMethod, this.data.phoneNumber).forEach((response) => {
        if(response){
          console.log("PAGAMENTO GROUP: " + JSON.stringify(response));
          this.currentPhase = 2;
          this.paymentResponse = response;

          this.payment = {
            paymentId: this.paymentResponse.paymentId,
            method: this.paymentResponse.method,
            amount: this.paymentResponse.amount,
            createdAt: new Date(this.paymentResponse.createdAt),
            status: this.paymentResponse.status,
            expiryDate: new Date(this.paymentResponse.expiryDate),
            entity: this.paymentResponse.entity,
            reference: this.paymentResponse.reference
          }
        }
      }).catch((err) => {
        console.error(err);
      });
    } else {
      this.orderService.createOrder(this.data.propertyId, this.data.startDate, this.data.endDate, this.paymentMethod, this.data.orderType, this.data.numberOfGuests, this.data.phoneNumber).forEach((response) => {
        if (response) {
          this.currentPhase = 2;
          this.paymentResponse = response;
          this.payment = {
            paymentId: this.paymentResponse.payment.paymentId,
            method: this.paymentResponse.payment.method,
            amount: this.paymentResponse.payment.amount,
            createdAt: new Date(this.paymentResponse.payment.createdAt),
            status: this.paymentResponse.payment.status,
            expiryDate: new Date(this.paymentResponse.payment.expiryDate),
            entity: this.paymentResponse.payment.entity,
            reference: this.paymentResponse.payment.reference
          }
          console.log(response);

          if (this.payment) {
            let url = environment.apiUrl;
            url = url.replace('https', 'wss');
            let ws = new WebSocket(`${url}/api/payments/ws?paymentId=${this.payment.paymentId}`);
            ws.onmessage = (event) => {
              let data = JSON.parse(event.data);
              if (data) {
                this.payment = {
                  paymentId: data["PaymentId"],
                  amount: data["Amount"],
                  method: data["Method"],
                  status: data["Status"],
                  expiryDate: new Date(data["ExpiryDate"]),
                  createdAt: new Date(data["CreatedAt"]),
                  entity: data["Entity"],
                  reference: data["Reference"],
                }
                console.log(this.payment);
                if (this.payment.status.toUpperCase() === 'PAID') {
                  ws.close();
                }
              }
            }
          }
        }
      }).catch((err) => {
        console.error("Erro no servidor: " + JSON.stringify(err));
      });
    }
  }

  // TODO: remover -> apenas de teste para simular a confirmação do pagamento
  public confirmarPagamento(): void {
    if(this.data.orderType == 'group-booking'){
      this.orderService.confirmOrder(this.data.groupBookingId, this.payment?.paymentId ?? "").forEach((response) => {
        if(response){
          console.log(response);
        }
        this.currentPhase = 3;
      }).catch((err) => {
        console.error("Erro no servidor: " + JSON.stringify(err));
      });
    } else {
      this.orderService.confirmOrder(this.paymentResponse.orderId, this.payment?.paymentId ?? "").forEach((response) => {
        if (response) {
          console.log(response);
        }
        this.currentPhase = 3;
      }).catch((err) => {
        console.error("Erro no servidor: " + JSON.stringify(err));
      });
    }
  }
}


