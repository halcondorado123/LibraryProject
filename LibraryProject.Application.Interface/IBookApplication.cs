using LibraryProject.Application.DTO.Library;
using LibraryProject.Transversal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.DTO
{
    public interface IBookApplication
    {
        Task<Response<IEnumerable<BookDto>>> GetBooksAsync(int page, int pageSize);
        Task<Response<BookDto>> GetBookByParametersAsync(BookFilterDto filter);
        Task<Response<IEnumerable<BookDto?>>> GetBookByDate(DateTime fecha);
        Task<Response<BookDto>> CreateBookAsync(BookDto book);
        Task<Response<UpdateBookDto?>> UpdateBookAsync(UpdateBookDto updatedBookDto);
        Task<Response<bool>> DeleteBookAsync(Guid bookId);
    }
}
