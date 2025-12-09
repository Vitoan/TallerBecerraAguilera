using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Repositorios; // Usando el repositorio sin interfaz
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TallerBecerraAguilera.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly OrdenTrabajoRepositorio _ordenTrabajoRepositorio;

        public HomeController(OrdenTrabajoRepositorio ordenTrabajoRepositorio)
        {
            _ordenTrabajoRepositorio = ordenTrabajoRepositorio;
        }

        // Soluciona el warning CS1998 usando 'await' y Task<IActionResult>
        public async Task<IActionResult> Index()
        {
            // Obtener contadores usando los métodos asíncronos del repositorio
            ViewBag.OrdenesPendientes = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.Pendiente);
            
            ViewBag.OrdenesEnReparacion = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.EnReparacion);
            
            // Contar Finalizada y Entregada juntas
            var finalizadas = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.Finalizada);
            var entregadas = await _ordenTrabajoRepositorio.GetCountByEstadoAsync(EstadoOrden.Entregada);
            ViewBag.OrdenesFinalizadas = finalizadas + entregadas;

            ViewBag.StockCritico = await _ordenTrabajoRepositorio.GetStockCriticoCountAsync();

            return View();
        }
        
    }
}