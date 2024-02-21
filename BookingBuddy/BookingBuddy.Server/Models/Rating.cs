using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models;

public class Rating
{
    [Key]
    public string RatingId { get; set; }
    
    public string PropertyId { get; set; }
    
    public string ApplicationUserId { get; set; }
    
    public int Value { get; set; }
    
    public ReturnUser? ApplicationUser { get; set; }
}