import { Component, inject } from '@angular/core';
import { BookingService } from './booking.service';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RouterLink } from '@angular/router';
import { AuxiliaryModule } from '../auxiliary/auxiliary.module';
import { CommonModule, NgIf, formatDate } from '@angular/common';
import { MatTooltip } from '@angular/material/tooltip';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.css'
})
export class BookingComponent {
  private modalService: NgbModal = inject(NgbModal);
  bookings: any[] = [];
  messages: any[] = [];
  selectedBooking: any;
  newMessage: string = "";
  canRate: boolean = false;

  constructor(private bookingService: BookingService) {
  }

  ngOnInit() {
    this.bookingService.getBookings().forEach( response => {
      if (response) {
        this.bookings = response as any[];
        this.selectBooking(this.bookings[0]);
        //console.log(this.bookings);
      }
    }).catch(error => {
      // TODO return error message
    })
  }

  selectBooking(booking: any) {
    this.selectedBooking = booking;
    console.log(booking);
    this.canRate = new Date(this.selectedBooking.checkOut) < new Date();
    this.getBookingMessages();
  }

  sendMessage() {
    this.bookingService.sendBookingMessage(this.selectedBooking.orderId, this.newMessage).forEach(response => {
      if (response) {
        this.newMessage = "";
        this.getBookingMessages();
      }
    }).catch(error => {
      // TODO return error message
    })
  }

  getBookingMessages() {
    this.bookingService.getBookingMessages(this.selectedBooking.orderId).forEach(response => {
      if (response) {
        //console.log("Recebi mensagens: " + response)
        this.messages = response as any[];
      }
    }).catch(error => {
      // TODO return error message
    })
  }

  formatDate(date: string) {
    return new Date(date).toLocaleDateString('pt-PT');
  }

  truncateText(text: string) {
    return text.length > 20 ? text.substring(0, 20) + "..." : text;
  }

  protected showRatingModal() {
    let modalRef = this.modalService.open(RatingModal,
      {
        animation: true,
        size: 'lg',
        centered: true,
      });

    modalRef.componentInstance.sendRating = async () => {
      if(modalRef.componentInstance.rating === 0) {
        return;
      }

      this.bookingService.sendRating(this.selectedBooking.orderId, modalRef.componentInstance.rating).forEach(response => {
        if (response) {
          console.log(response);
          this.canRate = false;
          //modalRef.close();
        }
      }).catch(error => {
        // TODO return error message
        console.log(error.error);
      });
    }
  }
}

@Component({
  selector: 'rating-modal',
  standalone: true,
  styleUrl: './rating-modal.css',
  template: `
    <div class="modal-header" aria-labelledby="concludeVoteFailedModalLabel">
      <h5 class="modal-title" id="concludeVoteFailedModalLabel">Classificar reserva</h5>
      <button type="button" class="btn-close" aria-label="Close" (click)="onClose()"></button>
    </div>
    <div class="modal-body">
      <div class="d-flex flex-column justify-content-between align-items-center">
        <div class="ratings p-3">
          <i class="bi rating-color ms-2" matTooltip="Fraca" matTooltipPosition="above" (click)="rateBooking(1)" [ngClass]="{ 'bi-star-fill': rating >= 1, 'bi-star': rating < 1 }"></i>
          <i class="bi rating-color ms-2" matTooltip="Razoável" matTooltipPosition="above" (click)="rateBooking(2)" [ngClass]="{ 'bi-star-fill': rating >= 2, 'bi-star': rating < 2 }"></i>
          <i class="bi rating-color ms-2" matTooltip="Boa" matTooltipPosition="above" (click)="rateBooking(3)" [ngClass]="{ 'bi-star-fill': rating >= 3, 'bi-star': rating < 3 }"></i>
          <i class="bi rating-color ms-2" matTooltip="Muito Boa" matTooltipPosition="above" (click)="rateBooking(4)" [ngClass]="{ 'bi-star-fill': rating >= 4, 'bi-star': rating < 4 }"></i>
          <i class="bi rating-color ms-2" matTooltip="Excelente" matTooltipPosition="above" (click)="rateBooking(5)" [ngClass]="{ 'bi-star-fill': rating >= 5, 'bi-star': rating < 5 }"></i>
        </div>
        <button type="button" class="btn btn-success" aria-label="Accept" (click)="sendRating()" [disabled]="rating === 0">Enviar</button>
      </div>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" aria-label="Cancel" (click)="onClose()">Cancelar</button>
    </div>
  `,

  imports: [
    RouterLink,
    AuxiliaryModule,
    NgIf,
    MatTooltip,
    CommonModule  
  ]
})


export class RatingModal {
  rating: number = 0;
  private activeModal: NgbActiveModal = inject(NgbActiveModal);
  protected onClose: Function = () => {
    this.activeModal.dismiss();
  }

  protected sendRating: Function = () => {}

  protected rateBooking: Function = (ratingValue: number) => {
    if (this.rating === ratingValue) {
      this.rating = 0;
      return;
    }

    this.rating = ratingValue;
  }
}
