using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;

public class UsuariosController : Controller
{
    private readonly UsuarioRepositorio _repo;
    private readonly PasswordHasher<Usuarios> _hasher;
    private readonly IConfiguration _config;

    public UsuariosController(
        UsuarioRepositorio repo,
        PasswordHasher<Usuarios> hasher,
        IConfiguration config)
    {
        _repo = repo;
        _hasher = hasher;
        _config = config;
    }

    [AllowAnonymous]
    // 1. Recibimos la returnUrl si el sistema nos redirigió aquí
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    // 2. Recibimos la returnUrl de vuelta desde el formulario
    public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
    {
        // La guardamos de nuevo por si falla el login y hay que volver a mostrar la vista
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

        // ==== CREAR COOKIE ====
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.rol),
            // 🔥 IMPORTANTE: Agregamos este Claim "Id" porque tus otros controladores lo buscan explícitamente
            new Claim("Id", user.id.ToString()) 
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        // ==== GENERAR JWT ====
        var token = GenerarJwt(user);
        TempData["jwt"] = token;

        // 3. Lógica de redirección inteligente
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
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
            new Claim("Id", user.id.ToString()) // También lo agregamos al JWT por consistencia
        };

        var token = new JwtSecurityToken(
            issuer: _config["TokenAuthentication:Issuer"],
            audience: _config["TokenAuthentication:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}