using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [PrimaryKey(nameof(pedido_id), nameof(repuesto_id))]
    [Index(nameof(repuesto_id))]
    public class PedidoRepuestos
    {
        [Display(Name = "Pedido")]
        public int pedido_id { get; set; }

        // FK correcta
        [ForeignKey(nameof(pedido_id))]
        public PedidosRepuestos? Pedido { get; set; }

        [Display(Name = "Repuesto")]
        public int repuesto_id { get; set; }

        // FK correcta
        [ForeignKey(nameof(repuesto_id))]
        public Repuestos? Repuesto { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Cantidad solicitada")]
        public int cantidad_solicitada { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Cantidad recibida")]
        public int cantidad_recibida { get; set; } = 0;

        public override string ToString()
        {
            return $"Pedido {pedido_id} - Rep {repuesto_id}";
        }
    }
}
