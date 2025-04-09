using AutoMapper;
using LibraryProject.Application.DTO.Library;
using LibraryProject.Application.Interface.Library;
using LibraryProject.Domain.Entities.Library;
using LibraryProject.Transversal.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryProject.Controllers.Library
{
    [AutoValidateAntiforgeryToken]
    [Route("[controller]")]
    [ApiController] // Recomendado para APIs
    public class CommentsController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IBookApplication _bookApplication;

        public CommentsController(IMapper mapper, IBookApplication bookApplication)
        {
            _mapper = mapper;
            _bookApplication = bookApplication;
        }


        // Endpoint para obtener comentarios por libro (versión de prueba)
        [HttpGet("ObtenerPorLibro/{libroId}")]
        public async Task<ActionResult<IEnumerable<CommentsUserDto>>> ObtenerPorLibroAsync(string libroId)
        {
            // Validación básica del ID
            if (string.IsNullOrWhiteSpace(libroId))
            {
                return BadRequest("El ID del libro no puede estar vacío");
            }

            if (!Guid.TryParse(libroId, out Guid libroGuid))
            {
                return BadRequest("El ID del libro debe ser un GUID válido");
            }

            // Datos de prueba mockeados - ESTO ES SOLO PARA PRUEBAS
            var mockComentarios = new List<CommentsUserDto>
        {
                new CommentsUserDto
        {
            BookId = libroGuid,
            BookTitle = "Libro de Prueba",
            Rating = 5,
            CommentaryText = $"Un libro excelente. Lo recomiendo totalmente. ID: {libroId}",
            CommentaryCreation = DateTime.UtcNow.AddDays(-5),
            UserId = "user1",
            UserName = "Ana Martínez"
        },
        new CommentsUserDto
        {
            BookId = libroGuid,
            BookTitle = "Libro de Prueba",
            Rating = 4,
            CommentaryText = $"Lectura muy interesante y bien escrita. ID: {libroId}",
            CommentaryCreation = DateTime.UtcNow.AddDays(-3),
            UserId = "user2",
            UserName = "Carlos Pérez"
        },
        new CommentsUserDto
        {
            BookId = libroGuid,
            BookTitle = "Libro de Prueba",
            Rating = 3,
            CommentaryText = $"Me gustó, aunque el final fue algo predecible. ID: {libroId}",
            CommentaryCreation = DateTime.UtcNow.AddDays(-2),
            UserId = "user3",
            UserName = "Laura Gómez"
        },
        new CommentsUserDto
        {
            BookId = libroGuid,
            BookTitle = "Libro de Prueba",
            Rating = 2,
            CommentaryText = $"Esperaba más del contenido. ID: {libroId}",
            CommentaryCreation = DateTime.UtcNow.AddDays(-1),
            UserId = "user4",
            UserName = "Miguel Torres"
        },
        new CommentsUserDto
        {
            BookId = libroGuid,
            BookTitle = "Libro de Prueba",
            Rating = 5,
            CommentaryText = $"Una joya literaria. Lo volvería a leer. ID: {libroId}",
            CommentaryCreation = DateTime.UtcNow,
            UserId = "user5",
            UserName = "Valentina Ruiz"
        },
        new CommentsUserDto
        {
            BookId = libroGuid,
            BookTitle = "Libro de Prueba",
            Rating = 1,
            CommentaryText = $"No cumplió mis expectativas. ID: {libroId}",
            CommentaryCreation = DateTime.UtcNow.AddHours(-5),
            UserId = "user6",
            UserName = "Luis Ramírez"
        },

            new CommentsUserDto
            {
                BookId = libroGuid, // Usamos el ID que recibimos
                BookTitle = "Libro de Prueba",
                Rating = 5,
                CommentaryText = $"Comentario de prueba para el libro {libroId}",
                CommentaryCreation = DateTime.UtcNow.AddDays(-5),
                UserId = "user1",
                UserName = "Usuario Prueba"
            },
            new CommentsUserDto
            {
                BookId = libroGuid, // Usamos el ID que recibimos
                BookTitle = "Libro de Prueba",
                Rating = 4,
                CommentaryText = $"Otro comentario para {libroId}",
                CommentaryCreation = DateTime.UtcNow.AddDays(-2),
                UserId = "user2",
                UserName = "Otro Usuario"
            }
        };

            // Filtramos por el ID recibido (aunque ya los creamos con ese ID)
            var resultado = mockComentarios.Where(c => c.BookId == libroGuid);

            return Ok(await Task.FromResult(resultado));
        }

        // Endpoint adicional para pruebas - puede eliminarse después
        [HttpGet("ObtenerEjemplo")]
        public ActionResult<CommentsUserDto> GetEjemplo()
        {
            return new CommentsUserDto
            {
                BookId = Guid.NewGuid(),
                BookTitle = "Ejemplo Automático",
                Rating = 3,
                CommentaryText = "Este es un comentario de ejemplo",
                CommentaryCreation = DateTime.UtcNow,
                UserId = "ejemplo",
                UserName = "Usuario Ejemplo"
            };
        }


        [HttpPost("CreateComment")]
        [ValidateAntiForgeryToken] // Make sure this is here
        public async Task<IActionResult> CreateComment([FromBody] CommentsUserDto model)
        {
            try
            {
                // Fill in missing fields if needed
                model.CommentaryCreation = DateTime.Now;
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.UserId = userId;
                model.UserName = User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";

                var result = await _bookApplication.CreateCommentAsync(model, userId);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { message = "Error interno del servidor" });
            }
        }








    }
}