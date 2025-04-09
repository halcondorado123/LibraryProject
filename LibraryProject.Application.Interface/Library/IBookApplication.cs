using LibraryProject.Application.DTO.Library;
using LibraryProject.Transversal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interface.Library
{
    public interface IBookApplication
    {
        Task<Response<IEnumerable<BookDto>>> GetAllBooksAsync();
        Task<ResponseGeneric<bool>> CreateCommentAsync(CommentsUserDto dto, string userId);

        //Task<(int totalPages, int totalCount)> GetFilteredPaginationDataAsync(BookFilterDto filter); 
        Task<Response<IEnumerable<BookDto?>>> GetBookByDate(DateTime fecha);
        //Task<Response<BookDto>> CreateBookAsync(BookDto book);
        Task<Response<UpdateBookDto?>> UpdateBookAsync(UpdateBookDto updatedBookDto);
        Task<Response<bool>> DeleteBookAsync(Guid bookId);
    }
}
