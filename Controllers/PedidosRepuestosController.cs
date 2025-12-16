using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TallerBecerraAguilera.Helpers; // Necesario para User.FindFirst

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

        public async Task<IActionResult> Index(
            int pageNumber = 1,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int? proveedorId = null,
            EstadoPedido? estado = null)
        {
            int pageSize = 10;

            var query = _repo.Query();

            // FILTROS
            if (fechaDesde.HasValue)
            {
                query = query.Where(p => p.Fecha.Date >= fechaDesde.Value.Date);
            }

            if (fechaHasta.HasValue)
            {
                query = query.Where(p => p.Fecha.Date <= fechaHasta.Value.Date);
            }

            if (proveedorId.HasValue)
            {
                query = query.Where(p => p.ProveedorId == proveedorId.Value);
            }

            if (estado.HasValue)
            {
            query = query.Where(p => p.Estado == estado.Value);
            }

            // ORDEN
            query = query.OrderByDescending(p => p.Fecha);
            // 📄 PAGINADO
            var paginated = await PaginatedList<PedidosRepuestos>
                .CreateAsync(query, pageNumber, pageSize);

            // DATOS PARA LA VISTA
            ViewBag.FechaDesde = fechaDesde;
            ViewBag.FechaHasta = fechaHasta;
            ViewBag.ProveedorId = proveedorId;
            ViewBag.Estado = estado;

            ViewBag.Proveedores = (await _proveedorRepo.GetAllAsync())
                .OrderBy(p => p.Nombre)
                .ToList();

            return View(paginated);
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

            if (User.IsInRole("Administrador"))
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

            if (!User.IsInRole("Administrador"))
            {
                if (empleado == null)
                    return BadRequest("No existe un empleado asociado a este usuario.");

                pedido.EmpleadoId = empleado.Id;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre");

                if (User.IsInRole("Administrador"))
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

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var pedido = await _repo.GetByIdAsync(id);
            if (pedido == null) return NotFound();

            ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre", pedido.ProveedorId);

            ViewBag.EmpleadoId = new SelectList(await _empleadoRepo.GetAllAsync(), "Id", "NombreCompleto", pedido.EmpleadoId);
            
            return View(pedido);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, PedidosRepuestos pedido)
        {
            if (id != pedido.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre", pedido.ProveedorId);

                ViewBag.EmpleadoId = new SelectList(await _empleadoRepo.GetAllAsync(), "Id", "NombreCompleto", pedido.EmpleadoId);
                
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

        public async Task<IActionResult> GetProveedores(string search)
        {
            var proveedores = await _proveedorRepo.GetAllAsync();
            var filtered = proveedores
                .Where(p => string.IsNullOrEmpty(search) || p.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase))
                .Select(p => new { id = p.Id, text = p.Nombre });

            return Json(new { results = filtered});
        }

        public async Task<IActionResult> GetEmpleados(string search)
        {
            var empleados = await _empleadoRepo.GetAllAsync();
            var filtered = empleados
                .Where(e => string.IsNullOrEmpty(search) ||
                            e.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                            e.Apellido.Contains(search, StringComparison.OrdinalIgnoreCase))
                .Select(e => new { id = e.Id, text = $"{e.Nombre} {e.Apellido}" });

            return Json(new { results = filtered });
        }
    }
}