using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Repositorio; // Usando el namespace de la carpeta raíz

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