using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models;

namespace TallerBecerraAguilera.Repositorios
{
    public class UsuarioRepositorio
    {
        public IQueryable<Usuarios> Query()
        {
            return _context.Usuarios.AsQueryable();
        }

        private readonly ApplicationDbContext _context;

        public UsuarioRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuarios>> ObtenerTodos()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuarios?> ObtenerPorEmail(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<Usuarios?> ObtenerPorId(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.id == id);
        }

        public async Task Crear(Usuarios user)
        {
            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Usuarios user)
        {
            user.updated_at = DateTime.Now;
            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var user = await ObtenerPorId(id);
            if (user != null)
            {
                _context.Usuarios.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
