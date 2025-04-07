using LibraryProject.Domain.Entities.Library;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Models
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options) // Pasa las opciones del contexto
        {
        }

        public DbSet<BookME> Books { get; set; }
    }
}
