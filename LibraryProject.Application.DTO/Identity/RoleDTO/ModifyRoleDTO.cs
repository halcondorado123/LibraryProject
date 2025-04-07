namespace LibraryProject.Application.DTO.Identity.RoleDTO
{
    public class ModifyRoleDTO
    {
        public string NombreRol { get; set; }
        public string IdRol { get; set; }
        public string[]? AgregarIds { get; set; }
        public string[]? EliminarIds { get; set; }
    }
}
