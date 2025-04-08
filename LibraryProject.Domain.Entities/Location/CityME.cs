using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Domain.Entities.Location
{
    public class CityME
    {
        public int CityId { get; set; }

        public int City { get; set; }

        public RegionME RegionId { get; set; }
    }
}
