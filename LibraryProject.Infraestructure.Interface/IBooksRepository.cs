﻿using LibraryProject.Application.DTO.Library;
using LibraryProject.Domain.Entities.Library;

namespace LibraryProject.Infraestructure.Interface
{
    public interface IBooksRepository
    {
        Task<IEnumerable<BookME>> GetBooksAsync(int page, int pageSize);
        Task<BookME?> GetByIdAsync(Guid bookId);    // Se neceita exclusivamente para el Update
        Task<BookME> GetBookByParametersAsync(string? authorFirstName, string? authorLastName, string? theme, string? bookTitle, string? place, string? publisher);
        Task<IEnumerable<BookME?>> GetBookByDate(DateTime fecha);
        Task<BookME> CreateBookAsync(BookME book);
        Task<BookME?> UpdateBookAsync(BookME updatedBookDto);
        Task<bool> DeleteBookAsync(Guid bookId);
    }
}