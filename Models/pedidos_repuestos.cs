using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    [Index(nameof(Estado))]
    [Index(nameof(ProveedorId))]
    [Index(nameof(EmpleadoId))]
    public class PedidosRepuestos
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fecha del pedido")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        [Required]
        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }

        [Required]
        [Display(Name = "Empleado")]
        public int EmpleadoId { get; set; }

        [Display(Name = "Creado")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Display(Name = "Actualizado")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"Pedido #{Id} - {Estado} ({Fecha:dd/MM/yyyy})";
        }
    }

    public enum EstadoPedido
    {
        Pendiente,
        Recibido,
        Cancelado
    }
}
