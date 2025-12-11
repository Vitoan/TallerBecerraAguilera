using TallerBecerraAguilera.Models;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;

namespace TallerBecerraAguilera.Repositorios
{
    public class PedidosRepuestosRepositorio
    {
        private readonly ApplicationDbContext _context;

        public PedidosRepuestosRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidosRepuestos>> GetAllAsync()
        {
            return await _context.PedidosRepuestos.ToListAsync();
        }

        public async Task<PedidosRepuestos?> GetByIdAsync(int id)
        {
            return await _context.PedidosRepuestos
            .Include(p => p.Proveedor)
            .Include(p => p.Empleado)
            .Include(p => p.Detalles) // Carga los detalles
                .ThenInclude(d => d.Repuesto) // Carga el repuesto de cada detalle
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> AddAsync(PedidosRepuestos pedido)
        {
            _context.PedidosRepuestos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido.Id;
        }

        public async Task UpdateAsync(PedidosRepuestos pedido)
        {
            _context.PedidosRepuestos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task CambiarEstadoAsync(int pedidoId, EstadoPedido estado)
        {
            var pedido = await _context.PedidosRepuestos.FindAsync(pedidoId);
            if (pedido != null)
            {
                pedido.Estado = estado;
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<PedidosRepuestos> Query()
        {
            return _context.PedidosRepuestos
                .Include(p => p.Proveedor)
                .Include(p => p.Empleado)
                .AsQueryable();
        }
    }
}
