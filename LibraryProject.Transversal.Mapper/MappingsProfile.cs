using AutoMapper;
using LibraryProject.Application.DTO.Library;
using LibraryProject.Domain.Entities.Library;

namespace LibraryProject.Transversal.Mapper
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<BookME, BookFilterDto>().ReverseMap();
            CreateMap<BookME, BookDto>().ReverseMap();
            CreateMap<BookME, UpdateBookDto>().ReverseMap();

        }
    }
}
