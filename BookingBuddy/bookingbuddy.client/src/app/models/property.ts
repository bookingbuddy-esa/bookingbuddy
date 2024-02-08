export interface Property {
  propertyId: string;
  landlordId: string;
  name: string,
  location: string;
  pricePerNight: number;
  amenityIds?: string[];
  imagesUrl?: string[];
}
