using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
                .Include(o => o.OrdenTrabajo);
        }

        public async Task<List<OtHerramientas>> ObtenerPorOTAsync(int otId)
        {
            return await _context.OtHerramientas
                .Include(o => o.Herramienta)
                .Include(o => o.Empleado)
                .Where(o => o.ot_id == otId)
                .ToListAsync();
        }

        public async Task<OtHerramientas?> ObtenerAsync(int otId, int herramientaId)
        {
            return await _context.OtHerramientas
                .Include(o => o.Herramienta)
                .Include(o => o.OrdenTrabajo)
                .Include(o => o.Empleado)
                .FirstOrDefaultAsync(o =>
                    o.ot_id == otId &&
                    o.herramienta_id == herramientaId);
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

        public async Task EliminarAsync(int otId, int herramientaId)
        {
            var entidad = await ObtenerAsync(otId, herramientaId);
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

        public async Task<bool> DevolverHerramienta(int otId, int herramientaId, int empleadoId)
        {
            var registro = await ObtenerAsync(otId, herramientaId);

            if (registro == null)
                return false;

            if (registro.empleado_id != empleadoId)
                return false;

            registro.fecha_devolucion = DateTime.Now;

            var herramienta = await _context.Herramientas
                .FirstOrDefaultAsync(x => x.Id == herramientaId);

            if (herramienta == null)
                return false;

            herramienta.Estado = EstadoHerramienta.Disponible;
            _context.Herramientas.Update(herramienta);

            _context.OtHerramientas.Remove(registro);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
