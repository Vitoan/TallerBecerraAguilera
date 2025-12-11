using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Helpers;

public class UsuariosController : Controller
{
    private readonly UsuarioRepositorio _repo;
    private readonly PasswordHasher<Usuarios> _hasher;
    private readonly IConfiguration _config;
    private readonly ApplicationDbContext _context;

    public UsuariosController(
        UsuarioRepositorio repo,
        PasswordHasher<Usuarios> hasher,
        IConfiguration config,
        ApplicationDbContext context)
    {
        _repo = repo;
        _hasher = hasher;
        _config = config;
        _context = context;
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        var user = await _repo.ObtenerPorEmail(email);
        if (user == null)
        {
            ModelState.AddModelError("", "Usuario no encontrado.");
            return View();
        }

        var result = _hasher.VerifyHashedPassword(user, user.password_hash!, password);
        if (result == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("", "Contraseña incorrecta.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.rol),
            new Claim("Id", user.id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        var token = GenerarJwt(user);
        TempData["jwt"] = token;

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login", "Usuarios");
    }

    private string GenerarJwt(Usuarios user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenAuthentication:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.rol),
            new Claim("Id", user.id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["TokenAuthentication:Issuer"],
            audience: _config["TokenAuthentication:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        int pageSize = 10;

        var query = _repo.Query()
                         .OrderBy(u => u.email);

        var paginated = await PaginatedList<Usuarios>.CreateAsync(query, pageNumber, pageSize);
        
        return View(paginated);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Empleados = await _context.Empleados
            .Where(e => e.UsuarioId == null)
            .OrderBy(e => e.Nombre)
            .ThenBy(e => e.Apellido)
            .ToListAsync();

        return View();
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Usuarios model, string password_hash, int? EmpleadoId)
    {
        if (string.IsNullOrWhiteSpace(password_hash))
        {
            ModelState.AddModelError("password_hash", "La contraseña es obligatoria.");

            ViewBag.Empleados = await _context.Empleados
                .Where(e => e.UsuarioId == null)
                .ToListAsync();

            return View(model);
        }

        model.password_hash = _hasher.HashPassword(model, password_hash);
        await _repo.Crear(model);

        if (EmpleadoId.HasValue)
        {
            var empleado = await _context.Empleados.FindAsync(EmpleadoId.Value);

            if (empleado != null)
            {
                if (empleado.UsuarioId != null)
                {
                    empleado.UsuarioId = null;
                    TempData["Info"] = "El empleado tenía otro usuario asignado. Se reemplazó correctamente.";
                }

                empleado.UsuarioId = model.id;
                await _context.SaveChangesAsync();
            }
        }

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _repo.ObtenerPorId(id);
        if (user == null) return NotFound();

        ViewBag.Empleados = await _context.Empleados
            .OrderBy(e => e.Nombre)
            .ThenBy(e => e.Apellido)
            .ToListAsync();

        var asignado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.UsuarioId == user.id);
        
        ViewBag.empleadoActual = asignado?.Id;
        return View(user);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Usuarios model, int? EmpleadoId)
    {
        var user = await _repo.ObtenerPorId(model.id);
        if (user == null) return NotFound();

        user.email = model.email;
        user.rol = model.rol;
        if (!string.IsNullOrEmpty(model.avatar_path))
            user.avatar_path = model.avatar_path;

        await _repo.Actualizar(user);

        var empleadoActual = await _context.Empleados.FirstOrDefaultAsync(e => e.UsuarioId == model.id);

        if (empleadoActual != null && EmpleadoId != empleadoActual.Id)
        {
            empleadoActual.UsuarioId = null;
        }

        if (EmpleadoId.HasValue)
        {
            var nuevoEmpleado = await _context.Empleados.FindAsync(EmpleadoId.Value);

            if (nuevoEmpleado != null)
            {
                if (nuevoEmpleado.UsuarioId != null && nuevoEmpleado.UsuarioId != user.id)
                {
                    nuevoEmpleado.UsuarioId = null;
                    TempData["Info"] = "El empleado estaba asignado a otro usuario. Se reasignó correctamente.";
                }

                nuevoEmpleado.UsuarioId = user.id;
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ResetPassword(int id)
    {
        var user = await _repo.ObtenerPorId(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(int id, string nuevaClave)
    {
        var user = await _repo.ObtenerPorId(id);
        if (user == null) return NotFound();

        user.password_hash = _hasher.HashPassword(user, nuevaClave);
        await _repo.Actualizar(user);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _repo.ObtenerPorId(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repo.Eliminar(id);
        return RedirectToAction("Index");
    }

    [Authorize]
    public async Task<IActionResult> Perfil()
    {
        int id = int.Parse(User.FindFirst("Id")!.Value);
        var user = await _repo.ObtenerPorId(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarDatosPersonales(Usuarios model)
    {
        var user = await _repo.ObtenerPorId(model.id);
        if (user == null) return NotFound();

        user.email = model.email;
        await _repo.Actualizar(user);

        TempData["Success"] = "Datos actualizados correctamente.";
        return RedirectToAction("Perfil");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarClave(int id, string ClaveActual, string NuevaClave, string ConfirmarClave)
    {
        var user = await _repo.ObtenerPorId(id);
        if (user == null) return NotFound();

        if (NuevaClave != ConfirmarClave)
        {
            TempData["Error"] = "Las contraseñas no coinciden.";
            return RedirectToAction("Perfil");
        }

        var result = _hasher.VerifyHashedPassword(user, user.password_hash!, ClaveActual);
        if (result == PasswordVerificationResult.Failed)
        {
            TempData["Error"] = "La contraseña actual es incorrecta.";
            return RedirectToAction("Perfil");
        }

        user.password_hash = _hasher.HashPassword(user, NuevaClave);
        await _repo.Actualizar(user);

        TempData["Success"] = "Contraseña actualizada.";
        return RedirectToAction("Perfil");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarAvatar(int id, IFormFile? Avatar, bool EliminarAvatar = false)
    {
        var user = await _repo.ObtenerPorId(id);
        if (user == null) return NotFound();

        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
        Directory.CreateDirectory(uploadPath);

        if (EliminarAvatar)
        {
            user.avatar_path = "/uploads/avatars/default.jpg";
        }
        else if (Avatar != null)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(Avatar.FileName)}";
            string filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Avatar.CopyToAsync(stream);
            }

            user.avatar_path = $"/uploads/avatars/{fileName}";
        }

        await _repo.Actualizar(user);
        TempData["Success"] = "Avatar actualizado correctamente.";
        return RedirectToAction("Perfil");
    }
}
