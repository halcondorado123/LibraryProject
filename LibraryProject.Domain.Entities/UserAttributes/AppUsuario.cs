using LibraryProject.Domain.Entities.Library;
using LibraryProject.Domain.Entities.Location;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Domain.Entities.UserAttributes
{
    public class AppUsuario : IdentityUser
    {
        //public CountryME? Pais { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string? Salary { get; set; }
        public string? PhoneNumber { get; set; }
        public bool AcceptedTerms { get; set; }

        // Relación inversa con comentarios
        public ICollection<CommentsME> Comments { get; set; }
    }
}
