namespace BookingBuddy.Server.Models
{
    public enum AmenityEnum {
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
    
    public static class AmenityExtension
    {
        public static string GetAmenityName(this AmenityEnum amenity)
        {
            return amenity switch
            {
                AmenityEnum.Estacionamento => "Estacionamento",
                AmenityEnum.Wifi => "Wifi",
                AmenityEnum.Cozinha => "Cozinha",
                AmenityEnum.Varanda => "Varanda",
                AmenityEnum.Frigorifico => "Frigorífico",
                AmenityEnum.Microondas => "Microondas",
                AmenityEnum.Quintal => "Quintal",
                AmenityEnum.MaquinaLavar => "Máquina de Lavar",
                AmenityEnum.PiscinaPartilhada => "Piscina Partilhada",
                AmenityEnum.PiscinaIndividual => "Piscina Individual",
                AmenityEnum.Animais => "Animais",
                AmenityEnum.Camaras => "Câmaras",
                AmenityEnum.Tv => "TV",
                _ => "Amenidade não encontrada"
            };
        }
    }
}