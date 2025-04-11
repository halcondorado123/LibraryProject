using LibraryProject.Application.DTO.Identity.InitialDTO;
using LibraryProject.Domain.Entities.Location;
using LibraryProject.Domain.Entities.UserAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace LibraryProject.Controllers.Identity
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUsuario> _userManager;
        private readonly SignInManager<AppUsuario> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<AppUsuario> usrManager, SignInManager<AppUsuario> signinManager, IEmailSender emailSender)
        {
            _userManager = usrManager;
            _signInManager = signinManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
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
                try
                {
                    // Buscar al usuario por su correo
                    var usuario = await _userManager.FindByEmailAsync(loginDto.Email);

                    if (usuario != null)
                    {
                        await _signInManager.SignOutAsync(); // Limpiar sesiones previas

                        // Intentar iniciar sesión con las credenciales
                        var resultado = await _signInManager.PasswordSignInAsync(
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
                catch (Exception ex)
                {
                    // Aquí podrías loguear el error con algún servicio de logging (como Serilog, NLog, etc.)
                    ModelState.AddModelError("", "Ha ocurrido un error inesperado. Intente nuevamente.");
                    Console.WriteLine(ex); // Solo para desarrollo, evita dejar esto en producción
                }
            }

            return View(loginDto);
        }

        [HttpGet]
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
                // Crear un nuevo usuario
                var user = new AppUsuario
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    Age = model.Age,
                    Salary = model.Salary,
                    PhoneNumber = model.PhoneNumber,
                    AcceptedTerms = model.AcceptedTerms
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Opcionalmente asignar rol al usuario
                    await _userManager.AddToRoleAsync(user, "User");

                    // Generar token de confirmación de email
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Crear URL de confirmación
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, token = token },
                        protocol: HttpContext.Request.Scheme);

                    // Enviar correo de confirmación
                    await _emailSender.SendEmailAsync(
                        model.Email,
                        "Confirma tu cuenta",
                        $"Por favor, confirma tu cuenta haciendo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clic aquí</a>.");

                    // Redirigir a una página de confirmación
                    return RedirectToAction("RegisterConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Si llegamos aquí, algo falló
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }




        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var usuario = new AppUsuario
        //        {
        //            UserName = model.Email,
        //            Email = model.Email,
        //            Edad = model.Edad,
        //            Salario = model.Salario
        //        };

        //        var resultado = await _userManager.CreateAsync(usuario, model.Password);

        //        if (resultado.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(usuario, isPersistent: false);
        //            return RedirectToAction("Index", "Home"); // O a donde prefieras redirigir
        //        }

        //        foreach (var error in resultado.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }

        //    return View(model);
        //}

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"No se pudo cargar el usuario con ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error al confirmar el correo electrónico del usuario con ID '{userId}':");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // No revelamos que el usuario no existe o no está confirmado
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                // Generar el token para resetear la contraseña
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Crear la URL de reseteo
                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { email = model.Email, token = token },
                    protocol: HttpContext.Request.Scheme);

                // Enviar correo
                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Recuperación de contraseña",
                    $"Por favor, recupera tu contraseña haciendo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clic aquí</a>.");

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token = null, string email = null)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var model = new ResetPasswordDTO { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // No revelamos que el usuario no existe
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> FirstSetup()
        {
            return RedirectToAction("_FirstSetupPartial");
        }

        [HttpPost]
        public async Task<IActionResult> FirstSetup(FirstAdminRegisterDTO model)
        {

            if (ModelState.IsValid)
            {
                // Crear un nuevo usuario
                var user = new AppUsuario
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    Age = model.Age,
                    PhoneNumber = model.PhoneNumber,
                    AcceptedTerms = model.AcceptedTerms
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Opcionalmente asignar rol al usuario
                    await _userManager.AddToRoleAsync(user, "User");

                    // Generar token de confirmación de email
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Crear URL de confirmación
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, token = token },
                        protocol: HttpContext.Request.Scheme);

                    // Enviar correo de confirmación
                    await _emailSender.SendEmailAsync(
                        model.Email,
                        "Confirma tu cuenta",
                        $"Por favor, confirma tu cuenta haciendo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clic aquí</a>.");

                    // Redirigir a una página de confirmación
                    return RedirectToAction("RegisterConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Si llegamos aquí, algo falló
            return View(model);
        }






























        // Cierre de sesión
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
