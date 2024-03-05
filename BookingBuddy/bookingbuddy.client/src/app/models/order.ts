import {ApplicationUser} from "./applicationUser";

export interface BookingOrder {
  bookingOrderId: string;
  applicationUser: ApplicationUser;
  startDate: Date;
  endDate: Date;
  amount: number;
}
