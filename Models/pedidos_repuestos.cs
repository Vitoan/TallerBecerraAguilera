using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic; // Necesario para ICollection

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
        [Column("fecha_pedido")]
        [Display(Name = "Fecha del pedido")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        // --- Relación con Proveedor ---
        [Required]
        [Column("proveedor_id")]
        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }

        // Propiedad de Navegación (Esto es lo que te falta para que funcione .Proveedor.Nombre)
        [ForeignKey("ProveedorId")]
        public Proveedores? Proveedor { get; set; }

        // --- Relación con Empleado ---
        [Required]
        [Column("empleado_id")]
        [Display(Name = "Empleado")]
        public int EmpleadoId { get; set; }

        // Propiedad de Navegación (Esto es lo que te falta para que funcione .Empleado.Nombre)
        [ForeignKey("EmpleadoId")]
        public Empleados? Empleado { get; set; }

        // --- Relación con Detalles (Muchos a Muchos) ---
        // Esto permite acceder a la lista de repuestos del pedido
        public ICollection<PedidoRepuestos> Detalles { get; set; }

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