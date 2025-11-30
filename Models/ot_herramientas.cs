using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [PrimaryKey(nameof(OtId), nameof(HerramientaId))]
    [Index(nameof(HerramientaId))]
    [Index(nameof(EmpleadoId))]
    public class OtHerramientas
    {
        [Display(Name = "Orden de Trabajo")]
        public int OtId { get; set; }

        [Display(Name = "Herramienta")]
        public int HerramientaId { get; set; }

        [Required]
        [Display(Name = "Fecha de préstamo")]
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de devolución")]
        public DateTime? FechaDevolucion { get; set; }

        [Required]
        [Display(Name = "Empleado responsable")]
        public int EmpleadoId { get; set; }

        public override string ToString()
        {
            return $"OT {OtId} -> Herramienta {HerramientaId} (Prestada: {FechaPrestamo})";
        }
    }
}
