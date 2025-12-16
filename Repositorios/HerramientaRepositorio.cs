using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models;

namespace TallerBecerraAguilera.Repositorios
{
    public class HerramientaRepositorio
    {
        private readonly ApplicationDbContext _context;

        public HerramientaRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Herramientas>> GetAllAsync()
        {
            return await _context.Herramientas.ToListAsync();
        }

        public async Task<Herramientas?> GetByIdAsync(int id)
        {
            return await _context.Herramientas.FindAsync(id);
        }

        public async Task AddAsync(Herramientas herramienta)
        {
            _context.Add(herramienta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Herramientas herramienta)
        {
            _context.Update(herramienta);
            await _context.SaveChangesAsync();
        }

        // Delete con manejo de FK
        public async Task<(bool ok, string mensaje)> DeleteAsync(int id)
        {
            var herramienta = await _context.Herramientas.FindAsync(id);
            if (herramienta == null)
                return (false, "La herramienta no existe.");

            try
            {
                _context.Herramientas.Remove(herramienta);
                await _context.SaveChangesAsync();
                return (true, "Herramienta eliminada correctamente.");
            }
            catch (DbUpdateException)
            {
                return (false, "No se pudo eliminar la herramienta porque está asociada a otras entidades.");
            }
            catch (Exception)
            {
                return (false, "Ocurrió un error inesperado al eliminar la herramienta.");
            }
        }

        public IQueryable<Herramientas> Query()
        {
            return _context.Herramientas.AsQueryable();
        }
    }
}
