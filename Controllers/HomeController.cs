using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace TallerBecerraAguilera.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly OrdenTrabajoRepositorio _ordenTrabajoRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio;

        public HomeController(OrdenTrabajoRepositorio ordenTrabajoRepositorio, EmpleadoRepositorio empleadoRepositorio)
        {
            _ordenTrabajoRepositorio = ordenTrabajoRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrador"))
            {
                // Dashboard de Admin
                ViewBag.OrdenesPendientes = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.Pendiente);
                ViewBag.OrdenesEnReparacion = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.EnReparacion);
                
                var finalizadas = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.Finalizada);
                var entregadas = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.Entregada);
                ViewBag.OrdenesFinalizadas = finalizadas + entregadas;
                
                // Asegúrate que tu repositorio tenga este método, si no, comenta esta línea temporalmente
                // ViewBag.StockCritico = await _ordenTrabajoRepositorio.GetStockCriticoCountAsync(); 
                ViewBag.StockCritico = 0; // Valor por defecto si no tienes el método aún

                return View(); // Carga Views/Home/Index.cshtml
            }
            else if (User.IsInRole("Empleado"))
            {
                // Dashboard de Empleado
                var claimId = User.FindFirst("Id");
                if (claimId == null || !int.TryParse(claimId.Value, out int usuarioId))
                {
                    return RedirectToAction("Login", "Usuarios");
                }

                var empleado = await _empleadoRepositorio.GetByUsuarioIdAsync(usuarioId);
                if (empleado == null) return BadRequest("Empleado no encontrado vinculado a este usuario.");

                ViewBag.MisOTsPendientes = await _ordenTrabajoRepositorio.GetCountByEmpleadoAndEstadoAsync(empleado.Id, EstadoOrden.Pendiente);
                ViewBag.MisOTsEnCurso = await _ordenTrabajoRepositorio.GetCountByEmpleadoAndEstadoAsync(empleado.Id, EstadoOrden.EnReparacion);
                ViewBag.MisOTsFinalizadas = await _ordenTrabajoRepositorio.GetCountByEmpleadoAndEstadoAsync(empleado.Id, EstadoOrden.Finalizada)
                                          + await _ordenTrabajoRepositorio.GetCountByEmpleadoAndEstadoAsync(empleado.Id, EstadoOrden.Entregada);

                // IMPORTANTE: Debes crear la vista 'IndexEmpleado.cshtml' si quieres un dashboard distinto
                // O usar la misma Index y ocultar cosas con if(User.IsInRole...)
                return View("IndexEmpleado", empleado); 
            }

            return RedirectToAction("Login", "Usuarios");
        }

        public IActionResult Restringido()
        {
            return View();
        }
    }
}