using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TallerBecerraAguilera.Models
{
    public class Usuarios
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int Id { get; set; }

        [Required, StringLength(255)]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(255)]
        [Display(Name = "Contraseña (Hash)")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = "Empleado"; // Valores permitidos: Administrador / Empleado

        [StringLength(500)]
        [Display(Name = "Avatar")]
        public string? AvatarPath { get; set; } = "/uploads/avatars/default.jpg";

        [Display(Name = "Creado")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Display(Name = "Actualizado")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Email} - {Rol}";
        }
    }
}
