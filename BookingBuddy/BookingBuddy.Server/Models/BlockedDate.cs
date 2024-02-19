using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa as datas bloquedas de uma propriedade.
    /// </summary>
    public class BlockedDate
    {
        /// <summary>
        /// Propriedade que diz respeito ao identificador das datas bloqueadas de uma propriedade.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Propriedade que diz respeito à data de incio.
        /// </summary>
        public String Start { get; set; }

        /// <summary>
        /// Propriedade que diz respeito à data final.
        /// </summary>
        public String End { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao identificador da propriedade das dadas bloqueadas.
        /// </summary>
        public String PropertyId {  get; set; }
    }
}
