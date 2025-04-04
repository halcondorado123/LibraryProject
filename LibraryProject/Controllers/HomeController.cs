using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryProject.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUsuario> _userManager;

        public HomeController(UserManager<AppUsuario> usrManager)
        {
            _userManager = usrManager;
        }

        // Solamente mostrara la vista si el usuario esta autenticado
        //[Authorize(Roles = "Administración")]
        [Authorize(Roles = "Administración")]
        public async Task<IActionResult> Index()
        {
            AppUsuario usuario = await _userManager.GetUserAsync(HttpContext.User);
            if (usuario == null)
            {
                // Manejar el caso en el que el usuario no se encuentra
                return RedirectToAction("Error", "Home"); // Redirige a una página de error o algo similar
            }
            // Obtener la fecha y hora actual en formato deseado
            string fechaHoraActual = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm"); // Ejemplo: "martes, 04 septiembre 2024 14:30"
                                                                                        // Crear el ViewModel con el nombre del usuario y la fecha/hora
            var model = new InfoAccesoApp
            {
                NombreUsuario = $"Bienvenido {usuario.UserName}",
                FechaHora = fechaHoraActual
            };

            // Retornar el ViewModel a la vista
            return View(model);
        }
    }
}
