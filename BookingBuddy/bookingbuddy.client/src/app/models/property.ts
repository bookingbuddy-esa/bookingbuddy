import { ApplicationUser } from "./applicationUser";
import { Amenity } from "./amenity";
export interface Property {
  propertyId: string;
  applicationUserId: string;
  applicationUser?: ApplicationUser;
  name: string;
  location: string;
  description: string;
  pricePerNight: number;
  amenities?: string[];
  amenityIds?: string[];
  imagesUrl?: string[];
}
