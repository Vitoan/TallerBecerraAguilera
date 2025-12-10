using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Data;

namespace TallerBecerraAguilera.Repositorios
{
    public class ImagenHerramientaRepositorio
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImagenHerramientaRepositorio(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task GuardarAsync(ImagenHerramienta imagen)
        {
            if (imagen.Archivo != null)
            {
                string carpeta = Path.Combine(_env.WebRootPath, "uploads/herramientas");
                Directory.CreateDirectory(carpeta);

                string nombreArchivo = Guid.NewGuid() + Path.GetExtension(imagen.Archivo.FileName);
                string ruta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await imagen.Archivo.CopyToAsync(stream);
                }

                imagen.Url = "/uploads/herramientas/" + nombreArchivo;
            }

            _context.ImagenHerramientas.Add(imagen);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ImagenHerramienta>> ObtenerPorHerramientaAsync(int herramientaId)
        {
            return await _context.ImagenHerramientas
                .Where(x => x.HerramientaId == herramientaId)
                .ToListAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var img = await _context.ImagenHerramientas.FindAsync(id);
            if (img == null) return;

            string rutaFisica = Path.Combine(_env.WebRootPath, img.Url.TrimStart('/'));
            if (File.Exists(rutaFisica))
                File.Delete(rutaFisica);

            _context.ImagenHerramientas.Remove(img);
            await _context.SaveChangesAsync();
        }
    }
}
