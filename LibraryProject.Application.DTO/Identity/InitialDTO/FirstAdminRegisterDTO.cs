using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Application.DTO.Identity.InitialDTO
{
    public class FirstAdminRegisterDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El UserName es obligatorio")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El edad es obligatoria")]
        public int Age { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Debes aceptar los términos y condiciones")]
        public bool AcceptedTerms { get; set; }

        [Required(ErrorMessage = "Debe ingresar el código especial")]
        public string CodigoValidacion { get; set; }        // Generado en Json
    }
}
