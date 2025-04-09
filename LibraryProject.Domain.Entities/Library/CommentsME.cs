using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryProject.Domain.Entities.UserAttributes;

namespace LibraryProject.Domain.Entities.Library
{
    [Table("COMMENTS", Schema = "LIB")]
    public class CommentsME
    {
        [Key]
        public int CommentaryId { get; set; }

        // Relación con BookME
        [Required]
        public Guid BookId { get; set; }

        [ForeignKey("BookId")]
        public BookME Book { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(250)]
        public string CommentaryText { get; set; } = string.Empty;

        [Required]
        public DateTime CommentaryCreation { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime CommentaryUpdate { get; set; } = DateTime.UtcNow;

        // Relación con AppUsuario (IdentityUser)
        [Required]
        public string UserId { get; set; } // IdentityUser usa string como Id

        [ForeignKey("UserId")]
        public AppUsuario User { get; set; }

    }
}