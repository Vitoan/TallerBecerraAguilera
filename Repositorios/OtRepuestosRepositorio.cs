using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace TallerBecerraAguilera.Repositorios
{
    public class OtRepuestosRepositorio
    {
        private readonly ApplicationDbContext _context;

        public OtRepuestosRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<OtRepuestos> Query()
        {
            return _context.OtRepuestos
                .Include(x => x.Repuesto)
                .Include(x => x.OrdenTrabajo)
                .Include(x => x.Empleado);
        }

        public async Task<List<OtRepuestos>> ObtenerTodosAsync()
        {
            return await Query()
                .OrderByDescending(x => x.id)
                .ToListAsync();
        }

        public async Task<List<OtRepuestos>> ObtenerPorOTAsync(int otId)
        {
            return await Query()
                .Where(x => x.ot_id == otId)
                .OrderByDescending(x => x.id)
                .ToListAsync();
        }

        public async Task<bool> UsarRepuestoAsync(
            int otId,
            int repuestoId,
            int empleadoId,
            int cantidad)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var repuesto = await _context.Repuestos
                    .FirstOrDefaultAsync(r => r.id == repuestoId);

                if (repuesto == null)
                    return false;

                if (repuesto.cantidadStock < cantidad)
                    return false;

                bool existe = await _context.OtRepuestos
                    .AnyAsync(x => x.ot_id == otId && x.repuesto_id == repuestoId);

                if (existe)
                    return false;

                var registro = new OtRepuestos
                {
                    ot_id = otId,
                    repuesto_id = repuestoId,
                    empleado_id = empleadoId,
                    cantidad_usada = cantidad
                };

                _context.OtRepuestos.Add(registro);

                repuesto.cantidadStock -= cantidad;
                repuesto.updated_at = DateTime.Now;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task EliminarAsync(int id)
        {
            var entidad = await _context.OtRepuestos.FindAsync(id);
            if (entidad != null)
            {
                _context.OtRepuestos.Remove(entidad);
                await _context.SaveChangesAsync();
            }
        }
    }
}
