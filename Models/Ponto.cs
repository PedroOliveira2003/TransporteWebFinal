using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransporteWeb.Models
{
    [Table("Pontos")]
    public class Ponto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id { get; set; }
        [Required(ErrorMessage = "CAMPO NOME É OBRIGATORIO")]
        [StringLength(30)]
        [Display(Name = "Ponto")]
        public string nomeponto { get; set; }
        
    }
}
