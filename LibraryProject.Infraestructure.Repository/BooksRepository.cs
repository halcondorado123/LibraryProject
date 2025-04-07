using LibraryProject.Application.DTO.Library;
using LibraryProject.Domain.Entities.Library;
using LibraryProject.Infraestructure.Interface;
using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Infraestructure.Repository
{
    public class BooksRepository : IBooksRepository
    {
        private readonly LibraryDbContext _dbContext;

        public BooksRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BookME>> GetBooksAsync(int page, int pageSize)
        {
            try
            {
                return await _dbContext.Books
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An unexpected exception was thrown: {ex.Message}", ex);
            }
        }


        public async Task<BookME?> GetByIdAsync(Guid bookId)
        {
            return await _dbContext.Books.FindAsync(bookId);
        }

        public async Task<BookME?> GetBookByParametersAsync(string? authorFirstName, string? authorLastName, string? theme, string? bookTitle, string? place, string? publisher)
        {
            try
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(l =>
                    (authorFirstName == null || l.AuthorFirstName == authorFirstName) &&
                    (authorLastName == null || l.AuthorLastName == authorLastName) &&
                    (theme == null || l.Theme == theme) &&
                    (bookTitle == null || l.BookTitle == bookTitle) &&
                    (place == null || l.Place == place) &&
                    (publisher == null || l.Publisher == publisher)
                );

                return book;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An unexpected exception was thrown {ex}");
            }
        }

        public async Task<IEnumerable<BookME>> GetBookByDate(DateTime date)
        {
            try
            {
                var books = await _dbContext.Books
                    .Where(b => b.RegistrationDate.Date == date.Date)
                    .ToListAsync();

                return books;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An unexpected exception was thrown: {ex}");
            }
        }

        public async Task<BookME> CreateBookAsync(BookME newBook)
        {
            try
            {
                // Buscar si ya existe un libro con los mismos datos clave
                var existingBook = await _dbContext.Books.FirstOrDefaultAsync(b =>
                    b.AuthorFirstName == newBook.AuthorFirstName &&
                    b.AuthorLastName == newBook.AuthorLastName &&
                    b.BookTitle == newBook.BookTitle &&
                    b.Theme == newBook.Theme &&
                    b.Place == newBook.Place &&
                    b.Publisher == newBook.Publisher
                );

                if (existingBook != null)
                {
                    // Si ya existe, incrementamos el stock
                    existingBook.Stock += newBook.Stock;

                    // Actualizamos disponibilidad
                    existingBook.IsAvailable = existingBook.Stock > 0;

                    await _dbContext.SaveChangesAsync();

                    return existingBook;
                }

                // Si no existe, creamos un nuevo registro
                newBook.BookId = Guid.NewGuid();
                newBook.RegistrationDate = DateTime.UtcNow;
                newBook.IsAvailable = newBook.Stock > 0;

                await _dbContext.Books.AddAsync(newBook);
                await _dbContext.SaveChangesAsync();

                return newBook;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error creating or updating workbook: {ex.Message}");
            }
        }

        public async Task<BookME?> UpdateBookAsync(BookME updatedBook)
        {
            try
            {
                var book = await _dbContext.Books.FindAsync(updatedBook.BookId);

                if (book == null)
                {
                    return null;
                }

                // Actualizamos solo los campos que hayan cambiado o estén disponibles
                book.AuthorFirstName = updatedBook.AuthorFirstName ?? book.AuthorFirstName;
                book.AuthorLastName = updatedBook.AuthorLastName ?? book.AuthorLastName;
                book.Theme = updatedBook.Theme ?? book.Theme;
                book.BookTitle = updatedBook.BookTitle ?? book.BookTitle;
                book.Place = updatedBook.Place ?? book.Place;
                book.Publisher = updatedBook.Publisher ?? book.Publisher;
                book.Stock = updatedBook.Stock;
                book.IsAvailable = updatedBook.IsAvailable;

                await _dbContext.SaveChangesAsync();

                return book;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating book: {ex.Message}", ex);
            }
        }


        public async Task<bool> DeleteBookAsync(Guid bookId)
        {
            try
            {
                var book = await _dbContext.Books.FindAsync(bookId);

                if (book == null) return false;

                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"SAn unexpected exception was thrown {ex}");
            }
        }
    }
}
