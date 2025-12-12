using TallerBecerraAguilera.Models;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;

namespace TallerBecerraAguilera.Repositorios
{
    public class PedidoRepuestosRepositorio
    {
        private readonly ApplicationDbContext _context;
        private readonly RepuestoRepositorio _repuestoRepo;

        public PedidoRepuestosRepositorio(ApplicationDbContext context, RepuestoRepositorio repuestoRepo)
        {
            _context = context;
            _repuestoRepo = repuestoRepo;
        }

        public async Task<IEnumerable<PedidoRepuestos>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.PedidoRepuestos
                .Where(p => p.pedido_id == pedidoId)
                .Include(p => p.Repuesto)
                .ToListAsync();
        }

        public async Task AddAsync(PedidoRepuestos item)
        {
            _context.PedidoRepuestos.Add(item);
            await _context.SaveChangesAsync();
        }

        // NUEVO: Eliminar un detalle del pedido
        public async Task DeleteAsync(int pedidoId, int repuestoId)
        {
            Console.WriteLine($"Delete pedidoId={pedidoId}, repuestoId={repuestoId}");
            var item = await _context.PedidoRepuestos
                .FirstOrDefaultAsync(p => p.pedido_id == pedidoId && p.repuesto_id == repuestoId);

            if (item != null)
            {
                _context.PedidoRepuestos.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RecibirRepuestoAsync(int pedidoId, int repuestoId, int cantidad)
        {
            var item = await _context.PedidoRepuestos
                .FirstOrDefaultAsync(p => p.pedido_id == pedidoId && p.repuesto_id == repuestoId);

            if (item != null)
            {
                item.cantidad_recibida += cantidad;
                await _context.SaveChangesAsync();
                await _repuestoRepo.AumentarStockAsync(repuestoId, cantidad);
            }
        }

        public async Task<IEnumerable<Repuestos>> GetAllRepuestosAsync()
        {
            return await _context.Repuestos.OrderBy(r => r.descripcion).ToListAsync();
        }
    }
}