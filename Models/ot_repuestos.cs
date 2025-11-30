using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [PrimaryKey(nameof(OtId), nameof(RepuestoId))]
    [Index(nameof(RepuestoId))]
    public class OtRepuestos
    {
        [Display(Name = "Orden de Trabajo")]
        public int OtId { get; set; }

        [Display(Name = "Repuesto")]
        public int RepuestoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        [Display(Name = "Cantidad usada")]
        public int CantidadUsada { get; set; }

        public override string ToString()
        {
            return $"OT {OtId} - Rep {RepuestoId} x {CantidadUsada}u";
        }
    }
}
