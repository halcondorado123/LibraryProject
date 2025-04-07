﻿namespace LibraryProject.Application.DTO.Identity
{
    public class UpdateRoleDTO
    {
        public class UpdateRoleDto
        {
            public string RoleId { get; set; }
            public string RoleName { get; set; }
            public List<UserInRoleDto> Members { get; set; }
            public List<UserInRoleDto> NonMembers { get; set; }
        }

        public class UserInRoleDto
        {
            public string UserId { get; set; }
            public string Email { get; set; }
        }
    }
}
