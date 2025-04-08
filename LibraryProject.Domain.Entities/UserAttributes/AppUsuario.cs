using LibraryProject.Domain.Entities.Location;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Domain.Entities.UserAttributes
{
    public class AppUsuario : IdentityUser
    {
        //public CountryME? Pais { get; set; }
        public int? Edad { get; set; }
        public string? Salario { get; set; }
    }
}
