using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [PrimaryKey(nameof(ot_id), nameof(herramienta_id))]
    [Index(nameof(herramienta_id))]
    [Index(nameof(empleado_id))]
    public class OtHerramientas
    {
        [Display(Name = "Orden de Trabajo")]
        public int ot_id { get; set; }

        [Display(Name = "Herramienta")]
        public int herramienta_id { get; set; }

        [Required]
        [Display(Name = "Fecha de préstamo")]
        public DateTime fecha_prestamo { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de devolución")]
        public DateTime? fecha_devolucion { get; set; }

        [Required]
        [Display(Name = "Empleado responsable")]
        public int empleado_id { get; set; }

        public override string ToString()
        {
            return $"OT {ot_id} -> Herramienta {herramienta_id} (Prestada: {fecha_prestamo})";
        }

        // 🔥 Navegaciones con FK explícitas

        [ForeignKey(nameof(ot_id))]
        public OrdenesTrabajo? OrdenTrabajo { get; set; }

        [ForeignKey(nameof(herramienta_id))]
        public Herramientas? Herramienta { get; set; }

        [ForeignKey(nameof(empleado_id))]
        public Empleados? Empleado { get; set; }
    }
}
