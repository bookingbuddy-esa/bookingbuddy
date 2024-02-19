import { NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PaymentService } from './payment.service';

@Component({
  standalone: true,
  imports: [NgIf, FormsModule],
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})

export class PaymentComponent {
  currentPhase: number = 1;
  @Input() data!: any;
  paymentMethod: string = "multibanco";
  phoneNumber: string | undefined;
  paymentResponse: any;

  constructor(private paymentService: PaymentService){}

  public dataToJson(): string {
    return JSON.stringify(this.data);
  }

  public addPaymentMethod(newPaymentMethod: string): void {
    this.paymentMethod = newPaymentMethod
  }

  public submitPayment(): void {
    this.data = {... this.data, paymentMethod: this.paymentMethod, phoneNumber: this.phoneNumber};
    console.log(JSON.stringify(this.data));

    this.paymentService.createOrder(this.data.propertyId, this.data.startDate, this.data.endDate, this.paymentMethod, "promote", this.phoneNumber).forEach((response) => {
      if(response){
        this.currentPhase = 2;
        this.paymentResponse = response;
        console.log(response);
      }
    }).catch((err) => {
      console.error("Erro no servidor: " + err);
    });
  }

  public confirmarPagamento(): void {
    this.paymentService.confirmOrder(this.paymentResponse.orderId, this.paymentResponse.paymentId).forEach((response) => {
      if(response){
        console.log(response);
      }
    }).catch((err) => {
      console.error("Erro no servidor: " + err);
    });
  }
}


