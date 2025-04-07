using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Policies.IdentityPolicies
{
    public class PoliticaUsuarioEmailPersonalizada : UserValidator<AppUsuario>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUsuario> manager, AppUsuario usuario)
        {
            IdentityResult resultado = await base.ValidateAsync(manager, usuario);
            List<IdentityError> errores = resultado.Succeeded ? new List<IdentityError>() : resultado.Errors.ToList();

            if (usuario.UserName == "Google")
            {
                errores.Add(new IdentityError
                {
                    Description = "No se puede usar el nombre de Google como un nombre de usuario"
                });
            }

            if (usuario.Email.ToLower().EndsWith("@hotmail.com"))
            {
                errores.Add(new IdentityError
                {
                    Description = "Solamente están permitidas las direcciones de correo que sean de hotmail"
                });
            }

            return errores.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errores.ToArray());
        }
    }
}

