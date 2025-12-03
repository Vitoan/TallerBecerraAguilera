using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;
using System.Threading.Tasks;
using TallerBecerraAguilera.Data;

namespace TallerBecerraAguilera.Repositorio // Namespace correcto para la raíz
{
    public class OrdenTrabajoRepositorio
    {
        private readonly ApplicationDbContext _context;

        public OrdenTrabajoRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCountByEstadoAsync(EstadoOrden estado)
        {
            return await _context.OrdenesTrabajo
                .CountAsync(o => o.Estado == estado);
        }

        public async Task<int> GetStockCriticoCountAsync()
        {
            return await _context.Repuestos
                .CountAsync(r => r.CantidadStock <= r.StockMinimo && r.StockMinimo > 0);
        }
    }
}