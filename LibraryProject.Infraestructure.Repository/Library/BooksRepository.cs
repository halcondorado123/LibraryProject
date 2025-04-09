using LibraryProject.Domain.Entities.Library;
using LibraryProject.Infraestructure.Interface.Library;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Infraestructure.Repository.Library
{
    public class BooksRepository : IBooksRepository
    {
        private readonly AppDbContext _dbContext;

        public BooksRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BookME>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Books.ToListAsync();
            }
            catch (Exception ex)
            {
                // Podrías registrar el error aquí si tienes un sistema de logging
                Console.WriteLine($"Error al obtener los libros: {ex.Message}");
                return new List<BookME>(); // O podrías lanzar una excepción personalizada si prefieres
            }
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

        public async Task<(IEnumerable<BookME> Items, int TotalCount)> GetFilteredBooksAsync(int page, int pageSize, string bookTitle, string authorFirstName, string authorLastName,
                                                                                             string theme, string publisher, string place)
        {
            var query = _dbContext.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(bookTitle))
                query = query.Where(b => b.BookTitle.Contains(bookTitle));

            if (!string.IsNullOrWhiteSpace(authorFirstName))
                query = query.Where(b => b.AuthorFirstName.Contains(authorFirstName));

            if (!string.IsNullOrWhiteSpace(authorLastName))
                query = query.Where(b => b.AuthorLastName.Contains(authorLastName));

            if (!string.IsNullOrWhiteSpace(theme))
                query = query.Where(b => b.Theme.Contains(theme));

            if (!string.IsNullOrWhiteSpace(publisher))
                query = query.Where(b => b.Publisher.Contains(publisher));

            if (!string.IsNullOrWhiteSpace(place))
                query = query.Where(b => b.Place.Contains(place));

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<int> GetFilteredCountAsync(string bookTitle, string authorFirstName, string authorLastName, string theme, string publisher, string place)
        {
            var query = _dbContext.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(bookTitle))
                query = query.Where(b => b.BookTitle.Contains(bookTitle));

            if (!string.IsNullOrWhiteSpace(authorFirstName))
                query = query.Where(b => b.AuthorFirstName.Contains(authorFirstName));

            if (!string.IsNullOrWhiteSpace(authorLastName))
                query = query.Where(b => b.AuthorLastName.Contains(authorLastName));

            if (!string.IsNullOrWhiteSpace(theme))
                query = query.Where(b => b.Theme.Contains(theme));

            if (!string.IsNullOrWhiteSpace(publisher))
                query = query.Where(b => b.Publisher.Contains(publisher));

            if (!string.IsNullOrWhiteSpace(place))
                query = query.Where(b => b.Place.Contains(place));

            return await query.CountAsync();
        }
    








        public async Task<BookME?> GetByIdAsync(Guid bookId)
        {
            return await _dbContext.Books.FindAsync(bookId);
        }

        //public async Task<BookME?> GetBookByParametersAsync(string? authorFirstName, string? authorLastName, string? theme, string? bookTitle, string? place, string? publisher)
        //{
        //    try
        //    {
        //        var book = await _dbContext.Books.FirstOrDefaultAsync(l =>
        //            (authorFirstName == null || l.AuthorFirstName == authorFirstName) &&
        //            (authorLastName == null || l.AuthorLastName == authorLastName) &&
        //            (theme == null || l.Theme == theme) &&
        //            (bookTitle == null || l.BookTitle == bookTitle) &&
        //            (place == null || l.Place == place) &&
        //            (publisher == null || l.Publisher == publisher)
        //        );

        //        return book;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException($"An unexpected exception was thrown {ex}");
        //    }
        //}

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



        public Task<BookME> GetBookByParametersAsync(string? authorFirstName, string? authorLastName, string? theme, string? bookTitle, string? place, string? publisher)
        {
            throw new NotImplementedException();
        }
    }
}
