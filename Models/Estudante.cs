using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransporteWeb.Models
{
    [Table("Estudantes")]
    public class Estudante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id { get; set; }

        [Display(Name = "NOME ESTUDANTE")]
        [Required(ErrorMessage = "NOME DO ESTUDANTE É OBRIGATORIO")]
        [StringLength(40)]
        public string nome { get; set; }


        [Display(Name = "TELEFONE")]
        [Required(ErrorMessage = "TELEFONE É OBRIGATORIO")]
        [StringLength(11)]
        public string telefone { get; set; }

        [Display(Name = "Curso")]
        public int IdCurso { get; set; }
        [ForeignKey("IdCurso")]
        public Curso curso { get; set; }
        
    }
}
