using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [Index(nameof(Dni), IsUnique = true)]
    [Index(nameof(UsuarioId), IsUnique = true)]
    public class Empleados
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "DNI")]
        public string? Dni { get; set; }

        [StringLength(20)]
        [Phone]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Usuario")]
        [Column("usuario_id")]
        public int? UsuarioId { get; set; }

        public Usuarios? Usuario { get; set; }

        [Column("created_at")]
        [Display(Name = "Creado")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Column("updated_at")]
        [Display(Name = "Actualizado")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;

        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        public override string ToString() => $"{Nombre} {Apellido}";
    }
}
