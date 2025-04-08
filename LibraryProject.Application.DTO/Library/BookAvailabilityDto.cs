using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.DTO.Library
{
    public class BookAvailabilityDto
    {
        public bool IsAvailable { get; set; }
        public string Message { get; set; } = string.Empty;
        public BookDto? Book { get; set; }
    }
}
