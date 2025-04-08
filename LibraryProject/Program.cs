using LibraryProject.Configurations.Identity;
using LibraryProject.Modules;
using LibraryProject.Transversal.Mapper;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------
// 1. Configuraci�n de servicios b�sicos (MVC + Razor Pages)
// -------------------------------------------------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// -------------------------------------------------------------------
// 2. Inyecci�n de servicios y m�dulos personalizados
// -------------------------------------------------------------------
builder.Services
    .AddCustomDbContexts(builder.Configuration)       // DbContext y cadenas de conexi�n
    .AddCustomIdentity();                              // Identity Core + EF

builder.Services
    .AddCustomIdentitySettings()                      // Configuraci�n de contrase�a, email, etc.
    .AddIdentityPolicies();                           // Pol�ticas de autorizaci�n

builder.Services
    .AddInjection(builder.Configuration)              // Inyecci�n de servicios: repositorios, dominio, etc.
    .AddAuthorizationHandlers();                      // IAuthorizationHandler personalizados

builder.Services.AddAutoMapper(typeof(MappingsProfile)); // AutoMapper


// -------------------------------------------------------------------
// 3. Construcci�n de la aplicaci�n
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
