using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [Index(nameof(Patente), IsUnique = true)]
    public class Vehiculos
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Patente { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2100)]
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Required]
        [Display(Name = "Tipo Vehículo")]
        public TipoVehiculo Tipo { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        // Propiedad de Navegación AÑADIDA
        public Clientes? Cliente { get; set; } 

        [Display(Name = "Creado")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Display(Name = "Actualizado")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Patente} - {Marca} {Modelo} ({Anio}) [{Tipo}]";
        }
    }

    public enum TipoVehiculo
    {
        Auto,
        Camioneta,
        Moto
    }
}