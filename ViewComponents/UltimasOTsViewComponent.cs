using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models.ViewModels;
using System.Linq; // Necesario para Select y OrderBy
using System.Threading.Tasks;

namespace TallerBecerraAguilera.ViewComponents
{
    public class UltimasOTsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public UltimasOTsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int cantidad = 5)
        {
            var ultimasOts = await _context.OrdenesTrabajo
                .Include(o => o.Vehiculo)
                .Include(o => o.Empleado)
                .OrderByDescending(o => o.FechaIngreso)
                .Take(cantidad)
                .Select(o => new UltimasOTsViewModel
                {
                    Id = o.Id,
                    DescripcionFalla = o.DescripcionFalla,
                    FechaIngreso = o.FechaIngreso,
                    Estado = o.Estado.ToString(),
                    Patente = o.Vehiculo != null ? o.Vehiculo.patente : "S/P",
                    EmpleadoAsignado = o.Empleado != null ? $"{o.Empleado.Nombre} {o.Empleado.Apellido}" : "Sin Asignar"
                })
                .ToListAsync();

            return View(ultimasOts);
        }
    }
}