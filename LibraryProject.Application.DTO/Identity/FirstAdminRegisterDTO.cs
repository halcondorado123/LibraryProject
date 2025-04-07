using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Application.DTO.Identity
{
    public class FirstAdminRegisterDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Debe tener al menos 6 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe confirmar su contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe ingresar el código especial")]
        public string CodigoValidacion { get; set; }
    }
}
