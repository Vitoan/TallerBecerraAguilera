using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [Index(nameof(Codigo))]
    [Index(nameof(Estado))]
    public class Herramientas
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Estado")]
        public EstadoHerramienta Estado { get; set; } = EstadoHerramienta.Disponible;

        [StringLength(100)]
        [Display(Name = "Ubicación")]
        public string? Ubicacion { get; set; }

        [Display(Name = "Creado")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Display(Name = "Actualizado")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;

        public List<ImagenHerramienta>? Imagenes { get; set; }

        public override string ToString()
        {
            return $"{Nombre} - {Codigo} ({Estado})";
        }
    }

    public enum EstadoHerramienta
    {
        Disponible,
        EnUso,
        EnMantenimiento
    }
}
