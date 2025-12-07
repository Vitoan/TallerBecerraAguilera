using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerBecerraAguilera.Models
{
    public class Repuestos
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        [Column("codigo")]
        public string Codigo { get; set; } = string.Empty;

        [Required, StringLength(200)]
        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Column("precio_unitario", TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column("cantidad_stock")]
        public int CantidadStock { get; set; }

        [Column("stock_minimo")]
        public int StockMinimo { get; set; }

        [Column("proveedor_id")]
        public int? ProveedorId { get; set; }

        public Proveedores? Proveedor { get; set; }

        [Column("created_at")]
        public DateTime? Created_at { get; set; }

        [Column("updated_at")]
        public DateTime? Updated_at { get; set; }
    }
}
