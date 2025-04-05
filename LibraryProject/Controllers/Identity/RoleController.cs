using LibraryProject.Models;
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
            var roleViewModels = new List<RoleViewModel>();

            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                roleViewModels.Add(new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    UserCount = usersInRole.Count // Cantidad de usuarios por rol
                });
            }

            // Paginación
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



        // Peticion HHTP GEt, para poder crear un rol
        public IActionResult Create()
        {
            return View();
        }

        // Permitira crear el usuario en base de datos.
        [HttpPost]
        public async Task<IActionResult> Create([Required] string nombre)
        {
            if (ModelState.IsValid)
            {
                IdentityResult resultado = await _roleManager.CreateAsync(new IdentityRole(nombre));

                if (resultado.Succeeded)
                    return RedirectToAction("Index");
                else
                    foreach (IdentityError error in resultado.Errors)
                        ModelState.AddModelError("", error.Description);
            }
            return View(nombre);
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
            List<AppUsuario> miembros = new List<AppUsuario>();
            List<AppUsuario> Nomiembros = new List<AppUsuario>();

            foreach (AppUsuario usuario in _userManager.Users)
            {
                // Obtener todos los roles del usuario
                var rolesUsuario = await _userManager.GetRolesAsync(usuario);

                // Verificar si el usuario pertenece al rol actual
                if (rolesUsuario.Contains(rol.Name))
                {
                    miembros.Add(usuario);
                }
                else
                {
                    // Si el usuario ya pertenece a otro rol, no lo agregamos a NonMembers
                    if (!rolesUsuario.Any())
                    {
                        Nomiembros.Add(usuario);
                    }
                }
            }

            return View(new RoleEditar
            {
                Role = rol,
                Members = miembros,
                NonMembers = Nomiembros
            });
        }

        // Metodo funcional para agregar o elimnar usuarios de role identity
        [HttpPost]
        public async Task<IActionResult> Update(RoleModificar modelo)
        {
            try
            {
                IdentityResult resultado;
                if (ModelState.IsValid)
                {
                    foreach (string usuarioId in modelo.AgregarIds ?? new string[] { })
                    {
                        AppUsuario usuario = await _userManager.FindByIdAsync(usuarioId);
                        if (usuario != null)
                        {
                            resultado = await _userManager.AddToRoleAsync(usuario, modelo.NombreRol);
                            if (!resultado.Succeeded)
                                ModelState.AddModelError("", "No se ha podido agregar el usuario al Rol");
                        }
                    }
                    foreach (string usuarioId in modelo.EliminarIds ?? new string[] { })
                    {
                        AppUsuario usuario = await _userManager.FindByIdAsync(usuarioId);
                        if (usuario != null)
                        {
                            resultado = await _userManager.RemoveFromRoleAsync(usuario, modelo.NombreRol);
                            if (!resultado.Succeeded)
                                ModelState.AddModelError("", "No se ha podido eliminar el usuario del rol");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            //try
            //{
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
                return await Update(modelo.IdRol);
            //}

            //catch (Exception ex)
            //{
            //    ex.Message.ToString();
            //}

        }

        [HttpGet]
        public async Task<IActionResult> GetUserCountInRole(string roleId)
        {
            var userCount = await _userManager.GetUsersInRoleAsync(roleId);
            return Json(userCount.Count);
        }
    }
}

