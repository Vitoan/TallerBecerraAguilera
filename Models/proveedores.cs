using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; // Necesario para [Index]
using System.Collections.Generic;

namespace TallerBecerraAguilera.Models
{
    // Aseguramos que el nombre sea único para evitar duplicados del mismo proveedor
    [Index(nameof(Nombre), IsUnique = true)] 
    public class Proveedores
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre/razón social es obligatorio."), StringLength(100)]
        [Column("nombre")]
        [Display(Name = "Nombre / Razón Social")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("contacto")]
        [Display(Name = "Persona de Contacto")]
        public string? Contacto { get; set; }

        [StringLength(20)]
        [Column("telefono")]
        [Phone(ErrorMessage = "Formato de teléfono no válido.")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Column("condiciones_compra")]
        [Display(Name = "Condiciones de Compra")]
        public string? CondicionesCompra { get; set; }
        
        // Propiedad de navegación para la relación 1:N con Repuestos
        public ICollection<Repuestos>? Repuestos { get; set; }

        [Column("created_at")]
        [Display(Name = "Fecha de Creación")]
        public DateTime? Created_at { get; set; }

        [Column("updated_at")]
        [Display(Name = "Última Actualización")]
        public DateTime? Updated_at { get; set; }

        public override string ToString()
        {
            return $"{Nombre} {(Contacto != null ? "- " + Contacto : "")}";
        }
    }
}