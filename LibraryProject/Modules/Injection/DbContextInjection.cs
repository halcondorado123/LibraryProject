using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Modules
{
    public static class DbContextInjection
    {
        public static IServiceCollection AddCustomDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            // Contexto principal
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SQLConnection")));

            // Aquí puedes agregar más contextos si lo necesitas
            // services.AddDbContext<OtroDbContext>(options =>
            //     options.UseSqlServer(configuration.GetConnectionString("OtraConexion")));

            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUsuario, IdentityRole>()
                .AddEntityFrameworkStores<LibraryDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
