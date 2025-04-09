using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Domain.Entities.Location
{
    public class CountryME
    {
        public int PaisId { get; set; }
        public string? IsoCode { get; set; }
        public string? Pais { get; set; }
    }
}
