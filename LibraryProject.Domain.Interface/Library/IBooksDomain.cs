using LibraryProject.Domain.Entities.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Interface.Library
{
    public interface IBooksDomain
    {
        Task<(IEnumerable<BookME> Items, int TotalCount)> GetByParametersAsync(int page, int pageSize, string bookTitle, string authorFirstName, string authorLastName,
                                                                                string theme, string publisher, string place);
        Task<int> GetFilteredCountAsync(string bookTitle, string authorFirstName, string authorLastName, string theme, string publisher, string place);



        Task<IEnumerable<BookME>> GetAllAsync(int page, int pageSize);
        Task<BookME?> GetByIdAsync(Guid bookId);

        Task<IEnumerable<BookME>> GetBookByDate(DateTime fecha);
        Task<BookME> CreateAsync(BookME book);
        Task<BookME> UpdateAsync(BookME book);
        Task<bool> DeleteAsync(Guid id);
    }
}
