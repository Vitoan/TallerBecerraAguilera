using System.ComponentModel.DataAnnotations;

namespace TallerBecerraAguilera.Models
{
    public class Clientes
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(20)]
        [Phone]
        public string? Telefono { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Creado")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Display(Name = "Actualizado")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Nombre} {Apellido}";
        }
    }
}
