using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.DTO.Identity.RoleDTO
{
    public class UpdateRoleDTO
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<UserInRoleDTO> Members { get; set; }
        public List<UserInRoleDTO> NonMembers { get; set; }

    }
}
