using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // <-- NECESARIO PARA EL ATRIBUTO [Column]
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    // [Table("repuestos")] // No es necesario si ya mapeaste en ApplicationDbContext
    public class Repuestos
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required]
        [Column("precio_unitario", TypeName = "decimal(10, 2)")]
        public decimal PrecioUnitario { get; set; }

        // Mapeo crucial para el error:
        [Column("cantidad_stock")] // <-- Corregido: CantidadStock -> cantidad_stock
        public int CantidadStock { get; set; } = 0;

        [Column("stock_minimo")] // <-- Corregido: StockMinimo -> stock_minimo
        public int StockMinimo { get; set; } = 0;

        // Clave Foránea
        [Column("proveedor_id")]
        public int? ProveedorId { get; set; }

        // Propiedad de navegación
        public Proveedores? Proveedor { get; set; }

        // Fechas y otros campos
        [Column("created_at")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;
    }
}