using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace TallerBecerraAguilera.Controllers
{
    [Authorize]
    public class OtRepuestosController : Controller
    {
        private readonly OtRepuestosRepositorio _repo;
        private readonly RepuestoRepositorio _repuestosRepo;
        private readonly OrdenTrabajoRepositorio _otRepo;

        public OtRepuestosController(
            OtRepuestosRepositorio repo,
            RepuestoRepositorio repuestosRepo,
            OrdenTrabajoRepositorio otRepo)
        {
            _repo = repo;
            _repuestosRepo = repuestosRepo;
            _otRepo = otRepo;
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _repo.ObtenerTodosAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Usar(int repuestoId)
        {
            int empleadoId = int.Parse(User.FindFirst("Id")!.Value);

            var repuesto = await _repuestosRepo.GetByIdAsync(repuestoId);
            if (repuesto == null)
                return NotFound();

            var ots = await _otRepo.ObtenerPorEmpleadoYEstadoAsync(
                empleadoId,
                EstadoOrden.EnReparacion);

            ViewBag.Repuesto = repuesto;
            ViewBag.OTs = ots.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"OT #{o.Id}"
            }).ToList();

            return View(new OtRepuestos
            {
                repuesto_id = repuestoId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Usar(OtRepuestos modelo)
        {
            int empleadoId = int.Parse(User.FindFirst("Id")!.Value);

            if (!ModelState.IsValid)
            {
                ViewBag.Repuesto = await _repuestosRepo.GetByIdAsync(modelo.repuesto_id);

                var ots = await _otRepo.ObtenerPorEmpleadoYEstadoAsync(
                    empleadoId,
                    EstadoOrden.EnReparacion);

                ViewBag.OTs = ots.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"OT #{o.Id}"
                }).ToList();

                return View(modelo);
            }

            bool ok = await _repo.UsarRepuestoAsync(
                modelo.ot_id,
                modelo.repuesto_id,
                empleadoId,
                modelo.cantidad_usada);

            TempData[ok ? "success" : "error"] =
                ok ? "Repuesto usado correctamente." :
                     "No se pudo usar el repuesto.";

            return RedirectToAction("Index", "Repuestos");
        }
    }
}
