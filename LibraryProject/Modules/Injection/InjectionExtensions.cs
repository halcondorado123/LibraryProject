using LibraryProject.Application.DTO;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Core;
using LibraryProject.Domain.Interface;
using LibraryProject.Infraestructure.Interface;
using LibraryProject.Infraestructure.Repository;
using LibraryProject.Transversal.Common;
using LibraryProject.Transversal.Logging;

namespace LibraryProject.Modules
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IBooksDomain, BooksDomain>();
            services.AddScoped<IBookApplication, BookApplication>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
