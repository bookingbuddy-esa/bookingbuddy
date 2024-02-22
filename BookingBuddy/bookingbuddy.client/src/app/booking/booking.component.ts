import { Component } from '@angular/core';
import { AppComponent } from '../app.component';
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

  constructor(private appComponent: AppComponent, private bookingService: BookingService) {
    this.appComponent.showChat = false;
  }

  ngOnInit() {
    this.getBookings();
  }

  selectBooking(booking: any) {
    this.selectedBooking = booking;
    this.getBookingMessages();
  }

  sendMessage() {
    this.bookingService.sendBookingMessage(this.selectedBooking.bookingOrderId, this.newMessage).forEach(
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
    this.bookingService.getBookingMessages(this.selectedBooking.bookingOrderId).forEach(
      response => {
        if (response) {
          this.messages = response as any[];
        }
      }).catch(
        error => {
          // TODO return error message
        }
    )
  }

  getBookings(){
    this.bookingService.getBookings().forEach(
      response => {
        if (response) {
          console.log(response);
          this.bookings = response as any[];
        }
      }).catch(
        error => {
          // TODO return error message
        }
    )
  }
}
