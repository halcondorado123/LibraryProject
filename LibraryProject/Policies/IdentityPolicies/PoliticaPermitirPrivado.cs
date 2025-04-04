﻿using Microsoft.AspNetCore.Authorization;

namespace LibraryProject.Policies.IdentityPolicies
{
    public class PoliticaPermitirPrivado : IAuthorizationRequirement
    {
    }

    public class PermitirControladorPrivado : AuthorizationHandler<PoliticaPermitirPrivado>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PoliticaPermitirPrivado requirement)
        {
            string[] UsuariosPermitidos = context.Resource as string[];

            if (UsuariosPermitidos.Any(usuario => usuario.Equals(context.User.Identity.Name, StringComparison.OrdinalIgnoreCase)))
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
