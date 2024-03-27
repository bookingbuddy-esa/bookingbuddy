namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Enumerado que representa as comodidades de uma propriedade física na plataforma.
    /// </summary>
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
    
    /// <summary>
    /// Classe de extensão para o enumerado <see cref="AmenityEnum"/>.
    /// </summary>
    public static class AmenityExtension
    {
        /// <summary>
        /// Método que retorna o nome de uma comodidade de uma propriedade física a anunciar.
        /// </summary>
        /// <param name="amenity"></param>
        /// <returns></returns>
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