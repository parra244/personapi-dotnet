using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace personapi_dotnet.Models.Entities
{
    [Table("estudios")]
    public class Estudio
    {
        [Column("id_prof")]
        [Display(Name = "Profesión")]
        public int IdProf { get; set; }

        [Column("cc_per")]
        [Display(Name = "Persona (CC)")]
        public long CcPer { get; set; }

        [Column("fecha")]
        [Display(Name = "Fecha de Grado")]
        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; }

        [StringLength(50)]
        [Column("univer")]
        [Display(Name = "Universidad")]
        public string? Univer { get; set; }

        // Navigation properties
        [ForeignKey("IdProf")]
        public virtual Profesion? Profesion { get; set; }

        [ForeignKey("CcPer")]
        public virtual Persona? Persona { get; set; }
    }
}
