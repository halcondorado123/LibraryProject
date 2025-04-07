using AutoMapper;
using LibraryProject.Application.DTO.Identity;
using LibraryProject.Application.DTO.Identity.AdminDTO;
using LibraryProject.Application.DTO.Library;
using LibraryProject.Domain.Entities.Library;
using LibraryProject.Domain.Entities.UserAttributes;

namespace LibraryProject.Transversal.Mapper
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            // Library
            CreateMap<BookME, BookFilterDto>().ReverseMap();
            CreateMap<BookME, BookDto>().ReverseMap();
            CreateMap<BookME, UpdateBookDto>().ReverseMap();

            // Identity

            // General
            CreateMap<AppUsuario, LoginDTO>().ReverseMap();
            CreateMap<AppUsuario, UpdateRoleDTO>().ReverseMap();
            //CreateMap<AppUsuario, UpdateBookDto>().ReverseMap();

            // AdminDTO
            CreateMap<UserDTO, AppUsuario>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PaisId, opt => opt.MapFrom(src => src.PaisId));

            CreateMap<AppUsuario, UpdateUserDTO>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.UserName));

            CreateMap<UpdateUserDTO, AppUsuario>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
