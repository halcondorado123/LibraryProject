using LibraryProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers.Identity
{
    public class AdminController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View(userManager.Users);
        //}

        private UserManager<AppUsuario> userManager;
        private IPasswordHasher<AppUsuario> passwordHasher;
        private IPasswordValidator<AppUsuario> passwordValidator;
        private IUserValidator<AppUsuario> userValidator;

        public AdminController(UserManager<AppUsuario> userManager, IPasswordHasher<AppUsuario> passwordHash, IPasswordValidator<AppUsuario> passwordValidator, IUserValidator<AppUsuario> userValidator)
        {
            this.userManager = userManager;
            passwordHasher = passwordHash;
            this.passwordValidator = passwordValidator;
            this.userValidator = userValidator;
        }


        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var totalUsuarios = userManager.Users.Count(); // Total de usuarios
            var usuarios = userManager.Users
                .OrderBy(u => u.UserName) // Ordenar por nombre
                .Skip((page - 1) * pageSize) // Saltar los registros de páginas anteriores
                .Take(pageSize) // Tomar solo los registros necesarios
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalUsuarios / pageSize); // Total de páginas

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(usuarios);
        }


        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                AppUsuario appUsuario = new AppUsuario
                {
                    UserName = usuario.Nombre,
                    Email = usuario.Email,
                    Pais = usuario.Pais,
                    Edad = usuario.Edad,
                    Salario = usuario.Salario
                };

                IdentityResult resultado = await userManager.CreateAsync(appUsuario, usuario.Password);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in resultado.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(usuario);
        }


        // Mediante Http Get

        public async Task<IActionResult> Update(string id)
        {
            AppUsuario usuario = await userManager.FindByIdAsync(id);
            if (usuario != null)
            {
                return View(usuario);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Update(string id, string email, string password)
        //{
        //    AppUsuario Usuario = await userManager.FindByIdAsync(id);

        //    if (Usuario != null)
        //    {
        //        if (!string.IsNullOrEmpty(email))
        //        {
        //            Usuario.Email = email;
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Por favor, el email no puede estar vacio");
        //        }
        //        if (!string.IsNullOrEmpty(password))
        //        {
        //            Usuario.PasswordHash = passwordHasher.HashPassword(Usuario, password);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Por favor, el password no puede estar vacio");
        //        }
        //        IdentityResult resultado = await userManager.UpdateAsync(Usuario);
        //        if (resultado.Succeeded)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            Errors(resultado);
        //        }
        //    }
        //    return View(Usuario);
        //}

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password, string pais, int edad, string salario)
        {
            AppUsuario usuario = await userManager.FindByIdAsync(id);

            if (usuario != null)
            {
                IdentityResult emailValido = null;
                if (!string.IsNullOrEmpty(email))
                {
                    emailValido = await userValidator.ValidateAsync(userManager, usuario);
                    if (emailValido.Succeeded)
                        usuario.Email = email;
                    else
                    {
                        Errors(emailValido);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El Email no puede estar vacio");
                }

                IdentityResult passValido = null;
                if (!string.IsNullOrEmpty(password))
                {
                    passValido = await passwordValidator.ValidateAsync(userManager, usuario, password);
                    if (passValido.Succeeded)
                        usuario.PasswordHash = passwordHasher.HashPassword(usuario, password);
                    else
                    {
                        Errors(passValido);
                    }
                }

                usuario.Edad = edad;
                //Pais miPais;
                //Enum.TryParse(pais, out miPais);
                //usuario.Pais = miPais;
                usuario.Salario = salario;

                IdentityResult resultado = await userManager.UpdateAsync(usuario);

                if (resultado.Succeeded)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("", "No se ha podido actualizar el registro");

            }
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUsuario Usuario = await userManager.FindByIdAsync(id);
            if (Usuario != null)
            {
                IdentityResult resultado = await userManager.DeleteAsync(Usuario);
                if (resultado.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    Errors(resultado);
                }
            }
            else
            {
                ModelState.AddModelError("", "Usuario no encontrado");
            }
            return View("Index", userManager.Users);
        }

        private void Errors(IdentityResult resultado)
        {
            foreach (IdentityError error in resultado.Errors)
                ModelState.AddModelError("", error.Description);
        }

    }
}

