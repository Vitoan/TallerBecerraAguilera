using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace TallerBecerraAguilera.Repositorios
{
    public class OtHerramientasRepositorio
    {
        private readonly ApplicationDbContext _context;

        public OtHerramientasRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<OtHerramientas> Query()
        {
            return _context.OtHerramientas
                .Include(o => o.Herramienta)
                .Include(o => o.Empleado)
                .Include(o => o.OrdenTrabajo);
        }

        public async Task<List<OtHerramientas>> ObtenerPorOTAsync(int otId)
        {
            return await _context.OtHerramientas
                .Include(o => o.Herramienta)
                .Include(o => o.Empleado)
                .Where(o => o.ot_id == otId)
                .OrderByDescending(o => o.fecha_prestamo)
                .ToListAsync();
        }

        public async Task<OtHerramientas?> ObtenerPorIdAsync(int id)
        {
            return await _context.OtHerramientas
                .Include(o => o.Herramienta)
                .Include(o => o.OrdenTrabajo)
                .Include(o => o.Empleado)
                .FirstOrDefaultAsync(o => o.id == id);
        }

        public async Task CrearAsync(OtHerramientas entidad)
        {
            _context.OtHerramientas.Add(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EditarAsync(OtHerramientas entidad)
        {
            _context.OtHerramientas.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var entidad = await ObtenerPorIdAsync(id);
            if (entidad != null)
            {
                _context.OtHerramientas.Remove(entidad);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<OtHerramientas>> ObtenerPorEmpleado(int empleadoId)
        {
            return await _context.OtHerramientas
                .Include(x => x.Herramienta)
                .Include(x => x.OrdenTrabajo)
                .Where(x => x.empleado_id == empleadoId)
                .OrderByDescending(x => x.fecha_prestamo)
                .ToListAsync();
        }

        public async Task<List<OtHerramientas>> ObtenerPendientesPorEmpleado(int empleadoId)
        {
            return await _context.OtHerramientas
                .Include(x => x.Herramienta)
                .Include(x => x.OrdenTrabajo)
                .Where(x =>
                    x.empleado_id == empleadoId &&
                    x.fecha_devolucion == null)
                .OrderByDescending(x => x.fecha_prestamo)
                .ToListAsync();
        }

        public async Task<bool> DevolverHerramienta(int id, int empleadoId)
        {
            var registro = await ObtenerPorIdAsync(id);

            if (registro == null)
                return false;

            if (registro.empleado_id != empleadoId)
                return false;

            if (registro.fecha_devolucion != null)
                return false;

            registro.fecha_devolucion = DateTime.Now;

            var herramienta = await _context.Herramientas
                .FirstOrDefaultAsync(x => x.Id == registro.herramienta_id);

            if (herramienta == null)
                return false;

            herramienta.Estado = EstadoHerramienta.Disponible;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
