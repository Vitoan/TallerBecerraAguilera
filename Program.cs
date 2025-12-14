using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// 1. Configuración de la Base de Datos (MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// 2. Inyección de Dependencias (Repositorios)
builder.Services.AddScoped<OrdenTrabajoRepositorio>();
builder.Services.AddScoped<ClienteRepositorio>();
builder.Services.AddScoped<VehiculoRepositorio>();
builder.Services.AddScoped<HerramientaRepositorio>();
builder.Services.AddScoped<RepuestoRepositorio>();
builder.Services.AddScoped<PedidosRepuestosRepositorio>();
builder.Services.AddScoped<PedidoRepuestosRepositorio>();
builder.Services.AddScoped<ProveedorRepositorio>();
builder.Services.AddScoped<EmpleadoRepositorio>();
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<ImagenHerramientaRepositorio>();
builder.Services.AddScoped<OtHerramientasRepositorio>();
builder.Services.AddScoped<OtRepuestosRepositorio>();

// Servicio para Hashing de Contraseñas
builder.Services.AddScoped<PasswordHasher<Usuarios>>();

// 3. Configuración de Políticas de Autorización
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// 4. Configuración de Autenticación (Cookies + JWT)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.LogoutPath = "/Usuarios/Logout";
        options.AccessDeniedPath = "/Home/Restringido";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    })
    .AddJwtBearer(options =>
    {
        var secret = configuration["TokenAuthentication:SecretKey"];
        if (string.IsNullOrEmpty(secret))
            throw new Exception("Falta configurar 'TokenAuthentication:SecretKey' en appsettings.json");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["TokenAuthentication:Issuer"],
            ValidAudience = configuration["TokenAuthentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))
        };
    });

var app = builder.Build();

// Pipeline de Manejo de Errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// =========================================================================
// 🚀 INICIALIZACIÓN DE DATOS (CREACIÓN AUTOMÁTICA DEL ADMIN)
// =========================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var hasher = services.GetRequiredService<PasswordHasher<Usuarios>>();
    
    // Aplica migraciones pendientes si las hay
    db.Database.Migrate();

    // 👉 AQUÍ ESTÁN TUS CREDENCIALES POR DEFECTO
    // Si no pones nada en appsettings.json, usará "admin@taller.local" y "Admin123!"
    var adminEmail = configuration["SeedAdmin:Email"] ?? "admin@taller.local";
    var adminPassword = configuration["SeedAdmin:Password"] ?? "Admin123!";

    // Buscamos si ya existe el usuario
    var admin = db.Set<Usuarios>().FirstOrDefault(u => u.email == adminEmail);

    // Solo entramos aquí si el usuario NO existe en la base de datos
    if (admin == null)
    {
        admin = new Usuarios
        {
            email = adminEmail,
            rol = "Administrador",
            avatar_path = "/uploads/avatars/default.jpg",
            created_at = DateTime.Now,
            updated_at = DateTime.Now
        };

        // Generamos el hash CORRECTO usando el servicio PasswordHasher
        admin.password_hash = hasher.HashPassword(admin, adminPassword);
        
        db.Set<Usuarios>().Add(admin);
        db.SaveChanges();
        Console.WriteLine($"[SISTEMA] Usuario Admin creado exitosamente. Email: {adminEmail}");
    }
}
// =========================================================================

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // <- Importante: Identifica QUIÉN es el usuario
app.UseAuthorization();  // <- Importante: Verifica QUÉ puede hacer

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();