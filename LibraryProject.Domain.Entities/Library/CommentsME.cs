using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Domain.Entities.Library
{ 
        [Table("COMMENTS", Schema = "LIB")]
        public class CommentsME
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

            [ForeignKey("BookId")]
            public BookME Book { get; set; } = null!;
    }
}
