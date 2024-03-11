import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-transaction-handler',
  templateUrl: './transaction-handler.component.html',
  styleUrl: './transaction-handler.component.css'
})

export class TransactionHandlerComponent implements OnInit {
  isLoaded: boolean = false;
  propertyId: string | null = null;
  startDate: string | null = null;
  endDate: string | null = null;
  orderType: string | null = null;
  numberOfGuests: number | null = null;
  data: any;

  constructor(private route: ActivatedRoute) { }

  // TODO: verificar se os dados estão completos e corretos consoante a orderType (booking precisa de numHospedes, promote nao precisa)

  ngOnInit(): void {
    this.route.queryParamMap.subscribe((params) => {
      this.propertyId = params.get('propertyId');
      this.startDate = params.get('startDate');
      this.endDate = params.get('endDate');
      this.orderType = params.get('orderType');
      this.numberOfGuests = Number(params.get('numberOfGuests'));

      if (!this.propertyId || !this.startDate || !this.endDate || !this.orderType) {
        console.error('Dados da ordem incompletos na rota.');
      } else {
        console.log('Property ID:', this.propertyId);
        console.log('Start Date:', this.startDate);
        console.log('End Date:', this.endDate);
        console.log('Order Type:', this.orderType);
        console.log('Num Guests:', this.numberOfGuests);
      }

      this.isLoaded = true;
      this.data = { propertyId: this.propertyId, startDate: this.startDate, endDate: this.endDate, orderType: this.orderType, numberOfGuests: this.numberOfGuests };
    });
  }
}
