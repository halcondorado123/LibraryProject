using AutoMapper;
using LibraryProject.Application.DTO.Library;
using LibraryProject.Application.Interface.Library;
using LibraryProject.Domain.Entities.Library;
using LibraryProject.Domain.Interface.Library;
using LibraryProject.Infraestructure.Interface.Library;
using LibraryProject.Transversal.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Application.Services.Library
{
    public class BookApplication : IBookApplication
    {
        private readonly IBooksDomain _booksDomain; 
        private readonly IBooksRepository _booksRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<BookApplication> _logger;

        public BookApplication(IBooksDomain booksDomain, IMapper mapper, IAppLogger<BookApplication> logger, IBooksRepository booksRepository)
        {
            _booksDomain = booksDomain;
            _mapper = mapper;
            _logger = logger;
            _booksRepository = booksRepository;
        }


        public async Task<Response<IEnumerable<BookDto>>> GetAllBooksAsync()
        {
            var books = await _booksRepository.GetAllAsync();
            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);
            var orderedBooks = bookDtos.OrderBy(b => b.BookTitle);

            return new Response<IEnumerable<BookDto>>
            {
                IsSuccess = true,
                Data = orderedBooks
            };
        }


        public async Task<ResponseGeneric<bool>> CreateCommentAsync(CommentsUserDto dto, string userId)
        {
            var response = new ResponseGeneric<bool>();

            // Validación básica del comentario
            if (string.IsNullOrWhiteSpace(dto.CommentaryText))
            {
                response.IsSuccess = false;
                response.Data = false;
                response.Message = "El comentario no puede estar vacío.";
                return response;
            }

            // Mapeo de DTO a entidad
            var entity = _mapper.Map<CommentsME>(dto);
            entity.UserId = userId;
            entity.CommentaryCreation = DateTime.UtcNow;

            var result = await _booksDomain.CreateCommentAsync(entity);

            // Estructura la respuesta
            response.IsSuccess = result;
            response.Data = result;
            response.Message = result
                ? "Comentario creado correctamente."
                : "No se pudo guardar el comentario.";

            return response;
        }







        //public async Task<Response<IEnumerable<BookDto>>> GetBooksAsync(BookFilterDto filter)
        //{
        //    var response = new Response<IEnumerable<BookDto>>();

        //    try
        //    {
        //        var result = await _booksDomain.GetByParametersAsync(
        //            filter.Page, filter.PageSize,
        //            filter.BookTitle,
        //            filter.AuthorFirstName,
        //            filter.AuthorLastName,
        //            filter.Theme,
        //            filter.Publisher,
        //            filter.Place);

        //        var librosDto = _mapper.Map<IEnumerable<BookDto>>(result.Items);

        //        if (librosDto == null || !librosDto.Any())
        //        {
        //            response.Message = "No books found with the provided filters.";
        //            response.IsSuccess = false;
        //            response.TotalCount = 0;

        //            _logger.LogInformation("No books found with filters: {@Filter}", filter);
        //            return response;
        //        }

        //        response.Data = librosDto;
        //        response.TotalCount = result.TotalCount;
        //        response.IsSuccess = true;
        //        response.Message = "Books loaded successfully.";

        //        _logger.LogInformation("Books loaded successfully. Total: {Count}", result.TotalCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = "Error loading books.";
        //        response.IsSuccess = false;

        //        _logger.LogError(ex, "Error in GetBooksAsync. Filters: {@Filter}", filter);
        //    }

        //    return response;
        //}



        //public async Task<(int totalPages, int totalCount)> GetFilteredPaginationDataAsync(BookFilterDto filter)
        //{
        //    try
        //    {
        //        var totalCount = await _booksDomain.GetFilteredCountAsync(
        //                filter.BookTitle,
        //                filter.AuthorFirstName,
        //                filter.AuthorLastName,
        //                filter.Theme,
        //                filter.Publisher,
        //                filter.Place);

        //        var totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

        //        if (totalCount == 0)
        //        {
        //            _logger.LogInformation("No books found with the provided filters");
        //        }

        //        return (totalPages, totalCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        return (0, 0);
        //    }
        //}


        //public async Task<Response<IEnumerable<BookDto>>> GetBookByParametersAsync(BookFilterDto filter)
        //{
        //    var response = new Response<IEnumerable<BookDto>>();

        //    try
        //    {
        //        var result = await _booksDomain.GetByParametersAsync(
        //            filter.Page,
        //            filter.PageSize,
        //            filter.BookTitle,
        //            filter.AuthorFirstName,
        //            filter.AuthorLastName,
        //            filter.Theme,
        //            filter.Publisher,
        //            filter.Place
        //        );

        //        var librosDto = _mapper.Map<IEnumerable<BookDto>>(result.Items);

        //        response.Data = librosDto;
        //        response.TotalCount = result.TotalCount;
        //        response.IsSuccess = true;
        //        response.Message = "Books loaded successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error loading books with filters");
        //        response.IsSuccess = false;
        //        response.Message = "Error loading books";
        //    }

        //    return response;
        //}
        //public async Task<Response<BookAvailabilityDto>> GetBookAvailabilityAsync(BookFilterDto filter)
        //{
        //    var response = new Response<BookAvailabilityDto>();

        //    try
        //    {
        //        var bookResponse = await GetBookByParametersAsync(filter);

        //        if (!bookResponse.IsSuccess || bookResponse.Data == null)
        //        {
        //            response.Data = new BookAvailabilityDto
        //            {
        //                IsAvailable = false,
        //                Message = bookResponse.Message ?? "No book found in the database."
        //            };
        //            response.IsSuccess = true;
        //            return response;
        //        }

        //        var book = bookResponse.Data;
        //        var isAvailable = book.IsAvailable && book.Stock > 0;

        //        response.Data = new BookAvailabilityDto
        //        {
        //            IsAvailable = isAvailable,
        //            Message = isAvailable
        //                ? "The book is available."
        //                : "The book exists, but is not available right now.",
        //            Book = book
        //        };

        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = "Error evaluating book availability.";
        //        _logger.LogError(ex, "Error in GetBookAvailabilityAsync");
        //    }

        //    return response;
        //}

        public async Task<Response<IEnumerable<BookDto>>> GetBookByDate(DateTime fecha)
        {
            var response = new Response<IEnumerable<BookDto>>();

            try
            {
                var books = await _booksDomain.GetBookByDate(fecha);
                response.Data = _mapper.Map<IEnumerable<BookDto>>(books);
                response.IsSuccess = true;
                response.Message = "Books retrieved by date successfully";
            }
            catch (KeyNotFoundException ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError(ex, "No books found for date: {Fecha}", fecha);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Error retrieving books by date";
                _logger.LogError(ex, "Unexpected error in GetBookByDate. Date: {Fecha}", fecha);
            }

            return response;
        }


        //public async Task<Response<BookDto>> CreateBookAsync(BookDto bookDto)
        //{
        //    var response = new Response<BookDto>();

        //    try
        //    {
        //        if (bookDto == null)
        //        {
        //            response.IsSuccess = false;
        //            response.Message = "Book data is required";
        //            return response;
        //        }

        //        // Mapear DTO a entidad de dominio
        //        var bookEntity = _mapper.Map<BookME>(bookDto);

        //        // Asignar valores generados
        //        bookEntity.BookId = Guid.NewGuid();
        //        bookEntity.RegistrationDate = DateTime.UtcNow;

        //        // Guardar libro en dominio/repositorio
        //        var createdBook = await _booksDomain.CreateAsync(bookEntity);

        //        // Mapear la entidad guardada a DTO para la respuesta
        //        response.Data = _mapper.Map<BookDto>(createdBook);
        //        response.IsSuccess = true;
        //        response.Message = "Book created successfully";
        //        _logger.LogInformation("Book created: {BookTitle}", bookDto.BookTitle);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = "Error creating book";
        //        _logger.LogError(ex, "Error in CreateBookAsync. Data: {@BookDto}", bookDto);
        //    }

        //    return response;
        //}


        public async Task<Response<UpdateBookDto>> UpdateBookAsync(UpdateBookDto bookDto)
        {
            var response = new Response<UpdateBookDto>();

            try
            {
                // Paso 1: Buscar el libro existente por ID
                var existingBook = await _booksDomain.GetByIdAsync(bookDto.BookId);
                if (existingBook == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Book not found";
                    return response;
                }

                // Paso 2: Mapear los campos actualizables
                _mapper.Map(bookDto, existingBook);

                // Paso 3: Guardar los cambios
                await _booksDomain.UpdateAsync(existingBook);

                response.Data = _mapper.Map<UpdateBookDto>(existingBook);
                response.IsSuccess = true;
                response.Message = "Book updated successfully";
                _logger.LogInformation("Book updated: {BookId}", bookDto.BookId);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Error updating book";
                _logger.LogError(ex, "Error in UpdateBookAsync. Data: {@BookDto}", bookDto);
            }

            return response;
        }



        public async Task<Response<bool>> DeleteBookAsync(Guid bookId)
        {
            var response = new Response<bool>();
            try
            {
                var result = await _booksDomain.DeleteAsync(bookId);
                if (!result)
                {
                    response.Message = "Book not found or already deleted";
                    return response;
                }

                response.Data = true;
                response.IsSuccess = true;
                response.Message = "Book deleted successfully";
                _logger.LogInformation($"Book deleted: {bookId}");
            }
            catch (Exception ex)
            {
                response.Message = "Error deleting book";
                _logger.LogError(ex, "Error in DeleteBookAsync. BookId: {BookId}", bookId);
            }
            return response;
        }
    }
}
