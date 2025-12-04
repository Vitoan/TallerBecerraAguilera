using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [Index(nameof(Patente), IsUnique = true)]
    public class Vehiculos
    {
        [Key]
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
        public TipoVehiculo Tipo { get; set; }

        public string? Observaciones { get; set; }

        [Required]
        [Column("cliente_id")]   // 🔥 ESTE ES EL ARREGLO CLAVE
        public int ClienteId { get; set; }

        public Clientes? Cliente { get; set; }

        public DateTime? Created_at { get; set; } = DateTime.Now;
        public DateTime? Updated_at { get; set; } = DateTime.Now;
    }

    public enum TipoVehiculo
    {
        Auto,
        Camioneta,
        Moto
    }
}
