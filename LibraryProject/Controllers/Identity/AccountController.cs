using LibraryProject.Application.DTO.Identity;
using LibraryProject.Domain.Entities.Location;
using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers.Identity
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private UserManager<AppUsuario> userManager;
        private SignInManager<AppUsuario> signInManager;

        public AccountController(UserManager<AppUsuario> usrManager, SignInManager<AppUsuario> signinManager)
        {
            userManager = usrManager;
            signInManager = signinManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            var loginDto = new LoginDTO
            {
                ReturnUrl = returnUrl
            };

            return View(loginDto);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                // Buscar al usuario por su correo
                var usuario = await userManager.FindByEmailAsync(loginDto.Email);

                if (usuario != null)
                {
                    await signInManager.SignOutAsync(); // Limpiar sesiones previas

                    // Intentar iniciar sesión con las credenciales
                    var resultado = await signInManager.PasswordSignInAsync(
                        usuario, loginDto.Password, isPersistent: false, lockoutOnFailure: false
                    );

                    if (resultado.Succeeded)
                        return Redirect(loginDto.ReturnUrl ?? "/");

                    ModelState.AddModelError("", "Se ha producido un error en el Login");
                }
                else
                {
                    ModelState.AddModelError("", "El usuario no existe");
                }
            }

            return View(loginDto);
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new AppUsuario
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Edad = model.Edad,
                    Salario = model.Salario,
                    Pais = new CountryME { CountryId = model.PaisId }, // Ajusta según tu implementación real
                };

                var resultado = await userManager.CreateAsync(usuario, model.Password);

                if (resultado.Succeeded)
                {
                    await signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Index", "Home"); // O a donde prefieras redirigir
                }

                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        // Cierre de sesión
        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
