using LibraryProject.Domain.Entities.UserAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infraestructure.Interface.Identity
{
    public interface IAdminRepository
    {
        Task<int> GetTotalUsuariosAsync();
        Task<IEnumerable<AppUsuario>> GetUsuariosPaginatedAsync(int page, int pageSize);
    }
}
