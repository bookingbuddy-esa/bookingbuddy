import { Landlord } from "./landlord";

export interface Property {
  propertyId: string;
  landlordId: string;
  landlord?: Landlord;
  name: string;
  location: string;
  description: string;
  pricePerNight: number;
  amenityIds?: string[];
  imagesUrl?: string[];
}
