using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necesario para DbUpdateConcurrencyException
using System.Linq;
using System.Threading.Tasks;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace TallerBecerraAguilera.Controllers
{
    [Authorize(Roles = "Administrador")] // O el rol que uses
    public class ClientesController : Controller
    {
        private readonly ClienteRepositorio _clienteRepositorio;

        public ClientesController(ClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // GET: Clientes (Con Paginación)
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            // Obtenemos la query sin ejecutar desde el repositorio
            var query = _clienteRepositorio.Query();
            
            // PaginatedList se encarga de ejecutar la consulta paginada
            var paginated = await PaginatedList<Clientes>.CreateAsync(query, pageNumber, pageSize);
            
            return View(paginated);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // NOTA: Asegúrate que tu repositorio haga el .Include(v => v.Vehiculos) 
            // dentro de GetByIdAsync si quieres ver la lista de autos en el detalle.
            var cliente = await _clienteRepositorio.GetByIdAsync(id.Value);
            
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,Dni,Telefono,Email")] Clientes cliente)
        {
            // Validación personalizada: Evitar DNIs duplicados
            if (cliente.Dni != null && await _clienteRepositorio.GetByDniAsync(cliente.Dni) != null)
            {
                ModelState.AddModelError("Dni", "Ya existe un cliente registrado con este DNI.");
            }

            if (ModelState.IsValid)
            {
                await _clienteRepositorio.AddAsync(cliente);
                TempData["Mensaje"] = "Cliente creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _clienteRepositorio.GetByIdAsync(id.Value);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Dni,Telefono,Email,Created_at")] Clientes cliente)
        {
            if (id != cliente.Id) return NotFound();

            // Validación DNI duplicado (excluyendo al propio usuario que se edita)
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _clienteRepositorio.ExistsAsync(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _clienteRepositorio.GetByIdAsync(id.Value);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (ok, mensaje) = await _clienteRepositorio.DeleteAsync(id);
            if (!ok)
            {
                TempData["Error"] = mensaje;
                return RedirectToAction(nameof(Index), new { id });
            }
            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        // ==========================================================
        //  ENDPOINT PARA SELECT2 (Buscador en Vehículos)
        // ==========================================================
        // GET: Clientes/BuscarClientes?term=juan
        [HttpGet]
        public async Task<IActionResult> BuscarClientes(string term)
        {
            // Debes tener este método 'BuscarPorTerminoAsync' en tu Repositorio
            var clientes = await _clienteRepositorio.BuscarPorTerminoAsync(term);

            // Formateamos la respuesta JSON como le gusta a Select2
            var resultadoJson = clientes.Select(c => new
            {
                id = c.Id,
                text = $"{c.Apellido}, {c.Nombre} (DNI: {c.Dni})"
            });

            return Json(new { results = resultadoJson });
        }
    }
}