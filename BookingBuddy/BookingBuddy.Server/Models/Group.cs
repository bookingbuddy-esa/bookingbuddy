using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa os Grupos de Viagem
    /// </summary>
    public class Group
    {
        [Key]
        public string GroupId {  get; set; }

        public string GroupOwnerId { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        public List<string> Members { get; set; }

        public List<string> Properties {  get; set; }

        public string ChoosenProperty { get; set; } 

    }
}
