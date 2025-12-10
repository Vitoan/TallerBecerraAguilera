using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
                ViewBag.OrdenesPendientes = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.Pendiente);
                ViewBag.OrdenesEnReparacion = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.EnReparacion);
                var finalizadas = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.Finalizada);
                var entregadas = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.Entregada);
                ViewBag.OrdenesFinalizadas = finalizadas + entregadas;
                ViewBag.StockCritico = await _ordenTrabajoRepositorio.GetStockCriticoCountAsync();
                return View(); // Index.cshtml para admins
            }
            else if (User.IsInRole("Empleado"))
            {
                var usuarioId = int.Parse(User.FindFirst("Id")!.Value);
                var empleado = await _empleadoRepositorio.GetByUsuarioIdAsync(usuarioId);
                if (empleado == null)
                    return BadRequest("Empleado no encontrado");

                ViewBag.MisOTsPendientes = await _ordenTrabajoRepositorio.GetCountByEmpleadoAndEstadoAsync(empleado.Id, EstadoOrden.Pendiente);
                ViewBag.MisOTsEnCurso = await _ordenTrabajoRepositorio.GetCountByEmpleadoAndEstadoAsync(empleado.Id, EstadoOrden.EnReparacion);
                ViewBag.MisOTsFinalizadas = await _ordenTrabajoRepositorio.GetCountByEmpleadoAndEstadoAsync(empleado.Id, EstadoOrden.Finalizada)
                                            + await _ordenTrabajoRepositorio.GetCountByEmpleadoAndEstadoAsync(empleado.Id, EstadoOrden.Entregada);

                return View("IndexEmpleado", empleado);
            }

            return RedirectToAction("Login", "Usuarios");
        }

        public IActionResult Restringido()
        {
            ViewData["Title"] = "Acceso Denegado";
            return View();
        }
    }
}
