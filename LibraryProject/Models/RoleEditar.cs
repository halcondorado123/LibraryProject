using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Models
{
    public class RoleEditar
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUsuario> Members { get; set; }
        public IEnumerable<AppUsuario> NonMembers { get; set; }
    }
}
