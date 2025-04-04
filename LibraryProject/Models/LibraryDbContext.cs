using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Models
{
    public class LibraryDbContext : IdentityDbContext<AppUsuario>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options) // Pasa las opciones del contexto
        {
        }
    }
}
