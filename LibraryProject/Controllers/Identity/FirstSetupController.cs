using LibraryProject.Application.DTO.Identity;
using LibraryProject.Domain.Entities.UserAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers.Identity
{
    public class FirstSetupController : Controller
    {
        // Este código debería estar en appsettings o en una variable segura
        private readonly string _codigoEspecial;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public FirstSetupController(UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _codigoEspecial = configuration["RegistroInicial:CodigoAdministrador"];
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Si ya hay usuarios, redirigir
            if (_userManager.Users.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(FirstAdminRegisterDTO model)
        {
            if (_userManager.Users.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
                return View(model);

            if (model.CodigoValidacion != CodigoEspecial)
            {
                ModelState.AddModelError("CodigoValidacion", "El código de validación no es válido.");
                return View(model);
            }

            var usuario = new AppUsuario
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(usuario, model.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Administrador"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Administrador"));
                }

                await _userManager.AddToRoleAsync(usuario, "Administrador");

                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
