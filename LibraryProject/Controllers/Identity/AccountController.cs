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
        //Realizar la autenticación de los usuario
        private SignInManager<AppUsuario> signInManager;

        public AccountController(UserManager<AppUsuario> usrManager, SignInManager<AppUsuario> signinManager)
        {
            userManager = usrManager;
            signInManager = signinManager;
        }

        // Muestra la vista asi no esten autenticados - Para que puedan ser autenticado
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]  // Evita multiples ataques de Hacking durante el proceso de Host
        public async Task<IActionResult> Login(Login login)
        {

            if (ModelState.IsValid)
            {
                // Toma la dirección del correo electronico que es proporcionada por el usuario en el inicio de sesion
                AppUsuario usuario = await userManager.FindByEmailAsync(login.Email);

                if (usuario != null)
                {
                    await signInManager.SignOutAsync();
                    // false(1) no ingresar cookie de persistencia o mantenerse incluso despues de cerrado el navegador
                    // false(2) Para no bloquear la cuenta, cuando falle el inicio de sesion
                    Microsoft.AspNetCore.Identity.SignInResult resultado = await signInManager.PasswordSignInAsync(usuario, login.Password, false, false);

                    if (resultado.Succeeded)
                        return Redirect(login.ReturnUrl ?? "/");
                    else
                        ModelState.AddModelError("", "Se ha producido un error en el Login");
                }
            }
            return View(login);
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
