using LibraryProject.Application.DTO.Identity.RoleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interface.Identity
{
    public interface IRoleService
    {
        Task<bool> DeleteRoleAsync(string roleId);
        Task<List<RoleViewDTO>> GetAllPaginatedAsync(int page, int pageSize);
        Task<(int totalPages, int totalCount)> GetPaginationDataAsync(int pageSize);
        Task<bool> CreateRoleAsync(CreateRoleDTO roleDto);
        Task<UpdateRoleDTO> GetRoleWithUsersAsync(string id);
        Task<bool> ModifyUsersInRoleAsync(ModifyRoleDTO dto);
        Task<List<UserWithRoleDTO>> GetUserWithRoleDTO(UserWithRoleDTO filterDto);
        Task<List<UserWithRoleDTO>> GetAllUsersWithRolesAsync();
    }
}
