using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerBecerraAguilera.Models
{
    public class Proveedores
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("contacto")]
        public string? Contacto { get; set; }

        [StringLength(20)]
        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("condiciones_compra")]
        public string? CondicionesCompra { get; set; }

        [Column("created_at")]
        public DateTime? Created_at { get; set; }

        [Column("updated_at")]
        public DateTime? Updated_at { get; set; }

        public override string ToString()
        {
            return $"{Nombre} {(Contacto != null ? "- " + Contacto : "")}";
        }
    }
}
