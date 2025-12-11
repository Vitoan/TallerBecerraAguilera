using TallerBecerraAguilera.Models;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TallerBecerraAguilera.Repositorios
{
    public class ProveedorRepositorio
    {
        public IQueryable<Proveedores> Query()
        {
            return _context.Proveedores.AsQueryable();
        }   

        private readonly ApplicationDbContext _context;

        public ProveedorRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedores>> GetAllAsync()
        {
            return await _context.Proveedores.ToListAsync();
        }

        public async Task<Proveedores?> GetByIdAsync(int id)
        {
            // Podríamos incluir Repuestos si fuera necesario, pero por ahora solo el proveedor
            return await _context.Proveedores.FindAsync(id); 
        }

        // **NUEVO:** Obtener por Nombre (para validación de unicidad)
        public async Task<Proveedores?> GetByNameAsync(string nombre)
        {
            return await _context.Proveedores.FirstOrDefaultAsync(p => p.Nombre == nombre);
        }

        public async Task AddAsync(Proveedores proveedor)
        {
            // Manejo de fechas de auditoría
            proveedor.Created_at = DateTime.Now;
            proveedor.Updated_at = DateTime.Now;
            
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Proveedores proveedor)
        {
            // Manejo de fechas de auditoría
            proveedor.Updated_at = DateTime.Now;
            
            _context.Proveedores.Update(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var proveedor = await GetByIdAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Proveedores.AnyAsync(p => p.Id == id);
        }
    }
}