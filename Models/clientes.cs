using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerBecerraAguilera.Models
{
    // Una buena práctica sería agregar un índice para búsquedas rápidas por DNI o Email
    [Index(nameof(Dni), IsUnique = true)] 
    [Index(nameof(Email))]
    public class Clientes
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        
        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [NotMapped] // Indica que esta propiedad no debe mapearse a la base de datos
        public string NombreCompleto => $"{Nombre} {Apellido}";

        // PROPIEDAD DNI AÑADIDA PARA SOLUCIONAR EL ERROR
        [StringLength(20)]
        public string? Dni { get; set; }

        [StringLength(20)]
        [Phone]
        public string? Telefono { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        // Propiedad de navegación para la relación 1:N con Vehículos
        public ICollection<Vehiculos>? Vehiculos { get; set; }


        [Display(Name = "Creado")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Display(Name = "Actualizado")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Nombre} {Apellido} ({Dni})";
        }
        
    }
}