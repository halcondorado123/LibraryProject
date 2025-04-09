namespace LibraryProject.Application.DTO.Library
{
    public class BookDto
    {
        public Guid BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? Author { get; set; }
        public string? Theme { get; set; }
        public string? Place { get; set; }
        public string? Publisher { get; set; }
        public string? Observations { get; set; }

    }
}
