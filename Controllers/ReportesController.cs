using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.ViewModels;
using TallerBecerraAguilera.Models;

namespace TallerBecerraAguilera.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            decimal precioHora = 15000m; 

            var ordenes = await _context.OrdenesTrabajo
                .Include(o => o.OtRepuestos)
                    .ThenInclude(otr => otr.Repuesto)
                .Where(o => o.Estado == EstadoOrden.Entregada && o.FechaIngreso >= DateTime.Now.AddMonths(-6))
                .ToListAsync();

            var datosIngresos = ordenes
                .GroupBy(o => new { o.FechaIngreso.Month, o.FechaIngreso.Year })
                .Select(g => new {
                    Etiqueta = $"{g.Key.Month}/{g.Key.Year}",
                    Total = g.Sum(o => 
                        (o.HorasEstimadas ?? 0) * precioHora + 
                        
                        // --- CORRECCIÓN AQUÍ ---
                        // Usamos '?' para ver si existe y '?? 0' por si es nulo
                        o.OtRepuestos.Sum(r => r.cantidad_usada * (r.Repuesto?.precioUnitario ?? 0))
                    )
                })
                .OrderBy(x => x.Etiqueta)
                .ToList();

            var datosEstados = await _context.OrdenesTrabajo
                .GroupBy(o => o.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            string NombreEstado(int e) => e switch { 
                0 => "Pendiente", 
                1 => "En Reparación", 
                2 => "Finalizada", 
                3 => "Entregada", 
                _ => "Desconocido" 
            };

            var modelo = new ReportesViewModel
            {
                Meses = datosIngresos.Select(x => x.Etiqueta).ToArray(),
                Totales = datosIngresos.Select(x => x.Total).ToArray(),
                EstadosNombres = datosEstados.Select(x => NombreEstado((int)x.Estado)).ToArray(),
                EstadosCantidades = datosEstados.Select(x => x.Cantidad).ToArray()
            };

            return View(modelo);
        }
    }
}