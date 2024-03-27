import {ApplicationUser} from "./applicationUser";
import {Amenity} from "./amenity";
import {BookingOrder} from "./order";

export interface Property {
  propertyId: string;
  applicationUserId: string;
  applicationUser?: ApplicationUser;
  name: string;
  location: string;
  description: string;
  maxGuestsNumber: number;
  roomsNumber: number;
  pricePerNight: number;
  amenities?: Amenity[];
  amenityIds?: string[];
  imagesUrl?: string[];
}

export interface PropertyCreate {
  name: string;
  location: string;
  description: string;
  pricePerNight: number;
  maxGuestNumber: number;
  roomsNumber: number;
  amenities: string[];
  imagesUrl: string[];
}

export interface PropertyMetrics {
  propertyId: string;
  clicks: number;
  ratings: Rating[];
  orders: BookingOrder[];
}

interface Rating {
  ratingId: string;
  applicationUser: ApplicationUser;
  value: number;
}
