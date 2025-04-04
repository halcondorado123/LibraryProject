using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Models
{
    public class AppUsuario : IdentityUser
    {
        // Aqui va vacio. Nos proporciona datos basicos de identidad, si se desean más atributos, se pueden agregar

        public Pais Pais { get; set; }
        public int Edad { get; set; }
        public string Salario { get; set; }
    }

    public enum Pais
    {
        España, Portugal, México, Argentina, Colombia, Chile
    }
}

