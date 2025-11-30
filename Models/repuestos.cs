using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    public class Repuestos
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required, StringLength(200)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Stock Actual")]
        public int CantidadStock { get; set; } = 0;

        [Precision(10, 2)]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Stock Mínimo")]
        public int StockMinimo { get; set; } = 0;

        [Display(Name = "Proveedores")]
        public int? ProveedorId { get; set; }
        public Proveedores? Proveedor { get; set; }

        [Display(Name = "Creado")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Display(Name = "Actualizado")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Codigo} - {Descripcion}";
        }
    }
}
