using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TallerBecerraAguilera.Repositorios
{
    public class OrdenTrabajoRepositorio
    {
        private readonly ApplicationDbContext _context;

        public OrdenTrabajoRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // =======================================================
        // CRUD BÁSICO
        // =======================================================

        // Obtener todas las Órdenes de Trabajo (LISTAR)
        public async Task<List<OrdenesTrabajo>> GetAllAsync()
        {
            return await _context.OrdenesTrabajo
                .Include(o => o.Vehiculo) // Eager loading: Carga el Vehículo
                    .ThenInclude(v => v!.Cliente) // Carga el Cliente del Vehículo
                .Include(o => o.Empleado) // Eager loading: Carga el Empleado
                .OrderByDescending(o => o.FechaIngreso)
                .ToListAsync();
        }

        // Obtener una OT por ID (LEER/BUSCAR - Incluye relaciones)
        public async Task<OrdenesTrabajo?> GetByIdAsync(int id)
        {
            return await _context.OrdenesTrabajo
                .Include(o => o.Vehiculo)
                    .ThenInclude(v => v!.Cliente) 
                .Include(o => o.Empleado)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        
        // Agregar una nueva OT (CREAR)
        public async Task AddAsync(OrdenesTrabajo ot)
        {
            ot.Created_at = DateTime.Now;
            ot.Updated_at = DateTime.Now;
            if (ot.FechaIngreso == default) 
            {
                ot.FechaIngreso = DateTime.Now;
            }
            
            _context.OrdenesTrabajo.Add(ot);
            await _context.SaveChangesAsync();
        }

        // Actualizar una OT existente (ACTUALIZAR)
        public async Task UpdateAsync(OrdenesTrabajo ot)
        {
            ot.Updated_at = DateTime.Now;
            _context.OrdenesTrabajo.Update(ot);
            await _context.SaveChangesAsync();
        }

        // Eliminar una OT por ID (ELIMINAR)
        public async Task DeleteAsync(int id)
        {
            var ot = await GetByIdAsync(id);
            if (ot != null)
            {
                _context.OrdenesTrabajo.Remove(ot);
                await _context.SaveChangesAsync();
            }
        }
        
        // =======================================================
        // MÉTODOS DE UTILIDAD Y DROPDOWNS
        // =======================================================

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.OrdenesTrabajo.AnyAsync(o => o.Id == id);
        }
        
        // Obtener todos los vehículos (para DropDownList en Create/Edit)
        public async Task<IEnumerable<Vehiculos>> GetAllVehiculosAsync()
        {
            return await _context.Vehiculos.Include(v => v.Cliente).ToListAsync();
        }

        // Obtener todos los empleados (para DropDownList en Create/Edit)
        public async Task<IEnumerable<Empleados>> GetAllEmpleadosAsync()
        {
            return await _context.Empleados.ToListAsync();
        }


        // Métodos de conteo (tal como los tenías)
        public async Task<int> GetCountByEstadoAsync(EstadoOrden estado)
        {
            return await _context.OrdenesTrabajo.CountAsync(o => o.Estado == estado);
        }

        public async Task<int> GetStockCriticoCountAsync()
        {
            // Nota: Este método idealmente iría en RepuestoRepositorio
            return await _context.Repuestos
                .CountAsync(r => r.CantidadStock <= r.StockMinimo && r.StockMinimo > 0);
        }
    }
}