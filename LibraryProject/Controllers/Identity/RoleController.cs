using LibraryProject.Application.DTO.Identity.RoleDTO;
using LibraryProject.Domain.Entities.UserAttributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Controllers.Identity
{
    public class RoleController : Controller
    {

        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUsuario> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUsuario> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
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

            var totalRoles = roleViewModels.Count;
            var totalPages = (int)Math.Ceiling((double)totalRoles / pageSize);

            var rolesPaginados = roleViewModels
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(rolesPaginados);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDTO roleDto)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(roleDto.Nombre));

                if (result.Succeeded)
                    return RedirectToAction("Index");

                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(roleDto);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult resultado = await _roleManager.DeleteAsync(role);
                if (resultado.Succeeded)
                    return RedirectToAction("Index");
                else
                    foreach (IdentityError error in resultado.Errors)
                        ModelState.AddModelError("", error.Description);
            }

            return View("Index", _roleManager.Roles);
        }


        // Este metodo ayuda a buscar metodos mediante peticion tipo GET; a miembros y no miembos
        public async Task<IActionResult> Update(string id)
        {
            IdentityRole rol = await _roleManager.FindByIdAsync(id);

            if (rol == null)
            {
                return NotFound();  // Si el rol no existe
            }

            // Inicializar las listas para los miembros y no miembros
            List<UpdateRoleDTO.UserInRoleDto> miembros = new List<UpdateRoleDTO.UserInRoleDto>();
            List<UpdateRoleDTO.UserInRoleDto> noMiembros = new List<UpdateRoleDTO.UserInRoleDto>();

            // Recorrer todos los usuarios para ver si pertenecen al rol
            foreach (AppUsuario usuario in _userManager.Users)
            {
                // Obtener todos los roles del usuario
                var rolesUsuario = await _userManager.GetRolesAsync(usuario);

                // Verificar si el usuario pertenece al rol actual
                if (rolesUsuario.Contains(rol.Name))
                {
                    miembros.Add(new UpdateRoleDTO.UserInRoleDto
                    {
                        UserId = usuario.Id,
                        Email = usuario.Email
                    });
                }
                else
                {
                    // Si el usuario no pertenece al rol, lo agregamos a NonMembers
                    noMiembros.Add(new UpdateRoleDTO.UserInRoleDto
                    {
                        UserId = usuario.Id,
                        Email = usuario.Email
                    });
                }
            }

            // Crear el DTO para la vista
            var roleDto = new UpdateRoleDTO
            {
                RoleId = rol.Id,
                RoleName = rol.Name,
                Members = miembros,
                NonMembers = noMiembros
            };

            // Retornar el DTO a la vista
            return View(roleDto);
        }

        // Metodo funcional para agregar o elimnar usuarios de role identity
        [HttpPost]
        public async Task<IActionResult> Update(ModifyRoleDTO rolDto)
        {
            try
            {
                IdentityResult result;
                if (ModelState.IsValid)
                {
                    // Agregar usuarios al rol
                    foreach (string usuarioId in rolDto.AgregarIds ?? new string[] { })
                    {
                        AppUsuario usuario = await _userManager.FindByIdAsync(usuarioId);
                        if (usuario != null)
                        {
                            if (!await _userManager.IsInRoleAsync(usuario, rolDto.NombreRol)) // Verifica que no esté ya en el rol
                            {
                                result = await _userManager.AddToRoleAsync(usuario, rolDto.NombreRol);
                                if (!result.Succeeded)
                                    ModelState.AddModelError("", "No se ha podido agregar el usuario al Rol");
                            }
                        }
                    }

                    // Eliminar usuarios del rol
                    foreach (string usuarioId in rolDto.EliminarIds ?? new string[] { })
                    {
                        AppUsuario usuario = await _userManager.FindByIdAsync(usuarioId);
                        if (usuario != null)
                        {
                            if (await _userManager.IsInRoleAsync(usuario, rolDto.NombreRol)) // Verifica que esté en el rol
                            {
                                result = await _userManager.RemoveFromRoleAsync(usuario, rolDto.NombreRol);
                                if (!result.Succeeded)
                                    ModelState.AddModelError("", "No se ha podido eliminar el usuario del rol");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error inesperado.");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index)); // Si todo fue bien, redirige al Index
            }
            else
            {
                return View(rolDto); // Si hubo errores, regresa a la vista de actualización con los errores
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCountInRole(string roleId)
        {
            // Obtener la lista de usuarios en el rol
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleId);

            // Devolver solo la cantidad de usuarios en el rol
            return Json(usersInRole.Count());
        }
    }
}

