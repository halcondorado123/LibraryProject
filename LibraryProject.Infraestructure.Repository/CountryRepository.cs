using LibraryProject.Domain.Entities.Location;
using LibraryProject.Infraestructure.Interface.Location;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Infraestructure.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryME>> GetAllCountriesAsync()
        {
            return await _context.Countries.ToListAsync();
        }
    }
}
