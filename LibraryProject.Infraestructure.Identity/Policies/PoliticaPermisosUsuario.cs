using Microsoft.AspNetCore.Authorization;

namespace LibraryProject.Policies.CustomPolicies
{
    public class PoliticaPermisosUsuario : IAuthorizationRequirement
    {
        public string[] PermitirUsuarios { get; set; }

        public PoliticaPermisosUsuario(params string[] usuario) 
        {
            PermitirUsuarios = usuario;
        }

    }
}
