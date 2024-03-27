using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models;

/// <summary>
/// Classe que representa uma avaliação relativamente a uma reserva.
/// </summary>
public class Rating
{
    /// <summary>
    /// Identificador da avaliação.
    /// </summary>
    [Key]
    public string RatingId { get; set; }
    
    /// <summary>
    /// Identificador da propriedade associada à avaliação.
    /// </summary>
    public string PropertyId { get; set; }

    /// <summary>
    /// Identificador do utilizador que fez a avaliação.
    /// </summary>
    public string ApplicationUserId { get; set; }
    
    /// <summary>
    /// Valor da avaliação.
    /// </summary>
    public int Value { get; set; }
    
    /// <summary>
    /// Utilizador que fez a avaliação.
    /// </summary>
    public ApplicationUser? ApplicationUser { get; set; }
}