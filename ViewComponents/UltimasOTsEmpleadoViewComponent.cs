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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var claimId = claimsPrincipal?.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(claimId))
                return Content("Empleado no identificado");

            if (!int.TryParse(claimId, out int empleadoId))
                return Content("Empleado no válido");

            var ordenes = await _repo.GetByEmpleadoAsync(empleadoId);

            var model = ordenes.Select(o => new UltimasOTsViewModel
            {
                Id = o.Id,
                DescripcionFalla = o.DescripcionFalla,
                FechaIngreso = o.FechaIngreso,
                Estado = o.Estado.ToString(),
                Patente = $"{o.Vehiculo?.Marca} {o.Vehiculo?.Modelo} ({o.Vehiculo?.Patente})",
                EmpleadoAsignado = o.Empleado?.NombreCompleto ?? ""
            }).ToList();

            return View(model);
        }
    }
}
