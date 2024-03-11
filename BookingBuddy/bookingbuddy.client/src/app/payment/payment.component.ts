import {NgIf} from '@angular/common';
import {Component, Input} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {PaymentService} from './payment.service';
import {Payment} from "../models/payment";
import {environment} from "../../environments/environment";

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

  constructor(private paymentService: PaymentService) {
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
    console.log(JSON.stringify(this.data));

    this.paymentService.createOrder(this.data.propertyId, this.data.startDate, this.data.endDate, this.paymentMethod, this.data.orderType, this.data.numberOfGuests, this.data.phoneNumber).forEach((response) => {
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

        console.log(this.payment);
        if(this.payment){
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
            }
          }
        }
      }
    }).catch((err) => {
      console.error("Erro no servidor: " + JSON.stringify(err));
    });
  }

  // TODO: remover -> apenas de teste para simular a confirmação do pagamento
  public confirmarPagamento(): void {
    this.paymentService.confirmOrder(this.paymentResponse.orderId, this.paymentResponse.paymentId).forEach((response) => {
      if (response) {
        console.log(response);
        alert('Esta mensagem é de teste - O pagamento foi confirmado com sucesso!')
      }
    }).catch((err) => {
      console.error("Erro no servidor: " + JSON.stringify(err));
    });
  }
}


