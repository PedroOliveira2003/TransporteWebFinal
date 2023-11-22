using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransporteWeb.Models
{
    [Table("Agendamentos")]
    public class Agendamento
    {
        [Key]
        [Display(Name = "Id")]
        public int id { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "DATA OBRIGATORIA")]
        public DateTime data { get; set; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "HORARIO OBRIGATORIO")]
        public TimeSpan horario { get; set; }

        [Display(Name = "Estudante")]
        public int IdEstudante { get; set; }
        [ForeignKey("IdEstudante")]
        public Estudante estudante { get; set; }

        [Display(Name = "Veiculo")]
        public int IdVeiculo { get; set; }
        [ForeignKey("IdVeiculo")]
        public Veiculo veiculo { get; set; }
    }
}
