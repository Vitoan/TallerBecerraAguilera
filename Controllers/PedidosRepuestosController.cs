using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using System.Security.Claims; // Necesario para User.FindFirst

namespace TallerBecerraAguilera.Controllers
{
    public class PedidosRepuestosController : Controller
    {
        private readonly PedidosRepuestosRepositorio _repo;
        private readonly ProveedorRepositorio _proveedorRepo;
        private readonly EmpleadoRepositorio _empleadoRepo;

        public PedidosRepuestosController(
            PedidosRepuestosRepositorio repo,
            ProveedorRepositorio proveedorRepo,
            EmpleadoRepositorio empleadoRepo)
        {
            _repo = repo;
            _proveedorRepo = proveedorRepo;
            _empleadoRepo = empleadoRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            // Gracias a la modificación en el Repositorio, esto traerá los detalles
            var pedido = await _repo.GetByIdAsync(id);
            if (pedido == null) return NotFound();
            return View(pedido);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre");

            // Asegúrate de que el usuario esté autenticado para evitar error aquí
            var claimId = User.FindFirst("Id");
            if (claimId == null) return RedirectToAction("Login", "Usuarios");

            var usuarioId = int.Parse(claimId.Value);
            var empleado = await _empleadoRepo.GetByUsuarioIdAsync(usuarioId);

            if (User.IsInRole("Admin"))
            {
                ViewBag.EmpleadoId = new SelectList(await _empleadoRepo.GetAllAsync(), "Id", "NombreCompleto");
            }
            else if (empleado != null)
            {
                ViewBag.EmpleadoId = empleado.Id;
            }
            else
            {
                return BadRequest("No existe un empleado asociado a este usuario.");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidosRepuestos pedido)
        {
            var claimId = User.FindFirst("Id");
            if (claimId == null) return RedirectToAction("Login", "Usuarios");

            var usuarioId = int.Parse(claimId.Value);
            var empleado = await _empleadoRepo.GetByUsuarioIdAsync(usuarioId);

            if (!User.IsInRole("Admin"))
            {
                if (empleado == null)
                    return BadRequest("No existe un empleado asociado a este usuario.");

                pedido.EmpleadoId = empleado.Id;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre");

                if (User.IsInRole("Admin"))
                    ViewBag.EmpleadoId = new SelectList(await _empleadoRepo.GetAllAsync(), "Id", "NombreCompleto");
                else
                    ViewBag.EmpleadoId = empleado?.Id;

                return View(pedido);
            }

            // Guardamos la cabecera del pedido
            var nuevoId = await _repo.AddAsync(pedido);
            
            // 👇 CAMBIO IMPORTANTE: Redirigimos a Details para agregar los repuestos
            return RedirectToAction(nameof(Details), new { id = nuevoId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pedido = await _repo.GetByIdAsync(id);
            if (pedido == null) return NotFound();

            ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre", pedido.ProveedorId);

            // Lógica de usuario
            var claimId = User.FindFirst("Id");
            // Nota: Aquí podrías manejar si claimId es null, pero asumimos auth activa
            var usuarioId = int.Parse(claimId!.Value); 
            var empleado = await _empleadoRepo.GetByUsuarioIdAsync(usuarioId);

            if (User.IsInRole("Admin"))
            {
                ViewBag.EmpleadoId = new SelectList(await _empleadoRepo.GetAllAsync(), "Id", "NombreCompleto", pedido.EmpleadoId);
            }
            else
            {
                if (empleado == null)
                    return BadRequest("No existe un empleado asociado a este usuario.");

                ViewBag.EmpleadoId = empleado.Id;
            }

            return View(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PedidosRepuestos pedido)
        {
            if (id != pedido.Id) return NotFound();

            var claimId = User.FindFirst("Id");
            var usuarioId = int.Parse(claimId!.Value);
            var empleado = await _empleadoRepo.GetByUsuarioIdAsync(usuarioId);

            if (!User.IsInRole("Admin"))
            {
                if (empleado == null)
                    return BadRequest("No existe un empleado asociado a este usuario.");

                pedido.EmpleadoId = empleado.Id;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre", pedido.ProveedorId);

                if (User.IsInRole("Admin"))
                    ViewBag.EmpleadoId = new SelectList(await _empleadoRepo.GetAllAsync(), "Id", "NombreCompleto", pedido.EmpleadoId);
                else
                    ViewBag.EmpleadoId = empleado?.Id;

                return View(pedido);
            }

            await _repo.UpdateAsync(pedido);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int pedidoId, EstadoPedido estado)
        {
            await _repo.CambiarEstadoAsync(pedidoId, estado);
            
            // Redirigimos a Details para ver el cambio reflejado en contexto
            return RedirectToAction(nameof(Details), new { id = pedidoId });
        }
    }
}