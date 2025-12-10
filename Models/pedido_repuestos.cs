using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Necesario para [ForeignKey]
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [PrimaryKey(nameof(PedidoId), nameof(RepuestoId))]
    [Index(nameof(RepuestoId))]
    public class PedidoRepuestos
    {
        [Display(Name = "Pedido")]
        public int PedidoId { get; set; }

        // Propiedad de Navegación hacia la cabecera del pedido
        [ForeignKey("PedidoId")]
        public PedidosRepuestos? Pedido { get; set; }

        [Display(Name = "Repuesto")]
        public int RepuestoId { get; set; }

        // Propiedad de Navegación hacia el Repuesto (Vital para mostrar el Nombre/Descripción)
        [ForeignKey("RepuestoId")]
        public Repuestos? Repuesto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad solicitada debe ser mayor a 0")]
        [Display(Name = "Cantidad solicitada")]
        public int CantidadSolicitada { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Cantidad recibida")]
        public int CantidadRecibida { get; set; } = 0;

        public override string ToString()
        {
            return $"Pedido {PedidoId} - Rep {RepuestoId} | Solicitado: {CantidadSolicitada}";
        }
    }
}