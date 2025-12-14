using System;

namespace TallerBecerraAguilera.Models.ViewModels
{
    public class UltimasOTsViewModel
    {
        public int Id { get; set; }

        // Agregamos '?' para permitir que sean nulos temporalmente
        public string? DescripcionFalla { get; set; }
        
        public DateTime FechaIngreso { get; set; }
        
        public string? Estado { get; set; } 
        
        public string? Patente { get; set; }
        
        public string? EmpleadoAsignado { get; set; }
    }
}