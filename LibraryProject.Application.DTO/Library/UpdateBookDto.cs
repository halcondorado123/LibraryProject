namespace LibraryProject.Application.DTO.Library
{
    public class UpdateBookDto
    {
        public Guid BookId { get; set; } // Requerido para identificar el libro
        public string? AuthorFirstName { get; set; }
        public string? AuthorLastName { get; set; }
        public string? Theme { get; set; }
        public string? BookTitle { get; set; }
        public string? Place { get; set; }
        public string? Publisher { get; set; }
        public int Stock { get; set; }
        public bool IsAvailable { get; set; }
    }
}
