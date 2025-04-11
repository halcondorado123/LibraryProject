using Microsoft.Extensions.DependencyInjection;
using AppEmailSender = LibraryProject.Application.Interface.Identity.IEmailSender;
using IdentityEmailSender = Microsoft.AspNetCore.Identity.UI.Services.IEmailSender;
using LibraryProject.Application.Services.Identity;

namespace LibraryProject.Modules.Extensions
{
    public static class EmailSenderServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services)
        {
            // Registra EmailSender una sola vez
            services.AddScoped<EmailSender>();

            // Ambas interfaces usan la misma implementación concreta
            services.AddScoped<AppEmailSender, EmailSender>();
            services.AddScoped<IdentityEmailSender, EmailSender>();

            return services;
        }
    }
}
