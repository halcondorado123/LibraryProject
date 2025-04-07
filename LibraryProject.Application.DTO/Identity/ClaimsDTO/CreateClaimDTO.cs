using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Application.DTO.Identity.ClaimsDTO
{
    public class CreateClaimDTO
    {
        [Required]
        public string ClaimType { get; set; }

        [Required]
        public string ClaimValue { get; set; }
    }
}
