using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [PrimaryKey(nameof(ot_id), nameof(repuesto_id))]
    [Index(nameof(repuesto_id))]
    public class OtRepuestos
    {
        [Display(Name = "Orden de Trabajo")]
        public int ot_id { get; set; }

        [Display(Name = "Repuesto")]
        public int repuesto_id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        [Display(Name = "Cantidad usada")]
        public int cantidad_usada { get; set; }

        public override string ToString()
        {
            return $"OT {ot_id} - Rep {repuesto_id} x {cantidad_usada}u";
        }
    }
}
