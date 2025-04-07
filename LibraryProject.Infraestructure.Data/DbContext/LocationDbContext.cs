using LibraryProject.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Infraestructure.Data.DbContext
{
    public class LocationDbContext : DbContext
    {
        // El constructor debe aceptar DbContextOptions<LocationDbContext>
        public LocationDbContext(DbContextOptions<LocationDbContext> options)
            : base(options) // Pasa las opciones del contexto a la clase base
        {
        }

        public DbSet<CountryME> Countries { get; set; }
    }
}
