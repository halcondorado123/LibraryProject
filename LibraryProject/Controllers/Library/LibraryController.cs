using AutoMapper;
using LibraryProject.Application.DTO;
using LibraryProject.Application.DTO.Library;
using LibraryProject.Domain.Entities.Library;
using LibraryProject.Infraestructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers.Library
{
    [ApiController]
    [Authorize(Roles = ("Administrador"))]
    [Route("[controller]")]
    public class LibraryController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IBookApplication _bookApplication;


        public LibraryController(IMapper mapper, IBookApplication bookApplication)
        {
            _mapper = mapper;
            _bookApplication = bookApplication;
        }

        [HttpGet("books")]
        //[SwaggerOperation(
        //    Summary = SwaggerClientsCommentsSPA.Clients.GetClientsSummary,
        //    Description = SwaggerClientsCommentsSPA.Clients.GetClientsDescription)]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(int page = 1, int pageSize = 10)
        {
            var response = await _bookApplication.GetBooksAsync(page, pageSize);

            if (!response.IsSuccess || response.Data == null || !response.Data.Any())
                return NotFound(new { message = response.Message });

            return Ok(response.Data);
        }

        [HttpGet("GetUserByParameters")]
        //[SwaggerOperation(
        //    Summary = SwaggerClientsCommentsSPA.Clients.GetClientsSummary,
        //    Description = SwaggerClientsCommentsSPA.Clients.GetClientsDescription)]
        public async Task<IActionResult> GetBookByParameters([FromQuery] BookFilterDto filters)
        {
            var response = await _bookApplication.GetBookByParametersAsync(filters);

            if (!response.IsSuccess || response.Data == null)
                return NotFound(new { message = response.Message });

            return Ok(response.Data);
        }

        [HttpGet("GetBookAvailability")]
        //[SwaggerOperation(
        //    Summary = SwaggerClientsCommentsSPA.Clients.GetClientsSummary,
        //    Description = SwaggerClientsCommentsSPA.Clients.GetClientsDescription)]
        public async Task<IActionResult> GetBookAvailability([FromQuery] BookFilterDto filters)
        {
            try
            {
                var response = await _bookApplication.GetBookByParametersAsync(filters);

                if (!response.IsSuccess || response.Data == null)
                {
                    return NotFound(new
                    {
                        available = false,
                        message = response.Message ?? "No book found in the database."
                    });
                }

                var bookDto = response.Data;

                if (!bookDto.IsAvailable || bookDto.Stock <= 0)
                {
                    return Ok(new
                    {
                        available = false,
                        message = "The book exists, but is not available right now.",
                        book = bookDto
                    });
                }

                return Ok(new
                {
                    available = true,
                    message = "The book is available.",
                    book = bookDto
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, message = ex.Message });
            }
        }

        [HttpGet("GetBookByDate")]
        //[SwaggerOperation(
        //    Summary = SwaggerClientsCommentsSPA.Clients.GetClientsSummary,
        //    Description = SwaggerClientsCommentsSPA.Clients.GetClientsDescription)]
        public async Task<IActionResult> GetBookByDate(DateTime date)
        {
            try
            {
                var response = await _bookApplication.GetBookByDate(date);

                if (!response.IsSuccess || response.Data == null || !response.Data.Any())
                {
                    return NotFound(new { message = response.Message ?? "No books found in the database." });
                }

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, message = ex.Message });
            }
        }

        [HttpPost]
        //[SwaggerOperation(
        //    Summary = SwaggerClientsCommentsSPA.Clients.GetClientsSummary,
        //    Description = SwaggerClientsCommentsSPA.Clients.GetClientsDescription)]
        public async Task<IActionResult> CreateNewBookRecord([FromBody] BookDto bookDto)
        {
            try
            {
                var response = await _bookApplication.CreateBookAsync(bookDto);

                if (!response.IsSuccess || response.Data == null)
                    return BadRequest(new { message = response.Message ?? "Book could not be created." });

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, message = ex.Message });
            }
        }

        [HttpPut("UpdateBook")]
        //[SwaggerOperation(
        //    Summary = SwaggerClientsCommentsSPA.Clients.GetClientsSummary,
        //    Description = SwaggerClientsCommentsSPA.Clients.GetClientsDescription)]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDto bookDto)
        {
            try
            {
                var response = await _bookApplication.UpdateBookAsync(bookDto);

                if (!response.IsSuccess || response.Data == null)
                    return NotFound(new { message = response.Message ?? "The book was not found in the database." });

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        //[SwaggerOperation(
        //    Summary = SwaggerClientsCommentsSPA.Clients.GetClientsSummary,
        //    Description = SwaggerClientsCommentsSPA.Clients.GetClientsDescription)]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            try
            {
                var response = await _bookApplication.DeleteBookAsync(id);

                if (!response.IsSuccess || !response.Data)
                    return NotFound(new { message = $"Book with ID {id} was not found to delete." });

                return Ok(new { message = $"Book with ID {id} successfully deleted." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, message = ex.Message });
            }
        }
    }
}
