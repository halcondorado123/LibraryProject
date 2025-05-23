﻿using AutoMapper;
using LibraryProject.Application.DTO.Identity;
using LibraryProject.Application.DTO.Identity.AdminDTO;
using LibraryProject.Application.DTO.Identity.InitialDTO;
using LibraryProject.Domain.Entities.UserAttributes;
using LibraryProject.Domain.Entities.Location;
using LibraryProject.Application.DTO.Identity.RoleDTO;
using LibraryProject.Application.DTO.Library;
using LibraryProject.Domain.Entities.Library;

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
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age));

            // UserDTO -> AppUsuario
            CreateMap<UserDTO, AppUsuario>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age));

            // LoginDTO <-> AppUsuario
            CreateMap<RegisterDTO, AppUsuario>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName)) // ← Aquí usamos el Nombre como UserName
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.AcceptedTerms, opt => opt.MapFrom(src => src.AcceptedTerms));


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

            CreateMap<BookME, BookDto>();
            CreateMap<BookDto, BookME>();

            CreateMap<BookME, BookDto>()
            .ForMember(dest => dest.Author, opt =>
                opt.MapFrom(src => $"{src.AuthorFirstName} {src.AuthorLastName}"));

            CreateMap<CommentsUserDto, CommentsME>(); // O el nombre real de tu entidad de comentarios
        }
    }
}
