using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Models;
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

        // ===========================
        // Lista de herramientas de una OT
        // ===========================
        public async Task<IActionResult> Index(int otId)
        {
            var lista = await _repo.ObtenerPorOTAsync(otId);
            ViewBag.OT = await _otRepo.GetByIdAsync(otId);
            return View(lista);
        }

        // ===========================
        // GET: formulario para pedir prestada la herramienta
        // ===========================
        public async Task<IActionResult> Create(int herramientaId)
        {
            int empleadoId = int.Parse(User.FindFirst("Id")!.Value);

            var herramienta = await _herramientasRepo.GetByIdAsync(herramientaId);
            if (herramienta == null || herramienta.Estado != EstadoHerramienta.Disponible)
                return NotFound("Herramienta no disponible.");

            // Traemos las OTs asignadas al empleado en estado EnReparacion
            var ots = await _otRepo.ObtenerPorEmpleadoYEstadoAsync(empleadoId, EstadoOrden.EnReparacion);

            ViewBag.Herramienta = herramienta!;
            ViewBag.OTs = ots ?? new List<OrdenesTrabajo>();

            return View(new OtHerramientas
            {
                herramienta_id = herramientaId,
                empleado_id = empleadoId,
                fecha_prestamo = DateTime.Now.Date
            });
        }

        // ===========================
        // POST: guardar herramienta pedida
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OtHerramientas modelo)
        {
            if (!ModelState.IsValid)
            {
                var herramienta = await _herramientasRepo.GetByIdAsync(modelo.herramienta_id);
                var ots = await _otRepo.ObtenerPorEmpleadoYEstadoAsync(modelo.empleado_id, EstadoOrden.EnReparacion);

                ViewBag.Herramienta = herramienta!;
                ViewBag.OTs = ots ?? new List<OrdenesTrabajo>();

                return View(modelo);
            }

            // Fecha de préstamo
            modelo.fecha_prestamo = DateTime.Now.Date;

            // Guardamos la relación OT-Herramienta
            await _repo.CrearAsync(modelo);

            // Marcar la herramienta como en uso
            var herramientaToUpdate = await _herramientasRepo.GetByIdAsync(modelo.herramienta_id);
            if (herramientaToUpdate != null)
            {
                herramientaToUpdate.Estado = EstadoHerramienta.EnUso;
                await _herramientasRepo.UpdateAsync(herramientaToUpdate);
            }

            return RedirectToAction("Index", "Herramientas");
        }

        // ===========================
        // Administrador: eliminar relación OT-Herramienta
        // ===========================
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int otId, int herramientaId)
        {
            var entidad = await _repo.ObtenerAsync(otId, herramientaId);
            if (entidad == null)
                return NotFound();

            await _repo.EliminarAsync(otId, herramientaId);
            return RedirectToAction("Index", new { otId });
        }

        // ===========================
        // Herramientas pedidas por el empleado
        // ===========================
        public async Task<IActionResult> MisHerramientas()
        {
            int empleadoId = int.Parse(User.FindFirst("Id")!.Value);
            var lista = await _repo.ObtenerPorEmpleado(empleadoId);
            return View(lista);
        }

        // ===========================
        // Devolver herramienta
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int otId, int herramientaId)
        {
            int empleadoId = int.Parse(User.FindFirst("Id")!.Value);

            bool ok = await _repo.DevolverHerramienta(otId, herramientaId, empleadoId);

            if (ok)
                TempData["success"] = "Herramienta devuelta correctamente.";
            else
                TempData["error"] = "No podés devolver una herramienta que no está asignada a vos.";

            return RedirectToAction("MisHerramientas");
        }
    }
}
