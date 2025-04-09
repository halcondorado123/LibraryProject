using LibraryProject.Application.DTO.Identity.RoleDTO;
using LibraryProject.Application.Interface.Identity;
using LibraryProject.Domain.Entities.UserAttributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static LibraryProject.Application.DTO.Identity.RoleDTO.UpdateRoleDTO;

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
                if (user != null)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    // Si ya está en el rol actual, lo ignoramos
                    if (currentRoles.Contains(dto.NombreRol, StringComparer.OrdinalIgnoreCase))
                        continue;

                    // Si está en otro rol diferente, podrías quitarlo antes de agregar (opcional)
                    foreach (var otherRole in currentRoles)
                    {
                        if (!string.Equals(otherRole, dto.NombreRol, StringComparison.OrdinalIgnoreCase))
                            await _userManager.RemoveFromRoleAsync(user, otherRole);
                    }

                    // Ahora sí lo agregamos
                    await _userManager.AddToRoleAsync(user, dto.NombreRol);
                }
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

        public async Task<List<UserWithRoleDTO>> GetUserWithRoleDTO(UserWithRoleDTO filterDto)
        {
            var usuarios = await _userManager.Users.ToListAsync();
            var lista = new List<UserWithRoleDTO>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                var rol = roles.FirstOrDefault() ?? "Sin rol";

                // Filtrado opcional por rol desde el filtro recibido
                if (string.IsNullOrEmpty(filterDto.RoleName) || rol == filterDto.RoleName)
                {
                    lista.Add(new UserWithRoleDTO
                    {
                        UserName = usuario.UserName,
                        RoleName = rol
                    });
                }
            }

            return lista;
        }

        // Para el paginado
        public async Task<UpdateRoleDTO> GetRoleWithUsersAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return null;

            var allUsers = _userManager.Users.ToList();

            var members = new List<UserInRoleDTO>();
            var nonMembers = new List<UserInRoleDTO>();

            foreach (var user in allUsers)
            {
                var userDto = new UserInRoleDTO
                {
                    UserId = user.Id,
                    Email = user.Email
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    members.Add(userDto);
                }
                else
                {
                    nonMembers.Add(userDto);
                }
            }

            return new UpdateRoleDTO
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Members = members,
                NonMembers = nonMembers
            };
        }




        public async Task<List<UserWithRoleDTO>> GetAllUsersWithRolesAsync()
        {
            var users = _userManager.Users.ToList();

            var list = new List<UserWithRoleDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                list.Add(new UserWithRoleDTO
                {
                    UserName = user.UserName,
                    RoleName = roles.FirstOrDefault() ?? "Sin rol"
                });
            }

            return list;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
    }

}
