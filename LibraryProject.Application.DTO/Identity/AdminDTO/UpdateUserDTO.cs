using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.DTO.Identity.AdminDTO
{
    public class UpdateUserDTO
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? PaisId { get; set; }
        public int? Edad { get; set; }
        public string? Salario { get; set; }
    }
}
