namespace LibraryProject.Application.DTO.Identity.ClaimsDTO
{
    public class DeleteClaimDTO
    {
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
        public string ClaimIssuer { get; set; } = string.Empty;
    }
}
