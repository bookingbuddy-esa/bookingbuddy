import {ApplicationUser} from "./application-user";
import {Group} from "./group";

export interface BookingOrder {
  bookingOrderId: string;
  applicationUser: ApplicationUser;
  startDate: Date;
  endDate: Date;
  amount: number;
}

export interface GroupBookingOrder {
  orderId: string
  applicationUser: ApplicationUser,
  group: Group,
  propertyId: string,
  startDate: Date,
  endDate: Date,
  totalAmount: number
  paidBy: ApplicationUser[]
  state: string
}
