using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models;

/// <summary>
/// Classe que representa as comodidades de uma propriedade física na plataforma.
/// </summary>
public class Amenity
{
    /// <summary>
    /// Propriedade que diz respeito ao identificador de uma comodidade de uma propriedade física a anunciar.
    /// </summary>
    [Key]
    public required string AmenityId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao nome de uma comodidade de uma propriedade física a anunciar.
    /// </summary>
    [Required(ErrorMessage = "O nome da comodidade é obrigatório")]
    [Display(Name = "Nome")]
    public required string Name { get; set; }

    /// <summary>
    /// Propriedade que diz respeito à descrição de uma comodidade de uma propriedade física a anunciar.
    /// </summary>
    public string? DisplayName { get; set; }
}

/// <summary>
/// Enumeração que representa as comodidades de uma propriedade física na plataforma.
/// </summary>
public enum AmenityEnum
{
    /// <summary>
    /// Comodidade que diz respeito ao estacionamento.
    /// </summary>
    Estacionamento,

    /// <summary>
    /// Comodidade que diz respeito ao wifi.
    /// </summary>
    Wifi,

    /// <summary>
    /// Comodidade que diz respeito à cozinha.
    /// </summary>
    Cozinha,

    /// <summary>
    /// Comodidade que diz respeito à varanda.
    /// </summary>
    Varanda,

    /// <summary>
    /// Comodidade que diz respeito ao frigorífico.
    /// </summary>
    Frigorifico,

    /// <summary>
    /// Comodidade que diz respeito ao microondas.
    /// </summary>
    Microondas,

    /// <summary>
    /// Comodidade que diz respeito ao quintal.
    /// </summary>
    Quintal,

    /// <summary>
    /// Comodidade que diz respeito à máquina de lavar.
    /// </summary>
    MaquinaLavar,

    /// <summary>
    /// Comodidade que diz respeito à piscina partilhada.
    /// </summary>
    PiscinaPartilhada,

    /// <summary>
    /// Comodidade que diz respeito à piscina individual.
    /// </summary>
    PiscinaIndividual,

    /// <summary>
    /// Comodidade que diz respeito aos animais.
    /// </summary>
    Animais,

    /// <summary>
    /// Comodidade que diz respeito às câmaras.
    /// </summary>
    Camaras,

    /// <summary>
    /// Comodidade que diz respeito à TV.
    /// </summary>
    Tv
}

/// <summary>
/// Classe de extensão para a enumeração de comodidades.
/// </summary>
public static class AmenityExtension
{
    /// <summary>
    /// Método que retorna o nome de uma comodidade.
    /// </summary>
    /// <param name="amenity">Comodidade a obter o nome.</param>
    /// <returns>Nome da comodidade.</returns>
    public static string GetDescription(this AmenityEnum amenity)
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