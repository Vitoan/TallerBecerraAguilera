using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Repositorio; 
using TallerBecerraAguilera.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar el DbContext con Pomelo + MySQL
// CORRECCIÓN: Usar "DefaultConnection"
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));
    
// 2. Servicios normales de MVC
builder.Services.AddControllersWithViews();

// 3. REGISTRO DE REPOSITORIOS (Solución al error de DI)
// Se registra la clase concreta del Repositorio de Ordenes de Trabajo
builder.Services.AddScoped<OrdenTrabajoRepositorio, OrdenTrabajoRepositorio>(); 
// Se registra la clase concreta del Repositorio de Clientes
builder.Services.AddScoped<ClienteRepositorio, ClienteRepositorio>(); 
builder.Services.AddScoped<VehiculoRepositorio, VehiculoRepositorio>();
builder.Services.AddScoped<HerramientaRepositorio, HerramientaRepositorio>();
builder.Services.AddScoped<RepuestoRepositorio,RepuestoRepositorio>();
builder.Services.AddScoped<PedidosRepuestosRepositorio, PedidosRepuestosRepositorio>();
builder.Services.AddScoped<PedidoRepuestosRepositorio, PedidoRepuestosRepositorio>();
builder.Services.AddScoped<ProveedorRepositorio, ProveedorRepositorio>();



var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();