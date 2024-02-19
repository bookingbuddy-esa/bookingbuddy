import {Amenity} from "./amenity";

export enum AmenityEnum {
  Estacionamento,
  Wifi,
  Cozinha,
  Varanda,
  Frigorifico,
  Microondas,
  Quintal,
  MaquinaLavar,
  PiscinaPartilhada,
  PiscinaIndividual,
  Animais,
  Camaras,
  Tv
}

export class AmenitiesHelper {
  public static getAmenities(): string[] {
    return Object.keys(AmenityEnum).filter(k => typeof AmenityEnum[k as any] === "number")
  }

  public static getAmenityDisplayName(amenity: string): string {
    switch (amenity) {
      case AmenityEnum![0]:
        return "Estacionamento";
      case AmenityEnum![1]:
        return "Wifi";
      case AmenityEnum![2]:
        return "Cozinha";
      case AmenityEnum![3]:
        return "Varanda";
      case AmenityEnum![4]:
        return "Frigorífico";
      case AmenityEnum![5]:
        return "Microondas";
      case AmenityEnum![6]:
        return "Quintal";
      case AmenityEnum![7]:
        return "Máquina de Lavar";
      case AmenityEnum![8]:
        return "Piscina Partilhada";
      case AmenityEnum![9]:
        return "Piscina Individual";
      case AmenityEnum![10]:
        return "Animais";
      case AmenityEnum![11]:
        return "Câmaras";
      case AmenityEnum![12]:
        return "TV";
      default:
        return "";

    }
  }

  public static getAmenityIcon(amenity: string): string {
    switch (amenity) {
      case AmenityEnum![0]:
        return "local_parking";
      case AmenityEnum![1]:
        return "wifi";
      case AmenityEnum![2]:
        return "oven";
      case AmenityEnum![3]:
        return "balcony";
      case AmenityEnum![4]:
        return "kitchen";
      case AmenityEnum![5]:
        return "microwave";
      case AmenityEnum![6]:
        return "yard";
      case AmenityEnum![7]:
        return "local_laundry_service";
      case AmenityEnum![8]:
        return "waves";
      case AmenityEnum![9]:
        return "pool";
      case AmenityEnum![10]:
        return "pets";
      case AmenityEnum![11]:
        return "videocam";
      case AmenityEnum![12]:
        return "tv";
      default:
        return "";
    }
  }
}
