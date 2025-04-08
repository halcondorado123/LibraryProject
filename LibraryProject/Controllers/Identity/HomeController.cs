using LibraryProject.Application.DTO.Identity.HomeDTO;
using LibraryProject.Domain.Entities.UserAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var usuario = await _userManager.GetUserAsync(HttpContext.User);

            string fechaHoraActual = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm", new CultureInfo("es-ES"));

            var model = new AccessInfoDTO
            {
                NombreUsuario = $"Bienvenido {usuario.UserName}",
                FechaHora = fechaHoraActual
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(); // Puedes mostrar una vista personalizada de error si quieres
        }
    }
}
