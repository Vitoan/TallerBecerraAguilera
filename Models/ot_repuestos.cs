using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [Index(nameof(ot_id), nameof(repuesto_id), IsUnique = true)]
    [Index(nameof(repuesto_id))]
    [Index(nameof(empleado_id))]
    public class OtRepuestos
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int ot_id { get; set; }

        [Required]
        public int repuesto_id { get; set; }

        [Required]
        public int empleado_id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int cantidad_usada { get; set; }

        [ForeignKey(nameof(ot_id))]
        public OrdenesTrabajo? OrdenTrabajo { get; set; }

        [ForeignKey(nameof(repuesto_id))]
        public Repuestos? Repuesto { get; set; }

        [ForeignKey(nameof(empleado_id))]
        public Empleados? Empleado { get; set; }
    }
}
