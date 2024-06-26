import { Component } from '@angular/core';
import { HostingService } from '../hosting.service';
import { BookingService } from '../../booking/booking.service';

@Component({
  selector: 'app-hosting-booking',
  templateUrl: './hosting-booking.component.html',
  styleUrl: './hosting-booking.component.css'
})

export class HostingBookingComponent {
  bookings: any[] = [];
  messages: any[] = [];
  selectedBooking: any;
  newMessage: string = "";

  constructor(private hostingService: HostingService, private bookingService: BookingService) {
  }

  ngOnInit() {
    this.hostingService.getAssociatedBookings().forEach(
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

  selectBooking(booking: any) {
    console.log("Booking: ");
    console.log(booking);
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
