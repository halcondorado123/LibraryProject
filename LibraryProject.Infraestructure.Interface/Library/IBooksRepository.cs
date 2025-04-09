using LibraryProject.Application.DTO.Library;
using LibraryProject.Domain.Entities.Library;

namespace LibraryProject.Infraestructure.Interface.Library
{
    public interface IBooksRepository
    {
        Task<IEnumerable<BookME>> GetAllAsync();
        

        Task<(IEnumerable<BookME> Items, int TotalCount)> GetFilteredBooksAsync(int page, int pageSize, string bookTitle, string authorFirstName, string authorLastName, string theme, string publisher, string place);
        Task<int> GetFilteredCountAsync(string bookTitle, string authorFirstName, string authorLastName, string theme, string publisher, string place);


        Task<IEnumerable<BookME>> GetBooksAsync(int page, int pageSize);
        Task<BookME?> GetByIdAsync(Guid bookId);    // Se neceita exclusivamente para el Update
        Task<BookME> GetBookByParametersAsync(string? authorFirstName, string? authorLastName, string? theme, string? bookTitle, string? place, string? publisher);
        Task<IEnumerable<BookME?>> GetBookByDate(DateTime fecha);
        Task<BookME> CreateBookAsync(BookME book);
        Task<BookME?> UpdateBookAsync(BookME updatedBookDto);
        Task<bool> DeleteBookAsync(Guid bookId);
    }
}