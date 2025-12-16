using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace TallerBecerraAguilera.Controllers
{
    [Authorize]
    public class OtHerramientasController : Controller
    {
        private readonly OtHerramientasRepositorio _repo;
        private readonly HerramientaRepositorio _herramientasRepo;
        private readonly OrdenTrabajoRepositorio _otRepo;

        public OtHerramientasController(
            OtHerramientasRepositorio repo,
            HerramientaRepositorio herramientasRepo,
            OrdenTrabajoRepositorio otRepo)
        {
            _repo = repo;
            _herramientasRepo = herramientasRepo;
            _otRepo = otRepo;
        }

        public async Task<IActionResult> Index(string empleado = "")
        {
            if (User.IsInRole("Administrador"))
            {
        // Obtenemos todos los registros
                var query = _repo.Query();

                // Filtrado por empleado si se pasó algo en el input
                if (!string.IsNullOrEmpty(empleado))
                {
                    query = query.Where(o =>
                        (o.Empleado != null &&
                        (o.Empleado.Nombre.Contains(empleado, StringComparison.OrdinalIgnoreCase) ||
                        o.Empleado.Apellido.Contains(empleado, StringComparison.OrdinalIgnoreCase)))
                    );
                }

                var lista = await query
                    .OrderByDescending(x => x.fecha_prestamo)
                    .ToListAsync();

                ViewBag.EmpleadoSeleccionado = empleado; // Para mantener el texto en el input

                return View(lista);
            }

            // Si es empleado, solo mostramos sus pedidos sin filtro
            int empleadoId = int.Parse(User.FindFirst("Id")!.Value);
            return View(await _repo.ObtenerPorEmpleado(empleadoId));
        }

        public async Task<IActionResult> Create(int herramientaId)
        {
            int empleadoId = int.Parse(User.FindFirst("Id")!.Value);

            var herramienta = await _herramientasRepo.GetByIdAsync(herramientaId);
            if (herramienta == null || herramienta.Estado != EstadoHerramienta.Disponible)
                return NotFound();

            var ots = await _otRepo.ObtenerPorEmpleadoYEstadoAsync(
                empleadoId,
                EstadoOrden.EnReparacion);

            ViewBag.Herramienta = herramienta;
            ViewBag.OTs = ots ?? new List<OrdenesTrabajo>();

            return View(new OtHerramientas
            {
                herramienta_id = herramientaId,
                empleado_id = empleadoId,
                fecha_prestamo = DateTime.Now
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OtHerramientas modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Herramienta = await _herramientasRepo.GetByIdAsync(modelo.herramienta_id);
                ViewBag.OTs = await _otRepo.ObtenerPorEmpleadoYEstadoAsync(
                    modelo.empleado_id,
                    EstadoOrden.EnReparacion);

                return View(modelo);
            }

            modelo.fecha_prestamo = DateTime.Now;

            await _repo.CrearAsync(modelo);

            var herramienta = await _herramientasRepo.GetByIdAsync(modelo.herramienta_id);
            if (herramienta != null)
            {
                herramienta.Estado = EstadoHerramienta.EnUso;
                await _herramientasRepo.UpdateAsync(herramienta);
            }

            return RedirectToAction("Index", "Herramientas");
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MisHerramientas()
        {
            int empleadoId = int.Parse(User.FindFirst("Id")!.Value);
            return View(await _repo.ObtenerPendientesPorEmpleado(empleadoId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int id)
        {
            int empleadoId = int.Parse(User.FindFirst("Id")!.Value);

            bool ok = await _repo.DevolverHerramienta(id, empleadoId);

            TempData[ok ? "success" : "error"] =
                ok ? "Herramienta devuelta correctamente." : "No podés devolver esta herramienta.";

            return RedirectToAction(nameof(MisHerramientas));
        }
    }
}
