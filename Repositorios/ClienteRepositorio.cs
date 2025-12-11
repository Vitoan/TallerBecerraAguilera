using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TallerBecerraAguilera.Repositorios
{
    public class ClienteRepositorio
    {
        public IQueryable<Clientes> Query()
        {
            return _context.Clientes.AsQueryable();
        }

        public async Task<List<Clientes>> BuscarPorTerminoAsync(string term)
        {
            if (string.IsNullOrEmpty(term)) 
                return new List<Clientes>();

            return await _context.Clientes
                .Where(c => c.Nombre.Contains(term) || 
                            c.Apellido.Contains(term) || 
                            c.Dni.Contains(term))
                .OrderBy(c => c.Apellido)
                .Take(10) // Limitamos a 10 para no saturar la red
                .ToListAsync();
        }
        private readonly ApplicationDbContext _context;

        public ClienteRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los clientes (LISTAR)
        public async Task<List<Clientes>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        // Obtener un cliente por ID (INCLUYE VEHÍCULOS para la vista Details)
        public async Task<Clientes?> GetByIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Vehiculos) // Carga los vehículos asociados
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        
        // Obtener por DNI (para validación de unicidad)
        public async Task<Clientes?> GetByDniAsync(string dni)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Dni == dni);
        }

        // Comprobar existencia (para manejo de concurrencia en Edit)
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Clientes.AnyAsync(c => c.Id == id);
        }

        // Agregar un nuevo cliente (CREAR)
        public async Task AddAsync(Clientes cliente)
        {
            cliente.Created_at = DateTime.Now;
            cliente.Updated_at = DateTime.Now;
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        // Actualizar un cliente existente (ACTUALIZAR)
        public async Task UpdateAsync(Clientes cliente)
        {
            cliente.Updated_at = DateTime.Now;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        // Eliminar un cliente por ID (ELIMINAR)
        public async Task DeleteAsync(int id)
        {
            var cliente = await GetByIdAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}