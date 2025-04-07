using LibraryProject.Domain.Entities.Location;

namespace LibraryProject.Infraestructure.Interface.Location
{
    public interface ICountryRepository
    {
        Task<IEnumerable<CountryME>> GetAllCountriesAsync();
    }
}
