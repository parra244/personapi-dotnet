using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace personapi_dotnet.Models.Entities
{
    [Table("persona")]
    public class Persona
    {
        [Key]
        [Column("cc")]
        [Display(Name = "Cédula")]
        public long Cc { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(45)]
        [Column("nombre")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(45)]
        [Column("apellido")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El género es obligatorio")]
        [Column("genero")]
        [Display(Name = "Género")]
        public string Genero { get; set; } = null!;

        [Column("edad")]
        [Display(Name = "Edad")]
        [Range(0, 150)]
        public int? Edad { get; set; }

        // Navigation properties
        public virtual ICollection<Estudio> Estudios { get; set; } = new List<Estudio>();
        public virtual ICollection<Telefono> Telefonos { get; set; } = new List<Telefono>();
    }
}
