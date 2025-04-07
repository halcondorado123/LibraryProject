using LibraryProject.Domain.Entities.UserAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interface.Identity
{
    public interface IAdminApplication
    {
        Task<int> GetTotalUsuariosAsync();
        Task<IEnumerable<AppUsuario>> GetUsuariosPaginatedAsync(int page, int pageSize);
    }
}
