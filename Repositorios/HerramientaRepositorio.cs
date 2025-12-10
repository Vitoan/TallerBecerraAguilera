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

        public async Task DeleteAsync(int id)
        {
            var herramienta = await _context.Herramientas.FindAsync(id);
            if (herramienta != null)
            {
                _context.Herramientas.Remove(herramienta);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Herramientas> Query()
        {
            return _context.Herramientas.AsQueryable();
        }
    }
}
