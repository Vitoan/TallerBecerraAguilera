using System.ComponentModel.DataAnnotations.Schema;

namespace TallerBecerraAguilera.Models
{
    public class ImagenHerramienta
    {
        public int Id { get; set; }
        public int HerramientaId { get; set; }
        public string Url { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? Archivo { get; set; }

        public Herramientas? Herramienta { get; set; }
    }
}
