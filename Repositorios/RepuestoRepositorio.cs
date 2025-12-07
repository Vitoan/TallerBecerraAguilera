using TallerBecerraAguilera.Models;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;

namespace TallerBecerraAguilera.Repositorios
{
    public class RepuestoRepositorio : RepositorioBase<Repuestos>
    {
        public RepuestoRepositorio(ApplicationDbContext context) : base(context) { }

        public async Task<int> ContarStockCritico() =>
            await _context.Repuestos
                .CountAsync(r => r.CantidadStock <= r.StockMinimo && r.StockMinimo > 0);

        public async Task<IEnumerable<Repuestos>> GetAllWithProveedorAsync()
        {
            return await _context.Repuestos
                .Include(r => r.Proveedor)
                .ToListAsync();
        }

        public async Task<Repuestos?> GetByIdWithProveedorAsync(int id)
        {
            return await _context.Repuestos
                .Include(r => r.Proveedor)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Repuestos>> GetDisponiblesAsync()
        {
            return await _context.Repuestos
                .Where(r => r.CantidadStock > 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Proveedores>> GetAllProveedoresAsync()
        {
            return await _context.Proveedores.ToListAsync();
        }

        public async Task DescontarStockAsync(int repuestoId, int cantidad)
        {
            var repuesto = await _context.Repuestos.FindAsync(repuestoId);
            if (repuesto != null)
            {
                repuesto.CantidadStock -= cantidad;
                if (repuesto.CantidadStock < 0)
                    repuesto.CantidadStock = 0;

                await _context.SaveChangesAsync();
            }
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

        public async Task<bool> ExisteRepuestoAsync(int id)
        {
            return await _context.Repuestos.AnyAsync(r => r.Id == id);
        }
    }
}
