using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryProject.Controllers
{
    public class ClaimsController : Controller
    {
        //[Authorize]
        //[Authorize(Policy = "Segundo Email")]
        public ViewResult Index()
        {
            return View(User?.Claims);
        }

        private readonly UserManager<AppUsuario> userManager;

        // Para la implementacion de acceso privado
        private IAuthorizationService authService;

        public ClaimsController(UserManager<AppUsuario> userManager, IAuthorizationService authService)
        {
            this.userManager = userManager;
            this.authService = authService;             // <-- Para la implementacion de acceso privado
        }


        public ViewResult Create()
        {
            return View();
        }


        // Obtener el usuario actual desde UserManager
        [HttpPost]
        public async Task<IActionResult> Create(string claimtype, string claimValue)
        {
            AppUsuario usuario = await userManager.GetUserAsync(HttpContext.User);
            Claim reclamo = new Claim(claimtype, claimValue, ClaimValueTypes.String);

            // Crear nuevo reclamo para nuestro usuario
            IdentityResult resultado = await userManager.AddClaimAsync(usuario, reclamo);
            if (resultado.Succeeded)
                return RedirectToAction("Index");
            else
                ModelState.AddModelError("", "No se ha podido crear el reclamo");

            return View();
        }


        // Eliminar un reclamo
        [HttpPost]
        public async Task<IActionResult> Delete(string claimValues)
        {
            try
            {
                AppUsuario usuario = await userManager.GetUserAsync(HttpContext.User);

                if (usuario == null)
                {
                    ModelState.AddModelError("", "Usuario no encontrado.");
                    return View();
                }

                string[] claimValueArray = claimValues.Split(";");
                if (claimValueArray.Length < 3)
                {
                    ModelState.AddModelError("", "Formato de reclamo inválido.");
                    return View();
                }

                string claimType = claimValueArray[0], claimValue = claimValueArray[1], claimIssuer = claimValueArray[2];

                Claim reclamo = User.Claims
                    .Where(x => x.Type == claimType && x.Value == claimValue && x.Issuer == claimIssuer)
                    .FirstOrDefault();

                if (reclamo == null)
                {
                    ModelState.AddModelError("", "Reclamo no encontrado.");
                    return View();
                }

                IdentityResult resultado = await userManager.RemoveClaimAsync(usuario, reclamo);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "No se ha podido eliminar el reclamo.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrió un error: {ex.Message}");
            }

            return View();
        }

        //[Authorize(Policy = "Segundo Email")]
        public ViewResult Proyecto()
        {
            return View("Index", User?.Claims);
        }

        [Authorize(Policy = "PermitirUsuarios")]
        public ViewResult SubirArchivos()
        {
            return View("Index", User?.Claims);
        }


        public async Task<IActionResult> AccesoPrivado(string titulo)
        {
            string[] UsuariosPermitidos = { "espana", "turbias" };
            AuthorizationResult resultado = await authService.AuthorizeAsync(User, UsuariosPermitidos, "AccesoPrivado");

            if (resultado.Succeeded)
            {
                return View("Index", User?.Claims);     // Trae todos los Claims al(los) usuarios autorizados
            }

            else
            {
                return Content("Ha sucedido un error en el acceso a este recurso");
            }
        }
    }
}
