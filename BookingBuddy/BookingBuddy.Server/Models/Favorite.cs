using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    public class Favorite
    {
        [Key]
        public int FavoriteId { get; set; }

        public string ApplicationUserId { get; set; }
        public string PropertyId { get; set; }


        public ApplicationUser ApplicationUser { get; set; }
        public Property Property { get; set; }
    }
}
