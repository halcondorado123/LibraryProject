namespace LibraryProject.Application.DTO.Library
{
    public class BookFilterDto
    {
        public string? AuthorFirstName { get; set; }
        public string? AuthorLastName { get; set; }
        public string? Theme { get; set; }
        public string? BookTitle { get; set; }
        public string? Place { get; set; }
        public string? Publisher { get; set; }
    }
}
