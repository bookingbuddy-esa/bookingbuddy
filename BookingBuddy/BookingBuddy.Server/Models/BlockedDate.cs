using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa as datas bloquedas de uma propriedade.
    /// </summary>
    public class BlockedDate
    {
        /// <summary>
        /// Identificador das datas bloqueadas de uma propriedade.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Data de incio do bloqueio.
        /// </summary>
        public String Start { get; set; }

        /// <summary>
        /// Data de fim do bloqueio.
        /// </summary>
        public String End { get; set; }

        /// <summary>
        /// Identificador da propriedade referente às datas bloqueadas.
        /// </summary>
        public String PropertyId {  get; set; }
    }
}
