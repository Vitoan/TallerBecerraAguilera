using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Models;
using System.Threading.Tasks;
// NECESARIO para DbUpdateConcurrencyException
using Microsoft.EntityFrameworkCore; 

namespace TallerBecerraAguilera.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using TallerBecerraAguilera.Helpers;

    [Authorize(Roles = "Administrador")]
    public class ClientesController : Controller
    {
        private readonly ClienteRepositorio _clienteRepositorio;

        public ClientesController(ClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // GET: Clientes (LISTAR TODOS)
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;

            var query = _clienteRepositorio.Query();

            var paginated = await PaginatedList<Clientes>.CreateAsync(query, pageNumber, pageSize);
            
            return View(paginated);
        }

        // GET: Clientes/Details/5 (VISTA DETALLES)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _clienteRepositorio.GetByIdAsync(id.Value); 
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // GET: Clientes/Create (VISTA CREAR)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create (ACCIÓN CREAR)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,Dni,Telefono,Email")] Clientes cliente)
        {
            // VALIDACIÓN: DNI duplicado
            if (cliente.Dni != null && await _clienteRepositorio.GetByDniAsync(cliente.Dni) != null)
            {
                ModelState.AddModelError("Dni", "Ya existe un cliente con este DNI.");
            }

            if (ModelState.IsValid)
            {
                await _clienteRepositorio.AddAsync(cliente);
                TempData["Mensaje"] = "Cliente creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5 (VISTA EDITAR)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _clienteRepositorio.GetByIdAsync(id.Value);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: Clientes/Edit/5 (ACCIÓN ACTUALIZAR)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Dni,Telefono,Email,Created_at")] Clientes cliente)
        {
            if (id != cliente.Id) return NotFound();
            
            // VALIDACIÓN: DNI duplicado, excluyendo el cliente que se está editando
            if (cliente.Dni != null)
            {
                var existingClient = await _clienteRepositorio.GetByDniAsync(cliente.Dni);
                if (existingClient != null && existingClient.Id != cliente.Id)
                {
                    ModelState.AddModelError("Dni", "El DNI ya está registrado para otro cliente.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _clienteRepositorio.UpdateAsync(cliente);
                    TempData["Mensaje"] = "Cliente actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException) // Manejo de concurrencia
                {
                    if (await _clienteRepositorio.ExistsAsync(cliente.Id) == false)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        
        // GET: Clientes/Delete/5 (VISTA ELIMINAR)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _clienteRepositorio.GetByIdAsync(id.Value);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: Clientes/Delete/5 (ACCIÓN ELIMINAR)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteRepositorio.DeleteAsync(id);
            TempData["Mensaje"] = "Cliente eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/BuscarClientes?term=juan
        [HttpGet]
        public async Task<IActionResult> BuscarClientes(string term)
        {
            var clientes = await _clienteRepositorio.BuscarPorTerminoAsync(term);

            // Select2 espera un formato específico: { results: [ { id: 1, text: 'Juan' }, ... ] }
            var resultadoJson = clientes.Select(c => new 
            { 
                id = c.Id, 
                text = $"{c.Apellido}, {c.Nombre} (DNI: {c.Dni})" 
            });

            return Json(new { results = resultadoJson });
        }
    }
}