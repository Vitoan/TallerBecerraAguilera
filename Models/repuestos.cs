using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerBecerraAguilera.Models
{
    public class Repuestos
    {
        [Key]
        public int id { get; set; }

        [Required, StringLength(50)]
        [Column("codigo")]
        public string codigo { get; set; } = string.Empty;

        [Required, StringLength(200)]
        [Column("descripcion")]
        public string descripcion { get; set; } = string.Empty;

        [Required]
        [Column("precio_unitario", TypeName = "decimal(10,2)")]
        public decimal precioUnitario { get; set; }

        [Column("cantidad_stock")]
        public int cantidadStock { get; set; }

        [Column("stock_minimo")]
        public int stockMinimo { get; set; }

        [Column("proveedor_id")]
        public int? proveedorId { get; set; }

        public Proveedores? Proveedor { get; set; }

        [Column("created_at")]
        public DateTime? created_at { get; set; }

        [Column("updated_at")]
        public DateTime? updated_at { get; set; }
    }
}
