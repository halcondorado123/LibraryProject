using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryProject.Domain.Entities.Library;
using LibraryProject.Domain.Entities.Location;
using LibraryProject.Domain.Entities.UserAttributes;
using static LibraryProject.Domain.Entities.Library.CommentsME;

public class AppDbContext : IdentityDbContext<AppUsuario>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<BookME> Books { get; set; }
    public DbSet<CommentsME> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
