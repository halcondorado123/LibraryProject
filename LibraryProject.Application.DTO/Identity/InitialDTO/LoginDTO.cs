using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Application.DTO.Identity.InitialDTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Email no válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
