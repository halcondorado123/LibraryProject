using AutoMapper;
using LibraryProject.Application.DTO;
using LibraryProject.Application.DTO.Library;
using LibraryProject.Application.Interface.Library;
using LibraryProject.Application.Services.Library;
using LibraryProject.Domain.Entities.Library;
using LibraryProject.Infraestructure.Interface.Library;
using LibraryProject.Infraestructure.Repository.Library;
using LibraryProject.Transversal.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace LibraryProject.Controllers.Library
{
    [ApiController]
    [Route("[controller]")] // Mejor práctica para rutas
    public class LibraryController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IBooksRepository _booksRepository;
        private readonly IBookApplication _booksApplication;


        public LibraryController(IMapper mapper, IBooksRepository booksRepository, IBookApplication booksApplication)
        {
            _mapper = mapper;
            _booksRepository = booksRepository;
            _booksApplication = booksApplication;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _booksApplication.GetAllBooksAsync();

            if (!result.IsSuccess || result.Data == null)
                return View(new List<BookDto>());

            return View(result.Data);
        }











        //[HttpGet("Index")]
        //public async Task<IActionResult> Index([FromQuery] BookFilterDto filter)
        //{
        //    // 1. Obtener datos
        //    var libros = await _bookApplication.GetBooksAsync(filter);
        //    (int totalPages, int totalCount) = await _bookApplication.GetFilteredPaginationDataAsync(filter);

        //    // 2. Configurar ViewBag para la vista
        //    ViewBag.CurrentPage = filter.Page;
        //    ViewBag.TotalPages = totalPages;
        //    ViewBag.Filters = filter;

        //    // 3. Detección de filtros activos para el JavaScript
        //    ViewBag.HasActiveFilters = !string.IsNullOrEmpty(filter.BookTitle) ||
        //                             !string.IsNullOrEmpty(filter.AuthorFirstName) ||
        //                             !string.IsNullOrEmpty(filter.AuthorLastName) ||
        //                             !string.IsNullOrEmpty(filter.Theme) ||
        //                             !string.IsNullOrEmpty(filter.Publisher) ||
        //                             !string.IsNullOrEmpty(filter.Place);

        //    // 4. Retornar vista
        //    return View(libros.Data ?? new List<BookDto>());
        //}



        //[HttpPost("SearchByParams")]
        //public async Task<IActionResult> SearchByParams([FromBody] BookFilterDto filter)
        //{
        //    var libros = await _bookApplication.GetBooksAsync(filter.Page, filter.PageSize,
        //        filter.BookTitle, filter.AuthorFirstName, filter.AuthorLastName,
        //        filter.Theme, filter.Publisher, filter.Place);

        //    var (totalPages, totalCount) = await _bookApplication.GetFilteredPaginationDataAsync(filter.PageSize,
        //        filter.BookTitle, filter.AuthorFirstName, filter.AuthorLastName,
        //        filter.Theme, filter.Publisher, filter.Place);

        //    var result = new
        //    {
        //        Data = libros,
        //        Pagination = new
        //        {
        //            filter.Page,
        //            filter.PageSize,
        //            TotalPages = totalPages,
        //            TotalCount = totalCount
        //        }
        //    };

        //    return Ok(result);
        //}


        //[HttpGet("SearchByDate")]
        //public async Task<IActionResult> SearchByDate([FromQuery] DateTime date)
        //{
        //    var response = await _bookApplication.GetBookByDate(date);
        //    if (!response.IsSuccess)
        //        return NotFound(response.Message);

        //    return Ok(response.Data);
        //}

        //[HttpPut("Update")]
        //public async Task<IActionResult> Update([FromBody] UpdateBookDto dto)
        //{
        //    var response = await _bookApplication.UpdateBookAsync(dto);
        //    if (!response.IsSuccess)
        //        return NotFound(response.Message);

        //    return Ok(response.Data);
        //}

        //[HttpDelete("Delete/{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var response = await _bookApplication.DeleteBookAsync(id);
        //    if (!response.IsSuccess)
        //        return NotFound(response.Message);

        //    return Ok(response.Data);
        //}

        //[HttpGet("GetBookAvailability")]
        //public async Task<IActionResult> GetBookAvailability([FromQuery] BookFilterDto filters)
        //{
        //    try
        //    {
        //        var response = await _bookApplication.GetBookAvailabilityAsync(filters);

        //        if (!response.IsSuccess || response.Data == null)
        //            return BadRequest(new { success = false, message = response.Message });

        //        return Ok(new
        //        {
        //            available = response.Data.IsAvailable,
        //            message = response.Data.Message,
        //            book = response.Data.Book
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { success = false, message = ex.Message });
        //    }
        //}
    }
}