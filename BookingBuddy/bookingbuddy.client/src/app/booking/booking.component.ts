import { Component } from '@angular/core';
import { BookingService } from './booking.service';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.css'
})
export class BookingComponent {
  bookings: any[] = [];
  messages: any[] = [];
  selectedBooking: any;
  newMessage: string = "";

  constructor(private bookingService: BookingService) {
  }

  ngOnInit() {
    this.bookingService.getBookings().forEach( response => {
      if (response) {
        this.bookings = response as any[];
        console.log("BAOS");
        console.log(this.bookings);
        this.selectedBooking = this.bookings[0];
      }
    }).catch(error => {
      // TODO return error message
    })
  }

  selectBooking(booking: any) {
    this.selectedBooking = booking;
    
    this.getBookingMessages();
  }

  sendMessage() {
    this.bookingService.sendBookingMessage(this.selectedBooking.orderId, this.newMessage).forEach(
      response => {
        if (response) {
          this.newMessage = "";
          this.getBookingMessages();
        }
      }).catch(
        error => {
          // TODO return error message
        }
    )
  }

  getBookingMessages() {
    this.bookingService.getBookingMessages(this.selectedBooking.orderId).forEach(
      response => {
        if (response) {
          console.log("Recebi mensagens: " + response)
          this.messages = response as any[];
        }
      }).catch(
        error => {
          // TODO return error message
        }
    )
  }

  formatDate(date: string) {
    return new Date(date).toLocaleDateString('pt-PT');
  }
}
