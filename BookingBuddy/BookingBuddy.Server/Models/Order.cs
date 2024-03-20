using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa uma order
    /// </summary>
    public class Order {
        [Key]
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string PropertyId { get; set; }
        public Property? Property { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool State { get; set; }
    }

    // Desconto
    public class PromotionOrder {
        [Key]
        public string PromotionOrderId { get; set; }
        public string OrderId { get; set; }
        public Order? Order { get; set; }
        public decimal Discount { get; set; }
    }

    public class PromoteOrder {
        [Key]
        public string PromoteOrderId { get; set; }
        public string OrderId { get; set; }
        public Order? Order { get; set; }
    }

    // Reserva
    public class BookingOrder {
        [Key]
        public string BookingOrderId { get; set; }
        public string OrderId { get; set; }
        public Order? Order { get; set; }
        public int NumberOfGuests { get; set; }
    }
    
    // Reserva de grupo
    public class GroupBookingOrder
    {
        [Key] public string GroupBookingOrderId { get; set; }

        public string ApplicationUserId { get; set; }
        
        public List<string> MembersId { get; set; }
        
        public string PropertyId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public decimal TotalPrice { get; set; }
        
        public List<string> PaymentsId { get; set; }
        
        public List<string> PaidById { get; set; }
        
        public ApplicationUser? ApplicationUser { get; set; }
        
        public List<ApplicationUser>? Members { get; set; }
        
        public Property? Property { get; set; }
        
        public List<Payment>? Payments { get; set; }
        
        public List<ApplicationUser>? PaidBy { get; set; }
        
        public bool State { get; set; }
    }
}