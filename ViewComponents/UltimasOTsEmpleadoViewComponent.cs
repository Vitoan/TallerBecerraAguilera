using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Models.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TallerBecerraAguilera.ViewComponents
{
    public class UltimasOTsEmpleadoViewComponent : ViewComponent
    {
        private readonly OrdenTrabajoRepositorio _repo;

        public UltimasOTsEmpleadoViewComponent(OrdenTrabajoRepositorio repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int empleadoId)
        {
            var ordenes = await _repo.GetByEmpleadoAsync(empleadoId);

            var model = ordenes
                .OrderByDescending(o => o.FechaIngreso)
                .Take(5)
                .Select(o => new UltimasOTsViewModel
                {
                    Id = o.Id,
                    DescripcionFalla = o.DescripcionFalla,
                    FechaIngreso = o.FechaIngreso,
                    Estado = o.Estado.ToString(),
                    Patente = $"{o.Vehiculo?.marca} {o.Vehiculo?.modelo} ({o.Vehiculo?.patente})",
                    EmpleadoAsignado = o.Empleado?.NombreCompleto ?? ""
                })
                .ToList();

            return View(model);
        }
    }
}
