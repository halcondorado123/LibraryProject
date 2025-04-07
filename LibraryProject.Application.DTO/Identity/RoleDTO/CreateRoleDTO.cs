using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Application.DTO.Identity.RoleDTO
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        public string Nombre { get; set; }
    }
}
