using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Annotations;

namespace LibraryProject.Application.DTO.Library
{
    public class CommentsUserDto
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty; // Para mostrar en la vista si lo necesitas
        public int Rating { get; set; }
        public string CommentaryText { get; set; } = string.Empty;
        public DateTime CommentaryCreation { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty; // Opcional: para mostrar nombre del usuario
    }

}

