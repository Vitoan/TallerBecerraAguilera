using TallerBecerraAguilera.Models;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;

namespace TallerBecerraAguilera.Repositorios
{
    public class RepuestoRepositorio
    {
        public IQueryable<Repuestos> Query()
        {
            return _context.Repuestos
                .Include(r => r.Proveedor)
                .AsQueryable();
        }

        private readonly ApplicationDbContext _context;

        public RepuestoRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Repuestos>> GetAllAsync()
        {
            return await _context.Repuestos
                .Include(r => r.Proveedor)
                .ToListAsync();
        }

        public async Task<Repuestos?> GetByIdAsync(int id)
        {
            return await _context.Repuestos
                .Include(r => r.Proveedor)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Repuestos repuesto)
        {
            _context.Repuestos.Add(repuesto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Repuestos repuesto)
        {
            _context.Repuestos.Update(repuesto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repuesto = await _context.Repuestos.FindAsync(id);
            if (repuesto != null)
            {
                _context.Repuestos.Remove(repuesto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Repuestos.AnyAsync(r => r.Id == id);
        }

        public async Task<int> ContarStockCritico()
        {
            return await _context.Repuestos
                .CountAsync(r => r.CantidadStock <= r.StockMinimo && r.StockMinimo > 0);
        }

        public async Task AumentarStockAsync(int repuestoId, int cantidad)
        {
            var repuesto = await _context.Repuestos.FindAsync(repuestoId);
            if (repuesto != null)
            {
                repuesto.CantidadStock += cantidad;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DescontarStockAsync(int repuestoId, int cantidad)
        {
            var repuesto = await _context.Repuestos.FindAsync(repuestoId);
            if (repuesto != null && repuesto.CantidadStock >= cantidad)
            {
                repuesto.CantidadStock -= cantidad;
                await _context.SaveChangesAsync();
            }
        }
    }
}
