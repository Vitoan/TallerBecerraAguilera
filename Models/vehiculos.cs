using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [Index(nameof(patente), IsUnique = true)]
    public class Vehiculos
    {
        [Key]
        public int id { get; set; }

        [Required, StringLength(10)]
        public string patente { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string marca { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string modelo { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2100)]
        [Display(Name = "Año")]
        public int anio { get; set; }

        [Required]
        public TipoVehiculo tipo { get; set; }

        public string? observaciones { get; set; }

        [Required]
        [Column("cliente_id")]   // 🔥 ESTE ES EL ARREGLO CLAVE
        public int clienteId { get; set; }

        public Clientes? Cliente { get; set; }

        public DateTime? created_at { get; set; } = DateTime.Now;
        public DateTime? updated_at { get; set; } = DateTime.Now;
    }

    public enum TipoVehiculo
    {
        Auto,
        Camioneta,
        Moto
    }
}
