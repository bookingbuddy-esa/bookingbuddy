using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    public class BlockedDate
    {
        [Key]
        public int Id { get; set; }

        public String Start { get; set; }
        public String End { get; set; }
    }
}
