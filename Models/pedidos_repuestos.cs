using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using Microsoft.EntityFrameworkCore;
using System;

namespace TallerBecerraAguilera.Models
{
    // Los índices están bien para la optimización de consultas
    [Index(nameof(Estado))]
    [Index(nameof(ProveedorId))]
    [Index(nameof(EmpleadoId))] 
    public class PedidosRepuestos
    {

        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required]
        [Column("fecha_pedido")] // Mapea a la columna SQL fecha_pedido
        [Display(Name = "Fecha del pedido")]
        public DateTime Fecha { get; set; } = DateTime.Now; 

        [Required]
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        // --- CLAVE FORÁNEA PROVEEDOR ---
        [Required]
        [Column("proveedor_id")] // Mapea a la columna SQL proveedor_id
        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }

        // --- CLAVE FORÁNEA EMPLEADO (SOLUCIÓN AL ERROR) ---
        [Required]
        [Column("empleado_id")] // <--- ESTO fuerzá a EF Core a buscar 'empleado_id' en MySQL
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