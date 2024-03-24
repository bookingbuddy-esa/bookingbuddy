import {ApplicationUser} from "./application-user";

export interface BookingOrder {
  bookingOrderId: string;
  applicationUser: ApplicationUser;
  startDate: Date;
  endDate: Date;
  amount: number;
}
