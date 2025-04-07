namespace LibraryProject.Domain.Entities.Location
{
    public class RegionME
    {
        public int RegionId { get; set; }
        public string? Region { get; set; }
        public CountryME? IdCountry { get; set; }
    }
}
