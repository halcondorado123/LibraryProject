using AutoMapper;
using LibraryProject.Application.DTO.Identity;
using LibraryProject.Application.DTO.Identity.AdminDTO;
using LibraryProject.Application.DTO.Identity.InitialDTO;
using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Domain.Entities.Location;
using LibraryProject.Application.DTO.Identity.RoleDTO;

namespace LibraryProject.Transversal.Mapper
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            // AppUsuario -> UserDTO
            CreateMap<AppUsuario, UserDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Salario, opt => opt.MapFrom(src => src.Salario))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.Edad));

            // UserDTO -> AppUsuario
            CreateMap<UserDTO, AppUsuario>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Salario, opt => opt.MapFrom(src => src.Salario))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.Edad));

            // LoginDTO <-> AppUsuario
            CreateMap<RegisterDTO, AppUsuario>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Nombre)) // ← Aquí usamos el Nombre como UserName
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.Edad))
                .ForMember(dest => dest.Salario, opt => opt.MapFrom(src => src.Salario));


            CreateMap<AppUsuario, LoginDTO>().ReverseMap();
            CreateMap<ModifyRoleDTO, UpdateRoleDTO>();

            CreateMap<UpdateUserDTO, AppUsuario>()
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Nombre));

            // AppUsuario -> UpdateUserDTO
            CreateMap<AppUsuario, UpdateUserDTO>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.UserName));

            // UpdateUserDTO -> AppUsuario
            CreateMap<UpdateUserDTO, AppUsuario>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
