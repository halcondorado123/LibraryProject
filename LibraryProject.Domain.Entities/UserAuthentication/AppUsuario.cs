using LibraryProject.Domain.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities.UserAuthentication
{
    public class AppUsuario
    {
        // Aqui va vacio. Nos proporciona datos basicos de identidad, si se desean más atributos, se pueden agregar

        public CountryME Pais { get; set; }
        public int Edad { get; set; }
        public string Salario { get; set; }
    }
}
