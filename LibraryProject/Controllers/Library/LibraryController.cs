using AutoMapper;
using LibraryProject.Application.DTO;
using LibraryProject.Application.DTO.Library;
using LibraryProject.Application.Interface.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace LibraryProject.Controllers.Library
{
    [ApiController]
    //[Authorize(Roles = ("Administrador"))]
    [AllowAnonymous]
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

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var response = await _bookApplication.GetBooksAsync(page, pageSize);
            var libros = response?.Data ?? new List<BookDto>();
            return View(libros);
        }


  

        [HttpPost("SearchByParams")]
        public async Task<IActionResult> SearchByParams([FromBody] BookFilterDto filter)
        {
            var response = await _bookApplication.GetBookByParametersAsync(filter);
            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpGet("SearchByDate")]
        public async Task<IActionResult> SearchByDate([FromQuery] DateTime date)
        {
            var response = await _bookApplication.GetBookByDate(date);
            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateBookDto dto)
        {
            var response = await _bookApplication.UpdateBookAsync(dto);
            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _bookApplication.DeleteBookAsync(id);
            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpGet("GetBookAvailability")]
        public async Task<IActionResult> GetBookAvailability([FromQuery] BookFilterDto filters)
        {
            try
            {
                var response = await _bookApplication.GetBookAvailabilityAsync(filters);

                if (!response.IsSuccess || response.Data == null)
                    return BadRequest(new { success = false, message = response.Message });

                return Ok(new
                {
                    available = response.Data.IsAvailable,
                    message = response.Data.Message,
                    book = response.Data.Book
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}