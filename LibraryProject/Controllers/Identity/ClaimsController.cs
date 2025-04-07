using LibraryProject.Application.DTO.Identity.ClaimsDTO;
using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryProject.Controllers.Identity
{
    public class ClaimsController : Controller
    {
        // Para la implementacion de acceso privado
        private IAuthorizationService _authService;
        private readonly UserManager<AppUsuario> _userManager;

        public ClaimsController(UserManager<AppUsuario> userManager, IAuthorizationService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        //[Authorize]
        //[Authorize(Policy = "Segundo Email")]
        public ViewResult Index()
        {
            return View(User?.Claims);
        }

        public ViewResult Create()
        {
            return View();
        }


        // Obtener el usuario actual desde UserManager
        [HttpPost]
        public async Task<IActionResult> Create(CreateClaimDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            var claim = new Claim(model.ClaimType, model.ClaimValue, ClaimValueTypes.String);

            var result = await _userManager.AddClaimAsync(user, claim);

            if (result.Succeeded)
                return RedirectToAction("Index");

            Errors(result); // Si tienes un método Errors como en los controladores anteriores

            return View(model);
        }

        // Eliminar un reclamo
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteClaimDTO dto)
        {
            try
            {
                AppUsuario user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    ModelState.AddModelError("", "Usuario no encontrado.");
                    return View("Index", User?.Claims);
                }

                var claims = await _userManager.GetClaimsAsync(user);
                Claim? reclamo = claims.FirstOrDefault(x =>
                    x.Type == dto.ClaimType &&
                    x.Value == dto.ClaimValue &&
                    x.Issuer == dto.ClaimIssuer
                );

                if (reclamo == null)
                {
                    ModelState.AddModelError("", "Reclamo no encontrado.");
                    return View("Index", claims);
                }

                IdentityResult result = await _userManager.RemoveClaimAsync(user, reclamo);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View("Index", claims);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrió un error: {ex.Message}");
                return View("Index", User?.Claims);
            }
        }


        //[Authorize(Policy = "Segundo Email")]
        // Sirve para mostrar los claims (modo libre).
        public ViewResult Proyecto()
        {
            return View("Index", User?.Claims);
        }

        // Política para proteger una acción específica basada en los claims del usuario.
        [Authorize(Policy = "PermitirUsuarios")]
        public ViewResult SubirArchivos()
        {
            return View("Index", User?.Claims);
        }


        // Eliminar un claim
        [HttpPost]
        public async Task<IActionResult> Delete(string claimValues)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    ModelState.AddModelError("", "Usuario no encontrado.");
                    return View("Index", User?.Claims);
                }

                var claimArray = claimValues.Split(";");
                if (claimArray.Length < 3)
                {
                    ModelState.AddModelError("", "Formato inválido para eliminar el claim.");
                    return View("Index", User?.Claims);
                }

                string type = claimArray[0];
                string value = claimArray[1];
                string issuer = claimArray[2];

                var claim = User.Claims.FirstOrDefault(c =>
                    c.Type == type && c.Value == value && c.Issuer == issuer);

                if (claim == null)
                {
                    ModelState.AddModelError("", "Reclamo no encontrado.");
                    return View("Index", User?.Claims);
                }

                var result = await _userManager.RemoveClaimAsync(user, claim);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error al eliminar el reclamo.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }

            return View("Index", User?.Claims);
        }
    
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

    }
}
