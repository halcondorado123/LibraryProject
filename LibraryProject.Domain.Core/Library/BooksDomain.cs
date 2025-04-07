using LibraryProject.Domain.Entities.Library;
using LibraryProject.Domain.Interface.Library;
using LibraryProject.Infraestructure.Interface;


namespace LibraryProject.Domain.Core.Library
{
    public class BooksDomain : IBooksDomain
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksDomain(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BookME>> GetAllAsync(int page, int pageSize)
        {
            var books = await _unitOfWork.Books.GetBooksAsync(page, pageSize);

            if (books == null)
                throw new InvalidOperationException("Failed to retrieve books from the database.");

            return books;
        }


        public async Task<BookME> GetByIdAsync(Guid bookId)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(bookId);

            if (book == null)
                throw new KeyNotFoundException($"Book with ID '{bookId}' was not found.");

            return book;
        }

        public async Task<BookME> GetByParametersAsync(string? authorFirstName, string? authorLastName, string? theme, string? bookTitle, string? place, string? publisher)
        {
            var book = await _unitOfWork.Books.GetBookByParametersAsync(authorFirstName, authorLastName, theme, bookTitle, place, publisher);

            return book ?? throw new KeyNotFoundException("No books found with the specified parameters.");
        }

        public async Task<IEnumerable<BookME>> GetBookByDate(DateTime fecha)
        {
            var books = await _unitOfWork.Books.GetBookByDate(fecha);

            if (books == null || !books.Any())
                throw new KeyNotFoundException($"No book found with registration date: {fecha:yyyy-MM-dd}");

            return books;
        }


        public async Task<BookME> CreateAsync(BookME book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book), "The book to create cannot be null.");

            var createdBook = await _unitOfWork.Books.CreateBookAsync(book);

            if (createdBook == null)
                throw new Exception("Failed to create the book.");

            return createdBook;
        }

        public async Task<BookME> UpdateAsync(BookME book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book), "The book to update cannot be null.");

            var updatedBook = await _unitOfWork.Books.UpdateBookAsync(book);

            if (updatedBook == null)
                throw new Exception($"Failed to update the book with ID {book.BookId}");

            return updatedBook;
        }

        public async Task<bool> DeleteAsync(Guid bookId)
        {
            if (bookId == Guid.Empty)
                throw new ArgumentException("The book ID cannot be empty.", nameof(bookId));

            var result = await _unitOfWork.Books.DeleteBookAsync(bookId);

            if (!result)
                throw new KeyNotFoundException($"No book found with ID {bookId} to delete.");

            return result;
        }
    }
}