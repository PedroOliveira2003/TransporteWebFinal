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

        [Display(Name = "Tipo de Viagem")]
        [Required(ErrorMessage = "TIPO DE VIAGEM OBRIGATÓRIO")]
        public Viagem TipoViagem { get; set; }

        [Display(Name = "Estudante")]
        public int IdEstudante { get; set; }
        [ForeignKey("IdEstudante")]
        public Estudante estudante { get; set; }

        [Display(Name = "Veiculo")]
        public int IdVeiculo { get; set; }
        [ForeignKey("IdVeiculo")]
        public Veiculo veiculo { get; set; }

        [Display(Name = "Ponto")]
        public int IdPonto { get; set;}
        [ForeignKey("IdPonto")]
        public Ponto ponto { get; set;}
    }
}


public enum Viagem
{
    Ida,
    Volta
}