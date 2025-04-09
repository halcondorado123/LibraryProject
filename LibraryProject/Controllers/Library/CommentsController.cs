using LibraryProject.Application.DTO.Library;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers.Library
{
    [Route("[controller]")]
    [ApiController] // Recomendado para APIs
    public class CommentsController : ControllerBase
    {
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
            CommentaryId = 1,
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
            CommentaryId = 2,
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
            CommentaryId = 3,
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
            CommentaryId = 4,
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
            CommentaryId = 5,
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
            CommentaryId = 6,
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
                CommentaryId = 7,
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
                CommentaryId = 8,
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
                CommentaryId = 99,
                BookId = Guid.NewGuid(),
                BookTitle = "Ejemplo Automático",
                Rating = 3,
                CommentaryText = "Este es un comentario de ejemplo",
                CommentaryCreation = DateTime.UtcNow,
                UserId = "ejemplo",
                UserName = "Usuario Ejemplo"
            };
        }
    }
}