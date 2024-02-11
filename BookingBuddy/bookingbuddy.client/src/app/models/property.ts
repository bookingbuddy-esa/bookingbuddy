import { Landlord } from "./landlord";
import { Amenity } from "./amenity";
export interface Property {
  propertyId: string;
  landlordId: string;
  landlord?: Landlord;
  name: string;
  location: string;
  description: string;
  pricePerNight: number;
  amenities?: Amenity[];
  amenityIds?: string[];
  imagesUrl?: string[];
}
