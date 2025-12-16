using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TallerBecerraAguilera.Helpers;

namespace TallerBecerraAguilera.Repositorios
{
    public class OrdenTrabajoRepositorio
    {
        
        public IQueryable<OrdenesTrabajo> Query()
        {
            return _context.OrdenesTrabajo
                .Include(o => o.Vehiculo)
                    .ThenInclude(v => v!.Cliente)
                .Include(o => o.Empleado)
                .AsQueryable();
        }


        public async Task<PaginatedList<OrdenesTrabajo>> GetAllPaginatedAsync(int pageIndex, int pageSize)
        {
            // Preparamos la consulta pero NO la ejecutamos todavía (no usamos ToListAsync aquí)
            var query = _context.OrdenesTrabajo
                .Include(o => o.Vehiculo).ThenInclude(v => v!.Cliente)
                .Include(o => o.Empleado)
                .OrderByDescending(o => o.FechaIngreso) // Ordenamos por fecha (las más nuevas primero)
                .AsQueryable();

            // El helper se encarga de ejecutar el SQL con LIMIT y OFFSET
            return await PaginatedList<OrdenesTrabajo>.CreateAsync(query, pageIndex, pageSize);
        }
        private readonly ApplicationDbContext _context;

        public OrdenTrabajoRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrdenesTrabajo>> GetAllAsync()
        {
            return await _context.OrdenesTrabajo
                .Include(o => o.Vehiculo)
                    .ThenInclude(v => v!.Cliente)
                .Include(o => o.Empleado)
                .OrderByDescending(o => o.FechaIngreso)
                .ToListAsync();
        }

        public async Task<OrdenesTrabajo?> GetByIdAsync(int id)
        {
            return await _context.OrdenesTrabajo
                .Include(o => o.Vehiculo)
                    .ThenInclude(v => v!.Cliente)
                .Include(o => o.Empleado)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddAsync(OrdenesTrabajo ot)
        {
            ot.Created_at = DateTime.Now;
            ot.Updated_at = DateTime.Now;
            if (ot.FechaIngreso == default)
                ot.FechaIngreso = DateTime.Now;

            _context.OrdenesTrabajo.Add(ot);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrdenesTrabajo ot)
        {
            ot.Updated_at = DateTime.Now;
            _context.OrdenesTrabajo.Update(ot);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ot = await GetByIdAsync(id);
            if (ot != null)
            {
                _context.OrdenesTrabajo.Remove(ot);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.OrdenesTrabajo.AnyAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Vehiculos>> GetAllVehiculosAsync()
        {
            return await _context.Vehiculos.Include(v => v.Cliente).ToListAsync();
        }

        public async Task<IEnumerable<Empleados>> GetAllEmpleadosAsync()
        {
            return await _context.Empleados.ToListAsync();
        }

        public async Task<int> GetCountByEstadoAsync(EstadoOrden estado)
        {
            return await _context.OrdenesTrabajo.CountAsync(o => o.Estado == estado);
        }

        public async Task<int> GetStockCriticoCountAsync()
        {
            return await _context.Repuestos
                .CountAsync(r => r.cantidadStock <= r.stockMinimo && r.stockMinimo > 0);
        }

        public async Task<int> GetCountByEmpleadoAndEstadoAsync(int empleadoId, EstadoOrden estado)
        {
            return await _context.OrdenesTrabajo
                .CountAsync(o => o.EmpleadoId == empleadoId && o.Estado == estado);
        }

        public async Task<List<OrdenesTrabajo>> GetByEmpleadoAsync(int empleadoId)
        {
            return await _context.OrdenesTrabajo
                .Where(o => o.EmpleadoId == empleadoId)
                .Include(o => o.Vehiculo)
                    .ThenInclude(v => v!.Cliente)
                .Include(o => o.Empleado)
                .OrderByDescending(o => o.FechaIngreso)
                .ToListAsync();
        }

        public async Task<PaginatedList<OrdenesTrabajo>> GetByEmpleadoPaginatedAsync(int empleadoId, int pageIndex, int pageSize)
        {
            var query = _context.OrdenesTrabajo
                .Where(o => o.EmpleadoId == empleadoId)
                .Include(o => o.Vehiculo)
                    .ThenInclude(v => v!.Cliente)
                .Include(o => o.Empleado)
                .OrderByDescending(o => o.FechaIngreso)
                .AsQueryable();

            return await PaginatedList<OrdenesTrabajo>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<List<OrdenesTrabajo>> ObtenerPorEmpleadoYEstadoAsync(int empleadoId, EstadoOrden estado)
    {
            return await _context.OrdenesTrabajo
                .Where(o => o.EmpleadoId == empleadoId && o.Estado == estado)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }
    }
}
