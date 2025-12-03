using System;

namespace TallerBecerraAguilera.Models.ViewModels
{
    public class UltimasOTsViewModel
    {
        public int Id { get; set; }
        public string DescripcionFalla { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Patente { get; set; } = string.Empty;
        public string EmpleadoAsignado { get; set; } = string.Empty;
    }
}