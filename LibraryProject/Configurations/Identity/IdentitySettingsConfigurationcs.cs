using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Policies.IdentityPolicies;
using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Configurations.Identity
{
    public static class IdentitySettingsConfiguration
    {
        public static IServiceCollection AddCustomIdentitySettings(this IServiceCollection services)
        {
            // Configuración de cookies
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.identity.Application";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            // Configuración de reglas de contraseñas
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";

                // Configuración de bloqueo de cuenta
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // Requiere confirmación de correo electrónico
                options.SignIn.RequireConfirmedEmail = false;
            });

            // Políticas personalizadas
            services.AddTransient<IPasswordValidator<AppUsuario>, PoliticaPassPersonalizada>();
            services.AddTransient<IUserValidator<AppUsuario>, PoliticaUsuarioEmailPersonalizada>();

            return services;
        }
    }
}
