using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Application.DTO.Identity.InitialDTO
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
