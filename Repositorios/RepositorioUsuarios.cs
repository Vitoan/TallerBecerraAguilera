using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models;

namespace TallerBecerraAguilera.Repositorios
{
    public class UsuarioRepositorio
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuarios?> ObtenerPorEmail(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.email == email);
        }
    }
}
