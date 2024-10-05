using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransporteWeb.Models
{
    [Table("PagamentosMulta")]
    public class PagamentoMulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdEstudante { get; set; }

        [ForeignKey("IdEstudante")]
        public Estudante Estudante { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        public DateTime DataPagamento { get; set; }
    }
}