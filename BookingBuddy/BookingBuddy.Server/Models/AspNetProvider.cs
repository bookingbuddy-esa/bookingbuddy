namespace BookingBuddy.Server.Models;

/// <summary>
/// Classe que representa um fornecedor de login.
/// </summary>
public class AspNetProvider
{
    /// <summary>
    /// Identificador do fornecedor de login.
    /// </summary>
    public string AspNetProviderId { get; init; }

    /// <summary>
    ///  Nome do fornecedor de login.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Nome normalizado do fornecedor de login.
    /// </summary>
    public string NormalizedName { get; init; }
}