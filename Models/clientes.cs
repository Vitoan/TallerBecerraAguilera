using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TallerBecerraAguilera.Models
{
    // Índice único para DNI (clave para la validación de unicidad)
    [Index(nameof(Dni), IsUnique = true)] 
    [Index(nameof(Email))]
    public class Clientes
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio."), StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio."), StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [NotMapped]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        // DNI requerido y con límite de longitud
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(20)]
        [Display(Name = "DNI")]
        public string Dni { get; set; } = string.Empty;

        [StringLength(20)]
        [Phone(ErrorMessage = "Formato de teléfono no válido.")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico no válido.")]
        public string? Email { get; set; }

        // Propiedad de navegación: Colección de vehículos de este cliente
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