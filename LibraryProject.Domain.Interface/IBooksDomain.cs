using LibraryProject.Domain.Entities.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Interface
{
    public interface IBooksDomain
    {
        Task<IEnumerable<BookME>> GetAllAsync(int page, int pageSize);
        Task<BookME?> GetByIdAsync(Guid bookId);
        Task<BookME> GetByParametersAsync(
            string? authorFirstName,
            string? authorLastName,
            string? theme,
            string? bookTitle,
            string? place,
            string? publisher);

        Task<IEnumerable<BookME>> GetBookByDate(DateTime fecha);
        Task<BookME> CreateAsync(BookME book);
        Task<BookME> UpdateAsync(BookME book);
        Task<bool> DeleteAsync(Guid id);
    }
}
