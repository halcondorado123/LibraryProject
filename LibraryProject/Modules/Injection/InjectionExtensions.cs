﻿using LibraryProject.Application.DTO;
using LibraryProject.Application.DTO.Identity.InitialDTO;
using LibraryProject.Application.Interface.Identity;
using LibraryProject.Application.Interface.Library;
using LibraryProject.Application.Services;
using LibraryProject.Application.Services.Identity;
using LibraryProject.Application.Services.Library;
using LibraryProject.Domain.Core.Library;
using LibraryProject.Domain.Interface.Library;
using LibraryProject.Infraestructure.Interface;
using LibraryProject.Infraestructure.Interface.Library;
using LibraryProject.Infraestructure.Repository;
using LibraryProject.Infraestructure.Repository.Identity;
using LibraryProject.Infraestructure.Repository.Library;
using LibraryProject.Transversal.Common;
using LibraryProject.Transversal.Logging;
using System.Configuration;

namespace LibraryProject.Modules
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IBooksDomain, BooksDomain>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<ICountryRepository, CountryRepository>();
            //services.AddScoped<CountryApplication>();
            services.AddScoped<IRoleService, RoleService>();
            // Repositorio y aplicación
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IBookApplication, BookApplication>();
            services.AddAntiforgery(options => {
                options.HeaderName = "RequestVerificationToken";
            });
            services.AddScoped<IEmailSender, EmailSender>();
           


            return services;
        }
    }
}
