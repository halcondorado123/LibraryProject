using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Policies.IdentityPolicies;
using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Configurations.Identity
{
    public static class IdentitySettingsConfigurationcs
    {
        public static IServiceCollection AddCustomIdentitySettings(this IServiceCollection services)
        {
            // Política de cookies
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.identity.Application";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            });

            // Reglas de contraseñas
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            });

            // Políticas personalizadas
            services.AddTransient<IPasswordValidator<AppUsuario>, PoliticaPassPersonalizada>();
            services.AddTransient<IUserValidator<AppUsuario>, PoliticaUsuarioEmailPersonalizada>();

            return services;
        }
    }
}
