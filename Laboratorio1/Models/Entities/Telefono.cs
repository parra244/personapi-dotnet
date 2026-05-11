using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace personapi_dotnet.Models.Entities
{
    [Table("telefono")]
    public class Telefono
    {
        [Key]
        [StringLength(15)]
        [Column("num")]
        [Display(Name = "Número")]
        public string Num { get; set; } = null!;

        [StringLength(45)]
        [Column("oper")]
        [Display(Name = "Operador")]
        public string? Oper { get; set; }

        [Column("duenio")]
        [Display(Name = "Dueño (CC)")]
        public long Duenio { get; set; }

        // Navigation property
        [ForeignKey("Duenio")]
        public virtual Persona? Persona { get; set; }
    }
}
