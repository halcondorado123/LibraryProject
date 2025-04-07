using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Transversal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Services.Identity
{
    public class AdminApplication
    {
        public class UsuarioService
        {
            private readonly IUsuarioRepository _usuarioRepository;

            public UsuarioService(IUsuarioRepository usuarioRepository)
            {
                _usuarioRepository = usuarioRepository;
            }

            public async Task<Response<IEnumerable<AppUsuario>>> GetUsuariosPaginatedAsync(int page = 1, int pageSize = 5)
            {
                try
                {
                    var totalUsuarios = await _usuarioRepository.GetTotalUsuariosAsync();
                    var usuarios = await _usuarioRepository.GetUsuariosPaginatedAsync(page, pageSize);

                    var totalPages = (int)Math.Ceiling((double)totalUsuarios / pageSize);

                    // Se envía la lista de usuarios con éxito
                    return new Response<IEnumerable<AppUsuario>>(usuarios, "Usuarios obtenidos correctamente.");
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, se maneja y devuelve una respuesta con error
                    return new Response<IEnumerable<AppUsuario>>(ex.Message, 500); // Internal Server Error
                }
            }
        }
    }
}
