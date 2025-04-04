using LibraryProject.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Policies.IdentityPolicies
{
    public class PoliticaPassPersonalizada : PasswordValidator<AppUsuario>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUsuario> manager, AppUsuario usuario, string password)
        {
            IdentityResult resultado = await base.ValidateAsync(manager, usuario, password);
            List<IdentityError> errores = resultado.Succeeded ? new List<IdentityError>() : resultado.Errors.ToList();

            // Se va aestablecer una politica para que no sean contraseñas predecibles o faciles de decifrar
            if (password.Contains("123"))
            {
                errores.Add(new IdentityError
                {
                    Description = "Password no puede contener secuencia númerica 123 "
                });
            }

            return errores.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errores.ToArray());
        }
    }
}
