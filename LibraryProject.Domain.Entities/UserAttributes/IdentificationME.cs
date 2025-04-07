using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities.UserAttributes
{
    public class IdentificationME
    {
        //[Key]
        //[Column("IdentificationId")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdentificationId { get; set; }

        //[Required]
        //[MaxLength(100)]
        //[Column(TypeName = "varchar(100)")] // Especifica el tipo de datos en la base de datos
        public string? IdentificationType { get; set; }
    }
}
