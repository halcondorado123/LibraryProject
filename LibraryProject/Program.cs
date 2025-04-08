using LibraryProject.Configurations.Identity;
using LibraryProject.Modules;
using LibraryProject.Transversal.Mapper;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------
// 1. Configuración de servicios básicos (MVC + Razor Pages)
// -------------------------------------------------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// -------------------------------------------------------------------
// 2. Inyección de servicios y módulos personalizados
// -------------------------------------------------------------------
builder.Services
    .AddCustomDbContexts(builder.Configuration)       // DbContext y cadenas de conexión
    .AddCustomIdentity();                              // Identity Core + EF

builder.Services
    .AddCustomIdentitySettings()                      // Configuración de contraseña, email, etc.
    .AddIdentityPolicies();                           // Políticas de autorización

builder.Services
    .AddInjection(builder.Configuration)              // Inyección de servicios: repositorios, dominio, etc.
    .AddAuthorizationHandlers();                      // IAuthorizationHandler personalizados

builder.Services.AddAutoMapper(typeof(MappingsProfile)); // AutoMapper


// -------------------------------------------------------------------
// 3. Construcción de la aplicación
// -------------------------------------------------------------------
var app = builder.Build();

// -------------------------------------------------------------------
// 4. Middleware HTTP
// -------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// -------------------------------------------------------------------
// 5. Rutas y endpoints
// -------------------------------------------------------------------
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
