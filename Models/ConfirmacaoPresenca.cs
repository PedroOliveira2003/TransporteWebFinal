using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TransporteWeb.Models
{
    [Table("ConfirmacaoPresencas")]
    public class ConfirmacaoPresenca
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Agendamento")]
        public int IdAgendamento { get; set; }
       
        [ForeignKey("IdAgendamento")]
        public Agendamento Agendamento { get; set; }

        public bool PresencaConfirmada { get; set; }



    }
}
