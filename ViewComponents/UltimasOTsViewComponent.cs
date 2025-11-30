using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;

namespace TallerBecerraAguilera.ViewComponents
{
    public class UltimasOTsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public UltimasOTsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var ultimas = await _context.OrdenesTrabajo
                .OrderByDescending(o => o.FechaIngreso)
                .Take(5)
                .ToListAsync();

            return View(ultimas); // <- Va a buscar la View automáticamente
        }
    }
}
