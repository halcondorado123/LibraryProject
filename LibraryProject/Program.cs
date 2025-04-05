using LibraryProject.Infraestructure.Interface;
using LibraryProject.Infraestructure.Repository;
using LibraryProject.Models;
using LibraryProject.Modules;
using LibraryProject.Policies.CustomPolicies;
using LibraryProject.Policies.IdentityPolicies;
using LibraryProject.Transversal.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura los servicios
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Configura el DbContext
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));

// Configura Identity
builder.Services.AddIdentity<AppUsuario, IdentityRole>()
    .AddEntityFrameworkStores<LibraryDbContext>()
    .AddDefaultTokenProviders();


// Esta funcion esta asociada a la politica de contraseñas de identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
});

// Establecer la politica de password personalizada en clase PoliticaPassPersonalizada
builder.Services.AddTransient<IPasswordValidator<AppUsuario>, PoliticaPassPersonalizada>();
builder.Services.AddTransient<IUserValidator<AppUsuario>, PoliticaUsuarioEmailPersonalizada>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// -- Es importante registrar los servicio en esta seccion
// Registrar la clase de autorizacion por IAuthorizationHandler con la clase o servicio ControladorPermitirUsuarios
builder.Services.AddTransient<IAuthorizationHandler, ControladorPermitirUsuarios>();
// Registrar la clase de autorizacion por IAuthorizationHandler con la clase o servicio PermitirControladorPrivado
builder.Services.AddTransient<IAuthorizationHandler, PermitirControladorPrivado>();

// Ruta de autenticacion - Si se cambia debe hacerse manualmente
builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.Name = ".AspNetCore.identity.Application";
    // Se almacena durante 20 minutos en el navegador
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
});


// Mapper
builder.Services.AddAutoMapper(typeof(MappingsProfile));
// Injection
builder.Services.AddInjection(builder.Configuration);

// Politica de servicio 01
builder.Services.AddAuthorization(options =>
{
    // Aqui se establece el nombre de la politica
    options.AddPolicy("Segundo Email", policy =>
    {
        policy.RequireRole("Administración");
        policy.RequireClaim("segundoemail", "Turbias@gmail.com");
    });
});

// Politica de servicio 01
builder.Services.AddAuthorization(options =>
{
    // Aqui se establece el nombre de la politica
    options.AddPolicy("PermitirUsuarios", policy =>
    {
        policy.AddRequirements(new PoliticaPermisosUsuario("espana"));
    });
});

// Politica de servicio 03
builder.Services.AddAuthorization(options =>
{
    // Aqui se establece el nombre de la politica
    options.AddPolicy("AccesoPrivado", policy =>
    {
        policy.AddRequirements(new PoliticaPermitirPrivado());
    });
});


var app = builder.Build();

// Configura el pipeline de solicitudes HTTP
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

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();