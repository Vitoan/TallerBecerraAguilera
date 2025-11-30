using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [Index(nameof(FechaIngreso))]
    [Index(nameof(Estado))]
    [Index(nameof(VehiculoId))]
    [Index(nameof(EmpleadoId))]
    public class OrdenesTrabajo
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Descripción de la falla")]
        public string DescripcionFalla { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [Display(Name = "Fecha estimada de entrega")]
        public DateTime? FechaEstimadaEntrega { get; set; }

        [Required]
        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

        [Display(Name = "Horas estimadas")]
        public decimal? HorasEstimadas { get; set; }

        [Required]
        [Display(Name = "Vehículo")]
        public int VehiculoId { get; set; }

        [Required]
        [Display(Name = "Empleado")]
        public int EmpleadoId { get; set; }

        [Display(Name = "Creado")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Display(Name = "Actualizado")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"OT #{Id} - {Estado}";
        }
    }

    public enum EstadoOrden
    {
        Pendiente,
        EnReparacion,
        Finalizada,
        Entregada
    }
}
