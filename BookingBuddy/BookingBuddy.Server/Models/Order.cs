using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models;

/// <summary>
///  Representa uma order.
/// </summary>
public class OrderBase
{
    /// <summary>
    ///  Identificador da order.
    /// </summary>
    [Key]
    public required string OrderId { get; set; }

    /// <summary>
    ///  Identificador do utilizador que fez a order.
    /// </summary>
    public required string ApplicationUserId { get; set; }

    /// <summary>
    ///  Identificador da propriedade associada à order.
    /// </summary>
    public required string PropertyId { get; set; }

    /// <summary>
    ///  Data de início da order.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    ///  Data de fim da order.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    ///  Estado da order.
    /// </summary>
    public OrderState State { get; set; } = OrderState.Pending;

    /// <summary>
    ///  Propriedade associada à order.
    /// </summary>
    public Property? Property { get; set; }

    /// <summary>
    ///  Utilizador que fez a order.
    /// </summary>
    public ApplicationUser? ApplicationUser { get; set; }
}

/// <summary>
///  Representa uma order de promoção.
/// </summary>
public class PromotionOrder : OrderBase
{
    /// <summary>
    /// Desconto a ser aplicado.
    /// </summary>
    public required decimal Discount { get; set; }

    /// <summary>
    /// Identificador do pagamento associado à order.
    /// </summary>
    public required string PaymentId { get; set; }

    /// <summary>
    /// Pagamento associado à order.
    /// </summary>
    public Payment? Payment { get; set; }
}

/// <summary>
/// Representa uma order de promover uma propriedade.
/// </summary>
public class PromoteOrder : OrderBase
{
    /// <summary>
    /// Identificador do pagamento associado à order.
    /// </summary>
    public required string PaymentId { get; set; }

    /// <summary>
    /// Pagamento associado à order.
    /// </summary>
    public Payment? Payment { get; set; }
}

/// <summary>
///  Representa uma reserva.
/// </summary>
public class BookingOrder : OrderBase
{
    /// <summary>
    /// Número de hóspedes.
    /// </summary>
    public required int NumberOfGuests { get; set; }

    /// <summary>
    /// Identificador do pagamento associado à reserva.
    /// </summary>
    public required string PaymentId { get; set; }

    /// <summary>
    /// Pagamento associado à reserva.
    /// </summary>
    public Payment? Payment { get; set; }
}

/// <summary>
/// Representa uma reserva de grupo.
/// </summary>
public class GroupBookingOrder : OrderBase
{
    /// <summary>
    ///  Identificador do grupo.
    /// </summary>
    public required string GroupId { get; set; }

    /// <summary>
    ///  Identificador dos pagamentos associados à reserva.
    /// </summary>
    public List<string> PaymentIds { get; set; } = [];

    /// <summary>
    ///  Identificador dos utilizadores que pagaram a reserva.
    /// </summary>
    public List<string> PaidByIds { get; set; } = [];

    /// <summary>
    /// Total da reserva.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    ///  Grupo associado à reserva.
    /// </summary>
    public Group? Group { get; set; }

    /// <summary>
    ///  Pagamentos associados à reserva.
    /// </summary>
    public List<Payment>? Payments { get; set; }

    /// <summary>
    ///  Utilizadores que pagaram a reserva.
    /// </summary>
    public List<ApplicationUser>? PaidBy { get; set; }
}

/// <summary>
/// Representa o tipo de uma order.
/// </summary>
public class Order
{
    /// <summary>
    /// Identificador da order.
    /// </summary>
    [Key]
    public required string OrderId { get; init; }

    /// <summary>
    /// Tipo da order.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Data de criação da order.
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}

/// <summary>
/// Representa o estado de uma order.
/// </summary>
public enum OrderState
{
    /// <summary>
    /// Order cancelada.
    /// </summary>
    Canceled,

    /// <summary>
    /// Order pendente.
    /// </summary>
    Pending,

    /// <summary>
    /// Order paga.
    /// </summary>
    Paid,

    /// <summary>
    /// Order à espera do check-in, no caso de uma reserva.
    /// </summary>
    AwaitingCheckIn,
}

/// <summary>
/// Extensão para o enum OrderState.
/// </summary>
public static class OrderStateExtension
{
    /// <summary>
    /// Obtém a descrição do estado da order.
    /// </summary>
    /// <param name="state">Estado da order.</param>
    /// <returns>Descrição do estado da order.</returns>
    public static string GetDescription(this OrderState state) => state switch
    {
        OrderState.Canceled => "Cancelado",
        OrderState.Pending => "Pendente",
        OrderState.Paid => "Pago",
        OrderState.AwaitingCheckIn => "À espera do check-in",
        _ => "Desconhecido"
    };
}