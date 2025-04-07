using LibraryProject.Application.DTO.Identity.HomeDTO;
using LibraryProject.Domain.Entities.UserAttributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers.Identity
{
    public class HomeController : Controller
    {
        private UserManager<AppUsuario> _userManager;

        public HomeController(UserManager<AppUsuario> usrManager)
        {
            _userManager = usrManager;
        }

        // Solamente mostrara la vista si el usuario esta autenticado
        public async Task<IActionResult> Index()
        {
            AppUsuario usuario = await _userManager.GetUserAsync(HttpContext.User);
            if (usuario == null)
            {
                return RedirectToAction("Error", "Home");
            }

            string fechaHoraActual = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm");

            var model = new AccessInfoDTO
            {
                NombreUsuario = $"Bienvenido {usuario.UserName}",
                FechaHora = fechaHoraActual
            };

            return View(model);
        }
    }
}
