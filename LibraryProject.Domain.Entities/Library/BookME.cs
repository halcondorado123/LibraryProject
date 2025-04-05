using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Domain.Entities.Library
{
    [Table("BOOKS", Schema = "LIB")]
    public class BookME
    {
        [Key]
        public Guid BookId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string AuthorFirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AuthorLastName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Theme { get; set; }

        [Required]
        [MaxLength(200)]
        public string BookTitle { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Place { get; set; }

        [MaxLength(100)]
        public string? Publisher { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public int Stock { get; set; }

        // Relación 1:N
        public ICollection<CommentsME> Commentaries { get; set; } = new List<CommentsME>();
    }
}
