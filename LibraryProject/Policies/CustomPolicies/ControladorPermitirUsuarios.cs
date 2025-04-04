using Microsoft.AspNetCore.Authorization;

namespace LibraryProject.Policies.CustomPolicies
{
    public class ControladorPermitirUsuarios : AuthorizationHandler<PoliticaPermisosUsuario>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PoliticaPermisosUsuario requirement)
        {
            if (requirement.PermitirUsuarios.Any(usuario => usuario.Equals(context.User.Identity.Name, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
