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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<OrdenTrabajoRepositorio, OrdenTrabajoRepositorio>();
builder.Services.AddScoped<ClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<VehiculoRepositorio, VehiculoRepositorio>();
builder.Services.AddScoped<HerramientaRepositorio, HerramientaRepositorio>();
builder.Services.AddScoped<RepuestoRepositorio, RepuestoRepositorio>();
builder.Services.AddScoped<PedidosRepuestosRepositorio, PedidosRepuestosRepositorio>();
builder.Services.AddScoped<PedidoRepuestosRepositorio, PedidoRepuestosRepositorio>();
builder.Services.AddScoped<ProveedorRepositorio, ProveedorRepositorio>();
builder.Services.AddScoped<EmpleadoRepositorio, EmpleadoRepositorio>();
builder.Services.AddScoped<PedidosRepuestosRepositorio, PedidosRepuestosRepositorio>();
builder.Services.AddScoped<PedidoRepuestosRepositorio, PedidoRepuestosRepositorio>();
builder.Services.AddScoped<UsuarioRepositorio, UsuarioRepositorio>();


builder.Services.AddScoped<PasswordHasher<Usuarios>>();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


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
            throw new Exception("Falta configurar TokenAuthentication:SecretKey");
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

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var hasher = services.GetRequiredService<PasswordHasher<Usuarios>>();
    db.Database.Migrate();
    var adminEmail = configuration["SeedAdmin:Email"] ?? "admin@taller.local";
    var adminPassword = configuration["SeedAdmin:Password"] ?? "Admin123!";
    var admin = db.Set<Usuarios>().FirstOrDefault(u => u.email == adminEmail);
    if (admin == null)
    {
        admin = new Usuarios
        {
            email = adminEmail,
            rol = "Administrador",
            avatar_path = "/uploads/avatars/default.jpg",
            Created_at = DateTime.Now,
            Updated_at = DateTime.Now
        };
        admin.password_hash = hasher.HashPassword(admin, adminPassword);
        db.Set<Usuarios>().Add(admin);
        db.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
