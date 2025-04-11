using LibraryProject.Configurations.Identity;
using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Modules;
using LibraryProject.Modules.Extensions;
using LibraryProject.Transversal.Mapper;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------
// 1. Configuración de servicios básicos (MVC + Razor Pages)
// -------------------------------------------------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// -------------------------------------------------------------------
// 2. Inyección de servicios y módulos personalizados
// -------------------------------------------------------------------
// Configuración del DbContext y servicios relacionados con la base de datos
builder.Services
    .AddCustomDbContexts(builder.Configuration);  // Configura DbContext y cadenas de conexión

// Configuración de Identity y servicios relacionados con autenticación y autorización
// Configura Identity Core + EF
// Configuración de contraseña, email, etc.
// Políticas de autorización personalizadas
// Conexión con EF Core
// Proveedores de token para autenticación

// 1. Registro de Identity (esto devuelve un IdentityBuilder)
var identityBuilder = builder.Services
    .AddIdentity<AppUsuario, IdentityRole>(options =>
    {
        // Puedes mover estas opciones a AddCustomIdentitySettings si quieres
        options.SignIn.RequireConfirmedEmail = true;
    });

// 2. Agrega EF Core y token providers
identityBuilder
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 3. Ahora aplica tus configuraciones personalizadas
builder.Services.AddCustomIdentitySettings();
builder.Services.AddIdentityPolicies();

builder.Services.AddInjection(builder.Configuration); // ⬅️ Esto va después

builder.Services
    .AddInjection(builder.Configuration)              // Inyección de servicios: repositorios, dominio, etc.
    .AddAuthorizationHandlers();                      // IAuthorizationHandler personalizados

builder.Services.AddAutoMapper(typeof(MappingsProfile)); // AutoMapper
builder.Services.AddSwaggerServices(); // << Aquí agregas Swagger
builder.Services.AddEmailSender();


// -------------------------------------------------------------------
// 3. Construcción de la aplicación
// -------------------------------------------------------------------
var app = builder.Build();

// -------------------------------------------------------------------
// 4. Middleware HTTP
// -------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Página de excepciones de desarrollo
    app.UseSwagger();  // Si estás usando Swagger
    app.UseSwaggerUI();
}
else
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

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerMiddleware(); // << Aquí activas el middleware
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
