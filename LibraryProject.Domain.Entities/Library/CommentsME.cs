using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static LibraryProject.Domain.Entities.Library.BookME;

namespace LibraryProject.Domain.Entities.Library
{
    public class CommentsME
    {
        [Table("COMMENTS", Schema = "LIB")]
        public class Commentary
        {
            [Key]
            public int CommentaryId { get; set; }

            [Required]
            [MaxLength(250)]
            public string CommentaryText { get; set; } = string.Empty;

            [Required]
            public DateTime CommentaryCreation { get; set; }

            [Required]
            public DateTime CommentaryUpdate { get; set; }

            [Range(1, 5)]
            public int Rating { get; set; }

            [Required]
            public int UsuId { get; set; }

            [Required]
            public Guid BookId { get; set; }

            // Relación N:1
            [ForeignKey("BookId")]
            public Book Book { get; set; } = null!;
        }
    }
}
