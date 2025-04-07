using LibraryProject.Infraestructure.Identity.IdentityPolicies;
using LibraryProject.Policies.CustomPolicies;
using Microsoft.AspNetCore.Authorization;

namespace LibraryProject.Configurations.Identity
{
    public static class IdentityPoliciesConfiguration
    {
        public static IServiceCollection AddIdentityPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PermitirUsuarios", policy =>
                {
                    policy.AddRequirements(new PoliticaPermisosUsuario("FalconCasallas"));
                });

                options.AddPolicy("AccesoPrivado", policy =>
                {
                    policy.AddRequirements(new PoliticaPermitirPrivado());
                });

                options.AddPolicy("Primer Email", policy =>
                {
                    policy.RequireRole("Administración");
                    policy.RequireClaim("primerEmail", "halcondorado123@live.com");
                });
            });

            services.AddTransient<IAuthorizationHandler, ControladorPermitirUsuarios>();
            services.AddTransient<IAuthorizationHandler, PermitirControladorPrivado>();

            return services;
        }
    }
}
