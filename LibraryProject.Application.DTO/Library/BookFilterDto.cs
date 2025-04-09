using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.DTO.Library
{
    public class BookFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortOrder { get; set; } = "asc";
        public string? Title { get; set; } // Para filtro por título, si quieres
    }
}
