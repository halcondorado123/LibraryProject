using LibraryProject.Domain.Entities.Location;
using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Domain.Entities.UserAttributes
{
    public class AppUsuario : IdentityUser
    {
        // Aqui va vacio. Nos proporciona datos basicos de identidad, si se desean más atributos, se pueden agregar

        public int? PaisId { get; set; }
        public CountryME? Pais { get; set; }
        public int? Edad { get; set; }
        public float? Salario { get; set; }
    }
}
