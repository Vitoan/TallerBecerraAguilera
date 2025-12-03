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
    }
}