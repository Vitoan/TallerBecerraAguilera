using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerBecerraAguilera.Models
{
    public class OtHerramientas
    {
        [Key]
        public int id { get; set; }

        public int ot_id { get; set; }
        public int herramienta_id { get; set; }
        public int empleado_id { get; set; }

        public DateTime fecha_prestamo { get; set; } = DateTime.Now;
        public DateTime? fecha_devolucion { get; set; }

        [ForeignKey(nameof(ot_id))]
        public OrdenesTrabajo? OrdenTrabajo { get; set; }

        [ForeignKey(nameof(herramienta_id))]
        public Herramientas? Herramienta { get; set; }

        [ForeignKey(nameof(empleado_id))]
        public Empleados? Empleado { get; set; }
    }
}
