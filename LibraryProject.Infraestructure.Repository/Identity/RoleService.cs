using LibraryProject.Application.DTO.Identity.RoleDTO;
using LibraryProject.Application.Interface.Identity;
using LibraryProject.Domain.Entities.UserAttributes;
using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Infraestructure.Repository.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUsuario> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<AppUsuario> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<RoleViewDTO>> GetAllPaginatedAsync(int page, int pageSize)
        {
            var roles = _roleManager.Roles.ToList();
            var roleViewModels = new List<RoleViewDTO>();

            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                roleViewModels.Add(new RoleViewDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    UserCount = usersInRole.Count
                });
            }

            return roleViewModels
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<(int totalPages, int totalCount)> GetPaginationDataAsync(int pageSize)
        {
            var totalCount = _roleManager.Roles.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return (totalPages, totalCount);
        }

        public async Task<bool> CreateRoleAsync(CreateRoleDTO roleDto)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleDto.Nombre));
            return result.Succeeded;
        }

        public async Task<bool> ModifyUsersInRoleAsync(ModifyRoleDTO dto)
        {
            var role = await _roleManager.FindByIdAsync(dto.IdRol);
            if (role == null)
                return false;

            // Si el nombre cambió, lo actualizamos
            if (!string.Equals(role.Name, dto.NombreRol, StringComparison.OrdinalIgnoreCase))
            {
                role.Name = dto.NombreRol;
                var updateResult = await _roleManager.UpdateAsync(role);
                if (!updateResult.Succeeded)
                    return false;
            }

            // Agregar usuarios al rol
            foreach (var userId in dto.AgregarIds ?? Enumerable.Empty<string>())
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null && !await _userManager.IsInRoleAsync(user, dto.NombreRol))
                    await _userManager.AddToRoleAsync(user, dto.NombreRol);
            }

            // Eliminar usuarios del rol
            foreach (var userId in dto.EliminarIds ?? Enumerable.Empty<string>())
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null && await _userManager.IsInRoleAsync(user, dto.NombreRol))
                    await _userManager.RemoveFromRoleAsync(user, dto.NombreRol);
            }

            return true;
        }

        public async Task<int> GetUserCountInRoleAsync(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.Count;
        }

        public Task<UpdateRoleDTO> GetRoleWithUsersAsync(string id)
        {
            throw new NotImplementedException();
        }
    }

}
