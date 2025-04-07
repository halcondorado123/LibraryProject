using LibraryProject.Infraestructure.Identity.IdentityPolicies;
using LibraryProject.Policies.CustomPolicies;
using Microsoft.AspNetCore.Authorization;

namespace LibraryProject.Modules
{
    public static class AuthorizationHandlerInjection
    {
        public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
        {
            services.AddTransient<IAuthorizationHandler, ControladorPermitirUsuarios>();
            services.AddTransient<IAuthorizationHandler, PermitirControladorPrivado>();

            return services;
        }
    }
}
